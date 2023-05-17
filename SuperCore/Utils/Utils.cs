using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
//using System.Windows.Forms;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore
{
    public class Utils
    {
        public static List<DictionaryItem> GetDictionaryElementList(string dictionaryName, out Type itemType, bool needEmptyItem = true)
        {
            itemType = null;
            if (dictionaryName == null)
                return null;

            if (!ProgramContext.Dictionaries.DictionaryList.ContainsKey(dictionaryName))
            {
                /*MessageBox.Show(
                    String.Format("Справочник [{0}] не найден", dictionaryName),
                    "Ошибка заполнения выпадающего списка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );*/
                return null;
            }
            else
            {
                var dictionaryElements = ((IBaseDictionary)ProgramContext.Dictionaries[dictionaryName]).DictionaryElements;
                itemType = dictionaryElements.GetType().GetGenericArguments()[0];
                var attributes = itemType.GetCustomAttributes(typeof(StaticDictionary), false);
                bool isStaticDictionary = attributes.Length > 0;
                var list = dictionaryElements.Cast<DictionaryItem>().ToList().FindAll(de => !de.Removed);
                if (isStaticDictionary)
                    list.Sort((i1, i2) => i1.Id.CompareTo(i2.Id));
                else
                    list.Sort((i1, i2) => i1.Name.CompareTo(i2.Name));

                if (needEmptyItem)
                {
                    DictionaryItem emptyItem = (DictionaryItem)Activator.CreateInstance(itemType);
                    emptyItem.Id = -1;
                    list.Insert(0, emptyItem);
                }

                return list;
            }
        }
        public static void PrintProcessInfo()
        {
            Process proc = Process.GetCurrentProcess();
            Log.WriteText("PID: " + proc.Id.ToString());
            Log.WriteText("WorkingSet64: " + ((long)(proc.WorkingSet64 / 1000000)).ToString() + " MB");
            Log.WriteText("PeakWorkingSet64: " + ((long)(proc.PeakWorkingSet64 / 1000000)).ToString() + " MB");
            Log.WriteText("VirtualMemorySize64: " + ((long)(proc.VirtualMemorySize64 / 1000000)).ToString() + " MB");
            Log.WriteText("PeakVirtualMemorySize64: " + ((long)(proc.PeakVirtualMemorySize64 / 1000000)).ToString() + " MB");
            Log.WriteText("HandleCount: " + proc.HandleCount.ToString());
            Log.WriteText("ThreadCount: " + proc.Threads.Count.ToString());
            Log.WriteText("ProcessorAffinity: " + proc.ProcessorAffinity.ToString());
        }

#if (DEBUG)
        public static void PrintUnannotatedTypes(string[] clientExploringNamespaces, Type[] clientExceptTypes)
        {
            var exceptPropNames = new List<string>() { "Id", "AsReference" };
            var exceptDictItemNames = new List<string>() { "Name", "Code", "Mnemonics", "Id", "AsReference", "Removed" };
            var exceptBaseExceptionPropNames = new List<string>() { "Message", "Data", "InnerException", "TargetSite", "StackTrace", "HelpLink", "Source", "HResult" };
            var exceptListPropNames = new List<string>() { "Count", "Capacity" };
            var exploringNamespaces = new string[] 
            {
                "ru.novolabs.SuperCore.HemDictionary",
                "ru.novolabs.SuperCore.HemBusinessObjects",
                "ru.novolabs.SuperCore.LimsDictionary",
                "ru.novolabs.SuperCore.LimsBusinessObjects.Exchange",
                "ru.novolabs.SuperCore.LimsBusinessObjects.Outsource",
                "ru.novolabs.SuperCore.LimsBusinessObjects"
                //,"ru.novolabs.SuperCore"
            };
            var exceptTypes = new Type[] 
            {
                typeof(ru.novolabs.SuperCore.HemDictionary.HemDictionaryCache),
                typeof(ru.novolabs.SuperCore.LimsDictionary.LimsDictionaryCache),
                typeof(ru.novolabs.SuperCore.LimsBusinessObjects.RegistrationJournalFilter),
                typeof(ru.novolabs.SuperCore.LimsBusinessObjects.WorkJournalFilter)
            };

            var types =
                (from t in Assembly.GetExecutingAssembly().GetTypes()
                 where ((!t.IsInterface) && exploringNamespaces.Contains(t.Namespace) && (!exceptTypes.Contains(t)))
                 select t).ToList();

            var clientTypes =
                (from t in Assembly.GetCallingAssembly().GetTypes()
                 where ((!t.IsInterface) && clientExploringNamespaces.Contains(t.Namespace) && (!clientExceptTypes.Contains(t)))
                 select t).ToList();

            types.AddRange(clientTypes);

            int propCount = 0;
            int typeCount = 0;
            Type firstType = null;
            foreach (var type in types)
            {
                var sb = new StringBuilder();
                int typePropCount = 0;
                foreach (var prop in type.GetProperties())
                {
                    // Пропускаем свойства, помеченные атрибутом "CSN"
                    if (prop.IsDefined(typeof(CSN), true))
                    {
                        ValidateAnnotation(type, prop);
                        continue;
                    }
                    // Пропускаем индексаторы
                    if (prop.GetIndexParameters().Length > 0)
                        continue;
                    var sendToServerAttributes = prop.GetCustomAttributes(typeof(SendToServer), true);
                    // Пропускаем свойства, унаследованные от BaseObject (перечислены в списке "exceptNames")
                    if (type.IsSubclassOf(typeof(BaseObject)) && exceptPropNames.Contains(prop.Name))
                        continue;
                    // Пропускаем свойства, унаследованные от DictionaryItem (перечислены в списке "exceptDictItemNames")
                    if (type.IsSubclassOf(typeof(DictionaryItem)) && exceptDictItemNames.Contains(prop.Name))
                        continue;
                    // Пропускаем свойства, унаследованные от Exception (перечислены в списке "exceptBaseExceptionPropNames")
                    if (type.IsSubclassOf(typeof(Exception)) && exceptBaseExceptionPropNames.Contains(prop.Name))
                        continue;
                    // Пропускаем свойства, унаследованные от List<T> (перечислены в списке "exceptBaseExceptionPropNames")
                    if ((type.GetInterface("IList") != null) && exceptListPropNames.Contains(prop.Name))
                        continue;

                    // List

                    sb.Append(String.Format("\t\t property {0}\r\n", prop.Name));
                    propCount++;
                    typePropCount++;
                    if (firstType == null)
                    {
                        firstType = type;
                    }
                }
                if (typePropCount > 0)
                {
                    typeCount++;
                    sb.Insert(0, String.Format("Type [{0}]:\r\n", type.FullName));
                    Console.WriteLine(sb.ToString());
                }
            }
            Trace.WriteLine("========================================");
            Trace.WriteLine(String.Format("Количество неаннотированных классов = {0}", typeCount));
            Trace.WriteLine(String.Format("Количество неаннотированных свойств = {0}", propCount));
            Trace.WriteLine("========================================");

            if (typeCount > 0)
            {
                var firstTypePropNames =
                    (from prop in firstType.GetProperties()
                     where !prop.IsDefined(typeof(CSN), true)
                     select prop.Name).ToList();

                var sb = new StringBuilder(firstTypePropNames.Count);
                firstTypePropNames.ForEach(n => sb.Append(n + "\n\r"));

                /*if (Environment.UserInteractive)
                {
                    MessageBox.Show(
                        String.Format("Остались классы, свойства которых неаннотированы атрибутом \"CSN\".\r\rПервый из классов:\r\r{0}\r\rСвойства: \r\r{1}\rПолный список неаннотированных свойств смотрите в окне \"Output\"", firstType.FullName, sb.ToString()),
                        "Недоработка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
                }*/
            }
        }

        private static void ValidateAnnotation(Type type, PropertyInfo prop)
        {
            string attributeArgumentValue = ((CSN)prop.GetCustomAttributes(typeof(CSN), true)[0]).GetSp3();
            /*if (attributeArgumentValue != prop.Name)
            {
                MessageBox.Show(
                        String.Format("В следующем классе обнаружено несоответствие значения аргумента атрибута \"CSN\" имени соответствующего свойства.\r\rКласс:\r\r{0}\r\rИмя свойства: {1} - значение аргумента атрибута: {2}", type.FullName, prop.Name, attributeArgumentValue),
                        "Недоработка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                        );
            }*/
        }
        #endif
    }
}
