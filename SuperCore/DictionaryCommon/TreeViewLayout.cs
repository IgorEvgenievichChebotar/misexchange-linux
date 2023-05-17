using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.DictionaryCommon
{
    /*public class TreeViewLayoutDictionaryItem : DictionaryItem
    {
        public Int32 TreeType { get; set; }
        public DepartmentDictionaryItem Department { get; set; }
        public List<TreeViewColumnLayout> Columns { get; set; }

        public String TreeTypeName { get; set; }
        public String DepartmentName { get; set; }
    }*/

    public class TreeViewLayoutDictionary : DictionaryClass<TreeViewLayout>
    {
        public TreeViewLayoutDictionary(String dictionaryName) : base(dictionaryName) { }

        // Имя свойства должно совпадать с идентификатором справочника (используется для заполнения коллекции Elements из XML). От регистра не зависит
        [CSN("TreeViewLayout")]
        public List<TreeViewLayout> TreeViewLayout
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}