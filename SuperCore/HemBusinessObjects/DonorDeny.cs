using ru.novolabs.SuperCore.HemDictionary;
using System;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{

    public enum DonorDenyStatus
    {
        BC_DONOR_DENY_STATUS_OK = 1, // Нет отвода.
        BC_DONOR_DENY_STATUS_ABSOLUTE = 2, // Абсолютный отвод (утверждён).
        BC_DONOR_DENY_STATUS_ABSOLUTE_WAITING = 3, // Абсолютный отвод до выяснения.
        BC_DONOR_DENY_STATUS_RELATIVE = 4, // Временный отвод.
        BC_DONOR_DENY_STATUS_RELATIVE_WAITING = 5, // Временный отвод до выяснения
        BC_DONOR_DENY_STATUS_EXPIRED = 6, // Истёкший временный отвод.
        BC_DONOR_DENY_STATUS_REMOVED = 7 // Снятый отвод.
    }

    public struct DonorDenyStatusName
    {
        public static String DonorDenyStatusOk = "ОК";
        public static String DonorDenyStatusAbsolute = "АБС. ОТВОД";
        public static String DonorDenyStatusRelative = "ВРЕМ. ОТВОД";
        public static String DonorDenyStatusExpired = "ИСТЕКШ. ОТВОД";
        public static String DonorDenyStatusRemoved = "СНЯТЫЙ ОТВОД";
    }

    public class DonorDeny : DonorHeader
    {
        public DonorDeny()
        {
            Deny = new DenyDictionaryItem();
            UserSet = new ObjectRef();
            UserRemove = new ObjectRef();
            Source = new DenySourceDictionaryItem();
            Donation = new ObjectRef();
            Parameter = new ObjectRef();
        }

        [SendToServer(false)]
        [CSN("Date")]
        public DateTime Date { get; set; }
        [CSN("DateFrom")]
        public DateTime DateFrom { get; set; }
        [CSN("DateTill")]
        public DateTime DateTill { get; set; }
        [CSN("Deny")]
        public DenyDictionaryItem Deny { get; set; }
        [SendToServer(false)]
        [CSN("Removed")]
        public Boolean Removed { get; set; }
        [SendToServer(false)]
        [CSN("DateRemove")]
        public DateTime? DateRemove { get; set; }
        [SendToServer(false)]
        [CSN("UserSet")]
        public ObjectRef UserSet { get; set; }
        [SendToServer(false)]
        [CSN("UserRemove")]
        public ObjectRef UserRemove { get; set; }
        [CSN("Comment")]
        public String Comment { get; set; }
        [CSN("Waiting")]
        public Boolean Waiting { get; set; }
        [CSN("Duration")]
        public Int32? Duration { get; set; }
        [CSN("DurationUnit")]
        public Int32 DurationUnit { get; set; }
        [CSN("Source")]
        public DenySourceDictionaryItem Source { get; set; }
        [CSN("Expired")]
        public Boolean Expired { get; set; }
        [CSN("Visit")]
        public ObjectRef Visit { get; set; }
        [CSN("VisitStartDate")]
        public DateTime? VisitStartDate { get; set; }
        [CSN("VisitNr")]
        public String VisitNr { get; set; }
        [CSN("Donation")]
        public ObjectRef Donation { get; set; }
        [CSN("DonationDate")]
        public DateTime? DonationDate { get; set; }
        [CSN("DonationNr")]
        public String DonationNr { get; set; }
        [CSN("Parameter")]
        public ObjectRef Parameter { get; set; }
        //property UserSetName: string read GetUserSetName;
        //property UserRemoveName: string read GetUserRemoveName;
    }
}
