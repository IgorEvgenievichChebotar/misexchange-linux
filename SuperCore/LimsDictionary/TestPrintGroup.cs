using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class TestPrintGroupDictionaryItem : DictionaryItem {
        public TestPrintGroupDictionaryItem()
        {
            TestGroups = new List<TestGroup>();
        }

        [CSN("TestGroups")]
        public List<TestGroup> TestGroups { get; set; }


        

    }

    [OldSaveMethod]
    public class TestPrintGroupDictionary : DictionaryClass<TestPrintGroupDictionaryItem>
    {
        public TestPrintGroupDictionary(String DictionaryName)
            : base(DictionaryName){}

        [CSN("TestPrintGroup")]
        public List<TestPrintGroupDictionaryItem> TestPrintGroup
        {
            get { return Elements; }
            set { Elements = value; }
        }

        public List<TestPrintGroupDictionaryItem> GetTestGroups(String testCode)
        {
            List<TestPrintGroupDictionaryItem> result = new List<TestPrintGroupDictionaryItem>();
            foreach (TestPrintGroupDictionaryItem element in Elements)
            {
                foreach (TestGroup testGroup in element.TestGroups)
                {
                    if (testGroup.Test != null)
                    {
                        if (testGroup.Test.Code.Equals(testCode))
                        {
                            result.Add(element);
                            break;
                        }
                    }
                }
            }
            return result;
        }

        
    }

    public class TestGroup
    {
        [CSN("Rank")]
        public Int32 Rank { get; set; }
        [CSN("Test")]
        public TestDictionaryItem Test { get; set; }
    }
}
