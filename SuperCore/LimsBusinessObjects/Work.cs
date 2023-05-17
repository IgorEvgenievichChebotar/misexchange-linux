using System;
using ru.novolabs.SuperCore.LimsDictionary;
using System.Collections.Generic;

public class WorkState
{
    public const Int32 Preview = 0;
    public const Int32 New = 1;
    public const Int32 Loaded = 2;
    public const Int32 Done = 3;
    public const Int32 Approved = 4;
    public const Int32 Cancelled = 5;
}

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class Work : BaseObject
    {
        public Work() {
            Test = new TestDictionaryItem();
        }
        private Int32 state;
        private Int64 timestamp;

        private List<FileInfo> images = new List<FileInfo>();
        /// <summary>
        /// Количественное/качественное значение работы(теста)
        /// </summary>
        /// 
        [CSN("Value")]
        public String Value { get; set; }

        private string m_Unit;

        /// <summary>
        /// Хранимая единица измерения
        /// </summary>
        [SendToServer(false)]
        [CSN("UnitName")]
        public String UnitName 
        { 
            get
            {
                if (string.IsNullOrEmpty(m_Unit) == false)
                    return m_Unit;
                if (StoredUnit != null && StoredUnit != "#")
                    return StoredUnit;

                if (Test.Unit != null)
                    return Test.Unit.Name;
                return "";
            }
            set
            {
                this.m_Unit = value;
            }
        }
        /// <summary>
        /// Имя единицы измерения
        /// </summary>
        [SendToServer(false)]
        [CSN("StoredUnit")]
        public String StoredUnit { get; set; }
        /// <summary>
        /// Состояние работы: 0 - Предпросмотр, 1 - Новая, 2 - Загружена, 3 - Сделана, 4 - Одобрена, 5 - Отменена
        /// </summary>
        /// 
        [CSN("State")]
        public Int32 State
        {
            get
            {
                return state;
            }

            set
            {
                state = value;
                workState = (WorkStateDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.WorkState, state);
            }
        }

        [SendToServer(false)]
        [CSN("StateName")]
        public String StateName
        {
            get
            {
                workState = (WorkStateDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.WorkState, state);
                return workState.Name;
            }
        }

        [SendToServer(false)]
        [CSN("workState")]
        public WorkStateDictionaryItem workState { get; set; }
        /// <summary>
        /// Одобривший врач
        /// </summary>
        /// 
        [CSN("ApprovingDoctor")]
        public String ApprovingDoctor { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        /// 
        [CSN("Comment")]
        public String Comment { get; set; }
        /// <summary>
        /// Ссылка на элемент справочника "Тесты"
        /// </summary>
        /// 
        [CSN("Test")]
        public TestDictionaryItem Test {
            get 
            { 
                    if (TestId > 0)
                    {
                        return (TestDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Test, TestId,skipRemoved: false);
                    }
                    return null; 
            }
            set
            {
                TestId = value.Id;
            }
        }
        /// <summary>
        /// Разведение
        /// </summary>
        /// 
        [CSN("Dilution")]
        public String Dilution { get; set; }
        /// <summary>
        /// Id теста
        /// </summary>
        [SendToServer(false)]
        [CSN("TestId")]
        public Int32 TestId { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        /// 
        [CSN("NormComment")]
        public String NormComment { get; set; }
        /// <summary>
        /// Фактическая дата завершения работы
        /// </summary>
        /// 
        [CSN("EndDate")]
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Максимальная дата завершения работы
        /// </summary>
        /// 
        [CSN("PlannedDate")]
        public DateTime? PlannedDate { get; set; }
        /// <summary>
        /// Цвет ячейки для веб-клиента
        /// </summary>
        /// 
        [CSN("Color")]
        public String Color { get; set; }


        [CSN("Timestamp")]
        public Int64 Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        /// <summary>
        /// Список изображений
        /// </summary>
        [CSN("Images")]
        public List<FileInfo> Images
        {
            get { return images; }
            set { images = value; }
        }

        [SendToServer(false)]
        [CSN("FormattedValue")]
        public String FormattedValue
        {
            get
            {
                TestDictionaryItem test = (TestDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Test, Test.Id];
                if (test != null)
                {
                    switch (test.ResultType)
                    {
                        case TestTypes.ENUM:
                            return Value;
                        case TestTypes.VALUE:
                            return Value;
                        case TestTypes.ANTIBIOTIC:
                            return Value;
                    }
                }
                return "";
            }
        }

        private String GetFormattedValue()
        {
            return "";
        }

        [CSN("StoredNorms")]
        public String StoredNorms { get; set; }

        [CSN("StoredNormName")]
        public String StoredNormName { get; set; }

        [SendToServer(false)]
        [CSN("Norms")]
        public String Norms
        { 
            get
            {
                if (!String.IsNullOrEmpty(StoredNormName))
                    return StoredNormName;
                return StoredNorms;
            } 
        }

        //Ссылка на микроорганизм в случае антибиотика
        [CSN("Microorganism")]
        public MicroOrganismDictionaryItem Microorganism { get; set; }
   
    }
}
