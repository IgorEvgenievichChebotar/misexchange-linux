//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ru.novolabs.SuperCore.LimsDictionary;

//namespace ru.novolabs.SuperCore.Core
//{
//    public class NlsSession : BaseObject { }


//    public class BloodSession : NlsSession
//    {
//        public BloodSession(BloodCommunicator communicator) : base()
//        {
//            Communicator = communicator;
//        }

//        protected BloodCommunicator Communicator { get; private set; }
//    }


//    public class BloodExternalSystemSession : BloodSession
//    {
//        public BloodExternalSystemSession(BloodCommunicator communicator) : base(communicator) { }
//    }


//    public class BloodUserSession : BloodSession
//    {
//        public BloodUserSession(BloodCommunicator communicator) : base(communicator) { }

//        public BloodDictionary.EmployeeDictionaryItem User { get; set; }
//    }


//    public class LisSession : NlsSession
//    {
//        public LisSession(LisCommunicator communicator) : base()
//        {
//            Communicator = communicator;
//        }

//        protected LisCommunicator Communicator { get; private set; }    
//    }


//    public class LisExternalSystemSession : LisSession
//    {
//        public LisExternalSystemSession(LisCommunicator communicator) : base(communicator) { }
    
//        /* public LisDictionary.ExternalSystemDictionaryItem ExternalSystem
//        {
//            get { return ((LisIntegrationService)Communicator).ExternalSystem; }
//        }*/
//    }


//   /* public class LisUserSession : LisSession
//    {
//        public LisUserSession(LisCommunicator communicator) : base(communicator) { }

//        public new String Id { get {return ((LisClient)Communicator).SessionId;}}

//        public LisDictionary.EmployeeDictionaryItem User
//        {
//            get { return ((LisClient)Communicator).CurrentUser; }
//        }

//        public LisDictionary.WorkPlaceDictionaryItem WorkPlace
//        {
//            get { return ((LisClient)Communicator).CurrentWorkPlace; }
//        }
//    }*/
//}
