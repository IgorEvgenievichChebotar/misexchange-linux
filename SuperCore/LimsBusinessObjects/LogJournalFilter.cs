using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class LogJournalFilter : BaseJournalFilter
    {
        public LogJournalFilter()
        {
            Employees = new List<ObjectRef>();
            ObjectTypes = new List<int>();
            OperationTypes = new List<int>();
            Clear();
        }

        [CSN("ObjectId")]
        public int? ObjectId { get; set; }

        [CSN("Machine")]
        public string Machine { get; set; }

        [CSN("Nr")]
        public string Nr { get; set; }
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

        [CSN("Employees")]
        public List<ObjectRef> Employees { get; set; }

        [CSN("ObjectTypes")]
        public List<Int32> ObjectTypes { get; set; }

        [CSN("OperationTypes")]
        public List<Int32> OperationTypes { get; set; }

        [CSN("NeedChildren")]
        public Boolean NeedChildren { get; set; }

        public override void Clear()
        {
            base.Clear();

            DateFrom = DateTill = DateTime.Now;
            Machine = Nr = String.Empty;

            NeedChildren = true;

            Employees.Clear();
            ObjectTypes.Clear();
            OperationTypes.Clear();
        }

        public override void PrepareToSend()
        {
            base.PrepareToSend();

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
