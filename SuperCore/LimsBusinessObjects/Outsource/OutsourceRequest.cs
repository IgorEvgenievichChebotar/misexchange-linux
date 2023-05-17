using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.LimsDictionary;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;

public struct OutsourceRequestResultState
{
    public const int NotDefined = 0;
    public const int Unsent = 1;
    public const int Ready = 2;
    public const int Sent = 3;
    public const int PartiallyReceived = 4;
    public const int TotallyReceived = 5;
    public const int Changed = 6;
    public const int CancelledByLis = 7;
    public const int CancelledByOutsourcer = 8;
}

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Outsource
{
    [XmlType(TypeName = "Request")]
    public class OutsourceRequest : BaseObject
    {
        public OutsourceRequest()
        {
            UserFields = new List<OutsourceUserField>();
            Samples = new List<OutsourceSample>();
            Patient = new OutsourcePatient();
            Request = new ObjectRef();
        }

        [CSN("ExternalCode")]
        public String ExternalCode { get; set; }

        [CSN("Contract")]
        public String Contract { get; set; }

        [CSN("Account")]
        public String Account { get; set; }

        [CSN("RequestCode")]
        public String RequestCode { get; set; }

        [SendToServer(false)]
        [CSN("SamplingDate")]
        public DateTime SamplingDate { get; set; }

        [SendToServer(false)]
        [CSN("SampleDeliveryDate")]
        public DateTime SampleDeliveryDate { get; set; }

        [SendToServer(false)]
        [CSN("CreationDate")]
        public DateTime CreationDate { get; set; }

        [SendToServer(false)]
        [CSN("PregnancyDuration")]
        public Int32 PregnancyDuration { get; set; }

        [SendToServer(false)]
        [CSN("CyclePeriod")]
        public Int32 CyclePeriod { get; set; }

        [SendToServer(false)]
        [CSN("ReadOnly")]
        public Boolean ReadOnly { get; set; }

        [SendToServer(false)]
        [CSN("Comments")]
        public String Comments { get; set; }

        [SendToServer(false)]
        [CSN("SendDate")]
        public DateTime SendDate { get; set; }
        [CSN("ResultState")]
        public Int32 ResultState { get; set; }

        [SendToServer(false)]
        [CSN("UserFields")]
        public List<OutsourceUserField> UserFields { get; set; }

        [SendToServer(false)]
        [CSN("Patient")]
        public OutsourcePatient Patient { get; set; }

        [SendToServer(false)]
        [CSN("Request")]
        public ObjectRef Request { get; set; }

        [CSN("Hospital")]
        public HospitalDictionaryItem Hospital { get; set; }

        [CSN("Outsourcer")]
        public OutsourcerDictionaryItem Outsourcer { get; set; }

        [CSN("Samples")]
        public List<OutsourceSample> Samples { get; set; }
    }
}
