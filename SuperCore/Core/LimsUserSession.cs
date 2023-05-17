using ru.novolabs.SuperCore.Crypto;
using System;

namespace ru.novolabs.SuperCore.Core
{
    public class BaseUserSession
    {
        public BaseUserSession()
        {
            SessionId = String.Empty;
            CLID = String.Empty;
            Login = String.Empty;
        }

        public String SessionId { get; set; }
        public String CLID { get; set; }
        public String Login { get; set; }
    }

    public class LimsUserSession : BaseUserSession
    {
        public LimsDictionary.WorkPlaceDictionaryItem WorkPlace { get; set; }
        public LimsDictionary.EmployeeDictionaryItem User { get; set; }
        public LimsDictionary.OfficeDictionaryItem Office { get; set; }
        public int DoctorId { get; set; }
        public string ServerVersion { get; set; }
    }

    public class HemUserSession : BaseUserSession
    {
        public HemDictionary.EmployeeDictionaryItem User { get; set; }

        private HemDictionary.DepartmentDictionaryItem currentDepartment = new HemDictionary.DepartmentDictionaryItem();
        public HemDictionary.DepartmentDictionaryItem CurrentDepartment
        {
            get
            {
                if ((currentDepartment != null) && (currentDepartment.Id > 0))
                    return currentDepartment;
                else
                {
                    if (this.User != null)
                        return this.User.HomeDepartment;
                    else
                        return null;
                }
            }
            set { currentDepartment = value; }
        }

        //private HemDictionary.HospitalDepartmentDictionaryItem currentHospitalDepartment = new HemDictionary.HospitalDepartmentDictionaryItem();
        //public HemDictionary.HospitalDepartmentDictionaryItem CurrentHospitalDepartment
        //{
        //    get
        //    {
        //        if ((currentHospitalDepartment != null) && (currentHospitalDepartment.Id > 0))
        //            return currentHospitalDepartment;
        //        else
        //        {
        //            //TODO: Home HospitalDepartment?! ><'
        //            return null; 
        //        }
        //    }
        //    set { currentHospitalDepartment = value; }
        //}
    }
}