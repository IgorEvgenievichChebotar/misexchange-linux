using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class DefectTypeDictionaryItem: DictionaryItem 
    {

        public DefectTypeDictionaryItem()
        {
            WorkComment = "";
            Biomaterials = new List<ObjectRef>();
            Tests = new List<ObjectRef>();
        }
        [CSN("AllBiomaterials")]
        public Boolean AllBiomaterials { get; set; }
        [CSN("SkipService")]
        public Boolean SkipService { get; set; }
        [CSN("WorkComment")]
        public String WorkComment { get; set; }
        [CSN("Biomaterials")]
        public List<ObjectRef> Biomaterials { get; set; }
        [CSN("Tests")]
        public List<ObjectRef> Tests { get; set; }
    }
    
    public class DefectTypeDictionary : DictionaryClass<DefectTypeDictionaryItem>
    {

        public DefectTypeDictionary(String DictionaryName) : base(DictionaryName) { }
        [CSN("DefectType")]
        public List<DefectTypeDictionaryItem> DefectType
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
