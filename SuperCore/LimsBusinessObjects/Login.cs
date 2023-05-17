using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class SystemLogin
    {
        public SystemLogin(String loggingLevel)
        {
            Machine = Environment.MachineName;
            Code = String.Empty;
            LoggingLevel = loggingLevel;
        }

        [CSN("Machine")]
        public String Machine { get; private set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("LoggingLevel")]
        public String LoggingLevel { get; set; }
    }

    public class LoginRequest
    {
        public LoginRequest()
        {
            Company = String.Empty;
            Lab = String.Empty;
            Login = String.Empty;
            Password = String.Empty;
            Machine = String.Empty;
            ClientId = String.Empty;
            SessionCode = String.Empty;
            InstanceCount = String.Empty;
        }

        [CSN("Company")]
        public String Company { get; set; }
        [CSN("Lab")]
        public String Lab { get; set; }
        [CSN("Login")]
        public String Login { get; set; }
        [CSN("Password")]
        public String Password { get; set; }
        [CSN("Machine")]
        public String Machine { get; set; }
        [CSN("ClientId")]
        public String ClientId { get; set; }
        [CSN("SessionCode")]
        public String SessionCode { get; set; }
        [CSN("InstanceCount")]
        public String InstanceCount { get; set; }
    }

    public class LoginResponce
    {
        public LoginResponce()
        {
            Rights = new List<ObjectRef>();
            Employee = new ObjectRef();
            WorkPlace = new ObjectRef();
            Departments = new List<ObjectRef>();
            Hospitals = new List<ObjectRef>();
            ServerVersion = String.Empty;
            SessionCode = 0;
            AdminMode = false;
            DoctorId = 0;
        }

        [CSN("Rights")]
        public List<ObjectRef> Rights { get; set; }
        [CSN("Employee")]
        public ObjectRef Employee { get; set; }
        [CSN("WorkPlace")]
        public ObjectRef WorkPlace { get; set; }
        [CSN("Departments")]
        public List<ObjectRef> Departments { get; set; }
        [CSN("Hospitals")]
        public List<ObjectRef> Hospitals { get; set; }
        [CSN("ServerVersion")]
        public String ServerVersion { get; set; }
        [CSN("SessionCode")]
        public Int64 SessionCode { get; set; }
        [CSN("AdminMode")]
        public Boolean AdminMode { get; set; }
        [CSN("DoctorId")]
        public int DoctorId { get; set; }
    }
}
