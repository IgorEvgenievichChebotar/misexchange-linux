using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    [StaticDictionary(true)]
    public class StateDictionaryItem : DictionaryItem
    {
        [CSN("Color")]
        public string Color
        {
            get;
            set;
        }
    }

    [OldSaveMethod]
    [StaticDictionary(true)]
    public class SampleStateDictionaryItem : StateDictionaryItem
    {
    }

    [OldSaveMethod]
    [StaticDictionary(true)]
    public class NormalityStateDictionaryItem : StateDictionaryItem
    {
    }

    [OldSaveMethod]
    [StaticDictionary(true)]
    public class BiomaterialStateDictionaryItem : StateDictionaryItem
    {
    }
}