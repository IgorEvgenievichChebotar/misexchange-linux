using LisServiceClients.BarsNomenclature;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.BARS
{
    class BarsNomenclatureProcessing
    {
        public BarsNomenclatureProcessing(ExportDirectoryHelperSettings helperSettings,
            List<NomenclatureElement> synchronizedTargets,
            List<NomenclatureElement> synchronizedDepartments, 
            List<TestWrapper> synchronizedDefaultTests, 
            List<BioWrapper> synchronizedDefaultBiomaterials)
        {                                                                               
            HelperSettings = helperSettings;
            SynchronizedTargets = synchronizedTargets;
            SynchronizedDepartments = synchronizedDepartments;
            SynchronizedDefaultTests = synchronizedDefaultTests;
            SynchronizedDefaultBiomaterials = synchronizedDefaultBiomaterials;
        }                                                                              
        ExportDirectoryHelperSettings HelperSettings { get; set; }
        List<NomenclatureElement> SynchronizedTargets { get; set; }
        List<NomenclatureElement> SynchronizedDepartments { get; set; }
        List<TestWrapper> SynchronizedDefaultTests { get; set; }
        List<BioWrapper> SynchronizedDefaultBiomaterials { get; set; }

        public List<Nomenclature> PrepareNomenclature()
        {
            List<Nomenclature> nomenclatures = new List<Nomenclature>();

            List<TargetDictionaryItem> targets = ((List<TargetDictionaryItem>)ProgramContext.Dictionaries.GetDictionaryElements(LimsDictionaryNames.Target)).FindAll(x => !x.Removed && x.IsSimple());
            List<TestDictionaryItem> tests = ((List<TestDictionaryItem>)ProgramContext.Dictionaries.GetDictionaryElements(LimsDictionaryNames.Test)).FindAll(x => !x.Removed);
            List<BiomaterialDictionaryItem> biomaterials = ((List<BiomaterialDictionaryItem>)ProgramContext.Dictionaries[LimsDictionaryNames.Biomaterial].DictionaryElements).FindAll(x => !x.Removed);
            nomenclatures.AddRange(PrepairDepartments());
            nomenclatures.AddRange(AddRemovedDepartments());

            foreach (TargetDictionaryItem target in targets)
            {
                if (target.Tests == null || target.Tests.Count == 0)
                    continue;
                Nomenclature nomenclature = new Nomenclature();
                //Код учреждения
                nomenclature.code_lpu = HelperSettings.HospitalCode;
                //Код исследования
                nomenclature.code_ta = target.Code;
                //Имя исследования
                nomenclature.name_ta = target.Name;

                nomenclature.is_group = ElementIsGroup.Target.GetHashCode();

                nomenclature.parent_ta = target.Department.Code;

                //1 - добавление 2 - изменение 3 - удаление
                var synchronizedTarget = SynchronizedTargets.Find(t => t.Id == target.Id);
                if (synchronizedTarget != null)
                {
                    if (!IsTargetChanged(target) && synchronizedTarget.Version >= target.Version)
                        continue;
                    nomenclature.action = ElementAction.Change.GetHashCode();
                }
                else
                {
                    nomenclature.action = ElementAction.New.GetHashCode();
                }
                if (target.Tests.Count > 1)
                    nomenclature.is_comp = 1;
                else
                    nomenclature.is_comp = 0;
                //Расчетный показатель?
                nomenclature.is_calc = 0;
                //Микробиология?
                if (target.Department.Micro)
                    nomenclature.is_micro = 1;
                else
                    nomenclature.is_micro = 0;

                nomenclature.bio = PrepareBiomaterials(target).ToArray();
                var testsTemp = PrepareTests(target);
                if (target.Department.Micro)
                    testsTemp.Add(BuildMicroTest());

                nomenclature.test = testsTemp.ToArray();

                nomenclature.parent_ta = target.Department.Code;
                nomenclatures.Add(nomenclature);
                if (synchronizedTarget != null && synchronizedTarget.Code != target.Code)
                {
                    var removedTarget = nomenclature.CopyViaSerialization();
                    removedTarget.code_ta = synchronizedTarget.Code;
                    removedTarget.action = ElementAction.Remove.GetHashCode();
                    nomenclature.action = ElementAction.New.GetHashCode();
                    nomenclatures.Add(removedTarget);
                }
            }
            nomenclatures.AddRange(AddRemovedTargets());
            if (!SynchronizedTargets.Exists(t => t.Code == "default"))
                nomenclatures.AddRange(AddDefaultTarget(tests, biomaterials));

            return nomenclatures;
        }

        private List<Nomenclature> AddRemovedTargets()
        {
            List<Nomenclature> nomenclatures = new List<Nomenclature>();
            List<TargetDictionaryItem> targets = ((List<TargetDictionaryItem>)ProgramContext.Dictionaries.GetDictionaryElements(LimsDictionaryNames.Target)).FindAll(x => x.Removed && x.IsSimple());

            foreach (TargetDictionaryItem target in targets)
            {
                if (target.Tests == null || target.Tests.Count == 0)
                    continue;
                if (!SynchronizedTargets.Exists(t => t.Id == target.Id))
                    continue;
                Nomenclature nomenclature = new Nomenclature();
                //Код учреждения
                nomenclature.code_lpu = HelperSettings.HospitalCode;
                //Код исследования
                nomenclature.code_ta = target.Code;
                //Имя исследования
                nomenclature.name_ta = target.Name;

                nomenclature.is_group = ElementIsGroup.Target.GetHashCode();

                nomenclature.parent_ta = target.Department.Code;

                //1 - добавление 2 - изменение 3 - удаление
                nomenclature.action = ElementAction.Remove.GetHashCode();

                if (target.Tests.Count > 1)
                    nomenclature.is_comp = 1;
                else
                    nomenclature.is_comp = 0;
                //Расчетный показатель?
                nomenclature.is_calc = 0;
                //Микробиология?
                if (target.Department.Micro)
                    nomenclature.is_micro = 1;
                else
                    nomenclature.is_micro = 0;

                nomenclature.bio = PrepareBiomaterials(target).ToArray();
                var testsTemp = PrepareTests(target);
                if (target.Department.Micro)
                    testsTemp.Add(BuildMicroTest());

                nomenclature.bio = PrepareBiomaterials(target).ToArray();
                nomenclature.test = testsTemp.ToArray();

                nomenclature.parent_ta = target.Department.Code;
                nomenclatures.Add(nomenclature);
            }
            return nomenclatures;

        }

        private List<Nomenclature> AddDefaultTarget(List<TestDictionaryItem> tests, List<BiomaterialDictionaryItem> biomaterials)
        {
            List<Nomenclature> nomenclatures = new List<Nomenclature>();
            Nomenclature nomenclatureDep = new Nomenclature();
            nomenclatureDep.code_lpu = HelperSettings.HospitalCode;
            //Код исследования
            nomenclatureDep.code_ta = "default";
            //Имя исследования
            nomenclatureDep.name_ta = "Назначено вручную";

            nomenclatureDep.is_group = ElementIsGroup.Department.GetHashCode();

            var synchronizedDepartment = SynchronizedDepartments.Find(d => d.Code == nomenclatureDep.code_ta);
            if (synchronizedDepartment != null)
                nomenclatureDep = null;
            else
            {
                nomenclatureDep.action = ElementAction.New.GetHashCode();
            }


            Nomenclature nomenclature = ProcessDefaultTarget(tests, biomaterials);
            if (nomenclatureDep != null)
                nomenclatures.Add(nomenclatureDep);
            if (nomenclature != null)
                nomenclatures.Add(nomenclature);
            return nomenclatures;

        }

        private Nomenclature ProcessDefaultTarget(List<TestDictionaryItem> tests, List<BiomaterialDictionaryItem> biomaterials)
        {
            Nomenclature nomenclature = new Nomenclature();
            nomenclature.code_lpu = HelperSettings.HospitalCode;
            //Код исследования
            nomenclature.code_ta = "default";
            //Имя исследования
            nomenclature.name_ta = "Назначено вручную";

            nomenclature.is_group = ElementIsGroup.Target.GetHashCode();

            nomenclature.parent_ta = "default";

            //1 - добавление 2 - изменение 3 - удаление
            if (SynchronizedTargets.Exists(t => t.Code == nomenclature.code_ta))
            {
                if (!IsDefaultChanged())
                    return null;
                nomenclature.action = ElementAction.Change.GetHashCode();
            }
            else
            {
                nomenclature.action = ElementAction.New.GetHashCode();
            }

            nomenclature.is_comp = 1;

            //Расчетный показатель?
            nomenclature.is_calc = 0;
            var biomaterialsDefault =
            nomenclature.bio = biomaterials.Select(b => new Bio() { bio_code = b.Code, bio_name = b.Name }).ToArray();
            nomenclature.test = (from test in tests select new Test { research_code = test.Code, research_name = test.Name }).ToArray();
            return nomenclature;
        }

        private List<Nomenclature> PrepairDepartments()
        {
            List<Nomenclature> nomenclatures = new List<Nomenclature>();
            List<DepartmentDictionaryItem> departments = ((List<DepartmentDictionaryItem>)ProgramContext.Dictionaries.GetDictionaryElements(LimsDictionaryNames.Department)).FindAll(d => !d.Removed);
            foreach (var department in departments)
            {
                Nomenclature nomenclature = new Nomenclature();
                nomenclature.code_lpu = HelperSettings.HospitalCode;
                //Код исследования
                nomenclature.code_ta = department.Code;
                //Имя исследования
                nomenclature.name_ta = department.Name;

                nomenclature.is_group = ElementIsGroup.Department.GetHashCode();

                //1 - добавление 2 - изменение 3 - удаление
                var synchronizedDepartment = SynchronizedDepartments.Find(d => d.Id == department.Id);
                if (synchronizedDepartment != null)
                {
                    if (synchronizedDepartment.Version >= department.Version)
                        continue;
                    nomenclature.action = ElementAction.Change.GetHashCode();
                }
                else
                {
                    nomenclature.action = ElementAction.New.GetHashCode();
                }
                if (synchronizedDepartment != null && synchronizedDepartment.Code != department.Code)
                {
                    var removedDepartment = nomenclature.CopyViaSerialization();
                    nomenclature.action = ElementAction.New.GetHashCode();
                    removedDepartment.code_ta = synchronizedDepartment.Code;
                    removedDepartment.action = ElementAction.Remove.GetHashCode();
                    nomenclatures.Add(removedDepartment); 
                }

                nomenclatures.Add(nomenclature);
            }

            return nomenclatures;
        }

        private List<Nomenclature> AddRemovedDepartments()
        {
            List<Nomenclature> nomenclatures = new List<Nomenclature>();
            List<DepartmentDictionaryItem> departments = ((List<DepartmentDictionaryItem>)ProgramContext.Dictionaries.GetDictionaryElements(LimsDictionaryNames.Department)).FindAll(d => d.Removed);
            foreach (var department in departments)
            {
                if (!SynchronizedDepartments.Exists(d => d.Id == department.Id))
                    continue;
                Nomenclature nomenclature = new Nomenclature();
                nomenclature.code_lpu = HelperSettings.HospitalCode;
                //Код исследования
                nomenclature.code_ta = department.Code;
                //Имя исследования
                nomenclature.name_ta = department.Name;

                nomenclature.is_group = ElementIsGroup.Department.GetHashCode();

                //1 - добавление 2 - изменение 3 - удаление

                nomenclature.action = ElementAction.Remove.GetHashCode();

                nomenclatures.Add(nomenclature);
            }
            return nomenclatures;
        }

        private List<Bio> PrepareBiomaterials(TargetDictionaryItem target)
        {
            List<Bio> result = new List<Bio>();
            foreach (BiomaterialDictionaryItem biomaterial in target.Biomaterials)
            {
                Bio bio = new Bio();
                bio.bio_code = biomaterial.Code;
                bio.bio_name = biomaterial.Name;
                result.Add(bio);
            }
            return result;
        }

        private List<Test> PrepareTests(TargetDictionaryItem target)
        {
            List<Test> result = new List<Test>();
            foreach (TestDictionaryItem testdi in target.Tests)
            {
                Test test = new Test();
                test.research_code = testdi.Code;
                test.research_name = testdi.Name;
                result.Add(test);
            }
            return result;
        }

        private Boolean IsDefaultChanged()
        {
            var testCodes = ((List<TestDictionaryItem>)ProgramContext.Dictionaries[LimsDictionaryNames.Test].DictionaryElements).Where(t => !t.Removed);
            var difference = testCodes.Select(t => new { NewName = t.Name, NewCode = t.Code, OldElement = SynchronizedDefaultTests.Find(s => s.Id == t.Id) })
                .Where(t => t.OldElement == null || t.NewName != t.OldElement.research_name || t.NewCode != t.OldElement.research_code);
            if (difference.Count() > 0)
                return true;
            var removedTests = SynchronizedDefaultTests.Select(t => t.Id).Except(testCodes.Select(t => t.Id));
            if (removedTests.Count() > 0)
                return true;
            var biomaterialCodes = ((List<BiomaterialDictionaryItem>)ProgramContext.Dictionaries[LimsDictionaryNames.Biomaterial].DictionaryElements).Where(b => !b.Removed);
            var differenceBiomaterials = biomaterialCodes.Select(b => new { NewName = b.Name, NewCode = b.Code, OldElement = SynchronizedDefaultBiomaterials.Find(s => s.Id == b.Id) })
                .Where(b => b.OldElement == null || b.NewName != b.OldElement.bio_name || b.NewCode != b.OldElement.bio_code);
            if (differenceBiomaterials.Count() > 0)
                return true;
            var removedBiomaterials = SynchronizedDefaultBiomaterials.Select(b => b.Id).Except(biomaterialCodes.Select(b => b.Id));
            if (removedBiomaterials.Count() > 0)
                return true;
            return false;
        }

        private Boolean IsTargetChanged(TargetDictionaryItem target)
        {
            var differenceTests = target.Tests.Select(t => new { NewName = t.Name, NewCode = t.Code, OldElement = SynchronizedDefaultTests.Find(s => s.Id == t.Id) })
                .Where(t => t.OldElement != null && (t.NewName != t.OldElement.research_name || t.NewCode != t.OldElement.research_code));
            if (differenceTests.Count() > 0)
                return true;
            var differenceBiomaterials = target.Biomaterials.Select(b => new { NewName = b.Name, NewCode = b.Code, OldElement = SynchronizedDefaultBiomaterials.Find(s => s.Id == b.Id) })
                .Where(b => b.OldElement != null && (b.NewName != b.OldElement.bio_name || b.NewCode != b.OldElement.bio_code));
            if (differenceBiomaterials.Count() > 0)
                return true;
            return false;
        }

        private Test BuildMicroTest()
        {
            Test test = new Test();
            test.research_code = HelperSettings.DefaultMicroorganismTestCode;
            test.research_name = HelperSettings.DefaultMicroorganismTestName;
            return test;
        }
    }
}
