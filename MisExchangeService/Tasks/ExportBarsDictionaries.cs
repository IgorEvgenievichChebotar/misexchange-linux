//using ru.novolabs.SuperCore;
//using ru.novolabs.SuperCore.LimsBusinessObjects;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Xml;
//using System.Xml.Serialization;
//using LisServiceClients.BarsNomenclature;
//using ru.novolabs.SuperCore.LimsDictionary;

//namespace ru.novolabs.MisExchange.Tasks
//{
//    public class ExportBarsDictionaries: Task
//    {
//        List<String> requiredDictionariesNames = new List<string>()
//        {
//            LimsDictionaryNames.Target,
//            LimsDictionaryNames.Test,
//            LimsDictionaryNames.Biomaterial
//        };
//        LisBarsSendNomenclatureClient client;

//        private String _fileVersionName = "dictionariesVersion.txt";
//        private Boolean _isFirstLoading = false;

//        public override void Execute()
//        {
           
//            return;
//            List<DirectoryVersionInfo> versions = Deserialize();
                       
//            List<DirectoryVersionInfo> directoryVersions = 
//                ProgramContext.LisCommunicator.DirectoryVersions();

//            Boolean changed = false;
            
            
//            foreach (DirectoryVersionInfo directoryVersion in directoryVersions)
//            {
//                if (requiredDictionariesNames.Contains(directoryVersion.Name))
//                {
//                    DirectoryVersionInfo version = versions.Find(x => x.Name == directoryVersion.Name);
//                    if (version == null || version.Version != directoryVersion.Version)
//                    {
//                        changed = true;
//                        //Update nomenclature
//                    }
//                }
//            }

//            if (changed) {
//                try
//                {
//                    List<Nomenclature> nomenclatures = PrepareNomenclature();
//                    System.Net.ServicePointManager.Expect100Continue = false;
//                    client = new LisServiceClients.BarsNomenclature.LisBarsSendNomenclatureClient();
//                    sendNomenclatureResponse response = client.sendNomenclature(nomenclatures.ToArray());
//                    Log.WriteText("Response: {0}", response.error_code);
//                    System.Net.ServicePointManager.Expect100Continue = true;
//                    Serialize(directoryVersions);
//                }
//                catch (Exception ex)
//                {
//                    Log.WriteError("Failed to send nomenclatures: {0} \r\n Stacktrace: {1}", ex.Message, ex.StackTrace);
//                }
//            }
//        }

//        private List<DirectoryVersionInfo> Deserialize()
//        {
//            List<DirectoryVersionInfo> versions = new List<DirectoryVersionInfo>();
//            if (File.Exists(_fileVersionName))
//            {
//                try
//                {
//                    XmlSerializer serializer = new XmlSerializer(typeof(List<DirectoryVersionInfo>));
//                    StreamReader reader = new StreamReader(_fileVersionName);
//                    reader.ReadToEnd();
//                    versions = (List<DirectoryVersionInfo>)serializer.Deserialize(reader);
//                    reader.Close();
//                }
//                catch (Exception ex)
//                {
//                    Log.WriteError("Can't deserialize versions: {0}", ex.Message);
//                }
//            }
//            else
//                _isFirstLoading = true;

//            return versions;
//        }

//        private void Serialize(List<DirectoryVersionInfo> versions)
//        {
//            try
//            {
//                XmlSerializer serializer = new XmlSerializer(typeof(List<DirectoryVersionInfo>));
//                StringWriter sww = new StringWriter();
//                using (XmlWriter writer = XmlWriter.Create(sww))
//                {
//                    serializer.Serialize(writer, versions);
//                    String xml = sww.ToString();
//                    File.WriteAllText(_fileVersionName, xml);
//                }
//            }
//            catch (Exception ex)
//            {
//                Log.WriteError("Can't serialize versions: {0}", ex.Message);
//            }
//        }

//        private List<Nomenclature> PrepareNomenclature()
//        {
//            List<Nomenclature> nomenclatures = new List<Nomenclature>();

//            List<TargetDictionaryItem> targets = ((List<TargetDictionaryItem>)ProgramContext.Dictionaries.GetDictionaryElements(LimsDictionaryNames.Target)).FindAll(x => !x.Removed);

//            foreach (TargetDictionaryItem target in targets)
//            {
//                Nomenclature nomenclature = new Nomenclature();
//                //Код учреждения
//                nomenclature.code_lpu = "";
//                //Код исследования
//                nomenclature.code_ta = target.Code;
//                //Имя исследования
//                nomenclature.name_ta = target.Name;
//                //Группа или профиль
//                if (target.IsGroup() || target.IsProfile())
//                {
//                    nomenclature.is_group = 1;
//                }
//                else nomenclature.is_group = 0;

//                //1 - добавление 2 - изменения 3 - удаление
//                if (_isFirstLoading)
//                    nomenclature.action = 1;
//                else
//                    nomenclature.action = 2;
//                //Один ли тест в составе исследования? (не очень ясно, зачем им это)
//                if (target.Tests.Count > 1)
//                    nomenclature.is_comp = 1;
//                else
//                    nomenclature.is_comp = 0;
//                //Расчетный показатель?
//                nomenclature.is_calc = 0;
//                //Микробиология?
//                if (target.Tests.Find(x => x.IsMicrobiology) != null)
//                    nomenclature.is_micro = 1;
//                else
//                    nomenclature.is_micro = 0;
//                nomenclature.bio = PrepareBiomaterials(target).ToArray();
//                nomenclature.test = PrepareTests(target).ToArray();
//                nomenclatures.Add(nomenclature);
//            }

//            return nomenclatures;
//        }

//        private List<Bio> PrepareBiomaterials(TargetDictionaryItem target)
//        {
//            List<Bio> result = new List<Bio>();
//            foreach (BiomaterialDictionaryItem biomaterial in target.Biomaterials)
//            {
//                Bio bio = new Bio();
//                bio.bio_code = biomaterial.Code;
//                bio.bio_name = biomaterial.Name;
//                result.Add(bio);
//            }
//            return result;
//        }

//        private List<Test> PrepareTests(TargetDictionaryItem target)
//        {
//            List<Test> result = new List<Test>();
//            foreach (TestDictionaryItem testdi in target.Tests)
//            {
//                Test test = new Test();
//                test.research_code = testdi.Code;
//                test.research_name = testdi.Name;
//                result.Add(test);
//            }
//            return result;
//        }

        
//    }
//}
