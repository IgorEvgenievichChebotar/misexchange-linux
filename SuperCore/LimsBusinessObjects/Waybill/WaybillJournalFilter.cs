using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class WaybillJournalFilter : BaseJournalFilter
    {
        public WaybillJournalFilter()
        {
            Clear();
        }

        /// <summary>
        /// номер (штрихкод) ведомости
        /// </summary>
        [CSN("Nr")]
        public String Nr { get; set; }
        /// <summary>
        /// дата/время создания ведомости "с"
        /// </summary>
        [CSN("CreateDateFrom")]
        public DateTime? CreateDateFrom { get; set; }
        /// <summary>
        /// дата/время создания ведомости "по" 
        /// </summary>
        [CSN("CreateDateTill")]
        public DateTime? CreateDateTill { get; set; }
        /// <summary>
        /// дата/время отправки ведомости "с"
        /// </summary>
        [CSN("SendDateFrom")]
        public DateTime? SendDateFrom { get; set; }
        /// <summary>
        /// дата/время отправки ведомости "по"
        /// </summary>
        [CSN("SendDateTill")]
        public DateTime? SendDateTill { get; set; }
        /// <summary>
        /// дата/время получения ведомости "с"
        /// </summary>
        [CSN("DeliveryDateFrom")]
        public DateTime? DeliveryDateFrom { get; set; }
        /// <summary>
        /// дата/время получения ведомости "по"
        /// </summary>
        [CSN("DeliveryDateTill")]
        public DateTime? DeliveryDateTill { get; set; }
        /// <summary>
        /// дата/время закрытия ведомости "с"
        /// </summary>
        [CSN("CloseDateFrom")]
        public DateTime? CloseDateFrom { get; set; }
        /// <summary>
        /// дата/время закрытия ведомости "по"
        /// </summary>
        [CSN("CloseDateTill")]
        public DateTime? CloseDateTill { get; set; }
        /// <summary>
        /// пользователь, создавший ведомость
        /// </summary>
        [CSN("SenderEmployeeName")]
        public String SenderEmployeeName { get; set; }
        /// <summary>
        /// пользователь, получивший ведомость
        /// </summary>
        [CSN("ReceiverEmployeeName")]
        public String ReceiverEmployeeName { get; set; }
        /// <summary>
        /// офисы назначения
        /// </summary>
        [CSN("TargetOffices")]
        public List<OfficeDictionaryItem> TargetOffices { get; set; }
        /// <summary>
        /// офисы, отправившие ведомость
        /// </summary>
        [CSN("SourceOffices")]
        public List<OfficeDictionaryItem> SourceOffices { get; set; }
        /// <summary>
        /// Имя курьера
        /// </summary>
        [CSN("CourierName")]
        public String CourierName { get; set; }
        /// <summary>
        /// комментарии
        /// </summary>
        [CSN("Comments")]
        public String Comments { get; set; }
        /// <summary>
        /// признак удаления (да/нет/не важно)
        /// </summary>
        [CSN("Removed")]
        public int? Removed { get; set; }
        /// <summary>
        /// признак отправки (да/нет/не важно)
        /// </summary>
        [CSN("Sent")]
        public int? Sent { get; set; }
        /// <summary>
        /// признак отправки (да/нет/не важно) 
        /// </summary>
        [CSN("Delivered")]
        public int? Delivered { get; set; }
        /// <summary>
        /// признак закрытия (да/нет/не важно) 
        /// </summary>
        [CSN("Closed")]
        public int? Closed { get; set; }
        /// <summary>
        /// номер пробы
        /// </summary>
        [CSN("SampleNr")]
        public String SampleNr { get; set; }


        public override void Clear()
        {
            //CreateDateFrom = CreateDateTill = SendDateFrom = SendDateTill = DeliveryDateFrom = DeliveryDateTill = DateTime.Now;
        }

        public override void PrepareToSend()
        {
            /*  if (Skip   Date)
              {
                  DateTill = null;
              }*/

            base.PrepareToSend();
        }
    }
}