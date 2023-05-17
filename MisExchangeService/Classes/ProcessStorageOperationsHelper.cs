using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.Classes
{
    internal abstract class ProcessStorageOperationsHelper
    {
        /// <summary>
        /// Обрабатывает данные из внешней складской системы в соответствии с внутренним алогритмом
        /// </summary>
        abstract public void ProcessData();
    }

    [AttributeUsage(AttributeTargets.Class)]
    class ProcessStorageOperationsHelperName : Attribute
    {
        private string helperName = String.Empty;

        public string Name
        {
            get { return helperName; }
        }

        public ProcessStorageOperationsHelperName(string processDataHelperName)
        {
            this.helperName = processDataHelperName;
        }
    }
}