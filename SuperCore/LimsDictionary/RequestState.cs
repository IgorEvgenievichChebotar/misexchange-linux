using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    [StaticDictionary(true)]
    public class RequestStateDictionaryItem : DictionaryItem
    {
        [CSN("Color")]
        public string Color
        {
            get;
            set;
        }
    }
}
