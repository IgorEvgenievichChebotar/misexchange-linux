using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Dynamic;

namespace ru.novolabs.SuperCore
{
    public class SettingNotFoundException : ApplicationException
    {
        public SettingNotFoundException(String message): base(String.Format("Setting not found by name [{0}]", message)){} 
    }    
    
    public class ProgramSettings
    {
        private Hashtable settingsTable = new Hashtable();
        
        /// <summary>
        /// Поставщик форматирования чисел для клиент-серверного взаимодействия
        /// </summary>
        [XmlIgnore]
        public System.Globalization.NumberFormatInfo NumberFormatInfo { get; set; }
        /// <summary>
        /// Поставщик форматирования даты/времени
        /// </summary>
        [XmlIgnore]
        public System.Globalization.DateTimeFormatInfo DateTimeFormatInfo { get; set; }

        private String loggingLevel = null;
        public String LoggingLevel
        {
            get { return loggingLevel; }
            set { loggingLevel = value; }
        }

        private bool notLogSQLQueries = false;
        public bool NotLogSQLQueries
        {
            get { return notLogSQLQueries; }
            set { notLogSQLQueries = value; }
        }

        public void RemoveSetting(string settingName)
        {
            if (settingsTable.ContainsKey(settingName))
                settingsTable.Remove(settingName);        
        }

        public object this[string settingName, Boolean isMandatory = true]
        {
            get
            {
                object result = settingsTable[settingName];
                if (isMandatory)
                {
                    if (result == null)
                        throw new SettingNotFoundException(settingName);
                }
                return result;
            }          
            set { settingsTable[settingName] = value; }
        }

        public ProgramSettings()
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.GetCultureInfo("ru");// CultureInfo.InstalledUICulture;
            NumberFormatInfo = (System.Globalization.NumberFormatInfo)ci.NumberFormat.Clone();
            DateTimeFormatInfo = (System.Globalization.DateTimeFormatInfo)ci.DateTimeFormat.Clone();          

            // Заменяем разделитель дробной части, взятый из региональных настроек точкой, т.к. в числах с плавающей точкой,
            // приходящих в XML от сервера ЛИМС, всегда дробная часть отделена точкой. Без этого действия ни один справочник,
            // содержащий числа с плавающей точкой не сможет быть загружен в DictionaryCache
            NumberFormatInfo.NumberDecimalSeparator = ".";
        }

        /// <summary>
        /// Загружает настройки из указанного файла
        /// </summary>
        /// <param name="settingsFileName"></param>
        public void LoadSettings(String settingsFileName)
        {
            if (!File.Exists(settingsFileName))
            {
                throw new ApplicationException(String.Format("File not found by path [{0}]", settingsFileName));
            }
            
            try
            {
                ObjectReader Reader = new ObjectReader();

                string xmlSettings = string.Empty;
                string errorMessage = string.Empty;
                int errorCode = 0;

                xmlSettings = File.ReadAllText(settingsFileName, Encoding.GetEncoding(1251));

                XMLConst.IsPhox = xmlSettings.Contains(XMLConst.DTDPhox);

                Reader.ReadXMLSettings(xmlSettings, settingsTable, ref errorCode, ref errorMessage);

                Object objLoggingLevel = this["loggingLevel", false];
                loggingLevel = (objLoggingLevel != null) ? (String)objLoggingLevel : SystemLoggingLevels.LOGIN_LEVEL_OFF;

                Object objNotLogSQLQueries = this["notLogSQLQueries", false];
                notLogSQLQueries = objNotLogSQLQueries != null ? (bool)objNotLogSQLQueries : false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Ошибка при загрузке настроек приложения: {0}", ex.Message), ex);
            }
        }

        /// <summary>
        /// Сохраняет настройки в указанный файл
        /// </summary>
        /// <param name="settingsFileName"></param>
        public void SaveSettings(String settingsFileName)
        {
            FileStream settingsFile = new FileStream(settingsFileName, FileMode. Create);
            StreamWriter fileWriter = new StreamWriter(settingsFile, Encoding.GetEncoding(1251));
            ObjectWriter writer = new ObjectWriter();
            String xmlSettings = String.Empty;

            xmlSettings = writer.MakeXMLSettings(settingsTable);

            fileWriter.WriteLine(xmlSettings);
            fileWriter.Close();
            settingsFile.Close();
        }
    }

    public class JournalExportSettings
    {
        public JournalExportSettings()
        {
            Journal_ID = String.Empty;
            FilterFileName = String.Empty;
        }

        public String Journal_ID { get; set; }
        public String FilterFileName { get; set; }
        public Int32 ProductJournalId { get; set; }
    }
}
