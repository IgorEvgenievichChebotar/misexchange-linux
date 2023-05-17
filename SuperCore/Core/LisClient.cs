using System;
using System.Windows.Forms;
using ru.novolabs.SuperCore.LisBusinessObjects;
using ru.novolabs.SuperCore.LisDictionary;
using ru.novolabs.WMI;
using ru.novolabs.SuperCore.Core;
using zlib;
using System.Collections;
using System.Threading;

namespace ru.novolabs.SuperCore
{
    public class LisClientDictionaryCache : LisDictionaryCache
    {
        public override void CreateDictionaries()
        {
            base.CreateDictionaries();
            AddDictionary(typeof(EmployeeDictionary), LisDictionaryNames.Employee);
            AddDictionary(typeof(WorkPlaceDictionary), LisDictionaryNames.WorkPlace);
        }
    }

    public class LisClient : LisCommunicator
    {
        private EmployeeDictionaryItem currentUser;
        private Hashtable userTable = new Hashtable();
        private WorkPlaceDictionaryItem currentWorkPlace;
        private Hashtable workPlaceTable = new Hashtable();

        /// <summary>
        /// Инициализирует новый экземпляр класса LisClient
        /// </summary>
        public LisClient() : base()
        {
            Session = new LisUserSession(this);
        }
        /// <summary>
        /// Инициализирует новый экземпляр класса LisClient с указанием режима (однопользовательский/многопользовательский)
        /// </summary>
        public LisClient(bool multiUserMode) : base(multiUserMode)
        {
            Session = new LisUserSession(this);
        }

        public LisUserSession Session { get; set; }

        public EmployeeDictionaryItem CurrentUser
        {
            get { return GetCurrentUser(); }
            protected set { SetCurrentUser(value); }
        }

        public WorkPlaceDictionaryItem CurrentWorkPlace
        {
            get { return GetCurrentWorkPlace(); }
            set { SetCurrentWorkPlace(value); }
        }


        protected override Type DictionaryCacheType()
        {
            return typeof(LisClientDictionaryCache);
        }

