using ru.novolabs.SuperCore.LimsDictionary;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.Core;
using System.Xml.Linq;
using ru.novolabs.SuperCore.Crypto;

namespace ru.novolabs.SuperCore.DictionaryCore
{
    public static class CacheHelper
    {
        private static List<DirectoryVersionInfo> versions;        
        private static Crypto.Cryptor cryptor;


        /// <summary>
        /// Загружает в DictionaryList закешированные справочники из папки cache        
        /// </summary>
        /// <returns>Количество загруженных справочников из кэша</returns>
        public static int GetCachedDictionaries(EventHandler<DictionaryLoadingEventArgs> DictionaryLoadingEvent, BaseUserSession session)
        {
            int result = 0;
            cryptor = new Cryptor(true);
            DirectoryInfo cacheDirectory = Directory.CreateDirectory(PathHelper.AssemblyDirectory + "/cache");
            if (session == null)
            {
                if (ProgramContext.SolutionType == ProgramContext.SolutionTypes.LIMS)
                    session = (BaseUserSession)ProgramContext.LisCommunicator.LimsUserSession;
                else
                    session = (BaseUserSession)ProgramContext.HemCommunicator.HemUserSession;
            }
            if (versions == null)
                versions = ProgramContext.BaseCommunicator.DirectoryVersions(session);
            foreach (string path in Directory.GetFiles(PathHelper.AssemblyDirectory + "/cache"))
            {
                try
                {
                    ObjectReader reader = new ObjectReader(ProgramContext.Dictionaries);
                    string xml = "";
                    string name = "";
                    int ver = 0;
                    string fileName = path.Split(new string[] { "\\" }, StringSplitOptions.None).LastOrDefault();
                    Log.WriteText("Reading dicitonary from cache file {0}", fileName);
#if DEBUG
                if (!path.Contains(".cache"))
                    continue;
                xml = File.ReadAllText(path);
                XElement xel = XElement.Parse(xml);
                name = xel.Descendants("s").First().Attribute("n").Value;
                xel = xel.Descendants("f").FirstOrDefault(x => x.Attribute("n") != null && x.Attribute("n").Value == "version" && x.Attribute("t") != null && x.Attribute("t").Value == "I");
                if (xel != null)
                    ver = Convert.ToInt32(xel.Attribute("v").Value);
#else
                    if (path.Contains(".cache"))
                        continue;

                    if(new System.IO.FileInfo(path).Length <= 0)
                    {
                        File.Delete(path);
                        continue;
                    }
                    List<byte> info = new List<byte>();
                    using (FileStream sr = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        byte[] readed = new byte[2];
                        int bufReadedCount = 0, totalReadedCount = 0;
                        do
                        {
                            bufReadedCount = sr.Read(readed, 0, 2);
                            totalReadedCount += bufReadedCount;
                            info = info.Concat(readed).ToList();
                        }
                        while (bufReadedCount != 0 && totalReadedCount < 512 && (readed[0] != 0x0D || readed[1] != 0x0A)); // Считываем первую строку (до /r/n), получаем метаинформацию
                    }
                    byte[] decrypted = cryptor.DecryptMessage(info.Take(info.Count - 2).ToArray(), cryptor.GetInitializationVector()); // Остальное до символов новой строки - имя и версия справочника
                    int nullsCount = 0;    // AES дописывает в конец нули, пока строка не станет кратным 16
                    for (int i = decrypted.Length - 1; i > 0; --i)
                        if (decrypted[i] == 0)
                            nullsCount++;
                        else
                            break;
                    name = Encoding.Default.GetString(decrypted, 0, decrypted.Length - 4 - nullsCount); // В строке 4 байта занимает int число версии (+не забываем про нули)
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(decrypted, name.Length, 4);
                    ver = BitConverter.ToInt32(decrypted, name.Length); // 4 байта после имени-число версии
#endif
                    var serverDict = versions.FirstOrDefault(x => x.Name == name);
                    if (serverDict == null || serverDict.Version != (ver))
                        File.Delete(path);

                    else
                    {
#if !DEBUG
                        using (FileStream sr = new FileStream(path, FileMode.Open, FileAccess.Read))
                        {
                            byte[] tmp = new byte[(int)sr.Length - info.Count];
                            sr.Seek(info.Count, SeekOrigin.Begin);
                            sr.Read(tmp, 0, (int)sr.Length - info.Count);
                            xml = cryptor.DecryptMessage(tmp, cryptor.GetInitializationVector(), Encoding.Default);                            
                        }
#endif
                        Log.WriteText(string.Format("Loading cached dictionary {0}", name));
                        if (DictionaryLoadingEvent != null)
                        {
                            int progressPercentage = (int)(result * 100) / ProgramContext.Dictionaries.DictionaryList.Count;
                            DictionaryLoadingEvent(null, new DictionaryLoadingEventArgs(progressPercentage, name));
                        }
                        int errorCode = 0; string errorMessage = "";
                        reader.ReadXMLObject(xml, ProgramContext.Dictionaries.DictionaryList[name], ref errorCode, ref errorMessage);
                        result++;
                    }
                }
                catch (Exception e) 
                {
                    if (File.Exists(path))
                        File.Delete(path);
                    Log.WriteError(e.ToString());
                }
            }
            return result;
        }

