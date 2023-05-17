using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;

namespace ru.novolabs.MisExchangeService.Classes
{
    public class MisExchangeDictionaryCache : LimsDictionaryCache
    {
        public override void CreateDictionaries()
        {
            base.CreateDictionaries();
            AddDictionary(typeof(BiomaterialDictionary), LimsDictionaryNames.Biomaterial);
            AddDictionary(typeof(CustDepartmentDictionary), LimsDictionaryNames.CustDepartment);
            AddDictionary(typeof(DepartmentDictionary), LimsDictionaryNames.Department);
            AddDictionary(typeof(DoctorDictionary), LimsDictionaryNames.Doctor);
            AddDictionary(typeof(EmployeeDictionary), LimsDictionaryNames.Employee);
            AddDictionary(typeof(EquipmentDictionary), LimsDictionaryNames.Equipment);
            AddDictionary(typeof(ExternalSystemDictionary), LimsDictionaryNames.ExternalSystem);
            AddDictionary(typeof(HospitalDictionary), LimsDictionaryNames.Hospital);
            AddDictionary(typeof(MicroOrganismDictionary), LimsDictionaryNames.MicroOrganism);
            AddDictionary(typeof(PayCategoryDictionary), LimsDictionaryNames.PayCategory);
            AddDictionary(typeof(RequestCustomStateDictionary), LimsDictionaryNames.RequestCustomState);
            AddDictionary(typeof(RequestFormDictionary), LimsDictionaryNames.RequestForm);
            AddDictionary(typeof(SampleBlankDictionary), LimsDictionaryNames.SampleBlank);
            AddDictionary(typeof(ServiceDictionary), LimsDictionaryNames.ServiceShort);
            AddDictionary(typeof(TargetDictionary), LimsDictionaryNames.Target);
            AddDictionary(typeof(TestDictionary), LimsDictionaryNames.Test);
            AddDictionary(typeof(PatientGroupDictionary), LimsDictionaryNames.PatientGroup);
            AddDictionary(typeof(UserDirectoryDictionary), LimsDictionaryNames.UserDirectory);
            AddDictionary(typeof(UserFieldDictionary), LimsDictionaryNames.UserField);
            AddDictionary(typeof(UnitDictionary), LimsDictionaryNames.Unit);
            AddDictionary(typeof(DefectTypeDictionary), LimsDictionaryNames.DefectType);
            AddDictionary(typeof(OrganizationDictionary), LimsDictionaryNames.Organization);
            AddDictionary(typeof(СolonyFormingUnitDictionary), LimsDictionaryNames.ColonyFormingUnit);
            AddDictionary(typeof(LegalEntityDictionary), LimsDictionaryNames.LegalEntity);

            // Загружаем складские справочники в случае, если "активирован складской модуль" (задана опция "processStorageOperationsMode")
            string processStorageOperationsMode = (string)ProgramContext.Settings["processStorageOperationsMode", false];
            if (processStorageOperationsMode != null)
            {
                AddDictionary(typeof(FundingSourceDictionary), LimsDictionaryNames.FundingSource);
                AddDictionary(typeof(SupplierDictionary), LimsDictionaryNames.Supplier);
                AddDictionary(typeof(ManufacturerDictionary), LimsDictionaryNames.Manufacturer);
                AddDictionary(typeof(MaterialUnitDictionary), LimsDictionaryNames.MaterialUnit);
                AddDictionary(typeof(MaterialDictionary), LimsDictionaryNames.Material);
                AddDictionary(typeof(UnitTreeNodeDictionary), LimsDictionaryNames.UnitTreeNode);
            }
        }
    }
}