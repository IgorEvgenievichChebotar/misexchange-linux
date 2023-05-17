using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class QuotaInfoList
    {
        [CSN("Quotas")]
        public List<QuotaInfo> Quotas { get; set; }

        public QuotaInfoList()
        {
            Quotas = new List<QuotaInfo>();
        }
    }

    public class QuotaInfo
    {
        [CSN("Id")]
        public int Id { get; set; }

        [CSN("QuotaDisplayName")]
        public string QuotaDisplayName { get; set; }

        [CSN("IsDefault")]
        public bool IsDefault { get; set; }

        [CSN("QuotaStartDate")]
        public DateTime? QuotaStartDate { get; set; }

    }
}
