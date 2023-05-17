using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class ArchiveRackSaveRequestParams : BaseObject
    {
        [CSN("Comment")]
        public String Comment { get; set; }
        [CSN("Barcode")]
        public String Barcode { get; set; }
        [CSN("ArchiveStorage")]
        public ArchiveStorageDictionaryItem ArchiveStorage { get; set; }
    }

    public class ArchiveRackCreateRequestParams
    {
        [CSN("Barcode")]
        public String Barcode { get; set; }
        [CSN("ArchiveRackType")]
        public ArchiveRackTypeDictionaryItem ArchiveRackType { get; set; }
        [CSN("StorageRule")]
        public StorageRuleDictionaryItem StorageRule { get; set; }
        // статический справочник "Направление заполнения штатива". 1 - слева-направо, сверху-вниз. 2 - сверху-вниз, слева-направо. 
        [CSN("FillingDirection")]
        public int? FillingDirection { get; set; }
        // тип нумерации горизонтальной шкалы. 1 - числовая, 2 - буквенная
        [CSN("HorizontalAxisNumerationType")]
        public int? HorizontalAxisNumerationType { get; set; }
        // тип нумерации вертикальной шкалы. 1 - числовая, 2 - буквенная
        [CSN("VerticalAxisNumerationType")]
        public int? VerticalAxisNumerationType { get; set; }
    }  
    
    public class ArchiveRackSlot : BaseObject
    {
        /// <summary>
        /// X координата слота в штативе 
        /// </summary>
        [CSN("X")]
        public int X { get; set; }
        /// <summary>
        /// Y координата слота в штативе
        /// </summary>
        [CSN("Y")]
        public int Y { get; set; }
        /// <summary>
        /// Дата помещения биоматериала на хранение
        /// </summary>
        [CSN("Date")]
        [SendToServer(false)]
        public DateTime Date { get; set; }
        /// <summary>
        /// Дата окончания хранения (допустимо null) 
        /// </summary>
        [CSN("ExpireDate")]
        public DateTime? ExpireDate { get; set; }
        /// <summary>
        /// Ссылка на заявку
        /// </summary>
        [CSN("Request")]
        public ObjectRef Request { get; set; }
        /// <summary>
        /// Ссылка на пробу
        /// </summary>
        [CSN("Sample")]
        public ObjectRef Sample { get; set; }
        /// <summary>
        /// Номер пробирки
        /// </summary>
        [CSN("TubeNr")]
        public String TubeNr { get; set; }
        /// <summary>
        /// Фамилия пациента
        /// </summary>
        [SendToServer(false)]
        [CSN("PatientLastName")]
        public String PatientLastName { get; set; }
        /// <summary>
        /// Имя пациента
        /// </summary>
        [SendToServer(false)]
        [CSN("PatientFirstName")]
        public String PatientFirstName { get; set; }
        /// <summary>
        /// Отчество пациента
        /// </summary>
        [SendToServer(false)]
        [CSN("PatientMiddleName")]
        public String PatientMiddleName { get; set; }

        /// <summary>
        /// Ссылка на штатив, которому принадлежит слот
        /// </summary>
        [CSN("Rack")]
        [SendToServer(false)]
        public ArchiveRack Rack { get; set; }

        /// <summary>
        /// Ссылка на биоматериал
        /// </summary>
        [CSN("Biomaterial")]
        public BiomaterialDictionaryItem Biomaterial { get; set; }
        /// <summary>
        /// Ссылка на подразделение
        /// </summary>
        [CSN("Department")]
        public DepartmentDictionaryItem Department { get; set; }

        #region CalculatedProperties

        /// <summary>
        /// Отображаемые пользователю координаты слота
        /// </summary>
        [CSN("Position")]
        [SendToServer(false)]
        public String Position
        {
            get { return String.Format("({0},{1})", this.X + 1, this.Y + 1); }
        }

        /// <summary>
        /// Отображаемый пользователю порядковый номер слота (с учётом направления заполнения штатива)
        /// </summary>
        [CSN("SequenceNumber")]
        [SendToServer(false)]
        public String SequenceNumber
        {
            get
            {
                string result = String.Empty;
                if (Rack != null)
                {
                    switch (Rack.FillingDirection)
                    {
                        case (int)FillingDirection.LeftToRightUpToDown:
                            result = (this.Y * Rack.Width + this.X + 1).ToString();
                            break;
                        default:
                            result = (this.X * Rack.Height + this.Y + 1).ToString();
                            break;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Фамилия + инициалы
        /// </summary>
        [SendToServer(false)]
        [CSN("PatientShortFullName")]
        public String PatientShortFullName
        {
            get
            {
                string result = String.Empty;
                if (!String.IsNullOrEmpty(PatientLastName))
                {
                    result += PatientLastName;
                    if (!String.IsNullOrEmpty(PatientFirstName))
                    {
                        result += " " + PatientFirstName.Substring(0, 1) + ".";
                        if (!String.IsNullOrEmpty(PatientMiddleName))
                            result += " " + PatientMiddleName.Substring(0, 1) + ".";
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Фамилия Имя Отчество (полностью)
        /// </summary>
        [SendToServer(false)]
        [CSN("PatientFullName")]
        public String PatientFullName
        {
            get
            {
                string result = String.Empty;
                if (!String.IsNullOrEmpty(PatientLastName))
                {
                    result += PatientLastName;
                    if (!String.IsNullOrEmpty(PatientFirstName))
                    {
                        result += " " + PatientFirstName;
                        if (!String.IsNullOrEmpty(PatientMiddleName))
                            result += " " + PatientMiddleName;
                    }
                }
                return result;
            }
        }      

        #endregion CalculatedProperties
    }

    public class ArchiveRackHeader : BaseObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ArchiveRackHeader
        /// </summary>
        public ArchiveRackHeader()
        { }

        [CSN("Barcode")]
        public String Barcode { get; set; }
        [CSN("ArchiveRackType")]
        public ArchiveRackTypeDictionaryItem ArchiveRackType { get; set; }
        [CSN("StorageRule")]
        public StorageRuleDictionaryItem StorageRule { get; set; }
        // статический справочник "Направление заполнения штатива". 1 - слева-направо, сверху-вниз. 2 - сверху-вниз, слева-направо. 
        [CSN("FillingDirection")]
        public int? FillingDirection { get; set; }
        // тип нумерации горизонтальной шкалы. 1 - числовая, 2 - буквенная
        [CSN("HorizontalAxisNumerationType")]
        public int? HorizontalAxisNumerationType { get; set; }
        // тип нумерации вертикальной шкалы. 1 - числовая, 2 - буквенная
        [CSN("VerticalAxisNumerationType")]
        public int? VerticalAxisNumerationType { get; set; }
        [CSN("Width")]
        public int Width { get; set; }
        [CSN("Height")]
        public int Height { get; set; }
        /// <summary>
        /// Штатив "Закрыт" (более не учитывается). Cм. LIS-6366, LIS-6014
        /// </summary>
        [CSN("Closed")]
        public Boolean Closed { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [CSN("Date")]
        [SendToServer(false)]
        public DateTime Date { get; set; }
        /// <summary>
        /// Признак удаления
        /// </summary>
        [CSN("Removed")]
        [SendToServer(false)]
        public Boolean Removed { get; set; }
        /// <summary>
        /// Ссылка на место хранения в архиве
        /// </summary>
        [CSN("ArchiveStorage")]
        public ArchiveStorageDictionaryItem ArchiveStorage { get; set; }
        /// <summary>
        /// Кол-во занятых слотов
        /// </summary>
        [CSN("SlotCount")]
        [SendToServer(false)]
        public Int64 SlotCount { get; set; }
        /// <summary>
        /// Дата истечения срока годности
        /// </summary>
        [CSN("ExpireDate")]
        [SendToServer(false)]
        public DateTime? ExpireDate { get; set; }

        /// <summary>
        /// Признак истечения срока годности
        /// </summary>
        [CSN("Expired")]
        [SendToServer(false)]
        public bool Expired { get; set; }
         

        #region CalculatedProperties
        /// <summary>
        /// Название места хранения
        /// </summary>
        [CSN("StorageName")]
        [SendToServer(false)]
        public String StorageName 
        {
            get { return this.ArchiveStorage != null && this.ArchiveStorage.Id != 0 ? this.ArchiveStorage.GetFullName() : String.Empty; }
        }
        /// <summary>
        /// Отображаемый пользователю признак удаления
        /// </summary>
        [CSN("RemovedCaption")]
        [SendToServer(false)]
        public String RemovedCaption
        {
            get { return Removed ? "Да" : "Нет"; }
        }
        /// <summary>
        /// Отображаемый пользователю размер штатива
        /// </summary>
        [CSN("Size")]
        [SendToServer(false)]
        public String Size
        {
            get { return (this.Width > 0 && this.Height > 0) ? String.Format("{0} x {1}", this.Width, this.Height) : String.Empty; }        
        }

        #endregion CalculatedProperties
    }

    public class ArchiveRack : ArchiveRackHeader
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса ArchiveRack 
        /// </summary>
        public ArchiveRack()
        {
            Slots = new List<ArchiveRackSlot>();
        }

        /// <summary>
        /// Комментарий
        /// </summary>
        [CSN("Comment")]
        public String Comment { get; set; }

        /// <summary>
        /// Слоты штатива
        /// </summary>
        [CSN("Slots")]
        public List<ArchiveRackSlot> Slots { get; set; }
    }
}
