using ru.novolabs.SuperCore.DictionaryCore;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public class UserGroupDictionaryItem : DictionaryItem
    {
        public UserGroupDictionaryItem()
        {
            Rights = new List<ObjectRef>();
        }

        [CSN("Rights")]
        public List<ObjectRef> Rights { get; set; }
    }
}
