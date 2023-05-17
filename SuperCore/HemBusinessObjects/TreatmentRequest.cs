using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class TreatmentRequest : TreatmentRequestShort
    {
        public TreatmentRequest()
        {
            RequestedDate = DateTime.Now;
            LabParamValues = new List<ParameterValue>();
            PhysioIndicatorValues = new List<ParameterValue>();
        }
        /// <summary>
        /// Ссылка на пациента
        /// </summary>
        [SendAsRef(true)]
        [CSN("Patient")]
        public Donor Patient { get; set; }

        [CSN("LabParamValues")]
        public List<ParameterValue> LabParamValues { get; set; }

        [CSN("PhysioIndicatorValues")]
        public List<ParameterValue> PhysioIndicatorValues { get; set; }

        /// <summary>
        /// Ссылка на справочник "Диагнозы" BLOOD-436
        /// </summary>
        [CSN("Diagnosis")]
        public DiagnosisDictionaryItem Diagnosis { get; set; }
    }

    public class TreatmentRequestSaveResult
    {
        public TreatmentRequestSaveResult()
        {
            Errors = new List<ErrorMessage>();
        }

        [CSN("Id")]
        public int Id { get; set; }

        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
    }

    public class TreatmentRequestJournalRow : TreatmentRequestShort
    {
        
    }

    public class TreatmentRequestShort : BaseObject
    {
        /// <summary>
        /// Номер заявки
        /// </summary>
        [CSN("Nr")]
        public String Nr { get; set; }

        /// <summary>
        /// Имя пациента
        /// </summary>
        [CSN("PatientFirstName")]
        public String PatientFirstName { get; set; }

        /// <summary>
        /// Фамилия пациента
        /// </summary>
        [CSN("PatientLastName")]
        public String PatientLastName { get; set; }

        /// <summary>
        /// Отчество пациента
        /// </summary>
        [CSN("PatientMiddleName")]
        public String PatientMiddleName { get; set; }

        /// <summary>
        /// Отделение ЛПУ/подразделение
        /// </summary>
        [SendToServer(false)]
        [CSN("HospitalDepartment")] 
        public DepartmentDictionaryItem HospitalDepartment { get; set; }

        /// <summary>
        /// Лечащий врач
        /// </summary>
        [SendToServer(false)] 
        [CSN("Doctor")]
        public EmployeeDictionaryItem Doctor { get; set; }
        
        /// <summary>
        /// Параметры крови
        /// </summary>
        [CSN("BloodParameters")]
        public List<BloodParameterValue> BloodParameters { get; set; }

        /// <summary>
        /// Ссылка на справочник "Шаблоны заявок лечебных процедур"
        /// </summary>
        [CSN("TreatmentRequestTemplate")]
        public TreatmentRequestTemplateDictionaryItem TreatmentRequestTemplate { get; set; }

        /// <summary>
        /// Ссылка на справочник "Шаблоны лечебных процедур"
        /// </summary>
        [CSN("TreatmentTemplate")]
        public TreatmentTemplateDictionaryItem TreatmentTemplate { get; set; }

        /// <summary>
        /// Ссылка на объект "Лечебная процедура"
        /// </summary>
        [SendAsRef(true)]
        [CSN("Treatment")]
        public Treatment Treatment { get; set; }

        /// <summary>
        /// Дата регистрации заявки в системе
        /// </summary>
        [SendToServer(false)]
        [CSN("CreationDate")]
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Дата, запрошенная лечащим врачом
        /// </summary>
        [CSN("RequestedDate")]
        public DateTime? RequestedDate { get; set; }

        /// <summary>
        /// Дата, запланированная трансфузиологом
        /// </summary>
        [CSN("PlannedDate")]
        public DateTime? PlannedDate { get; set; }

        /// <summary>
        /// Запрошенный объем
        /// </summary>
        [CSN("Volume")]
        public Int32 Volume { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        [CSN("Unit")]
        public Int32 Unit { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CSN("Comment")]
        public String Comment { get; set; }

        /// <summary>
        /// Тип венозного доступа
        /// </summary>
        [CSN("VeinAccessType")]
        public VeinAccessTypeDictionaryItem VeinAccessType { get; set; }

        /// <summary>
        /// Статус заявки (0 - новая, 1 - принята, 2 - обработана, 3 - отменена)
        /// </summary>
        [CSN("Status")]
        public Int32 Status { get; set; }

    }
}
