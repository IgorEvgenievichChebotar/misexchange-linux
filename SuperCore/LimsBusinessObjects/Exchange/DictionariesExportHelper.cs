using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    public class DictionariesExportHelper
    {
        [XmlType(TypeName = "SynchronizationItem")]
        public class SynchronizationItemOld
        {
            [CSN("LIS_Name")]
            public string LIS_Name { get; set; }
            [CSN("LIS_Code")]
            public string LIS_Code { get; set; }
        }

        [XmlType(TypeName = "DictionarySynchro")]
        public class DictionarySynchroOld
        {
            public DictionarySynchroOld()
            {
                Items = new List<SynchronizationItemOld>();
            }

            [CSN("LIS_Code")]
            public String LIS_Code { get; set; }
            [CSN("Items")]
            public List<SynchronizationItemOld> Items { get; set; }
        }

        public static void ExportDictionaryAsNameCodeMnemonicsOld(String dictionaryName, string exportFileName)
        {
            IBaseDictionary dict = (IBaseDictionary)ProgramContext.Dictionaries.GetDictionary(dictionaryName);
            DictionarySynchroOld dictionaryToExport = new DictionarySynchroOld();
            dictionaryToExport.LIS_Code = dict.Name;

            foreach (DictionaryItem dictItem in dict.DictionaryElements)
            {
                // Выгружаем только неудалённые элементы справочников
                if (!dictItem.Removed)
                {
                    SynchronizationItemOld si = new SynchronizationItemOld();
                    si.LIS_Code = dictItem.Code;
                    si.LIS_Name = dictItem.Name;
                    //si.Mnemonics = dictItem.Mnemonics;

                    dictionaryToExport.Items.Add(si);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(DictionarySynchroOld));

                using (TextWriter tw = new StreamWriter(exportFileName))
                    serializer.Serialize(tw, dictionaryToExport);
            }
        }

        public static void ExportAsIsTargetDictionary(string exportFileName)
        {
            TargetDictionary dict = (TargetDictionary)ProgramContext.Dictionaries.GetDictionary(LimsDictionaryNames.Target);
            ExternalTargetDictionaryInfo targetDictionaryInfo = new ExternalTargetDictionaryInfo();
            foreach (TargetDictionaryItem dictItem in dict.Target)
            {
                if (!dictItem.Removed)
                {
                    ExternalTargetInfo tagetInfo = new ExternalTargetInfo();
                    tagetInfo.TargetRef = new ObjectRef(dictItem.Id);
                    targetDictionaryInfo.Items.Add(tagetInfo);
                }
            }

            XmlSerializer serializer = new XmlSerializer(typeof(ExternalTargetDictionaryInfo));

            using (TextWriter tw = new StreamWriter(exportFileName))
                serializer.Serialize(tw, targetDictionaryInfo);

        }

        public static void RefreshDictionaries(List<string> dictionaryNames)
        {
            lock (ProgramContext.Dictionaries)
            {
                string oldLoggingLevel = ProgramContext.Settings.LoggingLevel;
                ProgramContext.Settings.LoggingLevel = SystemLoggingLevels.LOGIN_LEVEL_OFF;

                try
                {
                    // Выясняем, менялись ли выгружаемые справочники с момента последнего вызова функции
                    var versions = ProgramContext.LisCommunicator.DirectoryVersions();
                    foreach (var dictionaryName in dictionaryNames)
                    {
                        IBaseDictionary dictionary = ProgramContext.Dictionaries.GetIDictionary(dictionaryName);
                        if (dictionary == null)
                            throw new Exception(String.Format("Справочник [{0}] не найден", dictionaryName));
                        DirectoryVersionInfo vi = versions.Find(d => d.Name.Equals(dictionary.Name));
                        if (vi != null && dictionary.Version < vi.Version)
                        {
                            //Обновляем справочник
                            Log.WriteText(String.Format("Refreshing dictionary [{0}]...", dictionaryName));
                            ProgramContext.Dictionaries.RefreshDictionary(dictionary.Name, ProgramContext.LisCommunicator.LimsUserSession);
                        }
                    }
                    ProgramContext.Dictionaries.UpdateReferences();
                }
                finally
                {
                    ProgramContext.Settings.LoggingLevel = oldLoggingLevel;
                }
            }
        }
    }
}
