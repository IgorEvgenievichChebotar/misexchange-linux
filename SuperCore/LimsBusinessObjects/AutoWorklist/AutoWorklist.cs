using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public enum AutoWorklistStates : int
    {
        New = 0,
        Changed = 10,
        SentToAnalyzer = 20,
        ChangedAfterSendToAnalyzer = 21
    }

    public enum AutoWorklistTestStates : int
    {
        New = 0,
        Changed = 10,
        SentToAnalyzer = 20,
        ChangedAfterSendToAnalyzer = 21
    }

    // LIS-7139
    public class AutoWorklist : BaseObject
    {
        public AutoWorklist()
        {
            Orders = new List<AutoWorklistOrder>();
        }

        /// <summary>
        /// ссылка на анализатор
        /// </summary>
        [CSN("Equipment")]
        public EquipmentDictionaryItem Equipment { get; set; }
        /// <summary>
        /// ссылка на заявку. (Указывается в случае, если авто-рабочий список создавался по заявке - в запросе create-requests-xxx)
        /// </summary>
        [CSN("Request")]
        public ObjectRef Request { get; set; }
        /// <summary>
        /// номер заявки. (вычисляется и хранится, если указана request (ref)).
        /// </summary>
        [CSN("RequestNumber")]
        public String RequestNumber { get; set; }
        /// <summary>
        /// ссылка на пробу. (Указывается в случае, если авто-рабочий список создавался по пробе, а не по заявке - из карты/журнала проб)
        /// </summary>
        [CSN("Sample")]
        public ObjectRef Sample { get; set; }
        /// <summary>
        /// номер пробы (вычисляется и хранится, если указана sample (ref)).
        /// </summary>
        [CSN("SampleNumber")]
        public String SampleNumber { get; set; }
        /// <summary>
        /// статус автоматического рабочего списка: 0 - новый, 10 - изменён, 20 - отправлен в анализатор, 21 - изменён после отправки в анализатор
        /// </summary>
        [CSN("State")]
        public Int32 State { get; set; }
        /// <summary>
        /// заказы тестов
        /// </summary>
        [CSN("Orders")]
        public List<AutoWorklistOrder> Orders { get; set; }

        // DRV-365
        /// <summary>
        /// фамилия пациента
        /// </summary>
        [CSN("LastName")]
        public String LastName { get; set; }
        /// <summary>
        /// имя пациента
        /// </summary>
        [CSN("FirstName")]
        public String FirstName { get; set; }
        /// <summary>
        /// отчество пациента
        /// </summary>
        [CSN("MiddleName")]
        public String MiddleName { get; set; }
        /// <summary>
        /// дата рождения пациента
        /// </summary>
        [CSN("BirthDate")]
        public DateTime BirthDate { get; set; }
        /// <summary>
        /// пол пациента
        /// </summary>
        [CSN("Sex")]
        public int Sex { get; set; }
    }

    public class AutoWorklistOrder
    {
        public AutoWorklistOrder()
        {
            Tests = new List<AutoWorklistOrderTest>();
        }

        /// <summary>
        /// ссылка на пробу
        /// </summary>
        [CSN("Sample")]
        public ObjectRef Sample { get; set; }
        /// <summary>
        /// ссылка на пациента
        /// </summary>
        [CSN("Patient")]
        public ObjectRef Patient { get; set; }
        /// <summary>
        /// номер пробы
        /// </summary>
        [CSN("SampleNumber")]
        public String SampleNumber { get; set; }
        /// <summary>
        /// внутренний номер пробы
        /// </summary>
        [CSN("SampleDepartmentNumber")]
        public String SampleDepartmentNumber { get; set; }
        /// <summary>
        /// код биоматериала пробы
        /// </summary>
        [CSN("BiomaterialCode")]
        public String BiomaterialCode { get; set; }
        /// <summary>
        /// признак срочности
        /// </summary>
        [CSN("Cito")]
        public Boolean Cito { get; set; }
        /// <summary>
        /// тесты
        /// </summary>
        [CSN("Tests")]
        public List<AutoWorklistOrderTest> Tests { get; set; }
    }

    public class AutoWorklistOrderTest
    {
        /// <summary>
        /// ссылка на справочник "Тесты"
        /// </summary>
        [CSN("Test")]
        public TestDictionaryItem Test { get; set; }
        /// <summary>
        /// код теста в анализаторе
        /// </summary>
        [CSN("CodeInAnalyzer")]
        public String CodeInAnalyzer { get; set; }
        /// <summary>
        /// признак того, что тест необходимо отменить
        /// </summary>
        [CSN("Cancelled")]
        public Boolean Cancelled { get; set; }
        /// <summary>
        /// Статус теста: 0 - новый, 10 - изменён, 20 - отправлен в анализатор, 21 - изменён после отправки в анализатор
        /// </summary>
        [CSN("State")]
        public int State { get; set; }
    }
}