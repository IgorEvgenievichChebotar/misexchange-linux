using ru.novolabs.SuperCore.CommonBusinesObjects;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class OperationJournalFilter : BaseJournalFilter
    {
        public OperationJournalFilter()
        {
            OperationTypes = new List<int>();
            Services = new AndOrIdList();
            ServiceGroups = new AndOrIdList();
            Clear();
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

        public override void Clear()
        {
            base.Clear();
        }

        [CSN("DateFrom")]
        public DateTime? DateFrom { get; set; }
        [CSN("DateTill")]
        public DateTime? DateTill { get; set; }
        [CSN("OperationTypes")]
        public List<int> OperationTypes { get; set; }
        [CSN("Services")]
        public AndOrIdList Services { get; set; }
        [CSN("ServiceGroups")]
        public AndOrIdList ServiceGroups { get; set; }
    }

    public class OperationJournalResponce
    {
        public OperationJournalResponce()
        {
            Operations = new List<OperationJournalRow>();
        }

        [CSN("Operations")]
        public List<OperationJournalRow> Operations { get; set; }
    }
}
