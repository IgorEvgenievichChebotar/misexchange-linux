using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemDictionary
{
    [StaticDictionary(true)] 
    public class TransfusionRequestStateDictionaryItem: DictionaryItem
    {
        [SendToServer(false)]
        [CSN("Color")]
        public String Color { get; set; }
    }
}