        public void Login(string login, string password, string serverAddress, out LimsUserSession userSession)
        {
            Log.WriteText(String.Format("Logging in to server {0} as '{1}'", serverAddress, login));
            //DoLogin(login, password, "", serverAddress, null);

            this.ServerAddress = serverAddress;

            bool result = false;
            userSession = new LimsUserSession();
            Log.WriteText("ClientId: {0}", SimpleCodec.GetClientIdView());
            try
            {

                LoginRequest Login = new LoginRequest();
                LoginResponce loginResult = new LoginResponce();

                Login.Password = password;
                Login.Login = login;
                Login.Machine = Environment.MachineName;
                Login.ClientId = SimpleCodec.GetClientId();
                Login.InstanceCount = "0";
                Login.SessionCode = "19154";
                Login.Lab = "Лаборатория";
                Login.Company = "Компания";

                userSession.Cryptor = new Cryptor();
                SessionId = LoadObject(Login, loginResult, XMLConst.XML_METHOD_LOGIN, false, true, userSession);

                CurrentUser = new EmployeeDictionaryItem() { Id = loginResult.Employee.GetRef() };
                CurrentWorkPlace = new WorkPlaceDictionaryItem() { Id = loginResult.WorkPlace.GetRef() };
                userSession.SessionId = SessionId;
                
                result = !String.Empty.Equals(SessionId);
                //if (dictionaryCache != null)
                //{
                //    InitLocals();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось войти в систему:\n\r" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoggedIn = result;

        }

        public void Login(string login, string password, string serverAddress)
        {
            Log.WriteText(String.Format("Logging in to server {0} as '{1}'", serverAddress, login));
            

            this.ServerAddress = serverAddress;

            bool result = false;
            Log.WriteText("ClientId: {0}", SimpleCodec.GetClientIdView());
            try
            {
                LoginRequest Login = new LoginRequest();
                LoginResponce loginResult = new LoginResponce();

                Login.Password = password;
                Login.Login = login;
                Login.Machine = Environment.MachineName;
                Login.ClientId = SimpleCodec.GetClientId();
                Login.InstanceCount = "0";
                Login.SessionCode = "19154";
                Login.Lab = "Лаборатория";
                Login.Company = "Компания";

                SessionId = LoadObject(Login, loginResult, XMLConst.XML_METHOD_LOGIN, false, false);

                CurrentUser = new EmployeeDictionaryItem() { Id = loginResult.Employee.GetRef() };
                CurrentWorkPlace = new WorkPlaceDictionaryItem() { Id = loginResult.WorkPlace.GetRef() };
                result = !String.Empty.Equals(SessionId);
                //if (dictionaryCache != null)
                //{
                //    InitLocals();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось войти в систему:\n\r" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoggedIn = result;

        }

        protected override bool InternalDoLogin(string loginName, string password, string remoteIP, string serverAddress)
        {
            bool result = false;
            Log.WriteText("ClientId: {0}", SimpleCodec.GetClientIdView());
            try
            {
                LoginRequest login = new LoginRequest();
                LoginResponce loginResult = new LoginResponce();

                login.Password = password;
                login.Login = loginName;
                login.Machine = Environment.MachineName;
                login.ClientId = SimpleCodec.GetClientId();
                login.InstanceCount = "0";
                login.SessionCode = "19154";
                login.Lab = "Лаборатория";
                login.Company = "Компания";

                SessionId = LoadObject(login, loginResult, XMLConst.XML_METHOD_LOGIN, false, false);

                CurrentUser = new EmployeeDictionaryItem() { Id = loginResult.Employee.GetRef() };
                CurrentWorkPlace = new WorkPlaceDictionaryItem() { Id = loginResult.WorkPlace.GetRef() };
                result = !String.Empty.Equals(SessionId);
                //if (dictionaryCache != null)
                //{
                //    InitLocals();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось войти в систему:\n\r" + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        protected override void InitLocals()
        {
            CurrentUser = (EmployeeDictionaryItem)DictionaryCache[LisDictionaryNames.Employee, CurrentUser.Id];

            var workPlaceDictionary = (WorkPlaceDictionary)DictionaryCache[LisDictionaryNames.WorkPlace];
            CurrentWorkPlace = (workPlaceDictionary != null) ? (WorkPlaceDictionaryItem)DictionaryCache[LisDictionaryNames.WorkPlace, CurrentWorkPlace.Id] : null;
            
            LimsUserSession UserSession = new LimsUserSession();
            UserSession.User = CurrentUser;
            UserSession.WorkPlace = CurrentWorkPlace;

        }

        private EmployeeDictionaryItem GetCurrentUser()
        {
            if (multiUserMode)
            {
                lock (userTable)
                {
                    if (userTable.Count == 0)
                        throw new InvalidOperationException("Multi user mode. SessionId not registered");
                    if (!userTable.ContainsKey(SessionId))
                        throw new UserSessionNotFoundException();

                    return (EmployeeDictionaryItem)userTable[SessionId];
                }
            }
            else
                return currentUser;
        }

        private void SetCurrentUser(EmployeeDictionaryItem value)
        {
            if (multiUserMode)
                userTable[SessionId] = value;
            else
                this.currentUser = value;
        }

        private WorkPlaceDictionaryItem GetCurrentWorkPlace()
        {
            if (multiUserMode)
            {
                lock (workPlaceTable)
                {
                    if (workPlaceTable.Count == 0)
                        throw new InvalidOperationException("Multi user mode. SessionId not registered");
                    if (!workPlaceTable.ContainsKey(SessionId))
                        throw new UserSessionNotFoundException();

                    return (WorkPlaceDictionaryItem)workPlaceTable[SessionId];
                }
            }
            else
                return currentWorkPlace;
        }

        private void SetCurrentWorkPlace(WorkPlaceDictionaryItem value)
        {
            if (multiUserMode)
                workPlaceTable[SessionId] = value;
            else
                this.currentWorkPlace = value;
        }

        public override void UnregisterUserSession()
        {
            string sessionId = SessionId;

            base.UnregisterUserSession();

/*            if (!userSessionIdTable.ContainsValue(sessionId))
            {
                userTable.Remove(sessionId);
                workPlaceTable.Remove(sessionId);
            } */
        }
    }
}