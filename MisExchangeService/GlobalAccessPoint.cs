using System;
using System.Data.Odbc;
using System.Linq;
using System.Reflection;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsDictionary;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.Interfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.CommunicatorInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using ru.novolabs.MisExchange.MainDependenceImplementation;

namespace ru.novolabs.MisExchangeService
{
    /// <summary>
    /// GAP - Global Access Point. Глобальная точка доступа данного приложения
    /// </summary>
    public static class GAP
    {
        static GAP()
        {
            Logger = new LoggerNull();
        }

        private static ExternalSystemDictionaryItem externalSystem;
        private static ServiceDB serviceDBManager;
        private static ExchangeHelper exchangeHelper;
        private static ExchangeHelper3 exchangeHelper3;
        private static ExportDictionariesHelper exportDictionariesHelper;
        private static ProcessStorageOperationsHelper processStorageOperationsHelper;

        public static ILogger Logger { get; set; }
        public static IProcessResultSettings ResultSettings { get; set; }
        public static IProcessResultCommunicator ResultCommunicator { get; set; }
        public static IPDFSignerSettings PDFSignerSettings { get; set; }
        public static INotifierLis NotifierLis { get; set; }
        public static IDictionaryCache DictionaryCache { get; set; }
        public static Func<object> DictionarySynchronizerFactory { get; set; }

        public static Func<Type, ExchangeHelper3> ExchangeHelper3Factory { private get; set; }

        internal static ExternalSystemDictionaryItem ExternalSystem
        {
            get
            {
                if (externalSystem == null)
                {
                    string externalSystemCode = ResultSettings.ExternalSystemCode;
                    externalSystem = ProgramContext.Dictionaries[LimsDictionaryNames.ExternalSystem, externalSystemCode] as ExternalSystemDictionaryItem;
                    if ((externalSystem == null) || (externalSystem.Removed))
                    {
                        throw new ApplicationException(String.Format("Внешняя система не найдена по коду [{0}]", externalSystemCode));
                    }
                }

                return externalSystem;
            }
        }

        internal static ServiceDB ServiceDBManager
        {
            get
            {
                if (serviceDBManager == null)
                {
                    String serviceDBConnectionString = ResultSettings.ServiceDBConnectionString;
                    if (String.IsNullOrEmpty(serviceDBConnectionString))
                        throw new ApplicationException("Не указана строка подключения к служебной базе данных");
                    serviceDBConnectionString = ConnectionStringHelper.GetConnStr(serviceDBConnectionString);
                    serviceDBManager = new ServiceDB(serviceDBConnectionString);
                }

                return serviceDBManager;
            }
        }

        internal static ExchangeHelper ExchangeHelper
        {
            get
            {
                if (exchangeHelper == null)
                {
                    Type helperType = GetHelperType();
                    exchangeHelper = (ExchangeHelper)Activator.CreateInstance(helperType);
                }
                return exchangeHelper;
            }
        }

        internal static ExchangeHelper3 ExchangeHelper3
        {
            get
            {
                return exchangeHelper3Lazy.Value;
            }
        }

        private static Type GetHelperType()
        {
            string exchangeMode = ResultSettings.ExchangeMode;

            var exchangeHelperTypes =
                (from type in Assembly.GetExecutingAssembly().GetTypes()
                 where (type.IsDefined(typeof(ExchangeHelperName), true)
                 && ((ExchangeHelperName)(type.GetCustomAttributes(typeof(ExchangeHelperName), false)[0])).Name == exchangeMode)
                 select type).ToList();

            if (exchangeHelperTypes.Count == 0)
                throw new ApplicationException(String.Format("Не найден хелпер с именем [{0}]", exchangeMode));
            return exchangeHelperTypes.First();
        }

        private static ExchangeHelper3 GetExchangeHelper3()
        {
            Type helperType = GetHelperType();
            exchangeHelper3 = ExchangeHelper3Factory(helperType);
            return exchangeHelper3;
        }

