using LisServiceClients.BarsNomenclature;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.BARS
{
    enum ElementAction
    {
        New = 1,
        Change = 2,
        Remove = 3
    }
    enum ElementIsGroup
    {
        Target = 0,
        Department = 1
    }

    [ExportDictionariesHelperName("ExportDictionaries_Bars")]
    class ExportDictionariesHelper_Bars : ExportDictionariesHelper
    {
        private BARS.ExportDirectoryHelperSettings helperSettings = null;

        private List<NomenclatureElement> synchronizedTargets = new List<NomenclatureElement>();
        private List<NomenclatureElement> synchronizedDepartments = new List<NomenclatureElement>();
        private List<TestWrapper> synchronizedDefaultTests = new List<TestWrapper>();
        private List<BioWrapper> synchronizedDefaultBiomaterials = new List<BioWrapper>();
        List<String> requiredDictionariesNames = new List<string>()
        {
            LimsDictionaryNames.Department,
            LimsDictionaryNames.Target,
            LimsDictionaryNames.Test,
            LimsDictionaryNames.Biomaterial
        };

        LisBarsSendNomenclatureClient client;
        BarsSerializeHelper BarsSerializeHelper { get; set; }

        public ExportDictionariesHelper_Bars()
        {
            string exportSettingsFile = Path.Combine(PathHelper.AssemblyDirectory, "dictionaryExportSettings.xml");
            if (!File.Exists(exportSettingsFile))
                throw new Exception(String.Format("File [{0}] not found", exportSettingsFile));
            helperSettings = File.ReadAllText(exportSettingsFile, Encoding.UTF8).Deserialize<BARS.ExportDirectoryHelperSettings>(Encoding.UTF8);
            client = new LisServiceClients.BarsNomenclature.LisBarsSendNomenclatureClient();
            BarsSerializeHelper = new BarsSerializeHelper(helperSettings);
        }

        public override void DoExport()
        {
            // Если производится не первичная выгрузка справочников в МИС (режим промышленной эксплуатации), то обязательно наличие непустых файлов dictionariesVersion.txt,
            // synchronizedTargets.txt, synchronizedDepartments.txt, synchronizedDefaultTests.txt, synchronizedDefaultBiomaterials.txt.
            // Если файлов нет или они пусты, то по таймеру (раз в taskManagerWaitInterval секунд) будет производиться выгрузка в МИС всей номенклатуры. Такая выгрузка сильно
            // увеличивает трафик. Плюс ко всему внешний сервис умеет принимать полную номенклатуру только один раз и в ручном режиме. Далее он ждёт только изменённые данные
            // Выгрузка полной номенклатуры должна была быть выполнена однократно при начале эксплуатации интеграции,
            // далее в МИС должны выгружаться только изменённые данные и крайне редко (при внесении изменений в справочники ЛИС)
            if (!helperSettings.InitialNomenclatureExport)
                if (!CheckSynchronizationFilesAreValid())
                    return;

            CheckIfMicroTestExists();

            try
            {
                List<DirectoryVersionInfo> directoryVersions = ProgramContext.LisCommunicator.DirectoryVersions();
                if (helperSettings.RepairSynchronizationFiles)
                {
                    Log.WriteText("Восстановление файлов синхронизации справочников...");
                    RepairSynchronizationFiles(directoryVersions);
                }
                else
                {
                    Log.WriteText("Синхронизация номенклатуры исследований...");
                    
                    List<DirectoryVersionInfo> versions = BarsSerializeHelper.DeserializeVersions();
                    Init();
                    Boolean changed = IsDictionaryVersionsChanged(versions, directoryVersions);

                    if (!changed)
                        return;
                    DictionariesExportHelper.RefreshDictionaries(requiredDictionariesNames);
                    try
                    {
                        List<Nomenclature> nomenclatures = null;
                        BarsNomenclatureProcessing processing = new BarsNomenclatureProcessing(helperSettings, synchronizedTargets, synchronizedDepartments, synchronizedDefaultTests, synchronizedDefaultBiomaterials);
                        nomenclatures = processing.PrepareNomenclature();

                        //nomenclatures = nomenclatures.GetRange(0, 1);
                        System.Net.ServicePointManager.Expect100Continue = false;
                        
                        LisServiceClients.BarsNomenclature.Nomenclature[] request = nomenclatures.ToArray();
                        if (request.Length == 0)
                        {
                            Log.WriteText("Изменений в номенклатуре не обнаружено.");
                            return;
                        }
                        
                        Log.WriteText("SendNomenclature ...");//:\r\n{0}", nomenclatures.ToArray().Serialize(Encoding.UTF8));
                        sendNomenclatureResponse response = client.sendNomenclature(request);
                        Log.WriteText("SendNomenclature completed");

                        if (response.error != null && response.error.Length > 0)
                            Log.WriteText("При синхронизации номенклатуры произошли следующие ошибки:");
                        if (response.error == null)
                            response.error = new sendNomenclatureResponseError[0];
                        foreach (sendNomenclatureResponseError error in response.error)
                        {
                            Log.WriteError("code_lpu: {0}, code_ta: {1}, error_msg: {2}", error.code_lpu, error.code_ta, error.error_msg);
                        }

                        ProcessAfterSending(directoryVersions, nomenclatures, response);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteError("Failed to send nomenclatures: {0} \r\n Stacktrace: {1}", ex.Message, ex.StackTrace);
                    }
                }
            }
            catch (Exception)
            {
                Log.WriteError("ExportDictionariesHelper_Bars. DoExport() Failed");
            }
        }

        private bool CheckSynchronizationFilesAreValid()
        {
            if (helperSettings.RepairSynchronizationFiles)
                return true;

            if ((!File.Exists(BarsSerializeHelper._fileVersionName) || (new System.IO.FileInfo(BarsSerializeHelper._fileVersionName).Length == 0)) ||
            (!File.Exists(BarsSerializeHelper._fileSynchronizedName) || (new System.IO.FileInfo(BarsSerializeHelper._fileSynchronizedName).Length == 0)) ||
            (!File.Exists(BarsSerializeHelper._fileDepartmentSyncName) || (new System.IO.FileInfo(BarsSerializeHelper._fileDepartmentSyncName).Length == 0)) ||
            (!File.Exists(BarsSerializeHelper._fileDefaultTestsName) || (new System.IO.FileInfo(BarsSerializeHelper._fileDefaultTestsName).Length == 0)) ||
            (!File.Exists(BarsSerializeHelper._fileDefaultBiomaterialsName) || (new System.IO.FileInfo(BarsSerializeHelper._fileDefaultBiomaterialsName).Length == 0)))
            {
                Log.WriteError(IncorrectConfiguration.Error + "Экспорт справочников работает в режиме промышленной эксплуатации (в файле dictionaryExportSettings.xml отсутствует параметр <InitialNomenclatureExport>, или он имеет значение \"false\").\r\n" +
                    "При этом один или более файлов отсутствует или повреждён: dictionariesVersion.txt, synchronizedTargets.txt, synchronizedDepartments.txt, synchronizedDefaultTests.txt, synchronizedDefaultBiomaterials.txt.\r\n" +
                    "Процедура выгрузки справочников в МИС прервана. Для проведения первичной выгрузки и формирования корректных файлов установите параметр <InitialNomenclatureExport> в true.\r\n" +
                    "Для отключения экспорта справочников закомментируйте параметр <f t=\"s\" n=\"exportDictionaryMode\" v=\"ExportDictionaries_Bars\"/> в файле settings.xml\r\n");
                return false;
            }
            try
            {
                BarsSerializeHelper.DeserializeVersions();
                BarsSerializeHelper.DeserializeSynchronizedDefaultBiomaterials();
                BarsSerializeHelper.DeserializeSynchronizedDefaultTests();
                BarsSerializeHelper.DeserializeSynchronizedDepartments();
                BarsSerializeHelper.DeserializeSynchronizedTargets();
            }
            catch (Exception ex)
            {
                Log.WriteError(IncorrectConfiguration.Error + String.Format("Ошибка в файлах синхронизации справочников: dictionariesVersion.txt, synchronizedTargets.txt,"
                + "synchronizedDepartments.txt, synchronizedDefaultTests.txt, synchronizedDefaultBiomaterials.txt. Процедура экспорта справочников прервана.\r\n{0}", ex.Message));
                return false;
            }

            return true;
        }

        private void ProcessAfterSending(List<DirectoryVersionInfo> directoryVersions, List<Nomenclature> nomenclatures, sendNomenclatureResponse response)
        {
            // Коды всех переданных исследований неудаленных
            List<string> allTargetCodesNotRemoved = nomenclatures.Where(x => x.is_group == ElementIsGroup.Target.GetHashCode() && x.action != ElementAction.Remove.GetHashCode())
                .Select(x => x.code_ta).ToList();
            List<string> allDepartmentCodesNotRemoved = nomenclatures.Where(x => x.is_group == ElementIsGroup.Department.GetHashCode() && x.action != ElementAction.Remove.GetHashCode())
                .Select(x => x.code_ta).ToList();
            List<string> allTargetsCodesRemoved = nomenclatures.Where(x => x.is_group == ElementIsGroup.Target.GetHashCode() && x.action == ElementAction.Remove.GetHashCode())
                .Select(x => x.code_ta).ToList();
            List<string> allDepartmentCodesRemoved = nomenclatures.Where(x => x.is_group == ElementIsGroup.Department.GetHashCode() && x.action == ElementAction.Remove.GetHashCode())
                .Select(x => x.code_ta).ToList();
            // Коды исследований, при экспорте которых произошли ошибки
            List<string> failedTargetCodes = response.error.Select(x => x.code_ta).ToList();
            // Коды успешно переданных исследований
            List<NomenclatureElement> successfullySynchronizedTargetCodesNotRemoved = allTargetCodesNotRemoved.Except(failedTargetCodes)
                .Select(code => new NomenclatureElement(code, LimsDictionaryNames.Target, false)).ToList();
            List<NomenclatureElement> successfullySynchronizedDepartmentCodesNotRemoved = allDepartmentCodesNotRemoved.Except(failedTargetCodes)
                .Select(code => new NomenclatureElement(code, LimsDictionaryNames.Department, false)).ToList();
            List<NomenclatureElement> successfullySynchronizedTargetCodesRemoved = allTargetsCodesRemoved.Except(failedTargetCodes)
                .Select(code => new NomenclatureElement(code, LimsDictionaryNames.Target, true)).ToList();
            List<NomenclatureElement> successfullySynchronizedDepartmentCodesRemoved = allDepartmentCodesRemoved.Except(failedTargetCodes)
                .Select(code => new NomenclatureElement(code, LimsDictionaryNames.Department, true)).ToList();

            successfullySynchronizedTargetCodesNotRemoved.FindAll(t => synchronizedTargets.Exists(s => s.Code == t.Code))
                .ForEach(t => synchronizedTargets.Find(s => s.Code == t.Code).Version = t.Version);
            successfullySynchronizedDepartmentCodesNotRemoved.FindAll(t => synchronizedDepartments.Exists(s => s.Code == t.Code))
                .ForEach(t => synchronizedDepartments.Find(s => s.Code == t.Code).Version = t.Version);
            successfullySynchronizedTargetCodesNotRemoved.FindAll(t => !synchronizedTargets.Exists(s => s.Code == t.Code)).ForEach(code => synchronizedTargets.Add(code));
            successfullySynchronizedTargetCodesRemoved.ForEach(t => synchronizedTargets.RemoveAll(s => s.Code == t.Code));
            successfullySynchronizedDepartmentCodesNotRemoved.FindAll(t => !synchronizedDepartments.Exists(s => s.Code == t.Code)).ForEach(code => synchronizedDepartments.Add(code));
            successfullySynchronizedDepartmentCodesRemoved.ForEach(t => synchronizedDepartments.RemoveAll(s => s.Code == t.Code));

            BarsSerializeHelper.SerializeVersions(directoryVersions);
            synchronizedTargets.Add(new NomenclatureElement() { Code = "default" });            
            BarsSerializeHelper.SerializeSynchronizedTargets(synchronizedTargets);
            BarsSerializeHelper.SerializeSynchronizedDepartmets(synchronizedDepartments);
            if (successfullySynchronizedTargetCodesNotRemoved.Exists(t => t.Code == "default"))
            {
                var defaultTarget = nomenclatures.Find(n => n.code_ta == "default" && n.is_group == ElementIsGroup.Target.GetHashCode());
                BarsSerializeHelper.SerializeSynchronizedDefaultTests(defaultTarget.test.Select(t => new TestWrapper(t)).ToList());
                BarsSerializeHelper.SerializeSynchronizedDefaultBiomaterials(defaultTarget.bio.Select(b => new BioWrapper(b)).ToList());
            }

            int allTargetsCount = allTargetCodesNotRemoved.Count + allTargetsCodesRemoved.Count;
            int allDepartmentsCount = allDepartmentCodesNotRemoved.Count + allDepartmentCodesRemoved.Count;

            if (response.error.Length == 0)
            {
                Log.WriteText("Синхронизация номенклатуры завершена успешно. Всего исследований: {0} и подразделений: {1}", allTargetsCount, allDepartmentsCount);
            }
            else
            {
                int allSynchronizedTargetsCount = successfullySynchronizedTargetCodesNotRemoved.Count + successfullySynchronizedTargetCodesRemoved.Count;
                int allSynchronizedDepartmentsCount = successfullySynchronizedDepartmentCodesNotRemoved.Count + successfullySynchronizedDepartmentCodesRemoved.Count;
                Log.WriteError("Синхронизация номенклатуры завершена с ошибками (см. выше). Всего исследований: {0}, Успешно синхронизировано: {1}, Ошибок(общие с подразделениями): {2}.",
                   allTargetsCount, allSynchronizedTargetsCount, failedTargetCodes.Count);
                Log.WriteError("Всего подразделений: {0}, Успешно синхронизировано: {1}, Ошибок(общие с исследованиями): {2}.", allDepartmentsCount, allSynchronizedDepartmentsCount, failedTargetCodes.Count);
            }
        }

        private bool IsDictionaryVersionsChanged(List<DirectoryVersionInfo> versions, List<DirectoryVersionInfo> directoryVersions)
        {
            foreach (DirectoryVersionInfo directoryVersion in directoryVersions)
            {
                if (requiredDictionariesNames.Contains(directoryVersion.Name))
                {
                    DirectoryVersionInfo version = versions.Find(x => x.Name == directoryVersion.Name);
                    if (version == null || version.Version != directoryVersion.Version)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void Init()
        {
            synchronizedTargets = BarsSerializeHelper.DeserializeSynchronizedTargets();
            synchronizedDepartments = BarsSerializeHelper.DeserializeSynchronizedDepartments();
            synchronizedDefaultTests = BarsSerializeHelper.DeserializeSynchronizedDefaultTests();
            synchronizedDefaultBiomaterials = BarsSerializeHelper.DeserializeSynchronizedDefaultBiomaterials();
        }

        private void CheckIfMicroTestExists()
        {
            if (ProgramContext.Dictionaries[LimsDictionaryNames.Target, helperSettings.DefaultMicroorganismTestCode] != null)
            {
                throw new Exception(String.Format("Target has already existed in Dictionary with code [{0}]. This code is our DefaultMicroTestCode", helperSettings.DefaultMicroorganismTestCode));
            }
        }

        private void RepairSynchronizationFiles(List<DirectoryVersionInfo> directoryVersions)
        {
            List<DictionaryItem> allTargets = ProgramContext.Dictionaries.GetSortedNotRemoved(LimsDictionaryNames.Target);
            List<DictionaryItem> allDepartments = ProgramContext.Dictionaries.GetSortedNotRemoved(LimsDictionaryNames.Department);
            List<DictionaryItem> allTests = ProgramContext.Dictionaries.GetSortedNotRemoved(LimsDictionaryNames.Test);
            List<DictionaryItem> allBiomaterials = ProgramContext.Dictionaries.GetSortedNotRemoved(LimsDictionaryNames.Biomaterial);

            allTargets.FindAll(t => synchronizedTargets.Exists(s => s.Code == t.Code)).ForEach(t => synchronizedTargets.Find(s => s.Code == t.Code).Version = t.Version);
            allTargets.FindAll(t => !synchronizedTargets.Exists(s => s.Code == t.Code)).ForEach(d =>
                synchronizedTargets.Add(new NomenclatureElement() { Code = d.Code, Id = d.Id, Version = d.Version }));
            synchronizedTargets.Add(new NomenclatureElement() { Code = "default" });

            TargetDictionaryItem defaultTarget = new TargetDictionaryItem() { Code = "default" };
            foreach (var test in allTests)
                defaultTarget.Tests.Add((TestDictionaryItem)test);
            foreach (var biomaterial in allBiomaterials)
                defaultTarget.Biomaterials.Add((BiomaterialDictionaryItem)biomaterial);

            allDepartments.FindAll(t => synchronizedDepartments.Exists(s => s.Code == t.Code)).ForEach(t => synchronizedDepartments.Find(s => s.Code == t.Code).Version = t.Version);
            allDepartments.FindAll(t => !synchronizedDepartments.Exists(s => s.Code == t.Code)).ForEach(d =>
                synchronizedDepartments.Add(new NomenclatureElement() { Code = d.Code, Id = d.Id, Version = d.Version }));

            BarsSerializeHelper.SerializeVersions(directoryVersions);
            BarsSerializeHelper.SerializeSynchronizedTargets(synchronizedTargets);
            BarsSerializeHelper.SerializeSynchronizedDepartmets(synchronizedDepartments);
            BarsSerializeHelper.SerializeSynchronizedDefaultTests(allTests.Select(t => new TestWrapper(new Test() { research_code = t.Code, research_name = t.Name })).ToList());
            BarsSerializeHelper.SerializeSynchronizedDefaultBiomaterials(allBiomaterials.Select(b => new BioWrapper(new Bio() { bio_code = b.Code, bio_name = b.Name })).ToList());

            Log.WriteText("Восстановление файлов синхронизации справочников завершено");
        }
    }
}