//using System;
//namespace ru.novolabs.Exchange
//{
//    public interface IRequest
//    {
//        string RequestCode { get; set; }
//        string HospitalCode { get; set; }
//        string HospitalName { get; set; }
//        string DepartmentCode { get; set; }
//        string DepartmentName { get; set; }
//        string DoctorCode { get; set; }
//        string DoctorName { get; set; }
//        string RegistrationFormCode { get; set; }
//        DateTime SamplingDate { get; set; }
//        DateTime SampleDeliveryDate { get; set; }
//        int PregnancyDuration { get; set; }
//        int CyclePeriod { get; set; }
//        bool ReadOnly { get; set; }
//        ru.novolabs.Exchange.IPatient Patient { get; set; }
//        System.Collections.Generic.IList<ru.novolabs.Exchange.ISample> Samples { get; set; }
//        System.Collections.Generic.IList<ru.novolabs.Exchange.IUserField> UserFields { get; set; }
//    }
//}