        private static Lazy<ExchangeHelper3> exchangeHelper3Lazy = new Lazy<ExchangeHelper3>(() => GetExchangeHelper3());

        internal static ExportDictionariesHelper ExportDictionariesHelper
        {
            get
            {
                if (exportDictionariesHelper == null)
                {
                    string exportDictionaryMode = ResultSettings.ExportDictionaryMode;

                    if (String.IsNullOrEmpty(exportDictionaryMode))
                        return null;

                    var exportHelperTypes =
                        (from type in Assembly.GetExecutingAssembly().GetTypes()
                         where (type.IsDefined(typeof(ExportDictionariesHelperName), true)
                         && ((ExportDictionariesHelperName)(type.GetCustomAttributes(typeof(ExportDictionariesHelperName), false)[0])).Name == exportDictionaryMode)
                         select type).ToList();

                    if (exportHelperTypes.Count == 0)
                        throw new ApplicationException(String.Format("Не найден хелпер с именем [{0}]", exportDictionaryMode));

                    exportDictionariesHelper = (ExportDictionariesHelper)Activator.CreateInstance(exportHelperTypes.First());
                }
                return exportDictionariesHelper;
            }
        }

        internal static ProcessStorageOperationsHelper ProcessStorageOperationsHelper
        {
            get
            {
                if (processStorageOperationsHelper == null)
                {
                    string processStorageOperationsMode = ResultSettings.ProcessStorageOperationsMode;

                    if (String.IsNullOrEmpty(processStorageOperationsMode))
                        return null;

                    var processStorageOperationsHelperTypes =
                        (from type in Assembly.GetExecutingAssembly().GetTypes()
                         where (type.IsDefined(typeof(ProcessStorageOperationsHelperName), true)
                         && ((ProcessStorageOperationsHelperName)(type.GetCustomAttributes(typeof(ProcessStorageOperationsHelperName), false)[0])).Name == processStorageOperationsMode)
                         select type).ToList();

                    if (processStorageOperationsHelperTypes.Count == 0)
                        throw new ApplicationException(String.Format("Не найден хелпер обработки складских данных с именем [{0}]", processStorageOperationsMode));

                    processStorageOperationsHelper = (ProcessStorageOperationsHelper)Activator.CreateInstance(processStorageOperationsHelperTypes.First());
                }
                return processStorageOperationsHelper;
            }
        }

        public class ServiceDB
        {
            public String DefaultConnectionString { get; private set; }

            public ServiceDB(String defaultConnectionString)
            {
                DefaultConnectionString = defaultConnectionString;

            }

            public OdbcConnection GetConnection()
            {
                return GetConnection(DefaultConnectionString);
            }

            public OdbcConnection GetConnection(String connectionString)
            {
                return new OdbcConnection(connectionString);
            }
        }

        private static Lazy<System.Globalization.NumberFormatInfo> numberFormatInfo = new Lazy<System.Globalization.NumberFormatInfo>(() => GetNumberFormatInfo());
        public static System.Globalization.NumberFormatInfo NumberFormatInfo { get { return numberFormatInfo.Value; } }
        private static System.Globalization.NumberFormatInfo GetNumberFormatInfo()
        {

            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.GetCultureInfo("ru");// CultureInfo.InstalledUICulture;
            var _numberFormatInfo = (System.Globalization.NumberFormatInfo)ci.NumberFormat.Clone();

            // Заменяем разделитель дробной части, взятый из региональных настроек точкой, т.к. в числах с плавающей точкой,
            // приходящих в XML от сервера ЛИМС, всегда дробная часть отделена точкой. Без этого действия ни один справочник,
            // содержащий числа с плавающей точкой не сможет быть загружен в DictionaryCache
            _numberFormatInfo.NumberDecimalSeparator = ".";
            return _numberFormatInfo;
        }
    }
}