using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.HelperDependencies.SimpleRequestValidatorDependencies;
using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.SuperCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.HelperDependencies
{
    class ConfigGeneralRequestValidator : IRequestValidatorInner
    {
        protected List<ErrorTriplet> errors = new List<ErrorTriplet>();
        protected List<string> additionErrorsStr = new List<string>();
        protected BaseChecker BaseChecker { get; set; }

        public void CheckData(Request request)
        {
            BaseChecker = new SimpleRequestValidatorDependencies.BaseChecker(errors);
            CheckRequest(request);
            
            CustomizeRestricitons(request);

            if (errors.Count > 0)
            {
                List<string> allErrorsStr = errors.ConvertAll(er => er.ToString());
                allErrorsStr.AddRange(additionErrorsStr);
                throw new CustomDataCheckException(allErrorsStr);
            }
        }
        protected virtual void CustomizeRestricitons(Request request)
        {
            //Here should be filter to destroy some restrictions 
        }

        protected virtual void CheckRequest(Request request)
        {
            string typeStr = typeof(Request).Name;
            #region IsMandatoryCheck
            BaseChecker.CheckIsMandatoryNull(request.RequestCode, typeStr, RequestNames.RequestCodeStr);
            BaseChecker.CheckIsMandatoryNull(request.HospitalCode, typeStr, RequestNames.HospitalCodeStr);
            BaseChecker.CheckIsMandatoryNull(request.Patient, typeStr, RequestNames.PatientStr);
        //    CheckIsMandatoryNull(request.Samples, typeStr, "Samples");
            #endregion

            #region MaxLengthCheck
            BaseChecker.CheckMaxLength(request.RequestCode, 20, typeStr, RequestNames.RequestCodeStr);

            BaseChecker.CheckMaxLength(request.HospitalCode, 128, typeStr, RequestNames.HospitalCodeStr);
            BaseChecker.CheckMaxLength(request.HospitalName, 128, typeStr, RequestNames.HospitalNameStr);

            BaseChecker.CheckMaxLength(request.DepartmentCode, 128, typeStr, RequestNames.DepartmentCodeStr);
            BaseChecker.CheckMaxLength(request.DepartmentName, 128, typeStr, RequestNames.DepartmentNameStr);

            BaseChecker.CheckMaxLength(request.DoctorCode, 128, typeStr, RequestNames.DoctorCodeStr);
            BaseChecker.CheckMaxLength(request.DoctorName, 128, typeStr, RequestNames.DoctorNameStr);

            BaseChecker.CheckMaxLength(request.PayCategoryCode, 16, typeStr, RequestNames.PayCategoryCodeStr);
            BaseChecker.CheckMaxLength(request.Email, 128, typeStr, RequestNames.EmailStr);

            BaseChecker.CheckMaxLength(request.OrganizationCode, 128, typeStr, RequestNames.OrganizationCodeStr);
            #endregion

            if (request.Patient != null)
                CheckPatient(request.Patient);


            foreach (var sample in request.Samples)
            {
                CheckSample(sample);            
            }

            foreach (var userField in request.UserFields)
            {
                CheckUserField(userField);
            }
        }
        protected virtual void CheckPatient(Patient patient)
        {
            string typeStr = typeof(Patient).Name;

            #region IsMandatoryCheck
            BaseChecker.CheckIsMandatoryNull(patient.Code, typeStr, RequestNames.Patient.CodeStr);
            BaseChecker.CheckIsMandatoryNull(patient.FirstName, typeStr, RequestNames.Patient.FirstNameStr);
            BaseChecker.CheckIsMandatoryNull(patient.LastName, typeStr, RequestNames.Patient.LastNameStr);
            //  CheckIsMandatoryNull(patient.MiddleName, typeStr, RequestNames.Patient.MiddleNameStr);
            BaseChecker.CheckIsMandatoryNull(patient.Sex, typeStr, RequestNames.Patient.SexStr);
            #endregion

            #region MaxLengthCheck
            BaseChecker.CheckMaxLength(patient.Code, 128, typeStr, RequestNames.Patient.CodeStr);

            BaseChecker.CheckMaxLength(patient.FirstName, 64, typeStr, RequestNames.Patient.FirstNameStr);
            BaseChecker.CheckMaxLength(patient.LastName, 64, typeStr, RequestNames.Patient.LastNameStr);
            BaseChecker.CheckMaxLength(patient.MiddleName, 64, typeStr, RequestNames.Patient.MiddleNameStr);

            BaseChecker.CheckMaxLength(patient.Country, 128, typeStr, RequestNames.Patient.CountryStr);
            BaseChecker.CheckMaxLength(patient.City, 128, typeStr, RequestNames.Patient.CityStr);
            BaseChecker.CheckMaxLength(patient.Street, 128, typeStr, RequestNames.Patient.StreetStr);
            BaseChecker.CheckMaxLength(patient.Building, 128, typeStr, RequestNames.Patient.BuildingStr);
            BaseChecker.CheckMaxLength(patient.Flat, 128, typeStr, RequestNames.Patient.FlatStr);

            BaseChecker.CheckMaxLength(patient.InsuranceCompany, 1024, typeStr, RequestNames.Patient.InsuranceCompanyStr);
            BaseChecker.CheckMaxLength(patient.PolicySeries, 128, typeStr, RequestNames.Patient.PolicySeriesStr);
            BaseChecker.CheckMaxLength(patient.PolicyNumber, 128, typeStr, RequestNames.Patient.PolicyNumberStr);
            #endregion

            #region MinNMaxValueCheck
            BaseChecker.CheckMinValue(patient.BirthDay, 1, typeStr, RequestNames.Patient.BirthDayStr);
            BaseChecker.CheckMinValue(patient.BirthMonth, 1, typeStr, RequestNames.Patient.BirthMonthStr);
            BaseChecker.CheckMinValue(patient.BirthYear, 1890, typeStr, RequestNames.Patient.BirthYearStr);
            BaseChecker.CheckMinValue(patient.Sex, 0, typeStr, RequestNames.Patient.SexStr);

            BaseChecker.CheckMaxValue(patient.BirthDay, 31, typeStr, RequestNames.Patient.BirthDayStr);
            BaseChecker.CheckMaxValue(patient.BirthMonth, 12, typeStr, RequestNames.Patient.BirthMonthStr);
            BaseChecker.CheckMaxValue(patient.Sex, 2, typeStr, RequestNames.Patient.SexStr);
            #endregion

            if (patient.PatientCard != null)
                CheckPatientCard(patient.PatientCard);

            foreach (var userField in patient.UserFields)
            {
                CheckUserField(userField);
            }
        }
        protected virtual void CheckPatientCard(PatientCard patientCard)
        {
            string typeStr = typeof(PatientCard).Name;
            BaseChecker.CheckMaxLength(patientCard.CardNr, 128, typeStr, RequestNames.Patient.PatientCard.CardNrStr);
            foreach (var userField in patientCard.UserFields)
            {
                CheckUserField(userField);            
            }
        }
        protected virtual void CheckUserField(UserField userField)
        {
            string typeStr = typeof(UserField).Name;
            BaseChecker.CheckIsMandatoryNull(userField.Code, typeStr, RequestNames.UserField.CodeStr);
            BaseChecker.CheckMaxLength(userField.Code, 128, typeStr, RequestNames.UserField.CodeStr);
            BaseChecker.CheckMaxLength(userField.Name, 128, typeStr, RequestNames.UserField.NameStr);
            BaseChecker.CheckMaxLength(userField.Value, 1024, typeStr, RequestNames.UserField.ValueStr);

            if (String.IsNullOrEmpty(userField.Value))
            {
                additionErrorsStr.Add(String.Format("Свойство [Value] в объекте [UserField] с именем [Name] = {0} и кодом [Code] = {1} не может быть пустым",userField.Name,userField.Code));
            
            }
        }
        protected virtual void CheckSample(Sample sample)
        {
            string typeStr = typeof(Sample).Name;

            BaseChecker.CheckIsMandatoryNull(sample.BiomaterialCode, typeStr, RequestNames.Sample.BiomaterialCodeStr);
            BaseChecker.CheckIsMandatoryNull(sample.Targets, typeStr, RequestNames.Sample.TargetsStr);

            BaseChecker.CheckMaxLength(sample.Barcode, 16, typeStr, RequestNames.Sample.BarCodeStr);
            BaseChecker.CheckMaxLength(sample.BiomaterialCode, 128, typeStr, RequestNames.Sample.BiomaterialCodeStr);

            BaseChecker.CheckMinValue(sample.Priority, 0, typeStr, RequestNames.Sample.PriorityStr);
            BaseChecker.CheckMaxValue(sample.Priority, 1, typeStr, RequestNames.Sample.PriorityStr);

            foreach (var target in sample.Targets)
            {
                CheckTarget(target);
            }
        }
        protected virtual void CheckTarget(Target target)
        {
            string typeStr = typeof(Target).Name;

            BaseChecker.CheckIsMandatoryNull(target.Code, typeStr, RequestNames.Sample.Target.CodeStr);

            BaseChecker.CheckMaxLength(target.Code, 128, typeStr, RequestNames.Sample.Target.CodeStr);
            foreach (var test in target.Tests)
            {
                CheckTest(test);
            }
        }
        protected virtual void CheckTest(Test test)
        {
            string typeStr = typeof(Test).Name;

            BaseChecker.CheckIsMandatoryNull(test.Code, typeStr, RequestNames.Sample.Target.Test.CodeStr);
            BaseChecker.CheckMaxLength(test.Code, 16, typeStr, RequestNames.Sample.Target.Test.CodeStr);
        }
    }
    enum ErrorValidationType
    {
        IsMandatory,
        MaxLength,
        MaxValue,
        MinValue,
        Custom
    }
    class ErrorTriplet
    {
        public ErrorTriplet()
        {
            CustomTextFormat = "Значение свойства [{0}] в объекте [{1}] некорректно";     
        }
        const string emptyPropError = "Свойство [{0}] в объекте [{1}] не может быть пустым";
        const string maxLenghtPropError = "Длина свойства [{0}] в объекте [{1}] превышает масимально допустимое значение = {2}";
        const string minValuePropError = "Значение свойства [{0}] в объекте [{1}] меньше минимально допустимого значения = {2}";
        const string maxValuePropError = "Значение свойства [{0}] в объекте [{1}] больше максимально допустимого значения = {2}";

        public string ObjectName { get; set; }
        public string PropertyName { get; set; }
        public string ControlValue { get; set; }
        public string CustomTextFormat { get; set; }
        public ErrorValidationType ErrorValidationType { get; set; }

        public override string ToString()
        {
            string resultStr = null;
            switch (ErrorValidationType)
            {
                case ErrorValidationType.IsMandatory:
                    resultStr = String.Format(emptyPropError, PropertyName, ObjectName);
                    break;
                case ErrorValidationType.MaxLength:
                    resultStr = String.Format(maxLenghtPropError, PropertyName, ObjectName, ControlValue);
                    break;
                case ErrorValidationType.MinValue:
                    resultStr = String.Format(minValuePropError, PropertyName, ObjectName, ControlValue);
                    break;
                case ErrorValidationType.MaxValue:
                    resultStr = String.Format(maxValuePropError, PropertyName, ObjectName, ControlValue);
                    break;
                case ErrorValidationType.Custom:
                    resultStr = String.Format(CustomTextFormat, PropertyName, ObjectName, ControlValue);
                    break;
            }
            return resultStr;
        }
        public Tuple<string, string,ErrorValidationType> GetHashCode()
        {
            return Tuple.Create(ObjectName, PropertyName, ErrorValidationType);
        }
    }   
}
