using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class Waybill : BaseObject
    {
        public Waybill()
        {
            Content = new List<WaybillContent>();
        }

        /// <summary>
        /// номер (штрихкод) ведомости
        /// </summary>
        [CSN("Nr")]
        public String Nr { get; set; }
        /// <summary>
        /// дата/время создания ведомости
        /// </summary>
        [CSN("CreateDate")]
        public DateTime? CreateDate { get; set; }
        /// <summary>
        /// дата/время отправки ведомости
        /// </summary>
        [CSN("SendDate")]
        public DateTime? SendDate { get; set; }
        /// <summary>
        /// дата/время получения ведомости
        /// </summary>
        [CSN("DeliveryDate")]
        public DateTime? DeliveryDate { get; set; }
        /// <summary>
        /// дата/время закрытия ведомости
        /// </summary>
        [CSN("CloseDate")]
        public DateTime? CloseDate { get; set; }
        /// <summary>
        /// пользователь, создавший ведомость
        /// </summary>
        [CSN("SenderEmployee")]
        public EmployeeDictionaryItem SenderEmployee { get; set; }
        /// <summary>
        /// пользователь, получивший ведомость
        /// </summary>
        [CSN("ReceiverEmployee")]
        public EmployeeDictionaryItem ReceiverEmployee { get; set; }
        /// <summary>
        /// офис назначения
        /// </summary>
        [CSN("TargetOffice")]
        public OfficeDictionaryItem TargetOffice { get; set; }
        /// <summary>
        /// офис, отправивший ведомость
        /// </summary>
        [CSN("SourceOffice")]
        public OfficeDictionaryItem SourceOffice { get; set; }
        /// <summary>
        /// курьер
        /// </summary>
        [CSN("Courier")]
        public CourierDictionaryItem Courier { get; set; }
        /// <summary>
        /// комментарии
        /// </summary>
        [CSN("Comments")]
        public String Comments { get; set; }
        /// <summary>
        /// признак удаления
        /// </summary>
        [CSN("Removed")]
        public Boolean Removed { get; set; }
        /// <summary>
        /// признак отправки
        /// </summary>
        [CSN("Sent")]
        public Boolean Sent { get; set; }
        /// <summary>
        /// признак доставленности 
        /// </summary>
        [CSN("Delivered")]
        public Boolean Delivered { get; set; }
        /// <summary>
        /// признак закрытия
        /// </summary>
        [CSN("Closed")]
        public Boolean Closed { get; set; }
        /// <summary>
        /// содержимое ведомости
        /// </summary>
        [CSN("Content")]
        public List<WaybillContent> Content { get; set; }
    }

    public class WaybillContent : BaseObject
    {
        /// <summary>
        /// ссылка на пробу
        /// </summary>
        [CSN("Sample")]
        public ObjectRef Sample { get; set; }
        /// <summary>
        /// признак удаления элемента из ведомости
        /// </summary>
        [CSN("Removed")]
        public Boolean Removed { get; set; }
        /// <summary>
        /// признак того, что биоматериал успешно доставлен
        /// </summary>
        [CSN("Delivered")]
        public Boolean Delivered { get; set; }
        /// <summary>
        /// ссылка на Вид брака. Заполняется, если в процессе перевозки биоматериал был поврежден 
        /// </summary>
        [CSN("Defect")]
        public DefectTypeDictionaryItem Defect { get; set; }
        /// <summary>
        /// комментарий
        /// </summary>
        [CSN("Comment")]
        public String Comment { get; set; }

        /// <summary>
        /// Номер пробы
        /// </summary>
        [SendToServer(false)]
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
        
        // Дополнительные свойства. Необходимы для отображения в интерфейсе

        ///// <summary>
        ///// ссылка на биоматериал, находящийся в пробирке
        ///// </summary>
        //[SendToServer(false)]
        //[CSN("Biomaterial")]
        //public BiomaterialDictionaryItem Biomaterial { get; set; }
        ///// <summary>
        ///// ссылка на заявку, с которой связана пробирка 
        ///// </summary>
        //[SendToServer(false)]
        //[CSN("Request")]
        //public ObjectRef Request { get; set; }
        
        [SendToServer(false)]
        [CSN("State")]        
        public String State
        {
            get
            {
                 if (Defect != null)
                     return "дефект";
                 else if (Delivered)
                     return "доставлен";
                 else
                     return "добавлен";
            }
        }

        [SendToServer(false)]
        [CSN("Name")]
        public String Name
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
    }
}