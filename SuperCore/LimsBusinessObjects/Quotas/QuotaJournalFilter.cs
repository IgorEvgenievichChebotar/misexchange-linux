using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class QuotaJournalFilter : BaseJournalFilter
    {
        public QuotaJournalFilter()
        {
            States = new List<int>();
            Hospitals = new List<ObjectRef>();
            Clear();
        }

        /// <summary>
        /// Множество ссылок на заказчиков
        /// </summary>
        [CSN("Hospitals")]
        public List<ObjectRef> Hospitals { get; set; }
        /// <summary>
        /// Дата начала
        /// </summary>
        [CSN("DateFrom")]
        public DateTime? DateFrom { get; set; }
        /// <summary>
        /// Дата окончания
        /// </summary>
        [CSN("DateTill")]
        public DateTime? DateTill { get; set; }
        /// <summary>
        /// Номер договора с заказчиком
        /// </summary>
        [CSN("ContractNumber")]
        public string ContractNumber { get; set; }
        /// <summary>
        /// Статусы квот
        /// </summary>
        [CSN("States")]
        public List<Int32> States { get; set;}
        /// <summary>
        /// Определяет по какой дате искать квоты (сравнивать с dateFrom и dateTill ). Статический справочник: 0 - по дате начала (startDate), 1 - по дате окончания (endDate)
        /// </summary>
        [CSN("DateType")]
        public int? DateType { get; set; }
        /// <summary>
        /// Статус удаления
        /// </summary>
        [CSN("Removed")]
        public int? Removed { get; set; }
        /// <summary>
        /// Статус активности
        /// </summary>
        [CSN("Active")]
        public int? Active { get; set; }

        [SendToServer(false)]
        [CSN("SkipDate")]
        public Boolean SkipDate { get; set; }

        public override void Clear()
        {
            DateFrom = DateTill = DateTime.Now;
            ContractNumber = String.Empty;
            States.Clear();
            Hospitals.Clear();            
        }

        public override void PrepareToSend()
        {
            base.PrepareToSend();

            if (SkipDate)
            {
                DateFrom = null;
                DateTill = null;
                return;
            }

            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                if ((propInfo.PropertyType.Equals(typeof(DateTime?))) || (propInfo.PropertyType.Equals(typeof(DateTime))))
                {
                    if (propInfo.GetValue(this, null) != null)
                    {
                        if (propInfo.Name.EndsWith("From"))
                        {
                            if (null != propInfo.GetValue(this, null))
                                propInfo.SetValue(this, ((DateTime)propInfo.GetValue(this, null)).StartOfTheDay(), null);
                        }

                        if (propInfo.Name.EndsWith("Till"))
                        {
                            if (null != propInfo.GetValue(this, null))
                                propInfo.SetValue(this, ((DateTime)propInfo.GetValue(this, null)).EndOfTheDay(), null);
                        }
                    }
                }
            }
        }
    }
}
