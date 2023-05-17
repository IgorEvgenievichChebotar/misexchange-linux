// Данный файл содержит набор классов для синхронизации справочников ЛИС и МИС

using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    /// <summary>
    /// Класс описывает занчение названия и кода справочного элемента между 
    /// справочниками ЛИС и МИС
    /// </summary>
    [XmlType(TypeName = "Item")]
    public class SynchronizationItem
    {
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Mnemonics")]
        public String Mnemonics { get; set; }
        [CSN("SubItems")]
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
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Items")]
        public List<SynchronizationItem> Items { get; set; }
    }
}