        public static void LoadDictionaries()
        {
            foreach (String name in ProgramContext.Dictionaries.DictionaryList.Keys)
                LoadDictionary(name);
        }

        public static void CheckCachedDictionaries()
        {           
                        
        }
        /// <summary>
        /// Метод обновления справочников: сравнивает версию каждого загруженного словаря с серверной, и если она не совпадает, то обновляет словарь
        /// </summary>
        /// <param name="session"></param>
        public static void UpdateLoadedDictionaries(BaseUserSession session)
        {
            List<DirectoryVersionInfo> versions = ProgramContext.BaseCommunicator.DirectoryVersions(session);
            foreach (String name in ProgramContext.Dictionaries.DictionaryList.Keys)
            {
                IBaseDictionary dictionary = ProgramContext.Dictionaries[name];
                if (dictionary != null && ProgramContext.Dictionaries.IsStaticDictionary(dictionary) == false)
                {
                    DirectoryVersionInfo version = versions.FirstOrDefault(x => x.Name == name);
                    if (version == null || version.Version != dictionary.Version)
                    {
                        ProgramContext.Dictionaries.RefreshDictionary(name, session);
                    }
                }
            }
            ProgramContext.Dictionaries.UpdateReferences();
        }
        /// <summary>
        /// Сохранение серверных справочников в файловый кэш
        /// </summary>
        public static void SaveAllDictionaries()
        {
            Encoding encoding = Encoding.GetEncoding(1251);
            DirectoryInfo cacheDirectory = Directory.CreateDirectory(PathHelper.AssemblyDirectory + "/cache");
            foreach (Object dictionary in ProgramContext.Dictionaries.DictionaryList.Values)
            {

                Type baseType = dictionary.GetType().BaseType;
                Type elemType;
                Object res;

                DictionaryClass<DictionaryItem> dc = new DictionaryClass<DictionaryItem>();
                dc.Elements.Add(new HospitalDictionaryItem());
                if (ProgramContext.Dictionaries.IsStaticDictionary((IBaseDictionary)dictionary))
                    continue;
                if (baseType != typeof(Object))
                {
                    elemType = baseType.GetGenericArguments()[0];
                    Type generic = typeof(DictionaryClass<>);
                    Type[] typeArgs = { elemType };
                    Type Constructed = generic.MakeGenericType(typeArgs);                    
                    res = CopyDictionary(dictionary);
                }
                else
                {
                    res = dictionary;
                    elemType = dictionary.GetType().GetGenericArguments()[0];
                }
                File.WriteAllText(string.Format("{0}/cache/{1}.cache", PathHelper.AssemblyDirectory, ((IBaseDictionary)dictionary).Name), res.SerializeDictWithIgnoredReferences(encoding));
            }
        }

        public static void SaveDictionary(String DictionaryName)
        {
            Encoding encoding = Encoding.GetEncoding(1251);
            DirectoryInfo cacheDirectory = Directory.CreateDirectory(PathHelper.AssemblyDirectory + "/cache");
            Object dictionary = ProgramContext.Dictionaries[DictionaryName];
            //XmlSerializer xmlSer = new XmlSerializer(dictionary.GetType());
            
            //File.WriteAllText(cacheDirectory.FullName + "\\" + ((DictionaryClass<DictionaryItem>)dictionary).Name, xmlSer.Serialize(encoding));
            File.WriteAllText(cacheDirectory.FullName + "\\" + ((DictionaryClass<DictionaryItem>)dictionary).Name, dictionary.Serialize(encoding));
        }

        private static Object CopyDictionary(Object dictionary)
        {
            Object di;
            Type elemType;
            Type baseType = dictionary.GetType().BaseType;
            elemType = baseType.GetGenericArguments()[0];
            Type generic = typeof(DictionaryClass<>);
            Type[] typeArgs = { elemType };
            Type Constructed = generic.MakeGenericType(typeArgs);
            di = Activator.CreateInstance(Constructed);
            foreach (PropertyInfo propInfo in dictionary.GetType().GetProperties())
            {
                PropertyInfo subpropInfo = di.GetType().GetProperty(propInfo.Name);
                if (subpropInfo != null)
                {
                    if (subpropInfo.CanWrite)
                        subpropInfo.SetValue(di, propInfo.GetValue(dictionary, null), null);
                }
            }
            return di;
        }
        /// <summary>
        /// Загружает справочник из файлового кэша, если он есть
        /// </summary>
        /// <param name="Name">Имя справочника</param>
        /// <returns>Экземпляр загруженного справочника. Если нету, то null</returns>
        public static Object LoadDictionary(object dictionary)
        {
            string name = ((IBaseDictionary)dictionary).Name;
            String filePath = string.Format(@"{0}cache\{1}.cache", AppDomain.CurrentDomain.BaseDirectory, name);
            if (!File.Exists(filePath))
                return null;
            Log.WriteText("Loading cached dictionary " + name);
            Object res = null;
            if (dictionary.GetType().BaseType != typeof(Object))
            {
                object copy = CopyDictionary(dictionary);
                res = SerializeHelper.DeserializeDictWithIgnoredReferences(copy.GetType(), File.ReadAllText(filePath), Encoding.GetEncoding(1251));                
            }            
            return res;
        }

    }
}
