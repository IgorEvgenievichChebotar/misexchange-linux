using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ru.novolabs.MisExchange.DirectoryExport
{
    [DataContract]
    public class ReferenceInformation
    {
        public ReferenceInformation()
        {
            TargetGroups = new List<TargetGroup>();
            Hospitals = new List<Hospital>();
            Microorganisms = new List<Microorganism>();
            DefectTypes = new List<DefectType>();
            PayCategories = new List<PayCategory>();
            UserFields = new List<UserField>();            
        }
        [DataMember(IsRequired = true, Order = 1)]
        public List<TargetGroup> TargetGroups { get; set; }
        [DataMember(IsRequired = true, Order = 2)]
        public List<Hospital> Hospitals { get; set; }
        [DataMember(IsRequired = true, Order = 3)]
        public List<Microorganism> Microorganisms { get; set; }
        [DataMember(IsRequired = true, Order = 4)]
        public List<DefectType> DefectTypes { get; set; }
        [DataMember(IsRequired = true, Order = 5)]
        public List<PayCategory> PayCategories { get; set; }
        [DataMember(IsRequired = true, Order = 6)]
        public List<UserField> UserFields { get; set; }
    }
}
