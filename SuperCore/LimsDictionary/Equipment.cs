using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

public class EquipmentSaveAlgorithm
{
    public const int Worklist = 1;
    public const int SampleNumber = 2;
    public const int SampleNumberInWorklist = 3;
    public const int ClientCards = 4;
    public const int ClientRequest = 5;
}

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class TestMapValue
    {
        [CSN("Test")]
        public TestDictionaryItem Test { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("QcTestCode")]
        public String QcTestCode { get; set; }
    }

    public class MicroOrganismMapValue
    {
        [CSN("Microorganism")]
        public MicroOrganismDictionaryItem Microorganism { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
    }

    [OldSaveMethod]
    public class EquipmentDictionaryItem : DriverDictionaryItem
    {
        public EquipmentDictionaryItem()
        {
            TestMappings = new List<TestMapValue>();
            MicroorganismMappings = new List<MicroOrganismMapValue>();
            Departments = new List<DepartmentDictionaryItem>();
            Biomaterials = new List<BiomaterialDictionaryItem>();
        }

        /// <summary>
        /// Алгоритм поиска пробы
        /// </summary>
        /// 
        [CSN("SaveAlgorithm")]
        public Int32 SaveAlgorithm { get; set; }
        /// <summary>
        /// Признак использования внутреннего номера в подразделении
        /// </summary>
        /// 
        [CSN("UseDepartmentNr")]
        public bool UseDepartmentNr { get; set; }
        /// <summary>
        /// Признак использования маппинга микроорганизмов
        /// </summary>
        /// 
        [CSN("UseMicroorganismMappings")]
        public bool UseMicroorganismMappings { get; set; }
        /// <summary>
        /// Передавать позицию как координаты в штативе
        /// </summary>
        /// 
        [CSN("SendPositionAsCoordinates")]
        public bool SendPositionAsCoordinates { get; set; }
        /// <summary>
        /// Копировать дату сортировки в дату доставки биоматериала
        /// </summary>
        /// 
        [CSN("CopySortingDateToSampleDeliveryDate")]
        public bool CopySortingDateToSampleDeliveryDate { get; set; }
        /// <summary>
        /// Добавлять работы в статусе "Сделана"
        /// </summary>
        /// 
        [CSN("AddWorksDone")]
        public bool AddWorksDone { get; set; }
        /// <summary>
        /// Добавлять работы в статусе "Одобрена"
        /// </summary>
        /// 
        [CSN("AddWorksApproved")]
        public bool AddWorksApproved { get; set; }
        /// <summary>
        /// Коды тестов на анализаторе
        /// </summary>
        /// 
        [CSN("TestMappings")]
        public List<TestMapValue> TestMappings { get; set; }
        /// <summary>
        /// Коды микроорганизмов на анализаторе
        /// </summary>
        /// 
        [CSN("MicroorganismMappings")]
        public List<MicroOrganismMapValue> MicroorganismMappings { get; set; }
        /// <summary>
        /// Список подразделений
        /// </summary>
        [SendAsRef(true)]
        [CSN("Departments")]
        public List<DepartmentDictionaryItem> Departments { get; set; }
        /// <summary>
        /// Возвращает код теста в анализаторе
        /// </summary>
        public String GetTestCode(TestDictionaryItem test)
        {
            TestMapValue mapValue = TestMappings.Find(item => item.Test.Equals(test));
            return (mapValue != null) ? mapValue.Code : null;
        }

        /// <summary>
        /// Признак dotNet драйвера. Создание и настройка свойств производится только в "Сервере драйверов"
        /// </summary>
        /// 
        [CSN("RemoteDriver")]
        public bool RemoteDriver { get; set; }
        /// <summary>
        /// Настройки маппинга
        /// </summary>
        /// 
        [CSN("MappingValues")]
        public String MappingValues { get; set; }
        /// <summary>
        /// Синхронизация с толстым клиентом ЛИС
        /// </summary>
        ///
        [CSN("LotCount")]
        public Int32 LotCount { get; set; }
        [CSN("PositionCount")]
        public Int32 PositionCount { get; set; }
        [CSN("LotNumeringType")]
        public Int32 LotNumeringType { get; set; }
        [CSN("PositionNumeringType")]
        public Int32 PositionNumeringType { get; set; }
        [CSN("ResultsMode")]
        public Int32 ResultsMode { get; set; }
        [CSN("QueryMode")]
        public Int32 QueryMode { get; set; }
        [CSN("RequestForm")]
        public ObjectRef RequestForm { get; set; }

        [CSN("NeedReverseProcess")]
        public bool NeedReverseProcess { get; set; }
        [CSN("AutoWorkAdd")]
        public bool AutoWorkAdd { get; set; }
        [CSN("AllowLotNr")]
        public bool AllowLotNr { get; set; }
        [CSN("SkipShowInProcessView")]
        public bool SkipShowInProcessView { get; set; }
        //[CSN("IsMicroplate")]
        //public bool IsMicroplate { get; set; }
        [CSN("AutoChangeWorkStateOnQuery")]
        public bool AutoChangeWorkStateOnQuery { get; set; }

        /// <summary>
        /// Список биоматериалов
        /// </summary>
        [SendAsRef(true)]
        [CSN("Biomaterials")]
        public List<BiomaterialDictionaryItem> Biomaterials { get; set; }

        //FTests: TlisIdList;
        //FPipettedRacks: TlisEquipmentRackSet;

        [CSN("AllowAutoWorklists")]
        public bool AllowAutoWorklists { get; set; }

        [CSN("AllowMicrobyologyAutoWorklists")]
        public bool AllowMicrobyologyAutoWorklists { get; set; }

        /// <summary>
        /// Отправлять пробы только с внутренними номерами
        /// </summary>
        /// 
        [CSN("SendSamplesWithDepartmentNrOnly")]
        public bool SendSamplesWithDepartmentNrOnly { get; set; }
        /// <summary>
        /// Отправлять повторно перенумерованные пробы
        /// </summary>
        /// 
        [CSN("ResendRenumberedSamples")]
        public bool ResendRenumberedSamples { get; set; }
    }

    /// <summary>
    /// Представляет справочник "Анализаторы"
    /// </summary>
    public class EquipmentDictionary : DictionaryClass<EquipmentDictionaryItem>
    {
        public EquipmentDictionary(String dictionaryName) : base(dictionaryName) { }

        [CSN("Equipment")]
        public List<EquipmentDictionaryItem> Equipment
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
