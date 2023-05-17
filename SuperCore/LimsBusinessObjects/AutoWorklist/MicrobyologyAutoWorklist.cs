using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    // DRV-426
    public class MicrobyologyAutoWorklist : BaseObject
    {
        public MicrobyologyAutoWorklist()
        {
            Orders = new List<MicrobyologyAutoWorklistOrder>();
        }

        /// <summary>
        /// ссылка на анализатор
        /// </summary>
        [CSN("Equipment")]
        public EquipmentDictionaryItem Equipment { get; set; }
        /// <summary>
        /// ссылка на заявку
        /// </summary>
        [CSN("Request")]
        public ObjectRef Request { get; set; }
        /// <summary>
        /// номер заявки
        /// </summary>
        [CSN("RequestNumber")]
        public String RequestNumber { get; set; }
        /// <summary>
        /// статус микробиологического автоматического рабочего списка: 0 - новый, 10 - изменён, 20 - отправлен в анализатор, 21 - изменён после отправки в анализатор
        /// </summary>
        [CSN("State")]
        public Int32 State { get; set; }
        /// <summary>
        /// заказы микроорганизмов
        /// </summary>
        [CSN("Orders")]
        public List<MicrobyologyAutoWorklistOrder> Orders { get; set; }
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
        /// <summary>
        /// дата создания объекта
        /// </summary>
        [CSN("CreationDate")]
        public DateTime? CreationDate { get; set; }
        /// <summary>
        /// дата последнего обновления объекта
        /// </summary>
        [CSN("LastUpdate")]
        public DateTime? LastUpdate { get; set; }
    }

    public class MicrobyologyAutoWorklistOrder
    {
        public MicrobyologyAutoWorklistOrder()
        {
            Microorganisms = new List<AutoWorklistOrderMicroorganism>();
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
        /// микроорганизмы
        /// </summary>
        [CSN("Microorganisms")]
        public List<AutoWorklistOrderMicroorganism> Microorganisms { get; set; }
    }

    public class AutoWorklistOrderMicroorganism
    {
        /// <summary>
        /// ссылка на справочник "Микроорганизмы"
        /// </summary>
        [CSN("Microorganism")]
        public MicroOrganismDictionaryItem Microorganism { get; set; }
        /// <summary>
        /// код теста в анализаторе
        /// </summary>
        [CSN("CodeInAnalyzer")]
        public String CodeInAnalyzer { get; set; }
        /// <summary>
        /// Статус микроорганизма: 0 - новый, 10 - изменён, 20 - отправлен в анализатор, 21 - изменён после отправки в анализатор
        /// </summary>
        [CSN("State")]
        public int State { get; set; }
    }
}