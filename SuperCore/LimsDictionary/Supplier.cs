using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Xml.Serialization;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class SupplierDictionaryItem : DictionaryItem
    {
        public SupplierDictionaryItem()
        {
            //
        }
        [CSN("FullName")]
        public String FullName { get; set; }
        [CSN("JurAddress")]
        public String JurAddress { get; set; }
        [CSN("Director")]
        public String Director { get; set; }
        [CSN("FactAddress")]
        public String FactAddress { get; set; }
        [CSN("Kpp")]
        public String Kpp { get; set; }
        [CSN("Account")]
        public String Account { get; set; }
        [CSN("CorrAccount")]
        public String CorrAccount { get; set; }
        [CSN("Bank")]
        public String Bank { get; set; }
        [CSN("Phone")]
        public String Phone { get; set; }
        [CSN("Accountant")]
        public String Accountant { get; set; }
        [CSN("Email")]
        public String Email { get; set; }
        [CSN("Okpo")]
        public String Okpo { get; set; }
        [CSN("Okonh")]
        public String Okonh { get; set; }
        [CSN("Inn")]
        public String Inn { get; set; }
        [CSN("Fax")]
        public String Fax { get; set; }
        [CSN("CodeIn1C")]
        public String CodeIn1C { get; set; }
        [CSN("Bik")]
        public String Bik { get; set; }
    }

    public class SupplierDictionary : DictionaryClass<SupplierDictionaryItem>
    {
        public SupplierDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("Supplier")]
        public List<SupplierDictionaryItem> Supplier
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}