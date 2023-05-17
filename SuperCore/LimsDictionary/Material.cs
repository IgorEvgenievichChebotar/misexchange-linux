using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class MaterialDictionaryItem : DictionaryItem
    {
        public MaterialDictionaryItem()
        {
            BoxUnit = new ObjectRef();
            AtomUnit = new ObjectRef();

            Tests = new List<TestDictionaryItem>();
            ResourceType = 1;
        }
        [CSN("ResourceType")]
        public Int32 ResourceType { get; private set; }
        [CSN("Acc")]
        public String Acc { get; set; }
        [CSN("Price")]
        public Single Price { get; set; }
        [CSN("Barcode")]
        public String Barcode { get; set; }
        [CSN("CatalogNr")]
        public String CatalogNr { get; set; }
        [CSN("CodeIn1C")]
        public String CodeIn1C { get; set; }
        [CSN("Mol")]
        public String Mol { get; set; }
        [CSN("Reserve")]
        public Int32 Reserve { get; set; }
        [CSN("Tax")]
        public Int32 Tax { get; set; }
        [CSN("BoxUnit")]
        public ObjectRef BoxUnit { get; set; }
        [CSN("AtomUnit")]
        public ObjectRef AtomUnit { get; set; }
        [CSN("UnitsInBox")]
        public Int32 UnitsInBox { get; set; }
        [CSN("AlwaysUseBoxUnit")]
        public Boolean AlwaysUseBoxUnit { get; set; }
        [CSN("Tests")]
        public List<TestDictionaryItem> Tests { get; set; }
    }

    public class MaterialDictionary : DictionaryClass<MaterialDictionaryItem>
    {
        public MaterialDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("Material")]
        public List<MaterialDictionaryItem> Material
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }

    public class SystemNextMaterialCatalogNrResponce
    {
        [CSN("Nr")]
        public Int32 Nr { get; set; }
    }
}
