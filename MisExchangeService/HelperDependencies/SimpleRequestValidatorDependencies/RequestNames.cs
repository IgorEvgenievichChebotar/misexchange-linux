using ru.novolabs.ExchangeDTOs;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.HelperDependencies
{
    static class RequestNames
    {
        private static Request surrogateRequest;
        public static string RequestCodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.RequestCode);

        public static string HospitalCodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.HospitalCode);
        public static string HospitalNameStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.HospitalName);

        public static string PatientStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient);
        public static string OrganizationCodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.OrganizationCode);

        public static string DepartmentCodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.DepartmentCode);
        public static string DepartmentNameStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.DepartmentName);

        public static string DoctorCodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.DoctorCode);
        public static string DoctorNameStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.DoctorName);

        public static string PayCategoryCodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.PayCategoryCode);

        public static string EmailStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Email);

        public static string SamplesStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Samples);

        public static class Patient
        {
            public static string CodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.Code);

            public static string FirstNameStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.FirstName);
            public static string LastNameStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.LastName);
            public static string MiddleNameStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.MiddleName);

            public static string SexStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.Sex);

            public static string CountryStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.Country);
            public static string CityStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.City);
            public static string StreetStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.Street);
            public static string BuildingStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.Building);
            public static string FlatStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.Flat);

            public static string InsuranceCompanyStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.InsuranceCompany);
            public static string PolicySeriesStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.PolicySeries);
            public static string PolicyNumberStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.PolicyNumber);

            public static string BirthDayStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.BirthDay);
            public static string BirthMonthStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.BirthMonth);
            public static string BirthYearStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.BirthYear);

            public static class PatientCard
            {
                public static string CardNrStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Patient.PatientCard.CardNr);
            }

        }

        public static class Sample
        {
            public static string BiomaterialCodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Samples.First().BiomaterialCode);
            public static string BarCodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Samples.First().Barcode);
            public static string PriorityStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Samples.First().Priority);
            public static string TargetsStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Samples.First().Targets);

            public static class Target
            {
                public static string CodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Samples.First().Targets.First().Code);
                public static class Test
                {
                    public static string CodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.Samples.First().Targets.First().Tests.First().Code);
                }
            }
        }

        public static class UserField
        {
            public static string CodeStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.UserFields.First().Code);
            public static string NameStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.UserFields.First().Name);
            public static string ValueStr = ReflectionHelper.GetPropertyName(() => surrogateRequest.UserFields.First().Value);
        }
    }   
}
