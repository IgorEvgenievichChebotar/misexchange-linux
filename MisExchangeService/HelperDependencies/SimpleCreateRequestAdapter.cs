using Ninject;
using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.HelperDependencies.SimpleCreateRequestDependencies;
using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using ru.novolabs.MisExchangeService;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCommon;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.HelperDependencies
{
    class SimpleCreateRequestAdapter : ICreateRequestAdapter
    {
        private readonly object _syncObj = new object();

        [Inject]
        public SimpleCreateRequest3HospitalFieldsFactory HospitalFactory { get; set; }
        [Inject]
        public SimpleCreateRequest3SampleFactory SampleFactory { get; set; }
        [Inject]
        public SimpleCreateRequest3UserValuesFactory UserValuesFactory { get; set; }
        [Inject]
        public PatientSaver PatientSaver { get; set; }

        private IProcessRequestSettings Settings { get; set; }
        private IDictionaryCache DictionaryCache { get; set; }

        public SimpleCreateRequestAdapter(IProcessRequestSettings settings, IDictionaryCache dictionaryCache)
        {
            Settings = settings;
            DictionaryCache = dictionaryCache;
        }

        public CreateRequest3Request MakeRequest(Request requestDTO, int requestLisId, bool savePatient)
        {
            CreateRequest3Request request = new CreateRequest3Request
            {
                Request = new ObjectRef(requestLisId),
                InternalNr = requestDTO.RequestCode
            };
            // cohesive dictionary elements, prevent creating doubles
            lock (_syncObj)
            {
                request.CustHospital = HospitalFactory.BuildHospitalDictionaryItem(requestDTO.HospitalCode, requestDTO.HospitalName);
                request.CustDepartment = HospitalFactory.BuildCustDepartmentDictionaryItem(requestDTO.DepartmentCode, requestDTO.DepartmentName, request.CustHospital);
                request.CustDoctor = HospitalFactory.BuildDoctorDictionaryItem(requestDTO.DoctorCode, requestDTO.DoctorName, request.CustDepartment);
                request.PayCategory = HospitalFactory.BuildPayCategoryDictionaryItem(requestDTO.PayCategoryCode, request.CustHospital);
            }
            request.SamplingDate = requestDTO.SamplingDate;
            request.SampleDeliveryDate = PrepareSampleDeliveryDate(requestDTO.SampleDeliveryDate);
            request.PregnancyDuration = requestDTO.PregnancyDuration;
            request.CyclePeriod = requestDTO.CyclePeriod;
            request.Readonly = requestDTO.ReadOnly.HasValue ? requestDTO.ReadOnly.Value : false;
            request.Email = requestDTO.Email;
            request.Telephone = requestDTO.Telephone;
            request.CardAmbulatory = requestDTO.CardAmbulatory;
            request.CardStationary = requestDTO.CardStationary;
            request.CardExtraType1 = requestDTO.CardExtraType1;

            requestDTO.Patient.Code = string.Format("{0}{1}", Settings.PatientCodePrefix ?? "", requestDTO.Patient.Code);
            request.Code = requestDTO.Patient.Code;
            request.FirstName = requestDTO.Patient.FirstName;
            request.LastName = requestDTO.Patient.LastName;
            request.MiddleName = requestDTO.Patient.MiddleName;
            request.BirthDay = requestDTO.Patient.BirthDay > 0 ? (int?)requestDTO.Patient.BirthDay : null;
            request.BirthMonth = requestDTO.Patient.BirthMonth > 0 ? (int?)requestDTO.Patient.BirthMonth : null;
            request.BirthYear = requestDTO.Patient.BirthYear > 0 ? (int?)requestDTO.Patient.BirthYear : null;
            request.Sex = BuildSexDictionaryItem(requestDTO.Patient.Sex);
            request.Country = requestDTO.Patient.Country;
            request.Region = requestDTO.Patient.Region;
            request.Area = requestDTO.Patient.Area;
            request.City = requestDTO.Patient.City;
            request.Location = requestDTO.Patient.Location;
            request.Street = requestDTO.Patient.Street;
            request.Building = requestDTO.Patient.Building;
            request.Flat = requestDTO.Patient.Flat;
            request.LivingAddressCountry = requestDTO.Patient.LivingAddressCountry;
            request.LivingAddressRegion = requestDTO.Patient.LivingAddressRegion;
            request.LivingAddressArea = requestDTO.Patient.LivingAddressArea;
            request.LivingAddressCity = requestDTO.Patient.LivingAddressCity;
            request.LivingAddressLocation = requestDTO.Patient.LivingAddressLocation;
            request.LivingAddressStreet = requestDTO.Patient.LivingAddressStreet;
            request.LivingAddressBuilding = requestDTO.Patient.LivingAddressBuilding;
            request.LivingAddressFlat = requestDTO.Patient.LivingAddressFlat;
            request.InsuranceCompany = requestDTO.Patient.InsuranceCompany;
            request.PolicySeries = requestDTO.Patient.PolicySeries;
            request.PolicyNumber = requestDTO.Patient.PolicyNumber;
            request.Guardian = requestDTO.Guardian;
            request.Yandex_OrderId = requestDTO.Yandex_OrderId;
            request.Yandex_BucketId = requestDTO.Yandex_BucketId;

            if (requestDTO.Patient.PatientCard != null)
                request.CardNr = String.IsNullOrEmpty(requestDTO.Patient.PatientCard.CardNr) ? null : requestDTO.Patient.PatientCard.CardNr;
            request.Organization = HospitalFactory.BuildOrganizationDictionalyItem(requestDTO.OrganizationCode);
            request.ExecutorOrganization = HospitalFactory.BuildOrganizationDictionalyItem(requestDTO.ExecutorOrganizationCode);
            request.RequestForm = DictionaryCache.GetDictionaryItem<RequestFormDictionaryItem>(Settings.RegistrationFormCode);

            foreach (Sample sample in requestDTO.Samples)
            {
                SampleFactory.BuildCreateRequset3SampleInfo(sample, request.Samples, request.InternalNr);
            }

            request.Cito = (requestDTO.Priority > 0) || (request.Samples.Max(s => s.Priority) > 0);

            List<UserValue> userValues = UserValuesFactory.BuildUserValues(requestDTO);
            if (userValues.Count > 0)
                request.UserValues.AddRange(userValues);

            if (savePatient)
            {
                PatientSaver.PatientSave(request, requestDTO.Patient);
            }

            return request;
        }

        protected virtual SexDictionaryItem BuildSexDictionaryItem(int sexId)
        {
            SexDictionaryItem sexDictionaryItem = DictionaryCache.GetDictionaryItem<SexDictionaryItem>(sexId);
            return sexDictionaryItem;
        }
        protected DateTime? PrepareSampleDeliveryDate(DateTime? sampleDeliveryDate)
        {
            if (Settings.ResetSampleDeliveryDate)
                return null;

            if (sampleDeliveryDate != null)
                return sampleDeliveryDate;
            // Если МИС не указала дату доставки биоматериала в лабораторию
            DateTime? defaultSampleDeliveryDate = Settings.DefaultSampleDeliveryDate;

            if (defaultSampleDeliveryDate != null)
                return defaultSampleDeliveryDate; // Дату доставки биоматериала по умолчанию берём из файла настроек

            return DateTime.Now; // Дату доставки биоматериала делаем равной дате обработки файла заявки 
        }
    }
}
