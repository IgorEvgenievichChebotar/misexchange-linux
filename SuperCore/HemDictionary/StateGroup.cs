using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class StateGroupDictionaryItem: DictionaryItem
    {
        public StateGroupDictionaryItem()
        {
        }

        [CSN("BgColor")]
        public Int32 BgColor { get; set; }

        [CSN("FontColor")]
        public Int32 FontColor { get; set; }

        [CSN("ExternalCode")]
        public new String ExternalCode { get; set; }
    }

    public class StateGroupItem : DictionaryItem
    {
        public StateGroupItem()
        {
        }

        [CSN("StateGroup")]
        public StateGroupDictionaryItem StateGroup { get; set; }
    }
}
