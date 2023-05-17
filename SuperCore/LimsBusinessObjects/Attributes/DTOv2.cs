using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace ru.novolabs.ExchangeDTOs

{
    [AttributeUsage(AttributeTargets.Property)]
    public class DTOv2 : Attribute
    {
        public DTOv2(String field = null, String nameField = null, String codeField = null, String dictionaryName = null, Boolean canCreate = true, String parentName = null)
        {
            Field = field;
            NameField = nameField;
            CodeField = codeField;
            DictionaryName = dictionaryName;
            CanCreate = canCreate;
            ParentName = parentName;
        }


        public String Field { get; private set; }
        public String NameField { get; private set; }
        public String CodeField { get; private set; }
        public String DictionaryName { get; private set; }
        public Boolean CanCreate { get; private set; }
        public String ParentName { get; private set; }
    }
}
