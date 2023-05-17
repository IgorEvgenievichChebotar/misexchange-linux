using System;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace ru.novolabs.SuperCore.Reporting
{
    public class TaskDescriptionList
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса "Список задач" (TaskDescriptionList)
        /// </summary>
        public TaskDescriptionList()
        {
            Tasks = new List<TaskDescription>();
        }

        /// <summary>
        /// Список описаний задач
        /// </summary>
        public List<TaskDescription> Tasks { get; set; }
    }

    /// <summary>
    /// Представляет описание задачи построения отчётов
    /// </summary>
    public class TaskDescription
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса "Описание задачи" (TaskDescription)
        /// </summary>
        public TaskDescription()
        {
            Name = String.Empty;
            XmlDefinition = String.Empty;
            Actions = new List<TaskAction>();
        }

        /// <summary>
        /// Имя
        /// </summary>
        public String Name
        {
            get;
            set;
        }
        /// <summary>
        /// Действия, которые необходимо выполнить
        /// </summary>
        public List<TaskAction> Actions { get; set; }

        public String XmlDefinition { get; set; }

        public PrinterSettings PrinterSettings { get; set; }
    }

    /// <summary>
    /// Представляет действие, которое необходимо выполнить
    /// </summary>
    public class TaskAction
    {
        private List<DocumentTemplate> documentTemplates = new List<DocumentTemplate>();

        /// <summary>
        /// Инициализирует новый экземпляр класса "Действие" (TaskAction)
        /// </summary>
        public TaskAction()
        {
            ReportFileName = String.Empty;
            Parameters = new List<Parameter>();
            //Templates = new List<Template>();
        }

        /// <summary>
        /// Имя файла описания отчёта
        /// </summary>
        public String ReportFileName { get; set; }
        /// <summary>
        /// Папка для сохранения подготовленных отчётов
        /// </summary>
        public String DestinationFolder { get; set; }

        public List<Parameter> Parameters { get; set; }

        //public List<Template> Templates { get; set; }
        public List<DocumentTemplate> DocumentTemplates
        {
            get { return documentTemplates; }
            set { documentTemplates = value; }
        }

        public Boolean FindDocumentTemplate(String documentTemplateName, out Int32 index)
        {
            Boolean result = false;
            index = -1;

            for (Int32 i = 0; i < DocumentTemplates.Count; i++)
                if (DocumentTemplates[i].Name == documentTemplateName)
                {
                    result = true;
                    index = i;
                    break;
                }

            return result;
        }

        public Boolean FindParameter(String parameterName, out Int32 index)
        {
            Boolean result = false;
            index = -1;

            for (Int32 i = 0; i < Parameters.Count; i++)
                if (Parameters[i].Name == parameterName)
                {
                    result = true;
                    index = i;
                    break;
                }

            return result;
        }
    }

    //public class Template
    //{
    //    public Template()
    //    {
    //        Name = String.Empty;
    //        Format = DocumentFormat.PDF;
    //    }

    //    public String Name { get; set; }

    //    public DocumentFormat Format { get; set; }
    //}

    public class Parameter
    {
        public Parameter()
        {
            Name = String.Empty;
            Value = String.Empty;
        }

        public String Name { get; set; }

        public String Value { get; set; }
    }
}