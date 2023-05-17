using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    
    public class RequestCustomStateDictionaryItem : DictionaryItem { }

    public class RequestCustomStateDictionary : DictionaryClass<RequestCustomStateDictionaryItem>
    {
        public RequestCustomStateDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("RequestCustomState")]
        public List<RequestCustomStateDictionaryItem> RequestCustomState
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}