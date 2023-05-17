using ru.novolabs.SuperCore.CommonBusinesObjects;
using ru.novolabs.SuperCore.Core;
using ru.novolabs.SuperCore.Core.HardwareWork;
using ru.novolabs.SuperCore.Crypto;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace ru.novolabs.SuperCore
{
    public class NlsServerException : Exception
    {
        public NlsServerException(int errorCode, string errorMessage) : base(errorMessage) { ErrorCode = errorCode; ErrorMessage = errorMessage; }

        public int ErrorCode { get; private set; }
        public string ErrorMessage { get; private set; }
    }

    /// <summary>
    /// Класс исключения, предназначенный для прерывания долгого запроса к серверу
    /// </summary>
    public class NlsOperationCanceledException : Exception { }

    public class UserSessionNotFoundException : ApplicationException
    {
        public UserSessionNotFoundException() : base(String.Format("NLS User Session ID not registered for current thread. HashCode = [{0}]", Thread.CurrentThread.GetHashCode())) { }
    }

    public class BaseCommunicator
    {
        static bool firstRequest = true;
        private ProgramContext.SolutionTypes solutionType;
        protected static Cryptor Cryptor = new Cryptor();

        [CSN("SolutionType")]
        public ProgramContext.SolutionTypes SolutionType
        {
            get { return this.solutionType == ProgramContext.SolutionTypes.Unknown ? ProgramContext.SolutionType : this.solutionType; }
            set { this.solutionType = value; }
        }
        
        private string sessionId = string.Empty;
        protected BaseUserSession UserSession { get; set; }

        private bool loggedIn = false;

        [CSN("LoggedIn")]
        public bool LoggedIn
        {
            get { return loggedIn; }
            set { loggedIn = value; }
        }
        protected BaseDictionaryCache dictionaryCache = null;

        protected String ServerAddress { get; set; }

        public string GetServerAddress() { return ServerAddress; }
        
        private class CancellationObject
        {
            internal bool CancellationPending { get; set; }
        }


        public BaseCommunicator() { }

        private string GetCLID(BaseUserSession userSession)
        {
            String MacAddress = GetMacAddress();
            String Time = DateTime.Now.ToString() + DateTime.Now.Millisecond.ToString("D3");
            return Time + userSession.Login + MacAddress;
        }
        public virtual void Login(string login, string password, string serverAddress, out BaseUserSession userSession, AbstractCodec codec) { userSession = new BaseUserSession(); }

        public virtual void LoadServerOptions() { }

        private string GetREQID(bool useSessionKey, BaseUserSession userSession)
        {
            byte[] keyBytes = Cryptor.GetSessionKey();
            // Если useSessionKey == false, заполняем массив keyBytes случайными числами
            if (!useSessionKey)
            {
                Random r = new Random();
                r.NextBytes(keyBytes);
            }

            return Convert.ToBase64String(keyBytes);
        }

        public string LoadObject(object requestObj, object resultObj, string methodName, bool sendId, bool writeObjectTag, BaseUserSession userSession = null)
        {
            if (null == requestObj)
                throw new ArgumentNullException("requestObj");

            int errorCode = 0;
            string errorMessage = String.Empty;
            String responseXML;           

            BaseUserSession usrSession = (userSession != null) ? userSession : this.UserSession;

            if (requestObj.GetType().GetInterface("ISendableObject") != null)
            {
                ((ISendableObject)requestObj).PrepareToSend();
            }
            ObjectWriter writer = new ObjectWriter();
            ObjectReader reader = new ObjectReader(ProgramContext.Dictionaries);

            String requestXML = writer.MakeXMLRequest(requestObj, methodName, usrSession.SessionId, sendId, writeObjectTag);
            if (NeedLogging(methodName))
            {
                Log.WriteText(String.Format("\n\rRequest XML:\n\r{0}\n\r", requestXML));
            }


            bool useSessionKey = false;
            if (methodName.Equals(XMLConst.XML_METHOD_LOGIN))
            {
                usrSession.Login = requestObj.GetType().GetProperty("Login").GetValue(requestObj, null).ToString();
                usrSession.CLID = GetCLID(usrSession);
                useSessionKey = true;
            }
            else if (methodName.Equals(XMLConst.XML_METHOD_SYSTEM_LOGIN))
            {
                usrSession.Login = requestObj.GetType().GetProperty("Code").GetValue(requestObj, null).ToString();
                usrSession.CLID = GetCLID(usrSession);
                useSessionKey = true;
            }

            if (IsNeededEncryption())
            {
                responseXML = ServerRequestCrypt(requestXML, usrSession, useSessionKey);
            }
            else
            {
                // Эта ветвь кода и метод ServerRequest c двумя аргументами должны в скором времени умереть
                responseXML = ServerRequest(requestXML, this.ServerAddress);
            }
            
            if (NeedLogging(methodName))
            {
                responseXML = FormatWhitespaces(responseXML);
                Log.WriteText(String.Format("Responce XML:\n\r{0}\n\r", FormatXml(responseXML)));
            }

            String sessionIDOut = reader.ReadXMLObject(responseXML, resultObj, ref errorCode, ref errorMessage);
            // Кэширование
            if (methodName.Equals(XMLConst.XML_METHOD_DIRECTORY) && !requestXML.Contains("elements"))
            {
                CacheResponse(resultObj, responseXML);
            }
            if (errorCode != 0)
                throw new NlsServerException(errorCode, errorMessage);

            return sessionIDOut;
        }

        private bool IsNeededEncryption()
        {
            //return false;
            //Нужно ли нам вообще CRYPTOVANNE
            //object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false]; // Убрать, когда полностью перейдём на шированный трафик
            //bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false; // Убрать, когда полностью перейдём на шированный трафик
            bool versionAfter129 = true;
            Boolean useEncryption = true; // Убрать, когда полностью перейдём на шированный трафик
            if (!versionAfter129 || (this.SolutionType == ProgramContext.SolutionTypes.HEM))
                useEncryption = false;
            return useEncryption;
        }

        private string ServerRequestCrypt(String requestXML, BaseUserSession usrSession, bool useSessionKey)
        {
            //usrSession.CLID = "22.03.2023 17:48:589352000C29831B6C";

            String responseXML;
            const int Compression_Type_Deflate = 0x1; // Тип сжатия - алгоритм Deflate (RFC 1951)
            Encoding encoding = Encoding.GetEncoding(1251);
            // Сжимаем запрос перед шифрованием и отправкой серверу
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(usrSession.CLID));
            byte[] IV = hash.Take(Cryptor.GetInitializationVector().Count()).ToArray();
            byte[] compressedRequest = Compresser.Compress(requestXML, encoding);
            // Шифруем сжатый запрос
            byte[] encryptedRequest = Cryptor.EncryptMessage(compressedRequest, IV);
            var arrayList = new ArrayList(new byte[] { Compression_Type_Deflate });
            arrayList.AddRange(hash);
            string encodedCLID = Convert.ToBase64String(arrayList.Cast<byte>().ToArray());
            string REQID = GetREQID(useSessionKey, usrSession);
            //string REQID = "GNykhU/3wzPEQRnHDQNV3DY30FVKB3wk4yMdQEL+WCWIn6tITm04lKLTucRxRxazukadD964hWxC1yN84EKxGw==";

            // Посылаем запрос серверу
            byte[] enryptedResponse = null;
            try
            {
                enryptedResponse = ServerRequest(encryptedRequest, this.ServerAddress, encodedCLID, REQID);
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    // Если сервер перезагружался в процессе работы клиента, в кэше сервера будет утерян текущий сессионный ключ и сервер вернёт
                    // код состояния HTTP 401 (Unauthorized). В таком случае необходимо повторно отправить запрос с корректным значением REQID (содержащим ключ)
                    if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                    {
                        enryptedResponse = ServerRequest(encryptedRequest, this.ServerAddress, encodedCLID, GetREQID(true, usrSession));
                    }
                    else
                        throw ex;
                }
                else
                    throw ex;
            }

            // Расшифровываем ответ от сервера
            byte[] response = Cryptor.DecryptMessage(enryptedResponse, IV);
            // Распаковываем дешифрованный ответ
            responseXML = Compresser.Decompress(response, encoding);
            return responseXML;
        }

        private void CacheResponse(object resultObj, String responseXML)
        {
            IBaseDictionary dict = ((IBaseDictionary)resultObj);
            string cacheDir = string.Format("{0}/cache", PathHelper.AssemblyDirectory);
            if (Directory.Exists(cacheDir) == false)
                Directory.CreateDirectory(cacheDir);
#if DEBUG
            File.WriteAllText(string.Format("{0}/cache/{1}.cache", PathHelper.AssemblyDirectory, dict.Name), responseXML);
#else
            byte[] encrypted = null;
            byte[] nameAndVer = Encoding.Default.GetBytes(dict.Name);
            byte[] intBytes = BitConverter.GetBytes(dict.Version);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(intBytes);
            nameAndVer = nameAndVer.Concat(intBytes).ToArray();
            var cryptor = new Crypto.Cryptor(true);
            nameAndVer = cryptor.EncryptMessage(nameAndVer, cryptor.GetInitializationVector());
            encrypted = cryptor.EncryptMessage(responseXML, cryptor.GetInitializationVector(), Encoding.Default);
            using (var stream = new FileStream(string.Format("{0}\\cache\\{1}", AppDomain.CurrentDomain.BaseDirectory, Guid.NewGuid().ToString()), FileMode.Append))
            {
                stream.Write(nameAndVer, 0, nameAndVer.Length);
                stream.Write(Encoding.Default.GetBytes("\r\n"), 0, 2);
                stream.Write(encrypted, 0, encrypted.Length);
            }
#endif
        }
       
        public string LoadXml(object requestObj, string methodName, bool sendId, bool writeObjectTag, BaseUserSession userSession = null)
        {
            if (null == requestObj)
                throw new ArgumentNullException("requestObj");

            int errorCode = 0;
            string errorMessage = String.Empty;
            String responseXML;
            Encoding encoding = Encoding.GetEncoding(1251);

            BaseUserSession usrSession = (userSession != null) ? userSession : this.UserSession;

            if (requestObj.GetType().GetInterface("ISendableObject") != null)
            {
                ((ISendableObject)requestObj).PrepareToSend();
            }

            ObjectWriter writer = new ObjectWriter();
            ObjectReader reader = new ObjectReader(ProgramContext.Dictionaries);

            String requestXML = writer.MakeXMLRequest(requestObj, methodName, usrSession.SessionId, sendId, writeObjectTag);
            if (NeedLogging(methodName))
            {
                Log.WriteText(String.Format("\n\rRequest XML:\n\r{0}\n\r", requestXML));
            }

            //Нужно ли нам вообще CRYPTOVANNE
            object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false]; // Убрать, когда полностью перейдём на шированный трафик
            bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false; // Убрать, когда полностью перейдём на шированный трафик


            bool useSessionKey = false;
            if (methodName.Equals(XMLConst.XML_METHOD_LOGIN))
            {
                usrSession.Login = requestObj.GetType().GetProperty("Login").GetValue(requestObj, null).ToString();
                usrSession.CLID = GetCLID(usrSession);
                useSessionKey = true;
            }
            else if (methodName.Equals(XMLConst.XML_METHOD_SYSTEM_LOGIN))
            {
                usrSession.Login = requestObj.GetType().GetProperty("Code").GetValue(requestObj, null).ToString();
                usrSession.CLID = GetCLID(usrSession);
                useSessionKey = true;
            }

            const int Compression_Type_Deflate = 0x1; // Тип сжатия - алгоритм Deflate (RFC 1951)

            Boolean useEncryption = true; // Убрать, когда полностью перейдём на шированный трафик
            if (!versionAfter129)
                useEncryption = false;
            if (useEncryption)
            {
                // Сжимаем запрос перед шифрованием и отправкой серверу
                MD5 md5 = MD5.Create();
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(usrSession.CLID));
                byte[] IV = hash.Take(Cryptor.GetInitializationVector().Count()).ToArray();
                byte[] compressedRequest = Compresser.Compress(requestXML, encoding);
                // Шифруем сжатый запрос
                byte[] encryptedRequest = Cryptor.EncryptMessage(compressedRequest, IV);
                var arrayList = new ArrayList(new byte[] { Compression_Type_Deflate });
                arrayList.AddRange(hash);
                string encodedCLID = Convert.ToBase64String(arrayList.Cast<byte>().ToArray());
                string REQID = GetREQID(useSessionKey, usrSession);

                // Посылаем запрос серверу
                byte[] enryptedResponse = null;
                try
                {
                    enryptedResponse = ServerRequest(encryptedRequest, this.ServerAddress, encodedCLID, REQID);
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                    {
                        // Если сервер перезагружался в процессе работы клиента, в кэше сервера будет утерян текущий сессионный ключ и сервер вернёт
                        // код состояния HTTP 401 (Unauthorized). В таком случае необходимо повторно отправить запрос с корректным значением REQID (содержащим ключ)
                        if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                        {
                            enryptedResponse = ServerRequest(encryptedRequest, this.ServerAddress, encodedCLID, GetREQID(true, usrSession));
                        }
                    }
                    else
                        throw ex;
                }

                // Расшифровываем ответ от сервера
                byte[] response = Cryptor.DecryptMessage(enryptedResponse, IV);
                // Распаковываем дешифрованный ответ
                responseXML = Compresser.Decompress(response, encoding);
            }
            else
            {
                // Эта ветвь кода и метод ServerRequest c двумя аргументами должны в скором времени умереть
                responseXML = ServerRequest(requestXML, this.ServerAddress);
            }

            if (NeedLogging(methodName))
            {
                responseXML = FormatWhitespaces(responseXML);
                Log.WriteText(String.Format("Responce XML:\n\r{0}\n\r", FormatXml(responseXML)));
            }

            

            if (errorCode != 0)
                throw new NlsServerException(errorCode, errorMessage);

            return responseXML;
        }

        private bool NeedLogging(string methodName)
        {
            if (ProgramContext.Settings == null)
                return false;

            if ((ProgramContext.Settings.LoggingLevel == SystemLoggingLevels.LOGIN_LEVEL_DEBUG) || (ProgramContext.Settings.LoggingLevel == SystemLoggingLevels.LOGIN_LEVEL_ALL))
            {
                string[] skipLoggingRequestNames = new string[] {
                    XMLConst.XML_METHOD_LOAD_SERVICE_PRICE,
                    XMLConst.XML_METHOD_DIRECTORY,
                    XMLConst.XML_METHOD_USER_OPTIONS,
                    XMLConst.XML_METHOD_DIRECTORY_VERSIONS
                };

                return !skipLoggingRequestNames.Contains(methodName);
            }
            else
                return false;
        }

        /// <summary>
        /// Удаляет коды переноса строк #$D#$A, которые назначает Дельфи, и заменяет их на \r\n
        /// </summary>
        /// <param name="xml">Входная строка, в которой должны быть найдены вхождения</param>
        /// <returns></returns>
        string FormatWhitespaces(String xml)
        {
            return xml.Replace("#$D#$A", "\r\n");
        }

        // Быстрое форматирование XML с использованием класса XDocument
        string FormatXml(string xml)
        {
            try
            {
                return XDocument.Parse(xml).ToString();
            }
            catch (Exception)
            {
                return xml;
            }
        }

        private String ServerRequest(String requestString, String serverAddress)
        {
            HttpWebRequest request;
            WebResponse responce;
            Stream requestStream;
            Stream resultStream;
            Byte[] sendBytes;
            String resultString = String.Empty;
            StreamReader reader;

            if (!serverAddress.Contains(ServerConst.HTTP_http_Prefix))
            {
                serverAddress = ServerConst.HTTP_http_Prefix + serverAddress;
            }

            request = (HttpWebRequest)HttpWebRequest.Create(serverAddress);
            request.Method = ServerConst.HTTP_Method_Post;
            request.Timeout = 120000;
            request.ProtocolVersion = HttpVersion.Version10;
            request.UserAgent = ServerConst.HTTP_User_Agent;
            request.Accept = ServerConst.HTTP_Accept;
            request.ContentType = ServerConst.HTTP_Content_Type;
            request.Proxy = null;

            requestStream = request.GetRequestStream();
            sendBytes = Encoding.GetEncoding(1251).GetBytes(requestString);
            requestStream.Write(sendBytes, 0, sendBytes.Length);
            requestStream.Close();

            responce = request.GetResponse();
            resultStream = responce.GetResponseStream();

            reader = new StreamReader(resultStream, Encoding.GetEncoding(1251));
            resultString = reader.ReadToEnd();
            reader.Close();
            resultStream.Close();
            responce.Close();
            return resultString;
        }

        private byte[] ServerRequest(byte[] requestBytes, String serverAddress, String CLID, String REQID)
        {
            if (!serverAddress.Contains("http://"))
                serverAddress = "http://" + serverAddress;
            string resultString = String.Empty;
            byte[] resultBytes;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serverAddress);
            int serverResponseTimeout = 600000;
            if (ProgramContext.Settings != null)
                serverResponseTimeout = (int?)ProgramContext.Settings["serverResponseTimeout", false] ?? serverResponseTimeout;
            request.Timeout = serverResponseTimeout;
            request.Method = "POST";
            request.ProtocolVersion = new Version(1, 0);
            request.Headers.Add(RequestHeaderConst.Header_CLID, CLID);
            request.Headers.Add(RequestHeaderConst.Header_REQID, REQID);

            if (IsWinFormsApplication)
            {
                resultBytes = GetWFResponseAsync(requestBytes, request);
            }
            else
            {
                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(requestBytes, 0, requestBytes.Length);
                
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream responceStream = response.GetResponseStream())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            responceStream.CopyTo(ms);
                            resultBytes = ms.ToArray();
                        }
                    }
                }
            }
            return resultBytes;
        }

        // В случае WinForms-приложения выполняем запрос асинхронно с отображением progress-формы во время ожидания ответа от сервера.
        private byte[] GetWFResponseAsync(byte[] requestBytes, HttpWebRequest request)
        {
            return null;
            /*Form topForm = null;
            try
            {
                if (Application.OpenForms.Count > 0)
                {
                    for (int i = (Application.OpenForms.Count - 1); i < 1; i--)
                    {
                        if (!(Application.OpenForms[i] is AwaitForm))
                        {
                            topForm = Application.OpenForms[i];
                            break;
                        }
                    }
                }
            }
            catch { }

            AwaitForm awaitForm = new AwaitForm() { BackForm = topForm };

            DateTime requestStartTime = DateTime.Now;
            Log.Debug("request sent to appServer", requestStartTime);
            
            byte[] responseBytes = awaitForm.GetResponse(requestBytes, request, NeedShowAwaitForm);
            
            DateTime requestEndTime = DateTime.Now;
            Log.Debug(String.Format("response received in [{0} ms] from appServer", (requestEndTime - requestStartTime).Milliseconds.ToString()), requestEndTime);

// Это действие крадёт ~100 мс при каждом обращении к серверу из формы. Если нужно будет вернуть - необходимо оптимизировать
//            if (topForm != null)
//                topForm.Refresh();
            return responseBytes;*/
        }

        private delegate void MethodDelegate();

        public T GetObjectFromFileServer<T>(Int32 fileId, LimsUserSession userSession)
        {
            return GetObjectFromFileServer<T>(fileId, Encoding.UTF8, userSession);
        }

        protected T GetObjectFromFileServer<T>(Int32 fileId, Encoding encoding, LimsUserSession userSession)
        {
            String xml = GetFileFromServerAsString(fileId, encoding, userSession);
            xml = xml.Replace("True", "true");
            xml = xml.Replace("False", "false");

            if (!String.IsNullOrEmpty(xml))
                return xml.Deserialize<T>(encoding);
            else
                return default(T);
        }

        protected String GetFileFromServerAsString(Int32 fileId, Encoding encoding, LimsUserSession userSession)
        {
            BaseUserSession usrSession = (userSession != null) ? userSession : this.UserSession;
            String resultString = String.Empty;

            WebResponse responce = FileServerRequest(fileId, this.ServerAddress + ServerConst.HTTP_FileServer_URL + usrSession.SessionId);
            if (responce != null)
            {
                Stream resultStream = responce.GetResponseStream();
                StreamReader reader = new StreamReader(resultStream, encoding);
                resultString = reader.ReadToEnd();
                reader.Close();
                resultStream.Close();
                responce.Close();
            }
            return resultString;
        }

        protected String GetFileFromServerAsString(Int32 fileId, Encoding encoding, HemUserSession userSession)
        {
            BaseUserSession usrSession = (userSession != null) ? userSession : this.UserSession;
            String resultString = String.Empty;

            WebResponse responce = FileServerRequest(fileId, this.ServerAddress + ServerConst.HTTP_FileServer_URL + usrSession.SessionId);
            if (responce != null)
            {
                Stream resultStream = responce.GetResponseStream();
                StreamReader reader = new StreamReader(resultStream, encoding);
                resultString = reader.ReadToEnd();
                reader.Close();
                resultStream.Close();
                responce.Close();
            }
            return resultString;
        }

        /// <summary>
        /// Загружает в профайлы пользователей фильтр журнала
        /// </summary>
        public void GetUserProfileFilter(FilterInfo filterInfo, LimsUserSession userSession)
        {
            if (filterInfo.Filter > 0)
                filterInfo.JournalFilter = GetObjectFromFileServer<JournalFilterSettings>(filterInfo.Filter, Encoding.GetEncoding(1251), userSession) ?? new JournalFilterSettings();
            else
                filterInfo.JournalFilter = new JournalFilterSettings();
        }

        public byte[] GetFileFromServerWithResponse(int fileId, LimsUserSession userSession, out WebResponse response)
        {
            BaseUserSession usrSession = (userSession != null) ? userSession : this.UserSession;
            response = FileServerRequest(fileId, this.ServerAddress + ServerConst.HTTP_FileServer_URL + userSession.SessionId);
            return GetResponseContent(response);
        }

        public byte[] GetFileFromServerWithResponse(string path, LimsUserSession userSession, out WebResponse response)
        {
            BaseUserSession usrSession = (userSession != null) ? userSession : this.UserSession;
            response = FileServerRequest(path, userSession.SessionId);
            return GetResponseContent(response);
        }

        public Byte[] GetFileFromServer(Int32 fileId, LimsUserSession userSession)
        {
            BaseUserSession usrSession = (userSession != null) ? userSession : this.UserSession;
            var response = FileServerRequest(fileId, this.ServerAddress + ServerConst.HTTP_FileServer_URL + userSession.SessionId);
            return GetResponseContent(response);
        }

        public Byte[] GetFileFromServer(Int32 fileId, HemUserSession userSession)
        {
            BaseUserSession usrSession = (userSession != null) ? userSession : this.UserSession;
            WebResponse response = FileServerRequest(fileId, this.ServerAddress + ServerConst.HTTP_FileServer_URL + userSession.SessionId);
            return GetResponseContent(response);
        }

        private byte[] GetResponseContent(WebResponse responce)
        {
            //Stream resultStream = responce.GetResponseStream();
            //Byte[] result = new Byte[responce.ContentLength];
            //var reader = new BinaryReader(resultStream);
            //result = reader.ReadBytes((Int32)responce.ContentLength);
            //resultStream.Close();
            //responce.Close();
            //reader.Close();
            //return result;
            using (var memoryStream = new MemoryStream())
            {
                if (responce == null)
                    return null;
                responce.GetResponseStream().CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private WebResponse FileServerRequest(Int32 fileId, String serverAddress)
        {
            if (fileId == 0)
                throw new ArgumentException("Бессмысленно запрашивать у сервера файл с Id = 0", "fileId");

            HttpWebRequest request;
            WebResponse responce = null;

            if (!serverAddress.Contains(ServerConst.HTTP_http_Prefix))
            {
                serverAddress = ServerConst.HTTP_http_Prefix + serverAddress;
            }

            request = (HttpWebRequest)HttpWebRequest.Create(serverAddress);
            request.Method = ServerConst.HTTP_Method_Get;
            request.Timeout = 120000;
            request.ProtocolVersion = HttpVersion.Version11;
            request.UserAgent = ServerConst.HTTP_User_Agent;
            request.Accept = ServerConst.HTTP_File_Accept;
            request.ContentType = ServerConst.HTTP_File_Content_Type;
            request.Proxy = null;
            request.Headers.Add(ServerConst.HTTP_Lis_Id + fileId.ToString());

            try
            {
                responce = request.GetResponse();
            }
            catch (Exception ex)
            {
                string errorText = "Request to the FileServer:\r\n\r\n" + request.RequestUri.ToString() + "\r\n" + request.Headers.ToString();
                errorText += "Server Error:\r\n\r\n" + ex.ToString();
                Log.WriteError(errorText);
            }

            return responce;
        }

        private HttpWebRequest InitFileServerRequest(string filePath, string method, string aSessionid)
        {
            string serverAddress = this.ServerAddress + ServerConst.HTTP_FileServer_URL + aSessionid;
            if (filePath == "")
                throw new ArgumentException("Бессмысленно запрашивать у сервера файл без адреса", "filePath");

            HttpWebRequest request;
            

            if (!serverAddress.Contains(ServerConst.HTTP_http_Prefix))
            {
                serverAddress = ServerConst.HTTP_http_Prefix + serverAddress;
            }

            request = (HttpWebRequest)HttpWebRequest.Create(serverAddress);
            request.Method = method;
            request.Timeout = 120000;
            request.ProtocolVersion = HttpVersion.Version11;
            request.UserAgent = ServerConst.HTTP_User_Agent;
            request.Accept = ServerConst.HTTP_File_Accept;
            request.ContentType = ServerConst.HTTP_File_Content_Type;
            request.Proxy = null;
            request.Headers.Add(ServerConst.HTTP_Lis_Path + filePath);
            return request;
        }

        public WebResponse FileServerRequest(String filePath, String userSessionId)
        {
            HttpWebRequest request = InitFileServerRequest(System.Web.HttpUtility.UrlEncode(filePath), ServerConst.HTTP_Method_Get, userSessionId);
            WebResponse responce;
            try
            {
                responce = request.GetResponse();
            }
            catch (Exception ex)
            {
                string errorText = "Request to the FileServer:\r\n\r\n" + request.RequestUri.ToString() + "\r\n" + request.Headers.ToString();
                errorText += "Server Error:\r\n\r\n" + ex.ToString();
                Log.WriteError(errorText);
                throw ex;
            }

            return responce;
        }

        public WebResponse UploadFile(string filePath, byte[] content, string userSessionId)
        {
            if (content.Length == 0)
                throw new ArgumentException("Файл не может быть пустым", "content");

            WebResponse response;
            HttpWebRequest request = InitFileServerRequest(System.Web.HttpUtility.UrlEncode(filePath), ServerConst.HTTP_Method_Post, userSessionId);

            request.Headers.Add(ServerConst.HTTP_Lis_Description + "");
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(content, 0, content.Length);
            }
            try
            {
                response = request.GetResponse();
            }
            catch (Exception ex)
            {
                string errorText = "Request to the FileServer:\r\n\r\n" + request.RequestUri.ToString() + "\r\n" + request.Headers.ToString();
                errorText += "Server Error:\r\n\r\n" + ex.ToString();
                Log.WriteError(errorText);
                throw ex;
            }
            return response;
        }

        public List<string> GetFilesList(string filePath, string userSessionId)
        {
            HttpWebRequest request = InitFileServerRequest(filePath, ServerConst.HTTP_Method_List, userSessionId);
            WebResponse responce;
            try
            {
                responce = request.GetResponse();
            }
            catch (Exception ex)
            {
                string errorText = "Request to the FileServer:\r\n\r\n" + request.RequestUri.ToString() + "\r\n" + request.Headers.ToString();
                errorText += "Server Error:\r\n\r\n" + ex.ToString();
                Log.WriteError(errorText);
                throw ex;
            }

            byte[] content = GetResponseContent(responce);
            return Encoding.UTF8.GetString(content).Split(new string[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public void CreateDictionaryCache()
        {
            Log.WriteText("Creating Dictionary cache class " + DictionaryCacheType().Name);
            dictionaryCache = (BaseDictionaryCache)Activator.CreateInstance(DictionaryCacheType());
            dictionaryCache.Communicator = this;
        }


        protected virtual void InitLocals()
        {
            // Метод предназначен для переопределения в наследниках
        }
        /// <summary>
        /// Основной метод загрузки справочников с сервера
        /// </summary>
        /// <param name="userSession">LimsUserSession, HemUserSession</param>
        public void LoadDictionaries(BaseUserSession userSession)
        {
            if (dictionaryCache == null)
                CreateDictionaryCache();
            dictionaryCache.LoadDictionaries(userSession);
            InitLocals();
        }
        /// <summary>
        /// Метод загрузки одного конкретного справочника
        /// </summary>
        /// <param name="dictionary">Экземпляр справочника</param>
        /// <param name="userSession">LimsUserSession, HemUserSession</param>
        public void LoadDictionary(object dictionary, BaseUserSession userSession)
        {
            if (dictionaryCache == null)
                CreateDictionaryCache();
            dictionaryCache.LoadDictionary(dictionary, userSession);
        }

        protected virtual Type DictionaryCacheType()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Завершает сессию
        /// </summary>
        protected void Logout(BaseUserSession userSession = null)
        {
            try
            {
                LoadObject(new Object(), null, XMLConst.XML_METHOD_SYSTEM_LOGOUT, false, false, userSession);
            }
            catch { }
        }

        public virtual bool DictionaryIsCodeUnique(string DictionaryName, string code)
        {
            IList DictionaryElements;
            Object Dictionary = dictionaryCache.DictionaryList[DictionaryName];

            DictionaryElements = ((IBaseDictionary)Dictionary).DictionaryElements;

            foreach (DictionaryItem Item in DictionaryElements)
            {
                if (Item.Code.Equals(code))
                {
                    return false;
                }
            }
            return true;
        }

        internal void GetDictionary(string dictionaryName, object dictionary, BaseUserSession userSession)
        {
            bool writeObjectTag = false;
            try
            {
                object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
                bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
                if (versionAfter129)
                    writeObjectTag = true;
                if (ProgramContext.SolutionType == ProgramContext.SolutionTypes.HEM)
                    writeObjectTag = true;
            }
            catch { }

            if (this.SolutionType == ProgramContext.SolutionTypes.HEM) writeObjectTag = true;

            if (this.SolutionType == ProgramContext.SolutionTypes.HEM)
                LoadObject(new { Directory = dictionaryName }, dictionary, XMLConst.XML_METHOD_DIRECTORY, false, writeObjectTag, userSession);
            else
                LoadObject(new { Name = dictionaryName }, dictionary, XMLConst.XML_METHOD_DIRECTORY, false, writeObjectTag, userSession);
        }

        internal void GetDictionary(string dictionaryName, object dictionary, BaseUserSession userSession, List<ObjectRef> elementIds = null)
        {
            bool writeObjectTag = false;
            try
            {
                object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
                bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
                if (versionAfter129)
                    writeObjectTag = true;
                if (ProgramContext.SolutionType == ProgramContext.SolutionTypes.HEM)
                    writeObjectTag = true;
            }
            catch { }

            if (this.SolutionType == ProgramContext.SolutionTypes.HEM) writeObjectTag = true;

            if (this.SolutionType == ProgramContext.SolutionTypes.HEM)
                LoadObject(new { Directory = dictionaryName/*, Elements = elementIds - не уверен, нужно проверять */ }, dictionary, XMLConst.XML_METHOD_DIRECTORY, false, writeObjectTag, userSession);
            else
                LoadObject(new { Name = dictionaryName, Elements = elementIds }, dictionary, XMLConst.XML_METHOD_DIRECTORY, false, writeObjectTag, userSession);
        }




        internal Int32 SaveDictionary(string dictionaryName, object item, BaseUserSession userSession)
        {
            BaseObject result = new BaseObject();
            DirectorySaveRequest request = new DirectorySaveRequest() { Directory = dictionaryName, Element = item };

            Object[] attrs = item.GetType().GetCustomAttributes(typeof(OldSaveMethod), true);

            bool writeObjectTag = false;
            object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
            bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
            if (versionAfter129)
                writeObjectTag = true;
            LoadObject(request, result, attrs.Length == 0 ? XMLConst.XML_METHOD_DIRECTORY_SAVE_NEW : XMLConst.XML_METHOD_DIRECTORY_SAVE,
                false, writeObjectTag, userSession);
            return result.Id;
        }

        public Int32 SaveDictionary(DictionarySaveRequest request, BaseUserSession userSession, out int newDictionaryVersion)
        {
            DictionarySaveResponce result = new DictionarySaveResponce();
            bool writeObjectTag = false;
            object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
            bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
            if (versionAfter129)
                writeObjectTag = true;
            LoadObject(request, result, XMLConst.XML_METHOD_DIRECTORY_SAVE, false, writeObjectTag, userSession);
            request.Element.Id = result.Id;
            newDictionaryVersion = result.Version;
            return result.Id;
        }


       

        private string GetMacAddress()
        {
            string macAddresses = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }

        [CSN("IsWinFormsApplication")]
        public bool IsWinFormsApplication { get; set; }
        [CSN("NeedShowAwaitForm")]
        public bool NeedShowAwaitForm { get; set; }

        public DictionaryItem GetDictionaryElement(string dictionaryName, int elementId)
        {
            return GetDictionaryElements(dictionaryName, new List<ObjectRef>() { new ObjectRef(elementId) })[0];
        }

        public IList<DictionaryItem> GetDictionaryElements(string dictionaryName, List<ObjectRef> refs)
        {
            IBaseDictionary dictionary = (IBaseDictionary)ProgramContext.Dictionaries.GetDictionary(dictionaryName);
            IBaseDictionary tempDictionary = (IBaseDictionary)Activator.CreateInstance(dictionary.GetType(), dictionaryName);
            GetDictionary(dictionaryName, tempDictionary, null, refs);
            List<DictionaryItem> result = new List<DictionaryItem>();
            foreach (DictionaryItem item in tempDictionary.DictionaryElements)
                result.Add(item);
            return result;
        }

        public int RemoveDictionary(DictionaryRemoveRequest request, BaseUserSession userSession)
        {
            DictionaryRemoveResponse response = new DictionaryRemoveResponse();
            LoadObject(request, response, XMLConst.XML_METHOD_REMOVE_DICTIONARY, false, true, userSession);
            return response.Version;
        }

        /// <summary>
        /// Возвращает коллекцию версий справочников
        /// </summary>
        /// <returns></returns>
        public List<DirectoryVersionInfo> DirectoryVersions(BaseUserSession userSession)
        {
            DirectoryVesionInfoSet responce = new DirectoryVesionInfoSet();

            bool writeObjectTag = false;
            try
            {
                object versionAfter129Obj = ProgramContext.Settings["versionAfter129", false];
                bool versionAfter129 = versionAfter129Obj != null ? Boolean.Parse(versionAfter129Obj.ToString()) : false;
                if (versionAfter129)
                    writeObjectTag = true;
                if (ProgramContext.SolutionType == ProgramContext.SolutionTypes.HEM)
                    writeObjectTag = true;
            }
            catch { }

            if (this.SolutionType == ProgramContext.SolutionTypes.HEM) writeObjectTag = true;

            if (this.SolutionType == ProgramContext.SolutionTypes.HEM)
                LoadObject(new Object(), responce, XMLConst.XML_METHOD_DIRECTORY_VERSIONS, false, writeObjectTag, userSession);
            else
                LoadObject(new Object(), responce, XMLConst.XML_METHOD_DIRECTORY_VERSIONS, false, writeObjectTag, userSession);

            return responce.Versions;
        }

    }
}