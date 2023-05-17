using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Reflection;
using SuperCore.Core;
using SuperCore.External;
using SuperCore.BusinessObjectsBlood;
using SuperCore.DictionaryCore;

namespace SuperCore.BloodDictionary
{

	public class BloodDictionaryCash : BaseDictionaryCash
    {      
        
        
        public override void CreateDictionaries()
        {
            AddDictionary(typeof(EmployeeDictionaryClass<EmployeeDictionaryItem>), BloodDictionaryNamesConst.Employee);
            AddDictionary(typeof(DictionaryClass<DepartmentDictionaryItem>), BloodDictionaryNamesConst.Department);
            AddDictionary(typeof(DictionaryClass<DonorCategoryDictionaryItem>), BloodDictionaryNamesConst.DonorCategory);
            AddDictionary(typeof(DictionaryClass<DocumentTypeDictionaryItem>), BloodDictionaryNamesConst.DonationType);
            AddDictionary(typeof(DictionaryClass<DonationTemplateDictionaryItem>), BloodDictionaryNamesConst.DonationTemplate);
            AddDictionary(typeof(DictionaryClass<VisitReserveTypeDictionaryItem>), BloodDictionaryNamesConst.VisitReserveType);
            AddDictionary(typeof(DictionaryClass<DocumentTypeDictionaryItem>), BloodDictionaryNamesConst.DocumentType);
            AddDictionary(typeof(DictionaryClass<UserGroupDictionaryItem>), BloodDictionaryNamesConst.UserGroup);
            AddDictionary(typeof(DictionaryClass<PaymentCategoryDictionaryItem>), BloodDictionaryNamesConst.PaymentCategory);
            AddDictionary(typeof(DenyDictionaryClass<DenyDictionaryItem>), BloodDictionaryNamesConst.Deny);
            AddDictionary(typeof(DictionaryClass<HospitalDictionaryItem>), BloodDictionaryNamesConst.Hospital);
            AddDictionary(typeof(DictionaryClass<DenySourceDictionaryItem>), BloodDictionaryNamesConst.DenySource);
            AddDictionary(typeof(UserDirectoryDictionaryClass<UserDirectoryDictionaryItem>), BloodDictionaryNamesConst.UserDirectory);
            AddDictionary(typeof(ParameterGroupDictionaryClass<ParameterGroup>), BloodDictionaryNamesConst.ParameterGroup);
            AddDictionary(typeof(BloodParameterGroupDictionaryClass<BloodParameterGroup>), BloodDictionaryNamesConst.BloodParameterGroup);

            AddDictionary(typeof(DictionaryClass<ProductClassificationDictionaryItem>), BloodDictionaryNamesConst.ProductClassification);
            AddDictionary(typeof(DictionaryClass<EdcRecordType>), BloodDictionaryNamesConst.EdcRecordType);
            AddDictionary(typeof(DictionaryClass<SexDictionaryItem>), BloodDictionaryNamesConst.Sex);
            AddDictionary(typeof(DictionaryClass<DonorStatusDictionaryItem>), BloodDictionaryNamesConst.donorStatus);
            AddDictionary(typeof(DictionaryClass<ExpressDonationResultDictionaryItem>), BloodDictionaryNamesConst.ExpressDonationResult);
            AddDictionary(typeof(DictionaryClass<DenyDurationUnitsDictionaryItem>), BloodDictionaryNamesConst.DenyDurationUnits);
            AddDictionary(typeof(DictionaryClass<DefectDictionaryItem>), BloodDictionaryNamesConst.Defect);
            AddDictionary(typeof(DictionaryClass<AdditiveTypeDictionaryItem>), BloodDictionaryNamesConst.AdditiveType);
            AddDictionary(typeof(DictionaryClass<ContainerTypeDictionaryItem>), BloodDictionaryNamesConst.ContainerType);
            AddDictionary(typeof(DictionaryClass<ProductJournalDictionaryItem>), BloodDictionaryNamesConst.ProductJournal);
        }
    }
}
