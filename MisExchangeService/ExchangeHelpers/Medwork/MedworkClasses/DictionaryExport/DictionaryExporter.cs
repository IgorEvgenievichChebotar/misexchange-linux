using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.DictionaryExport
{
    public class DictionaryExporter
    {
        private const String defaultDictionaryExportDirectory = "Dictionaries";
        
        private IDictionaryCache DictionaryCache { get; set; }
        private ExportDirectoryHelperSettings helperSettings = new ExportDirectoryHelperSettings();
        private DictionaryVersions dictionaryVersions = new DictionaryVersions();

        private String dictionaryVersionsInfoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ExportedMedworkDictionaryVersions.xml");
        private String dictionaryExportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, defaultDictionaryExportDirectory);

        private Boolean updated;

        public DictionaryExporter(IDictionaryCache dictionaryCache)
        {
            DictionaryCache = dictionaryCache;

            if (File.Exists(dictionaryVersionsInfoPath))
                dictionaryVersions = File.ReadAllText(dictionaryVersionsInfoPath, Encoding.UTF8).Deserialize<DictionaryVersions>(Encoding.UTF8);
            else
            {
                dictionaryVersions.Initialize();
                File.WriteAllText(dictionaryVersionsInfoPath, dictionaryVersions.Serialize(Encoding.UTF8), Encoding.UTF8);
            }

            string exportSettingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dictionaryExportSettings.xml");
            if (!File.Exists(exportSettingsFile))
            {
                Log.WriteError(String.Format("File [{0}] not found", exportSettingsFile));
                return;
            }
            helperSettings = File.ReadAllText(exportSettingsFile, Encoding.UTF8).Deserialize<ExportDirectoryHelperSettings>(Encoding.UTF8);
            if (!String.IsNullOrEmpty(helperSettings.Output))
                dictionaryExportPath = GetCorrectPath(helperSettings.Output);

            if (!Directory.Exists(dictionaryExportPath))
                Directory.CreateDirectory(dictionaryExportPath);

            CheckDictionaries();
        }

        private string GetCorrectPath(string path)
        {
            string result = path;
            if (!Path.IsPathRooted(result))
                result = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

            return result;
        }

        public void CheckDictionaries()
        {
            //updated = false;

            //checkDictionary<SuperCore.LimsDictionary.BiomaterialDictionary, BiomaterialDictionaryItem, BiomaterialDictionary>(ExportedDictionaries.BiomaterialDictionary);
            //checkDictionary<SuperCore.LimsDictionary.DefectTypeDictionary, DefectTypeDictionaryItem, DefectTypeDictionary>(ExportedDictionaries.DefectTypeDictionary);
            //checkDictionary<SuperCore.LimsDictionary.HospitalDictionary, HospitalDictionaryItem, HospitalDictionary>(ExportedDictionaries.HospitalDictionary);
            //checkDictionary<SuperCore.LimsDictionary.UserFieldDictionary, UserFieldDictionaryItem, UserFieldDictionary>(ExportedDictionaries.UserFieldDictionary);
            //checkDictionary<SuperCore.LimsDictionary.TargetDictionary, TargetDictionaryItem, TargetDictionary>(ExportedDictionaries.TargetDictionary);
            //checkDictionary<SuperCore.LimsDictionary.TestDictionary, TestDictionaryItem, TestDictionary>(ExportedDictionaries.TestDictionary);
            //if (updated)
            //    File.WriteAllText(dictionaryVersionsInfoPath, dictionaryVersions.Serialize(Encoding.UTF8), Encoding.UTF8);

            ExportDictionaries();
        }

        private void ExportDictionaries()
        {
            string filePath = Path.Combine(GetCorrectPath(dictionaryExportPath), "ExportResearchesInfo.xml");
            ResearchesData researchesData = GetResearchesData();
            File.WriteAllText(filePath, researchesData.Serialize(Encoding.UTF8).RemoveEmptyAttributes(), Encoding.UTF8);
        }

        private ResearchesData GetResearchesData()
        {
            ResearchesData researchesData = new ResearchesData();
            
            ru.novolabs.SuperCore.LimsDictionary.TargetDictionary dict = (ru.novolabs.SuperCore.LimsDictionary.TargetDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.Target];
            dict.Target.RemoveAll(t => t.Removed);
            foreach (TargetDictionaryItem target in dict.Target)
            {
                ResearchInfo researchInfo = new ResearchInfo();
                researchInfo.Id = target.Code;
                researchInfo.Name = target.Name;
                researchInfo.Description = target.Name;

                researchInfo.GroupId = GetGroupId(target);
                researchInfo.GroupName = GetGroupName(target);
                researchInfo.GroupDescription = GetGroupDescription(researchInfo.GroupName);
                researchInfo.BiomaterialCode = target.DefaultBiomaterial != null ? target.DefaultBiomaterial.Code : "";
                researchInfo.BiomaterialName = target.DefaultBiomaterial != null ? target.DefaultBiomaterial.Name : "";

                foreach (TestDictionaryItem test in target.Tests)
                {
                    TestInfo testInfo = new TestInfo();
                    testInfo.Id = test.Code;
                    testInfo.Name = test.Name;
                    testInfo.BaseMU = test.Unit != null ? test.Unit.Name : null;
                    testInfo.BaseMUId = test.Unit != null ? test.Unit.Code : null;

                    string testType = "";
                    switch (test.ResultType)
                    {
                        case (int)TestTypes.VALUE: // Количественный тест
                            testType = "Text";
                            break;
                        
                        case (int)TestTypes.ENUM:
                        case (int)TestTypes.ANTIBIOTIC: // Качественный тест, Тест на устойчивость к антибиотику
                            testType = "Enum";
                            testInfo.Enum = new List<string>();
                            test.Values.ForEach(v => testInfo.Enum.Add(v.Value));
                            break;
                        // Тестов типа "Numeric" и "Bacteria" в нашей системе не существует
                    }
                    testInfo.TestType = testType;

                    researchInfo.Tests.Add(testInfo);
                }



                researchesData.Researches.Add(researchInfo);
            }

            return researchesData;
        }

        private string GetGroupId(TargetDictionaryItem target)
        {
            string biomaterialName = target.DefaultBiomaterial != null ? target.DefaultBiomaterial.Name : "";
            return target.Department.Code + "[" + biomaterialName + "]";
        }

        private string GetGroupName(TargetDictionaryItem target)
        {
            string biomaterialName = target.DefaultBiomaterial != null ? target.DefaultBiomaterial.Name : "";
            return target.Department.Name.Trim() + " [" + biomaterialName + "]";
        }

        private string GetGroupDescription(string groupName)
        {
            return groupName;
        }

        private Int64 getExportedDictionaryVersion(String dictionaryName)
        {
            Int64 Result = -1;
            Int32 index;

            if (dictionaryVersions.FindDictionary(dictionaryName, out index))
                Result = dictionaryVersions.Dictionaries[index].Version;
 
            return Result;
        }

        private void checkDictionary<T, U, E>(String dictionaryType)
            where T : DictionaryClass<U>
            where U : DictionaryItem
            where E : ExportedDictionary
        {
            var dict = DictionaryCache.GetDictionary<T, U>();
            var exportedVersion = getExportedDictionaryVersion(dict.Name);
            if (dict.Version <= exportedVersion)
                return;

            object exportedDictionary = null;
            switch (dictionaryType)
            {
                case ExportedDictionaries.BiomaterialDictionary:
                    exportedDictionary = new BiomaterialDictionary();
                    break;
                case ExportedDictionaries.DefectTypeDictionary:
                    exportedDictionary = new DefectTypeDictionary();
                    break;
                case ExportedDictionaries.HospitalDictionary:
                    exportedDictionary = new HospitalDictionary();
                    break;
                case ExportedDictionaries.UserFieldDictionary:
                    exportedDictionary = new UserFieldDictionary();
                    break;
                case ExportedDictionaries.TargetDictionary:
                    exportedDictionary = new TargetDictionary();
                    break;
                case ExportedDictionaries.TestDictionary:
                    exportedDictionary = new TestDictionary();
                    break;
            }

            dict.Elements.ForEach(item => ((E)exportedDictionary).AddItem(item));
            File.WriteAllText(Path.Combine(dictionaryExportPath, String.Format("{0}.xml", exportedDictionary.GetType().Name)), exportedDictionary.Serialize(Encoding.UTF8), Encoding.UTF8);
            dictionaryVersions.UpdateVersion(dict.Name, dict.Version);
            updated = true;
        }
    }
}