using System;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class DictionaryRequest 
    {
        public DictionaryRequest()
        {
            Directory = String.Empty;
        }
        [CSN("Directory")]
        public String Directory { get; set; }
    }
}
