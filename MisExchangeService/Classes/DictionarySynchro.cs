// Данный файл содержит набор классов для синхронизации справочников ЛИС и МИС

using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace BusinessObjectsExchange
{

    /// <summary>
    /// Класс описывает занчение названия и кода справочного элемента между 
    /// справочниками ЛИС и МИС
    /// </summary>
    [XmlType(TypeName = "Item")]
    public class SynchronizationItem
    {
        //public String Name { get; set; }
        //public String MIS_Name { get; set; }
        public String Name { get; set; }

        //public String Code { get; set; }
        //public String MIS_Code { get; set; }
        public String Code { get; set; }
        public String Mnemonics { get; set; }

        public List<SynchronizationItem> SubItems { get; set; }
    }

    /// <summary>
    /// Класс описывает занчение названия и кода справочника, а так же его элементов
    /// справочниками ЛИС и МИС
    /// </summary>

    [XmlType(TypeName = "Dictionary")]
    public class DictionarySynchro
    {
        public DictionarySynchro()
        {
            Items = new List<SynchronizationItem>();
        }

        //public String Name { get; set; }
        //public String MIS_Name { get; set; }
        public String Name { get; set; }

        //public String Code { get; set; }
        //public String MIS_Code { get; set; }
        public String Code { get; set; }

        public List<SynchronizationItem> Items { get; set; }
    }
}
