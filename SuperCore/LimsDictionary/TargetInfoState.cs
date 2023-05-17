using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [StaticDictionary(true)]
    public class TargetInfoStateDictionaryItem : DictionaryItem
    {
        [CSN("Color")]
        public string Color
        {
            get;
            set;
        }
    }
}
