using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class RankedNamedObject : NamedObject
    {
        [CSN("Rank")]
        public Int32 Rank { get; set; }
    }

    public class RequestFormGroup : RankedNamedObject
    {
        public RequestFormGroup()
            : base()
        {
            Targets = new List<TargetRank>();
        }
        [CSN("ButtonUpColor")]
        public Int32 ButtonUpColor { get; set; }
        [CSN("ButtonDownColor")]
        public Int32 ButtonDownColor { get; set; }
        [CSN("Targets")]
        public List<TargetRank> Targets { get; set; }
    }

    public class RequestFormGroupSet
    {
        public RequestFormGroupSet()
        {
            RequestFormGroups = new List<RequestFormGroup>();
        }

        [Unnamed]
        [CSN("RequestFormGroups")]
        public List<RequestFormGroup> RequestFormGroups { get; set; }
    }
}