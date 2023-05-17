using System;
using System.IO;
using System.Reflection;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.DictionaryCommon;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class LimsDictionaryCache : BaseDictionaryCache
    {
        public override void CreateDictionaries()
        {
            if (String.IsNullOrEmpty(StaticPath))
            {
                StaticPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName)
                + SettingsConst.Lims_Dictionary_File_Name;
            }

            // Регистрация статических справочников
            if (File.Exists(StaticPath))
            {
                AddDictionary(typeof(DictionaryClass<SexDictionaryItem>), LimsDictionaryNames.Sex);
                AddDictionary(typeof(DictionaryClass<SexFilterDictionaryItem>), LimsDictionaryNames.SexFilter);
                AddDictionary(typeof(DictionaryClass<RequestStateDictionaryItem>), LimsDictionaryNames.RequestState);
                AddDictionary(typeof(DictionaryClass<GuiElementReportExecutorsDictionaryItem>), LimsDictionaryNames.GuiElementReportExecutors);
                AddDictionary(typeof(DictionaryClass<YesNoIgnoreDictionaryItem>), LimsDictionaryNames.YesNoIgnore);
                AddDictionary(typeof(DictionaryClass<BilledDictionaryItem>), LimsDictionaryNames.Billed);
                AddDictionary(typeof(DictionaryClass<WorkStateDictionaryItem>), LimsDictionaryNames.WorkState);
                AddDictionary(typeof(DictionaryClass<SampleStateDictionaryItem>), LimsDictionaryNames.SampleState);
                AddDictionary(typeof(DictionaryClass<CyclePeriodDictionaryItem>), LimsDictionaryNames.CyclePeriod);
                AddDictionary(typeof(DictionaryClass<NormalityStateDictionaryItem>), LimsDictionaryNames.NormalityState);
                AddDictionary(typeof(DictionaryClass<BiomaterialStateDictionaryItem>), LimsDictionaryNames.BiomaterialStateEx);
                AddDictionary(typeof(DictionaryClass<PiorityDictionaryItem>), LimsDictionaryNames.Priority);
                AddDictionary(typeof(DictionaryClass<HasDefectsDictionaryItem>), LimsDictionaryNames.HasDefects);
                AddDictionary(typeof(DictionaryClass<RemovedDictionaryItem>), LimsDictionaryNames.Removed);
                AddDictionary(typeof(DictionaryClass<NormalityDictionaryItem>), LimsDictionaryNames.Normality);
                AddDictionary(typeof(DictionaryClass<DocumentStateDictionaryItem>), LimsDictionaryNames.DocumentState);
                AddDictionary(typeof(DictionaryClass<FillingDirectionDictionaryItem>), LimsDictionaryNames.FillingDirection);
                AddDictionary(typeof(DictionaryClass<AxisNumerationTypeDictionaryItem>), LimsDictionaryNames.AxisNumerationType);
                AddDictionary(typeof(DictionaryClass<DefectStateDictionaryItem>), LimsDictionaryNames.DefectState);
                AddDictionary(typeof(DictionaryClass<QuotaStateDictionaryItem>), LimsDictionaryNames.QuotaState);
                AddDictionary(typeof(DictionaryClass<RequestFilterStateDictionaryItem>), LimsDictionaryNames.RequestFilterState);
                AddDictionary(typeof(DictionaryClass<DateTypeDictionaryItem>), LimsDictionaryNames.DateType);
                AddDictionary(typeof(DictionaryClass<QuotaControlTypeDictionaryItem>), LimsDictionaryNames.QuotaControlType);
                AddDictionary(typeof(DictionaryClass<LogObjectTypeDictionaryItem>), LimsDictionaryNames.LogObjectType);
                AddDictionary(typeof(DictionaryClass<LogOperationTypeDictionaryItem>), LimsDictionaryNames.LogOperationType);
                AddDictionary(typeof(DictionaryClass<TargetInfoStateDictionaryItem>), LimsDictionaryNames.TargetInfoState);
                AddDictionary(typeof(DictionaryClass<PaymentStateDictionaryItem>), LimsDictionaryNames.PaymentState);
                
            }

            // Регистрация динамических справочников производится в классах-наследниках
        }
    }
}
