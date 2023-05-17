using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchangeService.Adapters;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.MisExchange.Classes
{
    public class ExternalResultAdapter : ExternalResultAdapterBase
    {
        public ExternalResultAdapter(IDictionaryCache dictionaryCache)
        {
            DictionaryCache = dictionaryCache;
        }
        private IDictionaryCache DictionaryCache { get; set; }

        protected override Work CopyWork(SuperCore.LimsBusinessObjects.Exchange.ExternalWork work)
        {
            Work workDTO = base.CopyWork(work);
            if (workDTO == null)
            {
                return null;
            }

            PatientGroupNameProcess(workDTO);
            return workDTO;
        }
        private void PatientGroupNameProcess(Work work)
        {
            if (string.IsNullOrEmpty(work.PatientGroupCode))
            {
                return;
            }

            PatientGroupDictionaryItem patientGroupDictionaryItem = DictionaryCache.GetDictionaryItem<PatientGroupDictionaryItem>(work.PatientGroupCode);
            if (patientGroupDictionaryItem == null)
            {
                return;
            }

            work.PatientGroupName = patientGroupDictionaryItem.Name;
        }
    }
}
