using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System.Collections.Generic;
using System.Linq;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.MisExchange.DirectoryExport
{
    class ReferenceInformationExportProvider
    {
        public ReferenceInformationExportProvider(IDictionaryCache dictionaryCache)
        {
            DictionaryCache = dictionaryCache;
        }
        IDictionaryCache DictionaryCache { get; set; }

        public ReferenceInformation GetReferenceInformation()
        {
            //Для начала обновляеим все справочники для того, чтобы обновить информацию в кеше без перезапуска обменной службы
            CacheHelper.UpdateLoadedDictionaries(ProgramContext.LisCommunicator.LimsUserSession);
            ReferenceInformation referenceInformation = new ReferenceInformation();

            HandleTargetDictionary(referenceInformation);

            // Взаимосвязанные справочники Hospital -> CustDepartment -> Doctor
            HandleHospitalRelatedDictionaries(referenceInformation);

            // выгружаем только неудалённые микроорганизмы
            HandleMicroorganismDictionary(referenceInformation);

            // выгружаем только неудалённые типы браков
            HandleDefectDictionary(referenceInformation);

            // выгружаем только неудалённые категории оплаты
            HandlePayCategoryDictionary(referenceInformation);

            // выгружаем только неудалённые пользовательские поля
            HandleUserFieldDictionary(referenceInformation);

            return referenceInformation;
        }

        private void HandleUserFieldDictionary(ReferenceInformation referenceInformation)
        {
            UserFieldDictionary userFieldDict = DictionaryCache.GetDictionary<UserFieldDictionary, UserFieldDictionaryItem>();
            List<UserFieldDictionaryItem> userFieldDictionaryItems = userFieldDict.Elements.FindAll(uf => !uf.Removed);
            foreach (UserFieldDictionaryItem userFieldDictionaryItem in userFieldDictionaryItems)
            {
                UserField userField = new UserField();
                referenceInformation.UserFields.Add(userField);
                userField.Code = userFieldDictionaryItem.Code;
                userField.Name = userFieldDictionaryItem.Name;
            }
        }

        private void HandlePayCategoryDictionary(ReferenceInformation referenceInformation)
        {
            PayCategoryDictionary payCategoryDict = DictionaryCache.GetDictionary<PayCategoryDictionary, PayCategoryDictionaryItem>();
            List<PayCategoryDictionaryItem> payCategoryDictionaryItems = payCategoryDict.Elements.FindAll(pc => !pc.Removed);
            foreach (PayCategoryDictionaryItem payCategoryDictionaryItem in payCategoryDictionaryItems)
            {
                PayCategory payCategory = new PayCategory();
                referenceInformation.PayCategories.Add(payCategory);
                payCategory.Code = payCategoryDictionaryItem.Code;
                payCategory.Name = payCategoryDictionaryItem.Name;
            }
        }

        private void HandleDefectDictionary(ReferenceInformation referenceInformation)
        {
            DefectTypeDictionary defectTypeDict = DictionaryCache.GetDictionary<DefectTypeDictionary, DefectTypeDictionaryItem>();
            List<DefectTypeDictionaryItem> defectTypeDictionaryItems = defectTypeDict.Elements.FindAll(d => !d.Removed);
            foreach (DefectTypeDictionaryItem defectTypeDictionaryItem in defectTypeDictionaryItems)
            {
                DefectType defectType = new DefectType();
                referenceInformation.DefectTypes.Add(defectType);
                defectType.Code = defectTypeDictionaryItem.Code;
                defectType.Name = defectTypeDictionaryItem.Name;
            }
        }

        private void HandleMicroorganismDictionary(ReferenceInformation referenceInformation)
        {
            MicroOrganismDictionary microorganismsDict = DictionaryCache.GetDictionary<MicroOrganismDictionary, MicroOrganismDictionaryItem>();
            List<MicroOrganismDictionaryItem> microOrganismDictionaryItems = microorganismsDict.Elements.FindAll(m => !m.Removed);
            foreach (MicroOrganismDictionaryItem microOrganismDictionaryItem in microOrganismDictionaryItems)
            {
                Microorganism microorganism = new Microorganism();
                referenceInformation.Microorganisms.Add(microorganism);
                microorganism.Code = microOrganismDictionaryItem.Code;
                microorganism.Name = microOrganismDictionaryItem.Name;
            }
        }

        private void HandleTargetDictionary(ReferenceInformation referenceInformation)
        {
            TargetDictionary targetsDict = DictionaryCache.GetDictionary<TargetDictionary, TargetDictionaryItem>();
            // выгружаем только простые неудалённые исследования
            List<TargetDictionaryItem> simpleTargets = targetsDict.Elements.FindAll(t => (!t.Removed && t.IsSimple()));

            var targetGroupInfoQuery =
                from target in simpleTargets
                group target by new { Department = target.Department } into departmentInfo
                select new
                {
                    TargetGroupName = departmentInfo.Key.Department.Name,
                    TargetGroupCode = departmentInfo.Key.Department.Code,
                    Department = departmentInfo.Key.Department
                };

            // группируем исследования (в данный момент на основе их связи с подразделениями). Скорее всего, нужно переделать на группировку по типу (Биохимические, гематологические и т.д.)
            var targetGroupInfos = targetGroupInfoQuery.ToList();
            foreach (var targetGroupInfo in targetGroupInfos)
            {
                TargetGroup targetGroup = new TargetGroup();
                referenceInformation.TargetGroups.Add(targetGroup);

                targetGroup.Code = targetGroupInfo.TargetGroupCode;
                targetGroup.Name = targetGroupInfo.TargetGroupName;

                List<TargetDictionaryItem> targetDictionaryItems = simpleTargets.FindAll(t => t.Department.Id == targetGroupInfo.Department.Id);
                foreach (TargetDictionaryItem targetDictionaryItem in targetDictionaryItems)
                {
                    Target targetInfo = new Target();
                    targetGroup.Targets.Add(targetInfo);

                    targetInfo.Code = targetDictionaryItem.Code;
                    targetInfo.Name = targetDictionaryItem.Name;

                    foreach (BiomaterialDictionaryItem biomaterialDictionaryItem in targetDictionaryItem.Biomaterials)
                    {
                        Biomaterial biomaterialInfo = new Biomaterial();
                        targetInfo.Biomaterials.Add(biomaterialInfo);

                        biomaterialInfo.Code = biomaterialDictionaryItem.Code;
                        biomaterialInfo.Name = biomaterialDictionaryItem.Name;
                        biomaterialInfo.Mnemonics = biomaterialDictionaryItem.Mnemonics;
                    }

                    foreach (TestDictionaryItem testDictionaryItem in targetDictionaryItem.Tests)
                    {
                        Test testInfo = new Test();
                        targetInfo.Tests.Add(testInfo);

                        testInfo.Code = testDictionaryItem.Code;
                        testInfo.Name = testDictionaryItem.Name;
                    }

                    targetInfo.RequiresAdditionalTube = targetDictionaryItem.AdditionalTube;
                }
            }
        }

        private void HandleHospitalRelatedDictionaries(ReferenceInformation referenceInformation)
        {
            HospitalDictionary hospitalDict = DictionaryCache.GetDictionary<HospitalDictionary, HospitalDictionaryItem>();
            List<HospitalDictionaryItem> hospitalDictionaryItems = hospitalDict.Elements.FindAll(h => !h.Removed);
            foreach (HospitalDictionaryItem hospitalDictionaryItem in hospitalDictionaryItems)
            {
                Hospital hospital = new Hospital();
                referenceInformation.Hospitals.Add(hospital);
                hospital.Code = hospitalDictionaryItem.Code;
                hospital.Name = hospitalDictionaryItem.Name;

                List<CustDepartmentDictionaryItem> departments = hospitalDictionaryItem.CustDepartments.FindAll(d => !d.Removed);
                foreach (CustDepartmentDictionaryItem custDepartment in departments)
                {
                    Department department = new Department();
                    hospital.Departments.Add(department);
                    department.Code = custDepartment.Code;
                    department.Name = custDepartment.Name;

                    List<DoctorDictionaryItem> doctors = custDepartment.Doctors.FindAll(d => !d.Removed);
                    foreach (DoctorDictionaryItem doctorDictionaryItem in doctors)
                    {
                        Doctor doctor = new Doctor();
                        department.Doctors.Add(doctor);
                        doctor.Code = doctorDictionaryItem.Code;
                        doctor.Name = doctorDictionaryItem.Name;
                    }
                }
            }
        }
    }
}
