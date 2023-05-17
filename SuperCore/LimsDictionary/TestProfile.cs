using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class TestProfileDictionaryItem : DictionaryItem
    {
        [CSN("Department")]
        public DepartmentDictionaryItem Department { get; set; }
        [CSN("Tests")]
        public List<TestDictionaryItem> Tests { get; set; }

        public TestProfileDictionaryItem()
        {
            Tests = new List<TestDictionaryItem>();
        }
    }

    public class TestProfileDictionary : DictionaryClass<TestProfileDictionaryItem>
    {
        public TestProfileDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("Profile")]
        public List<TestProfileDictionaryItem> Profile
        {
            get { return Elements; }
            set { Elements = value; }
        }

        public List<TestProfileDictionaryItem> GetTestGroups(TestDictionaryItem test)
        {
            return GetTestGroups(test.Code);
        }


        public List<TestProfileDictionaryItem> GetTestGroups(String testCode)
        {
            List<TestProfileDictionaryItem> result = new List<TestProfileDictionaryItem>();

            foreach (TestProfileDictionaryItem element in Elements)
            {
                if (element.Tests != null && element.Tests.Count != 0)
                if (!element.Removed && (element.Tests.Find((t) => t.Code.Equals(testCode)) != null))  result.Add(element);
            }

            return result;
        }
    }




}
