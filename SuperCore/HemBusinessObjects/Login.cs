using System;
using ru.novolabs.SuperCore.HemBusinessObjects;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{

    public class LoginRequest
    {
        public string login;
        public string password;
        public string machine;
        public string remoteIp;

        [CSN("Login")]
        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        [CSN("Password")]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [CSN("Machine")]
        public string Machine
        {
            get { return machine; }
            set { machine = value; }
        }

        [CSN("RemoteIp")]
        public string RemoteIp
        {
            get { return remoteIp; }
            set { remoteIp = value; }
        }
    }

    public class LoginResponce
    {
        private ObjectRef employee = new ObjectRef();
        private List<ObjectRef> rights = new List<ObjectRef>();

        [CSN("Rights")]
        public List<ObjectRef> Rights
        {
            get { return rights; }
            set { rights = value; }
        }

        [CSN("Employee")]
        public ObjectRef Employee
        {
            get { return employee; }
            set { employee = value; }
        }
    }
}