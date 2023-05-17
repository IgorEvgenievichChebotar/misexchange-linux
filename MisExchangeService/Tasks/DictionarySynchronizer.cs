using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using ru.novolabs.MisExchangeService;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using System;
using System.Collections.Generic;
using System.Timers;

namespace MisExchange.User.Tasks
{
    public class DictionarySynchronizer
    {
        private const int defaultSynchronizeDictionaryInterval = 3;
        private int synchronizeDictionaryInterval = defaultSynchronizeDictionaryInterval;

        private Timer syncTimer = new Timer();

        private IProcessResultSettings Settings { get; set; }
        private IDictionaryCache DictionaryCache { get; set; }
        List<string> DictionaryNamesList { get; set; }

        public DictionarySynchronizer()
        {
            //
        }

        private void prepareSynchronizer()
        {
            Settings = GAP.ResultSettings;
            if (Settings.SynchronizeDictionaryInterval > 0)
            {
                if (Settings.SynchronizeDictionaryInterval < 60)
                    throw new ApplicationException(String.Format("Значение настройки synchronizeDictionaryInterval должно быть не менее \"{0}\" (секунд). Приложение не запущено", 60));

                synchronizeDictionaryInterval = Settings.SynchronizeDictionaryInterval;
            }
        }

        public void Run()
        {
            prepareSynchronizer();
            syncTimer.Interval = Convert.ToDouble(synchronizeDictionaryInterval * 1000);
            syncTimer.Elapsed += onTimerElapsed;
            syncTimer.Start();
        }
        private void onTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                DictionaryNamesList = new List<string>();
                foreach (IBaseDictionary dic in ProgramContext.Dictionaries.DictionaryList.Values)
                {
                    if (!ProgramContext.Dictionaries.IsStaticDictionary(dic))
                        DictionaryNamesList.Add(dic.Name);
                }
                if (Settings.IsSynchronizeDictionary)
                {
                    syncTimer.Stop();
                    DictionariesExportHelper.RefreshDictionaries(DictionaryNamesList);
                }
            }
            catch (Exception ex)
            {
                GAP.Logger.WriteError(ex.ToString());
            }
            finally
            {
                syncTimer.Start();
            }
        }
    }
}