using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class TargetRank
    {
        [CSN("Target")]
        public TargetDictionaryItem Target { get; set; }

        [CSN("Rank")]
        public int Rank { get; set; }
    }

    public class TargetRankSet
    {
        public TargetRankSet()
        {
            TargetRanks = new List<TargetRank>();
        }

        [Unnamed]
        [CSN("TargetRanks")]
        public List<TargetRank> TargetRanks { get; set; }
    }
}
