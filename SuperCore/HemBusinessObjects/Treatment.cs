using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class Treatment : BaseObject
    {
        public Treatment()
        {
            LabParamsObservations = new List<IndicatorsObservation>();
            PhysioIndicatorsObservations = new List<IndicatorsObservation>();
            TechParamsObservations = new List<IndicatorsObservation>();
            LiquidBalanceParamsObservations = new List<IndicatorsObservation>();
            TotalVolumeObservations = new List<IndicatorsObservation>();
            InputProducts = new List<ProductDescription>();
            OutputProducts = new List<ProductDescription>();
            AllergicReactions = new List<AllergicReaction>(); 
            AdditionalVolumes = new List<AdditionalVolume>();
            Complications = new List<Complication>();
        }

        /// <summary>
        /// Номер лечебной процедуры
        /// </summary>
        [SendToServer(false)]
        [CSN("Nr")]
        public string Nr { get; set; }

        /// <summary>
        /// Ссылка на пациента
        /// </summary>
        [SendToServer(false)]
        [SendAsRef(true)]
        [CSN("Patient")]
        public Donor Patient { get; set; }

        /// <summary>
        /// Ссылка на персону, для которой предназначаются выходные продукты ЛП
        /// </summary>
        [SendAsRef(true)]
        [CSN("TargetPerson")]
        public Donor TargetPerson { get; set; }

        /// <summary>
        /// ФИО персоны, для которой предназначаются выходные продукты ЛП
        /// </summary>
        [SendToServer(false)]
        [CSN("TargetPersonFullName")]
        public string TargetPersonFullName { get; set; }
        
        /// <summary>
        /// Статус процедуры (1 - требуется подбор продуктов, 5 - готова к выполнению, 10 - начата, 15 - отменена, 20 - завершена)
        /// </summary>
        [SendToServer(false)]
        [CSN("Status")]
        public int Status { get; set; }

        [SendAsRef(true)]
        [CSN("Department")]
        public DepartmentDictionaryItem Department { get; set; }

        [SendAsRef(true)]
        [CSN("TransfusionRequests")]
        public TransfusionRequest TransfusionRequests { get; set; }

        /// <summary>
        /// Ссылка на справочник "Типы венозного доступа" BLOOD-417
        /// </summary>
        [SendAsRef(true)]
        [CSN("VeinAccessType")]
        public VeinAccessTypeDictionaryItem VeinAccessType { get; set; }

        /// <summary>
        /// Ссылка на справочник "Диагнозы" BLOOD-436
        /// </summary>
        [SendAsRef(true)]
        [CSN("Diagnosis")] 
        public DiagnosisDictionaryItem Diagnosis { get; set; }
 
        /// <summary>
        /// Ссылка на аппарат, используемый в ходе процедуры BLOOD-425
        /// </summary>
        [SendAsRef(true)]
        [CSN("Apparatus")] 
        public ApparatusDictionaryItem Apparatus { get; set; }
  
        /// <summary>
        /// Ссылка на справочник "Операционные блоки" BLOOD-437
        /// </summary>
        [SendAsRef(true)]
        [CSN("OperatingRoom")]
        public OperatingRoomDictionaryItem OperatingRoom { get; set; }

        [SendToServer(false)]
        [CSN("TreatmentTemplate")]
        public TreatmentTemplateDictionaryItem TreatmentTemplate { get; set; }

        [SendToServer(false)]
        [CSN("TreatmentRequest")]
        public TreatmentRequest TreatmentRequest { get; set; }

        /// <summary>
        /// Множество объектов, описывающих входящие продукты из заявки на трансфузию (если она есть, согласно шаблону лечебной процедуры). Список только для чтения. Создается в момент приемки продуктов по связанной заявке на трансфузию. 
        /// </summary>
        [SendToServer(false)]
        [CSN("InputProducts")]
        public List<ProductDescription> InputProducts { get; set; }

        /// <summary>
        /// Множество объектов, описывающих выходящие продукты (если они есть, согласно шаблону лечебной процедуры). Список редактируемый. Ссылка на реальный продукт появляется только после завершения лечебной процедуры. 
        /// </summary>
        [CSN("OutputProducts")]
        public List<ProductDescription> OutputProducts { get; set; }

        /// <summary>
        /// Наблюдения физиологических параметров. Множество объектов типа IndicatorsObservation 
        /// </summary>
        [CSN("PhysioIndicatorsObservations")]
        public List<IndicatorsObservation> PhysioIndicatorsObservations { get; set; }

        /// <summary>
        /// Наблюдения лабораторных параметров. Множество объектов типа IndicatorsObservation 
        /// </summary>
        [CSN("LabParamsObservations")]
        public List<IndicatorsObservation> LabParamsObservations { get; set; }

        /// <summary>
        /// Наблюдения технических/технологических параметров. Множество объектов типа IndicatorsObservation 
        /// </summary>
        [CSN("TechParamsObservations")]
        public List<IndicatorsObservation> TechParamsObservations { get; set; }

        /// <summary>
        /// Наблюдения параметров баланса жидкостей. Множество объектов типа IndicatorsObservation 
        /// </summary>
        [CSN("LiquidBalanceParamsObservations")]
        public List<IndicatorsObservation> LiquidBalanceParamsObservations { get; set; }

        /// <summary>
        /// Значения финальных и удалённых(извлечённых) объемов. Множество объектов типа IndicatorsObservation. Коллекция должна иметь только один элемент(не давать возможность добавлять наблюдения в интерфейсе) 
        /// </summary>
        [CSN("TotalVolumeObservations")]
        public List<IndicatorsObservation> TotalVolumeObservations { get; set; } 
        
        /// <summary>
        /// Mножество аллегических реакций 
        /// </summary>
        [CSN("AllergicReactions")]
        public List<AllergicReaction> AllergicReactions { get; set; }

        /// <summary>
        /// Дополнительные компоненты 
        /// </summary>
        [CSN("AdditionalVolumes")]
        public List<AdditionalVolume> AdditionalVolumes { get; set; } 
        
        /// <summary>
        /// Осложнения 
        /// </summary>
        [CSN("Complications")]
        public List<Complication> Complications { get; set; }
    }

    public class Complication : BaseObject
    {
        public Complication()
        {
            TherapyTypes = new List<TherapyTypeDictionaryItem>();             
        }

        /// <summary>
        /// Ссылка на справочник "Виды осложнений" BLOOD-438
        /// </summary>
        [SendAsRef(true)]
        [CSN("ComplicationType")]
        public ComplicationTypeDictionaryItem ComplicationType { get; set; }

        /// <summary>
        /// Ссылка на справочник "Причины осложнений" BLOOD-439
        /// </summary>
        [SendAsRef(true)]
        [CSN("ComplicationReason")]
        public ComplicationReasonDictionaryItem ComplicationReason { get; set; }
 
        /// <summary>
        /// Множество ссылок на справочник "Виды терапии" BLOOD-440
        /// </summary>
        [SendAsRef(true)]
        [CSN("TherapyTypes")]
        public List<TherapyTypeDictionaryItem> TherapyTypes { get; set; } 
 
        [CSN("Comment")]
        public string Comment { get; set; }
    }
     

    public class AdditionalVolume : BaseObject
    {
        /// <summary>
        /// Ссылка на справочник дополнительных компонентов
        /// </summary>
        [SendAsRef(true)]
        [CSN("AdditionalComponent")]
        public AdditionalComponentDictionaryItem AdditionalComponent { get; set; }

        /// <summary>
        /// Объём компонента
        /// </summary>
        [CSN("Volume")]
        public float? Volume { get; set; }
 
        /// <summary>
        /// Номер компонента
        /// </summary>
        [CSN("Nr")]
        public string Nr { get; set; }
  
        /// <summary>
        /// Дата производства
        /// </summary>
        [CSN("ProductionDate")]
        public DateTime? ProductionDate { get; set; } 

        /// <summary>
        /// Ссылка на справочник "Производители"
        /// </summary>
        [SendAsRef(true)]
        [CSN("Manufacturer")]
        public ManufacturerDictionaryItem Manufacturer { get; set; }
  
        /// <summary>
        /// Биологическая проба. Значение статического справочника да/нет/не важно
        /// </summary>
        [CSN("Bioassay")]
        public int Bioassay { get; set; }
    }

    public class AllergicReaction : BaseObject
    {
        /// <summary>
        /// Ссылка на справочник "Типы аллергических реакций" BLOOD-444
        /// </summary>
        [SendAsRef(true)]
        [CSN("AllergicReactionType")]
        public AllergicReactionTypeDictionaryItem AllergicReactionType { get; set; }
  
        [CSN("Comment")]
        public string Comment { get; set; }
    }

    public class IndicatorsObservation : BaseObject
    {
        public IndicatorsObservation()
        {
            IndicatorValues = new List<ParameterValue>();
        }

        /// <summary>
        /// Время наблюдения, автофиксируется при событии observation-save (нередактируемое)
        /// </summary>
        [CSN("Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Ссылка на шаблон наблюдения (ObservationTemplate) из шаблона трансфузии, к которому относится этот набор показателей
        /// </summary>
        [CSN("Template")]
        public ObservationTemplate Template { get; set; }

        /// <summary>
        /// Множество значений измерений показателей. Коллекция объектов ParameterValue
        /// </summary>
        [CSN("IndicatorValues")]
        public List<ParameterValue> IndicatorValues { get; set; }

        /// <summary>
        /// Ссылка на заявку в ЛИС
        /// </summary>
        [CSN("Request")]
        public int Request { get; set; }

        [CSN("Name")]
        public string Name { get; set; }        
    }

    public class ProductDescription : BaseObject
    {
        /// <summary>
        /// Ссылка на тип продукта
        /// </summary>
        [CSN("ProductType")]
        public ProductTypeDictionaryItem ProductType { get; set; }
        
        /// <summary>
        /// Ссылка на продукт
        /// </summary>
        [CSN("Product")]
        public ObjectRef Product { get; set; }

        /// <summary>
        /// Номер продукта
        /// </summary>
        [CSN("ProductNr")]
        public string ProductNr { get; set; }
        
        /// <summary>
        /// Ссылка на номенклатуру продукта
        /// </summary>
        [CSN("Classification")]
        public int Classification { get; set; }

        /// <summary>
        /// Кол-во клеток
        /// </summary>
        [CSN("CellCount")]
        public long CellCount { get; set; }

        /// <summary>
        /// Кол-во клеток в дозе
        /// </summary>
        [CSN("CellsInDose")]
        public long CellsInDose { get; set; }

        /// <summary>
        /// Параметры крови
        /// </summary>
        [CSN("BloodParameters")]
        public List<BloodParameterValue> BloodParameters { get; set; }

        /// <summary>
        /// Кол-во доз
        /// </summary>
        [CSN("DoseCount")]
        public int DoseCount { get; set; }

        [CSN("DoseMode")]
        public int DoseMode { get; set; }

        /// <summary>
        /// Объём продукта
        /// </summary>
        [CSN("Volume")]
        public int Volume { get; set; }
    }

    public class TreatmentSaveResult : TreatmentCreateResult
    {
        //
    }
    
    public class TreatmentCreateResult
    {
        public TreatmentCreateResult()
        {
            Errors = new List<ErrorMessage>();
        }

        [CSN("Id")]
        public int Id { get; set; }

        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
    }

    public class TreatmentJournalRow : TreatmentShort
    {

    }

    public class TreatmentShort : BaseObject
    {
        /// <summary>
        /// Номер лечебной процедуры
        /// </summary>
        [CSN("Nr")]
        public String Nr { get; set; }

        /// <summary>
        /// Имя пациента
        /// </summary>
        [CSN("PatientFirstName")]
        public string PatientFirstName { get; set; }

        /// <summary>
        /// Фамилия пациента
        /// </summary>
        [CSN("PatientLastName")]
        public string PatientLastName { get; set; }

        /// <summary>
        /// Отчество пациента
        /// </summary>
        [CSN("PatientMiddleName")]
        public string PatientMiddleName { get; set; }

        /// <summary>
        /// Подразделение
        /// </summary>
        [CSN("Department")]
        public DepartmentDictionaryItem Department { get; set; }

        /// <summary>
        /// Дата регистрации заявки в системе
        /// </summary>
        [SendToServer(false)]
        [CSN("CreationDate")]
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Дата начала процедуры
        /// </summary>
        [CSN("StartDate")]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата завершения процедуры
        /// </summary>
        [CSN("FinishDate")]
        public DateTime? FinishDate { get; set; }

        /// <summary>
        /// Параметры крови
        /// </summary>
        [CSN("BloodParameters")]
        public List<BloodParameterValue> BloodParameters { get; set; }

        /// <summary>
        /// Ссылка на справочник "Шаблоны лечебных процедур"
        /// </summary>
        [CSN("TreatmentTemplate")]
        public TreatmentTemplateDictionaryItem TreatmentTemplate { get; set; }

        /// <summary>
        /// Ссылка на справочник "Операционные блоки"
        /// </summary>
        [CSN("OperatingRoom")]
        public OperatingRoomDictionaryItem OperatingRoom { get; set; }

        /// <summary>
        /// Ссылка на врача-трансфузиолога
        /// </summary>
        [CSN("Transfusiologist")]
        public EmployeeDictionaryItem Transfusiologist { get; set; }

        /// <summary>
        /// Ссылка на справочник "Диагнозы"
        /// </summary>
        [CSN("Diagnosis")]
        public DiagnosisDictionaryItem Diagnosis { get; set; }

        /// <summary>
        /// Тип венозного доступа
        /// </summary>
        [CSN("VeinAccessType")]
        public VeinAccessTypeDictionaryItem VeinAccessType { get; set; }

        /// <summary>
        /// Статус процедуры (1 - требуется подбор продуктов, 5 - готова к выполнению, 10 - начата, 15 - отменена, 20 - завершена)
        /// </summary>
        [CSN("Status")]
        public Int32 Status { get; set; }

    }
}
