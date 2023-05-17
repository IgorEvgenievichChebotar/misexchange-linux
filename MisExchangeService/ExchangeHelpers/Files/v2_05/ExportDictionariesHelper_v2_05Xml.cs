using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.ExchangeHelpers.Files.v2_05.DirectoryClasses;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Files.v2_05
{
    [ExportDictionariesHelperName("ExportDictionaries_v2_05+_Xml")]
    class ExportDictionariesHelper_v2_05Xml : ExportDictionariesHelper
    {
        private ExportDirectoryHelperSettings helperSettings = null;

        public ExportDictionariesHelper_v2_05Xml()
        {
            string exportSettingsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dictionaryExportSettings.xml");
            if (!File.Exists(exportSettingsFile))
                throw new Exception(String.Format("File [{0}] not found", exportSettingsFile));
            helperSettings = File.ReadAllText(exportSettingsFile, Encoding.UTF8).Deserialize<ExportDirectoryHelperSettings>(Encoding.UTF8);
        }

        public override void DoExport()
        {
            ExportTestDictionary();
            ExportBiomaterialDictionary();
            ExportTargetDictionary();
            ExportDefectTypeDictionary();
            ExportUserFieldDictionary();
            ExportMicroorganismDictionary();
            ExportPayCategoryDictionary();
            ExportHospitalDictionary();
        }

        private void ExportTestDictionary()
        {
            TestDictionary testsDict = (TestDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.Test];
            var dic = new TestsDictionary();

            foreach (TestDictionaryItem tst in testsDict.Test)
            {
                Test test = new Test(tst);
                dic.Tests.Add(test);
            }
            dic.Tests = dic.Tests.OrderBy(t => t.Removed).ToList();

            File.WriteAllText(Path.Combine(helperSettings.Output, "TestDictionary.xml"), dic.Serialize(Encoding.UTF8), Encoding.UTF8);
        }

        private void ExportBiomaterialDictionary()
        {
            BiomaterialDictionary biomaterialDict = (BiomaterialDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.Biomaterial];
            var dic = new BiomaterialsDictionary();

            foreach (BiomaterialDictionaryItem biom in biomaterialDict.BioMaterial)
            {
                Biomaterial biomaterial = new Biomaterial(biom);
                dic.Biomaterials.Add(biomaterial);
            }
            dic.Biomaterials = dic.Biomaterials.OrderBy(t => t.Removed).ToList();

            File.WriteAllText(Path.Combine(helperSettings.Output, "BiomaterialDictionary.xml"), dic.Serialize(Encoding.UTF8), Encoding.UTF8);
        }

        private void ExportTargetDictionary()
        {
            TargetDictionary targets = (TargetDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.Target];
            var dic = new TargetsDictionary();

            // выгружаем только простые исследования
            List<TargetDictionaryItem> simpleTargets = targets.Elements.FindAll(t => (!t.Removed && t.IsSimple()));

            foreach (TargetDictionaryItem tg in simpleTargets)
            {
                Target target = new Target() { Name = tg.Name, Code = tg.Code, Mnemonics = tg.Mnemonics, Removed = tg.Removed };
                foreach (BiomaterialDictionaryItem bm in tg.Biomaterials)
                {
                    Biomaterial bm2 = new Biomaterial(bm);
                    target.Biomaterials.Add(bm2);
                }
                foreach (TestDictionaryItem tst in tg.Tests)
                {
                    Test test = new Test(tst);
                    target.Tests.Add(test);
                }

                dic.Targets.Add(target);
            }
            dic.Targets = dic.Targets.OrderBy(t => t.Removed).ToList();

            File.WriteAllText(Path.Combine(helperSettings.Output, "TargetDictionary.xml"), dic.Serialize(Encoding.UTF8), Encoding.UTF8);
        }

        private void ExportDefectTypeDictionary()
        {
            DefectTypeDictionary defectsDict = (DefectTypeDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.DefectType];
            var dic = new DefectTypesDictionary();

            foreach (DefectTypeDictionaryItem def in defectsDict.DefectType)
            {
                DefectType defectType = new DefectType(def);
                dic.DefectTypes.Add(defectType);
            }
            dic.DefectTypes = dic.DefectTypes.OrderBy(dt => dt.Removed).ToList();

            File.WriteAllText(Path.Combine(helperSettings.Output, "DefectTypeDictionary.xml"), dic.Serialize(Encoding.UTF8), Encoding.UTF8);
        }

        private void ExportUserFieldDictionary()
        {
            UserFieldDictionary userFieldDict = (UserFieldDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.UserField];
            var dic = new UserFieldsDictionary();

            foreach (UserFieldDictionaryItem uf in userFieldDict.UserField)
            {
                UserField userField = new UserField(uf);
                dic.UserFields.Add(userField);
            }
            dic.UserFields = dic.UserFields.OrderBy(uf => uf.Removed).ToList();

            File.WriteAllText(Path.Combine(helperSettings.Output, "UserFieldDictionary.xml"), dic.Serialize(Encoding.UTF8), Encoding.UTF8);
        }

        private void ExportMicroorganismDictionary()
        {
            MicroOrganismDictionary microorganismDict = (MicroOrganismDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.MicroOrganism];
            var dic = new MicroorganismsDictionary();

            foreach (MicroOrganismDictionaryItem mo in microorganismDict.MicroOrganism)
            {
                Microorganism microOrg = new Microorganism(mo);
                dic.Microorganisms.Add(microOrg);
            }
            dic.Microorganisms = dic.Microorganisms.OrderBy(m => m.Removed).ToList();

            File.WriteAllText(Path.Combine(helperSettings.Output, "MicroorganismDictionary.xml"), dic.Serialize(Encoding.UTF8), Encoding.UTF8);
        }
        private void ExportPayCategoryDictionary()
        {
            PayCategoryDictionary payCategoryDict = (PayCategoryDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.PayCategory];
            var dic = new PayCategoriesDictionary();

            foreach (PayCategoryDictionaryItem payCat in payCategoryDict.PayCategory)
            {
                PayCategory payCategory = new PayCategory(payCat);
                dic.PayCategories.Add(payCategory);
            }
            dic.PayCategories = dic.PayCategories.OrderBy(pc => pc.Removed).ToList();

            File.WriteAllText(Path.Combine(helperSettings.Output, "PayCategoryDictionary.xml"), dic.Serialize(Encoding.UTF8), Encoding.UTF8);
        }
        private void ExportHospitalDictionary()
        {
            HospitalDictionary hospitalDict = (HospitalDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.Hospital];
            var dic = new HospitalsDictionary();

            foreach (HospitalDictionaryItem hosp in hospitalDict.Hospital)
            {
                Hospital hospital = new Hospital(hosp);
                dic.Hospitals.Add(hospital);
            }
            dic.Hospitals = dic.Hospitals.OrderBy(h => h.Removed).ToList();

            File.WriteAllText(Path.Combine(helperSettings.Output, "HospitalDictionary.xml"), dic.Serialize(Encoding.UTF8), Encoding.UTF8);
        }
    }

    public class ExportDirectoryHelperSettings
    {
        public string Output { get; set; }
        public ExportDirectoryHelperSettings()
        {
            Output = "DirectoryExport";
        }
    }
}
