using Ninject;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces;
using ru.novolabs.MisExchangeService;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.Core;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.HelperDependencies.SimpleCreateRequestDependencies
{
    class SimpleCreateRequest3HospitalFieldsFactory
    {
        public SimpleCreateRequest3HospitalFieldsFactory(IDictionaryCache dictionaryCache, IProcessRequestCommunicator communicator)
        {
            DictionaryCache = dictionaryCache;
            LimsCommunicator = communicator;
        }

        protected IDictionaryCache DictionaryCache { get; set; }
        protected IProcessRequestCommunicator LimsCommunicator { get; set; }

        public virtual OrganizationDictionaryItem BuildOrganizationDictionalyItem(string organizationCode)
        {
            OrganizationDictionaryItem organizationDictionaryItem = DictionaryCache.GetDictionaryItem<OrganizationDictionaryItem>(organizationCode);
            return organizationDictionaryItem;
        }
        public virtual HospitalDictionaryItem BuildHospitalDictionaryItem(string hospitalCode, string hospitalName)
        {
            HospitalDictionaryItem hospitalDictionaryItem = DictionaryCache.GetDictionaryItem<HospitalDictionaryItem>(hospitalCode);
            if (hospitalDictionaryItem == null && !String.IsNullOrEmpty(hospitalCode) && !String.IsNullOrEmpty(hospitalName))
            {
                hospitalDictionaryItem = DictionaryCache.CreateItem<HospitalDictionaryItem>(hospitalCode, hospitalName);
            }
            return hospitalDictionaryItem;
        }
        public virtual CustDepartmentDictionaryItem BuildCustDepartmentDictionaryItem(string departmentCode, string departmentName, HospitalDictionaryItem hospitalDictionaryItem)
        {
            if (hospitalDictionaryItem == null)
                return null;

            CustDepartmentDictionaryItem custDepartmentDictionaryItem = DictionaryCache.GetDictionary<CustDepartmentDictionary, CustDepartmentDictionaryItem>()
                .Elements.Find(custDepartment => custDepartment.Code == departmentCode
                    && custDepartment.Hospital.Code == hospitalDictionaryItem.Code);

            if (custDepartmentDictionaryItem == null && !String.IsNullOrEmpty(departmentCode) && !String.IsNullOrEmpty(departmentName))
            {
                custDepartmentDictionaryItem = DictionaryCache.CreateItem<CustDepartmentDictionaryItem, HospitalDictionaryItem>(departmentCode,
                    departmentName, hospitalDictionaryItem);
            }
            return custDepartmentDictionaryItem;
        }
        public virtual DoctorDictionaryItem BuildDoctorDictionaryItem(string doctorCode, string doctorName, CustDepartmentDictionaryItem custDepartmentDictionaryItem)
        {
            if (custDepartmentDictionaryItem == null)
                return null;

            DoctorDictionaryItem doctorDictionaryItem = DictionaryCache.GetDictionary<DoctorDictionary, DoctorDictionaryItem>()
                .Elements.Find(doctor => doctor.Code == doctorCode
                    && doctor.CustDepartment.Code == custDepartmentDictionaryItem.Code
                    && doctor.CustDepartment.Hospital.Code == custDepartmentDictionaryItem.Hospital.Code);

            if (doctorDictionaryItem == null && !String.IsNullOrEmpty(doctorCode) && !String.IsNullOrEmpty(doctorName))
            {
                doctorDictionaryItem = DictionaryCache.CreateItem<DoctorDictionaryItem, CustDepartmentDictionaryItem>(doctorCode, doctorName, custDepartmentDictionaryItem);
            }
            else if (doctorDictionaryItem != null && !String.IsNullOrEmpty(doctorName) && doctorDictionaryItem.Name != doctorName)
            {
                doctorDictionaryItem.Name = doctorName;
                doctorDictionaryItem = DictionaryCache.UpdateItem<DoctorDictionaryItem>(doctorDictionaryItem);
            }
            return doctorDictionaryItem;
        }
        public virtual PayCategoryDictionaryItem BuildPayCategoryDictionaryItem(string payCategoryCode, HospitalDictionaryItem hospitalDictionaryItem)
        {
            PayCategoryDictionaryItem payCategoryDictionaryItem = DictionaryCache.GetDictionary<PayCategoryDictionary, PayCategoryDictionaryItem>()
                .Elements.Find(payCategory => payCategory.Code == payCategoryCode
                    && payCategory.Hospitals.Exists(hospital => hospital.Code == hospitalDictionaryItem.Code));
            return payCategoryDictionaryItem;
        }
    }
}
