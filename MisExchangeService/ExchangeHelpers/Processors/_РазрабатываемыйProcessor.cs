/*
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCore;

namespace MisExchange.User.Processors
{
    public class RemoteActivateContent
    {
        public Int32 Action { get; set; }
    }

    [ProcessorName(LisProcessorsNames.RemoteActivate)]
    public class RemoteActivateProcessor : Processor
    {
        public override void Execute(XmlNode content)
        {
            RemoteActivateContent contentObject = new RemoteActivateContent();

            ObjectReader reader = new ObjectReader();
            reader.ReadContent(content, contentObject);

            switch (contentObject.Action)
            {
                case 0: ExportDictionaries(); break;
                //case 1: {}; break;
                default: { }; break;
            }

            RequestDone = true;
        }

        private void ExportDictionaries()
        {
            lock (ProgramContext.Dictionaries)
            {
                foreach (DictionaryEntry dicInfo in ProgramContext.Dictionaries.DictionaryList)
                    ExportDictionary(dicInfo.Key.ToString());

                ExportTargetDictionary();
            }
        }

        private void ExportDictionary(String dictionaryName)
        {
            IBaseDictionary dict = (IBaseDictionary)ProgramContext.Dictionaries.GetDictionary(dictionaryName);
                
            DictionarySynchro dictionaryToExport = new DictionarySynchro();
            // dictionaryToExport.Name = "";
            dictionaryToExport.Code = dict.Name;

            foreach (DictionaryItem dictItem in dict.DictionaryElements)
            {
                // Выгружаем только неудалённые элементы справочников
                if (!dictItem.Removed)
                {
                    SynchronizationItem si = new SynchronizationItem();
                    si.Code = dictItem.Code;
                    si.Name = dictItem.Name;

                    dictionaryToExport.Items.Add(si);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(DictionarySynchro));

                Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Export"));
                String ExportFileName = Path.Combine(Application.StartupPath, "Export", dict.Name + ".xml");

                using (TextWriter tw = new StreamWriter(ExportFileName))
                    serializer.Serialize(tw, dictionaryToExport);
            }
            Log.WriteText(String.Format("Dictionary \"{0}\" export succeeded ...", dictionaryName));
        }

        private void ExportTargetDictionary()
        {
            String dictionaryName = LisDictionaryNames.Target;
            TargetDictionary dict = (TargetDictionary)ProgramContext.Dictionaries.GetDictionary(dictionaryName);

            DictionarySynchro dictionaryToExport = new DictionarySynchro();
            // dictionaryToExport.Name = "";
            dictionaryToExport.Code = dict.Name;

            foreach (TargetDictionaryItem target in dict.Target)
            {
                // Выгружаем только неудалённые элементы справочников
                if (!target.Removed)
                {
                    SynchronizationItem si = new SynchronizationItem();
                    si.Code = target.Code;
                    si.Name = target.Name;

                    if (target.Tests.Count > 0)
                        si.SubItems = new List<SynchronizationItem>();
                        
                    foreach (ObjectRef testRef in target.Tests)
                    {
                        TestDictionaryItem test = (TestDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LisDictionaryNames.Test, testRef.ID);
                        if ((test != null) && (!test.Removed))
                        { 
                            SynchronizationItem subItem = new SynchronizationItem();
                            subItem.Name = test.Name;
                            subItem.Code = test.Code;

                            si.SubItems.Add(subItem);
                        }
                    }

                    dictionaryToExport.Items.Add(si);
                }
            }
            
            XmlSerializer serializer = new XmlSerializer(typeof(DictionarySynchro));

            Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Export"));
            String ExportFileName = Path.Combine(Application.StartupPath, "Export", LisDictionaryNames.Target + ".xml");

            using (TextWriter tw = new StreamWriter(ExportFileName))
                serializer.Serialize(tw, dictionaryToExport);

            Log.WriteText(String.Format("Dictionary \"{0}\" export succeeded ", LisDictionaryNames.Target));
        }

    }
}
*/