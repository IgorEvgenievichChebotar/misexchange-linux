using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace ru.novolabs.SuperCore
{
    /// <summary>
    /// Представляет класс, исполняющий SQL-запросы к любой СУБД через ODBC интерфейс
    /// </summary>
    public static class SqlQueryExecuter
    {
        private const Char CONST_PARAMETER_CHAR = ':';

        /// <summary>
        /// Выполняет SQL-запрос к любой СУБД через OLEDB/ODBC интерфейс, возвращая таблицу данных DataTable
        /// </summary>
        /// <param name="connectionString">ODBC-строка подключения</param>
        /// <param name="query">Текст SQL-запроса. Может содержать параметры</param>
        /// <param name="paramValues">Параметры запроса</param>
        /// <returns>Таблица данных, размещённая в памяти</returns>
        public static DataTable ExecuteSqlQuery(string connectionString, string query, Dictionary<string, string> paramValues)
        {
            // Пример строки подключения к Microsoft SQL Server:
            // 1) OLEDB интерфейс:  Provider=SQLOLEDB.1;Data Source=ms_sql_server_instance_name;Initial Catalog=database_name;User ID=user_name;Password=password_of_user
            // 2) ODBC  интерфейс:  Driver={SQL Server};Server=ms_sql_server_instance_name;Database=database_name;Uid=user_name;Pwd=password_of_user

            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentException(ru.novolabs.SuperCore.Properties.Resources.EmptyConnectionString);
            if (String.IsNullOrEmpty(query))
                throw new ArgumentException(ru.novolabs.SuperCore.Properties.Resources.EmptyQueryText);

            // Проверяем текст запроса на наличие запрещённых ключевых слов INSERT, UPDATE, DELETE
            string checkupString = query;
            Regex regEx = new Regex(@"insert[\s]*into[\s]*@", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in regEx.Matches(checkupString))
                checkupString = checkupString.Replace(m.Value, String.Empty);
            if ((checkupString.ToUpper().Contains("UPDATE")) || (checkupString.ToUpper().Contains("DELETE")))
                throw new InvalidOperationException("Only \"SELECT\" and \"INSERT INTO @\" (\"INSERT INTO #\") clauses are allowed.");

            DbConnection connection;
            // В строке подключения есть слово "Provider", что является признаком OLEDB-строки подключения
            if (connectionString.ToLower().IndexOf("provider") >= 0)
                connection = new OleDbConnection(connectionString);
            // В строке подключения есть слово "Driver", что является признаком ODBC-строки подключения
            else if (connectionString.ToLower().IndexOf("driver") >= 0)
                connection = new OdbcConnection(connectionString);
            else
                throw new ApplicationException("Database interface not defined");

            DataTable table = new DataTable();
            DbCommand command = connection.CreateCommand();
            command.CommandTimeout = 600;
            using (connection)
            {
                using (command)
                {
                    connection.Open();
                    PrepareCommand(command, query, paramValues);
                    using (var reader = command.ExecuteReader())
                        table.Load(reader);
                }
            }
            return table;
        }

        ///<summary>
        ///Выполняет SQL-запрос к любой СУБД через OLEDB/ODBC интерфейс, возвращая таблицу данных DataTable
        ///</summary>
        ///<param name="connectionString">OLEDB/ODBC-строка подключения</param>
        ///<param name="query">Текст SQL-запроса</param>
        ///<returns>Таблица данных, размещённая в памяти</returns>
        public static DataTable ExecuteSqlQuery(string connectionString, string query)
        {
            // Пример строки подключения к Microsoft SQL Server:
            // 1) OLEDB интерфейс:  Provider=SQLOLEDB.1;Data Source=ms_sql_server_instance_name;Initial Catalog=database_name;User ID=user_name;Password=password_of_user
            // 2) ODBC  интерфейс:  Driver={SQL Server};Server=ms_sql_server_instance_name;Database=database_name;Uid=user_name;Pwd=password_of_user

            return ExecuteSqlQuery(connectionString, query, null);
        }

        /// <summary>
        /// Подготавливает OleDb/Odbc-команду, к выполнению, обрабатывая текст запроса и добавляя параметры
        /// </summary>
        /// <param name="command">OleDb/Odbc-команда</param>
        /// <param name="query">Текст SQL-запроса</param>
        /// <param name="paramValues">Параметры запроса</param>
        private static void PrepareCommand(DbCommand command, string query, Dictionary<string, string> paramValues)
        {
            if ((paramValues == null) || (paramValues.Count == 0))
            {
                command.CommandText = query;
                return;
            }

            // Задачей подготовки команды является корректное добавление позиционных параметров (соблюдение порядка добавлениия в команду в соответствии с позицией в запросе). Всвязи с тем,
            // что имя каждого параметра может встречаться в тексте запроса неограниченное кол-во раз, каждое вхождение имени параметра необходимо рассматривать независимо и определить 
            // индексы всех вхождений имён параметров в запросе. Для этого формируем список кортежей, которые содержат индекс вхождения имени параметра в строке запроса и само имя параметра
            try
            {
                var parametersIndexes = new List<Tuple<int, string>>();
                foreach (string parameterName in paramValues.Keys)
                {
                    List<Tuple<int, string>> indexes = GetParameterNameIndexes(query, parameterName);
                    parametersIndexes.AddRange(indexes);
                }
                // Если в запросе нет параметров
                if (parametersIndexes.Count == 0)
                {
                    command.CommandText = query;
                }
                else
                {
                    parametersIndexes.Sort((i1, i2) => i1.Item1.CompareTo(i2.Item1));
                    // Далее для каждого вхождения имени параметра заменяем его на символ '?' и добавляем соответствующий параметр со значением в объект "Команда"
                    var sb = new StringBuilder(query.Length);
                    for (int i = 0; i < parametersIndexes.Count; i++)
                    {
                        var parameterIndex = parametersIndexes[i];
                        string parameterName = parameterIndex.Item2;
                        int startIndex = i == 0 ? 0 : parametersIndexes[i - 1].Item1 + (parametersIndexes[i - 1].Item2.Length + 1/*Длина "маркера параметра" CONST_PARAMETER_CHAR*/);
                        //Log.WriteText("startIndex = [{0}], parameterIndex.Item1 = [{1}], parameterIndex.Item1 - startIndex = [{2}]", startIndex, parameterIndex, parameterIndex.Item1 - startIndex); 
                        sb.Append(query.Substring(startIndex, parameterIndex.Item1 - startIndex) + '?');
                        DbParameter parameter = command.GetType().Equals(typeof(OleDbCommand)) ? (DbParameter)new OleDbParameter(parameterName, paramValues[parameterName]) : new OdbcParameter(parameterName, paramValues[parameterName]);
                        command.Parameters.Add(parameter);
                    }
                    // Добавляем остаток запроса
                    var lastParameterIndex = parametersIndexes[parametersIndexes.Count - 1];
                    int endOfLastParameterIndex = lastParameterIndex.Item1 + lastParameterIndex.Item2.Length + 1/*Длина "маркера параметра" CONST_PARAMETER_CHAR*/;
                    string s = query.Substring(endOfLastParameterIndex, query.Length - endOfLastParameterIndex);
                    sb.Append(query.Substring(endOfLastParameterIndex, query.Length - endOfLastParameterIndex));
                    command.CommandText = sb.ToString();
                }

                if ((ProgramContext.Settings != null) && (ProgramContext.Settings.LoggingLevel == SystemLoggingLevels.LOGIN_LEVEL_DEBUG) && (!ProgramContext.Settings.NotLogSQLQueries))
                {
                    Log.WriteText(String.Format("Executing Query: {0}", command.CommandText));
                    foreach (DbParameter param in command.Parameters)
                        Log.WriteText(String.Format("Parameter name=[{0}], type=[{1}], value=[{2}]", param.ParameterName, param.DbType, param.Value));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new ApplicationException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Получает индексы всех вхождений имени параметра в запрос
        /// </summary>
        /// <param name="query">Текст SQL-запроса</param>
        /// <param name="parameterName">Имя параметра</param>
        /// <returns>Список кортежей, которые содержат индекс вхождения имени параметра в строке запроса и само имя параметра</returns>
        private static List<Tuple<int, string>> GetParameterNameIndexes(string query, string parameterName)
        {
            var indexes = new List<Tuple<int, string>>();
            int index = -1;
            int offset = 0;
            while ((index = GetParameterNameIndex(query, parameterName, offset)) >= 0)
            {
                indexes.Add(Tuple.Create<int, string>(index, parameterName));
                offset = index + 1 /*Длина "маркера параметра" CONST_PARAMETER_CHAR*/ + parameterName.Length;
            }
            return indexes;
        }

        /// <summary>
        /// Получает индекс первого вхождения имени параметра в запрос с учётом смещения интересующей подстроки в строке query
        /// </summary>
        /// <param name="query">Текст SQL-запроса</param>
        /// <param name="parameterName">Имя параметра</param>
        /// <param name="offset">Смещение интересующей подстроки в строке SQL-запроса</param>
        /// <returns>Индекс первого вхождения имени параметра в запрос</returns>
        private static int GetParameterNameIndex(string query, string parameterName, int offset)
        {
            string tmpStr = query.Substring(offset);
            int parameterNameIndex = tmpStr.IndexOf(CONST_PARAMETER_CHAR + parameterName);
            return parameterNameIndex != -1 ? parameterNameIndex + offset : parameterNameIndex;
        }

        /// <summary>
        /// Выполняет SQL-команду для любой СУБД через OLEDB/ODBC интерфейс, возвращая кол-во обработанных записей
        /// </summary>
        /// <param name="connectionString">OLEDB/ODBC-строка подключения</param>
        /// <param name="commandText">Текст SQL-команды. Может содержать параметры</param>
        /// <param name="paramValues">Параметры запроса</param>
        /// <returns>количество обработанных записей</returns>
        public static int ExecuteSqlCommand(string connectionString, string commandText, Dictionary<string, string> paramValues)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentException(ru.novolabs.SuperCore.Properties.Resources.EmptyConnectionString);
            if (String.IsNullOrEmpty(commandText))
                throw new ArgumentException(ru.novolabs.SuperCore.Properties.Resources.EmptyCommandText);

            // Пример строки подключения к Microsoft SQL Server:
            // 1) OLEDB интерфейс:  Provider=SQLOLEDB.1;Data Source=ms_sql_server_instance_name;Initial Catalog=database_name;User ID=user_name;Password=password_of_user
            // 2) ODBC  интерфейс:  Driver={SQL Server};Server=ms_sql_server_instance_name;Database=database_name;Uid=user_name;Pwd=password_of_user

            DbConnection connection;
            // В строке подключения есть слово "Provider", что является признаком OLEDB-строки подключения
            if (connectionString.ToLower().IndexOf("provider") >= 0)
                connection = new OleDbConnection(connectionString);
            // В строке подключения есть слово "Driver", что является признаком ODBC-строки подключения
            else if (connectionString.ToLower().IndexOf("driver") >= 0)
                connection = new OdbcConnection(connectionString);
            else
                throw new ApplicationException("Database interface not defined");

            DataTable table = new DataTable();
            DbCommand command = connection.CreateCommand();
            using (connection)
            {
                using (command)
                {
                    connection.Open();
                    PrepareCommand(command, commandText, paramValues);
                    return command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Выполняет SQL-команду для любой СУБД через OLEDB/ODBC интерфейс, возвращая кол-во обработанных записей
        /// </summary>
        /// <param name="connectionString">OLEDB/ODBC-строка подключения</param>
        /// <param name="commandText">Текст SQL-команды. Может содержать параметры</param>
        /// <returns>количество обработанных записей</returns>
        /// 
        public static int ExecuteSqlCommand(string connectionString, string commandText)
        {
            // Пример строки подключения к Microsoft SQL Server:
            // 1) OLEDB интерфейс:  Provider=SQLOLEDB.1;Data Source=ms_sql_server_instance_name;Initial Catalog=database_name;User ID=user_name;Password=password_of_user
            // 2) ODBC  интерфейс:  Driver={SQL Server};Server=ms_sql_server_instance_name;Database=database_name;Uid=user_name;Pwd=password_of_user

            return ExecuteSqlCommand(connectionString, commandText, null);
        }
    }
}