using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes
{
    internal abstract class ExportDictionariesHelper
    {
        /// <summary>
        /// Экспортирует справочные данные в соответсвии с внутренним алогритмом
        /// </summary>
        abstract public void DoExport();
    }

    [AttributeUsage(AttributeTargets.Class)]
    class ExportDictionariesHelperName : Attribute
    {
        private string helperName = String.Empty;

        public string Name
        {
            get { return helperName; }
        }

        public ExportDictionariesHelperName(string exchangeHelperName)
        {
            this.helperName = exchangeHelperName;
        }
    }
}
