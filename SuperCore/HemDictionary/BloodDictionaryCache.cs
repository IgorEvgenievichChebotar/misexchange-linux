using System;
using System.IO;
using System.Reflection;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Collections;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCommon;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class HemDictionaryCache : BaseDictionaryCache
    {
        public override void CreateDictionaries()
        {
            if (String.IsNullOrEmpty(StaticPath))
            {
                StaticPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName)
                + SettingsConst.Hem_Dictionary_File_Name;
            }

            // Регистрация статических справочников
            if (File.Exists(StaticPath))
            {
                AddDictionary(typeof(DictionaryClass<SexDictionaryItem>), HemDictionaryNames.Sex);
                AddDictionary(typeof(DictionaryClass<FilterSexDictionaryItem>), HemDictionaryNames.FSex);
                AddDictionary(typeof(DictionaryClass<DonorStatusDictionaryItem>), HemDictionaryNames.DonorStatus);
                AddDictionary(typeof(DictionaryClass<EdcRecordTypeDictionaryItem>), HemDictionaryNames.EdcRecordType);
                AddDictionary(typeof(DictionaryClass<ExpressDonationResultDictionaryItem>), HemDictionaryNames.ExpressDonationResult);
                AddDictionary(typeof(DictionaryClass<VisitReserveTypeDictionaryItem>), HemDictionaryNames.VisitReserveType);
                AddDictionary(typeof(DictionaryClass<DenyDurationUnitsDictionaryItem>), HemDictionaryNames.DenyDurationUnits);
                AddDictionary(typeof(DictionaryClass<DonorTypeDictionaryItem>), HemDictionaryNames.DonorType);
                AddDictionary(typeof(DictionaryClass<DenyTypeDictionaryItem>), HemDictionaryNames.DenyType);
                AddDictionary(typeof(DictionaryClass<OccupationDictionaryItem>), HemDictionaryNames.Occupation);
                AddDictionary(typeof(DictionaryClass<TransfusionRequestStateDictionaryItem>), HemDictionaryNames.TransfusionRequestState);
                AddDictionary(typeof(DictionaryClass<ListTypeDictionaryItem>), HemDictionaryNames.ListType);
                AddDictionary(typeof(DictionaryClass<ProductUnitTypeDictionaryItem>), HemDictionaryNames.ProductUnitType);
                AddDictionary(typeof(DictionaryClass<FBooleanDictionaryItem>), HemDictionaryNames.FBoolean);
                AddDictionary(typeof(DictionaryClass<TransfusionStateDictionaryItem>), HemDictionaryNames.TransfusionState);
                AddDictionary(typeof(DictionaryClass<TreatmentRequestStateDictionaryItem>), HemDictionaryNames.TreatmentRequestState);
                AddDictionary(typeof(DictionaryClass<TreatmentStateDictionaryItem>), HemDictionaryNames.TreatmentState);
                AddDictionary(typeof(DictionaryClass<BooleanValueDictionaryItem>), HemDictionaryNames.BooleanValue);
                AddDictionary(typeof(DictionaryClass<TransfusionRequestTypeDictionaryItem>), HemDictionaryNames.TransfusionRequestType);
            }

            // Регистрация динамических справочников производится в классах-наследниках
        }

        public IList GetDictionaryElementsForFilters(String dictionaryName, EmployeeDictionaryItem employee, DepartmentDictionaryItem currentDepartment)
        {
            List<DictionaryItem> result = new List<DictionaryItem>();
            IBaseDictionary dictionary = GetIDictionary(dictionaryName);

            if (dictionary != null)
            {
                result.Add(new DictionaryItem() { Id = 0, Name = " " });
                if (dictionaryName != HemDictionaryNames.Department)
                {
                    foreach (Object item in dictionary.DictionaryElements)
                    {
                        if (
                            !((DictionaryItem)item).Removed
                            || employee.AccessAllDepartments
                            || (item.GetType().GetCustomProperty("Departments") != null
                            && (
                            ((List<DepartmentDictionaryItem>)item.GetType()
                            .GetCustomProperty("Departments")
                            .GetValue(item, null))
                            .Find(x => x.Id == currentDepartment.Id) != null
                            ||
                            ((List<DepartmentDictionaryItem>)item.GetType()
                            .GetCustomProperty("Departments")
                            .GetValue(item, null)).Count == 0
                                )
                              )
                            )
                        {
                            result.Add(new DictionaryItem() { Id = ((DictionaryItem)item).Id, Name = ((DictionaryItem)item).Name });
                        }
                    }
                }
                else
                {
                    foreach (DepartmentDictionaryItem item in dictionary.DictionaryElements)
                    {
                        if (!item.Removed && (employee.AccessAllDepartments || employee.DepartmentAccess.Find(x => x.Id == item.Id) != null))
                        {
                            result.Add(new DictionaryItem() { Id = item.Id, Name = item.Name });
                        }
                    }
                }

                result.Sort((a, b) => a.Name.CompareTo(b.Name));
            }
            return result;
        }


    }
}