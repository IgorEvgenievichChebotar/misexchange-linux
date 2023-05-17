using System;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace ru.novolabs.SuperCore.HardwareWork.WMI
{
    internal static class NlsWMIProvider
    {
        private static string GetClassFullInfo(string wmiClassName)
        {
            SelectQuery query = new SelectQuery(wmiClassName);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);

            StringBuilder sb = new StringBuilder(5000);
            foreach (ManagementObject managementObject in searcher.Get())
            {
                sb.Append(managementObject.GetText(TextFormat.Mof));
                sb.Append("");
            }

            return sb.ToString();
        }

        internal static string GetProcessorFullInfo()
        {
            // Все свойства перечислены здесь: http://msdn.microsoft.com/en-us/library/windows/desktop/aa394373(v=vs.85).aspx 
            return GetClassFullInfo("Win32_Processor");
        }

        internal static string GetMotherboardFullInfo()
        {
            // Все свойства перечислены здесь: http://msdn.microsoft.com/en-us/library/windows/desktop/aa394072(v=vs.85).aspx
            return GetClassFullInfo("Win32_BaseBoard");
        }

        internal static string GetBIOSFullInfo()
        {
            // Все свойства перечислены здесь: http://msdn.microsoft.com/en-us/library/windows/desktop/aa394077(v=vs.85).aspx 
            return GetClassFullInfo("Win32_BIOS");
        }

        internal static string GetDiskDriveFullInfo()
        {
            // Все свойства перечислены здесь: http://msdn.microsoft.com/en-us/library/windows/desktop/aa394132(v=vs.85).aspx
            return GetClassFullInfo("Win32_DiskDrive");
        }

        internal static string GetNetworkAdapterFullInfo()
        {
            // Все свойства перечислены здесь: http://msdn.microsoft.com/en-us/library/windows/desktop/aa394216(v=vs.85).aspx
            return GetClassFullInfo("Win32_NetworkAdapter");
        }

        private static string[] FilterPropNames(string[] propNames, string className)
        {
            List<String> list = new List<string>();

            foreach (String propName in propNames)
            {
                string query = "SELECT " + propName + " FROM " + className;
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(query); // Инициализируем новый экземпляр класса ManagementObjectSearcher запросом query
                    var count = searcher.Get().Count; // Обращаемся к свойству коллекции searcher.Get(), для того чтобы запрос действительно выполнился (lazy evaluations)
                    list.Add(propName);
                }
                catch { }
            }

            return list.ToArray();
        }

        internal static string[] GetWMIClassPropertiesInfo(string[] propNames, string className, string[] conditions = null, bool skipNames = true)
        {
            string[] columnNames = FilterPropNames(propNames, className);

            List<string> result = new List<string>();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT " + String.Join(", ", columnNames));
            sb.Append(" FROM " + className);

            if (conditions != null && conditions.Length > 0)
            {
                sb.Append(" WHERE " + conditions[0]);
                if (conditions.Length > 1)
                {
                    for (int i = 1; i < conditions.Length; i++)
                        sb.Append(" AND " + conditions[i]);
                }
            }

            string query = sb.ToString();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            foreach (var obj in searcher.Get())
            {
                string currentObjectInfo = String.Empty;

                if (columnNames.Length == 1 && columnNames[0].Equals("*"))
                {
                    foreach (var propData in obj.Properties)
                    {
                        if (!skipNames)
                        {
                            currentObjectInfo += propData.Name + " = ";
                        }
                        currentObjectInfo += (propData.Value != null) ? propData.Value.ToString() : "";
                        currentObjectInfo += ";";
                    }
                }
                else
                    for (int j = 0; j < columnNames.Length; j++)
                    {
                        if (!skipNames)
                        {
                            currentObjectInfo += columnNames[j] + " = ";
                        }
                        currentObjectInfo += (obj.Properties[columnNames[j]].Value != null) ? obj.Properties[columnNames[j]].Value.ToString() : "";
                        currentObjectInfo += ";";
                    }

                result.Add(currentObjectInfo);
            }

            return result.ToArray();
        }
    }
}