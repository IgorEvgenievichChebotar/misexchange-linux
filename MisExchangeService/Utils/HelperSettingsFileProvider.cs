using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.SuperCore;

namespace ru.novolabs.MisExchange.Utils
{
    
    class HelperSettingsFileProvider : ru.novolabs.MisExchange.Utils.IHelperSettingsProvider
    {
        public T GetHelperSettings<T>()
        {
            return File.ReadAllText(Path.Combine(FileHelper.AssemblyDirectory, "exchangeHelperSettings.xml"), Encoding.UTF8).Deserialize<T>(Encoding.UTF8);       
        }
    }
}
