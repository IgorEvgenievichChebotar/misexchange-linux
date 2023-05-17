using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class TargetGroupDictionaryItem : DictionaryItem
    { }

    /// <summary>
    /// Представляет справочник "Группы исследований"
    /// </summary>
    public class TargetGroupDictionary : DictionaryClass<TargetGroupDictionaryItem>
    {
        public TargetGroupDictionary(String dictionaryName) : base(dictionaryName) { }

        [CSN("TargetGroup")]
        public List<TargetGroupDictionaryItem> TargetGroup
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}