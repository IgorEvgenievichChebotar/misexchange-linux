using System.Collections.Generic;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.CommonBusinesObjects;
using System.ComponentModel;
using System;

namespace ru.novolabs.SuperCore.Reporting
{
    public class DocumentTemplate
    {
        public DocumentTemplate()
        {
            Format = DocumentFormat.PDF;
            ExportSettings = new PdfExportSettings();
        }

        [Browsable(false)]
        public string TemplateFileName { get; set; }
        [Category("1. Основные")]
        public string Name { get; set; }
        [Category("1. Основные")]
        public bool IsNotificationBody { get; set; }
        [Category("1. Основные")]
        public DocumentFormat Format { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("2. Настройки экспорта")]
        public ExportSettings ExportSettings { get; set; }

        public void UpdateFormat(DocumentFormat newFormat)
        {
            ExportSettings = CreateExportSettings(newFormat);
        }

        private ExportSettings CreateExportSettings(DocumentFormat newFormat)
        {
            switch (newFormat)
            {                   
                case DocumentFormat.PDF:
                    return new PdfExportSettings();
                case DocumentFormat.Excel2007:
                    return new Excel2007ExportSettings();
                case DocumentFormat.JPEG:
                    return new JpegExportSettings();
                case DocumentFormat.PNG:
                    return new PngExportSettings();
                case DocumentFormat.HTML:
                    return new HtmlExportSettings();
                case DocumentFormat.RTF:
                    return new RtfExportSettings();
                case DocumentFormat.Word2007:
                    return new Word2007ExportSettings();
                case DocumentFormat.BMP:
                    return new ImageExportSettings();
                default:
                    return null;
            }
        }
    }

    public class AttachmentInfo
    {
        public string FileName { get; set; }    
    }
    
    public class ReportDescription
    {
        private bool isEncryptedConnStr = false;
        private string connectionString = string.Empty;
        private string name = string.Empty;
        private List<Query> dataSetQueries = new List<Query>();
        private List<DocumentTemplate> documentTemplates = new List<DocumentTemplate>();
        private List<AttachmentInfo> attachments = new List<AttachmentInfo>();

        public ReportDescription()
        {
            DefaultDocumentFormat = DocumentFormat.PDF;
        }
        
        public long Id { get; set; }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string ConnectionString
        {
            get
            {
                if (!isEncryptedConnStr && !String.IsNullOrEmpty(connectionString))
                {
                    isEncryptedConnStr = true;
                    connectionString = ConnectionStringCryptor.DecryptFullConnectionString(connectionString);
                    return connectionString;
                }
                return connectionString;
            }
            set { connectionString = value; }
        }

        public List<Query> DataSetQueries
        {
            get { return dataSetQueries; }
            set { dataSetQueries = value; }
        }

        public List<DocumentTemplate> DocumentTemplates
        {
            get { return documentTemplates; }
            set { documentTemplates = value; }
        }

        public List<AttachmentInfo> Attachments
        {
            get { return attachments; }
            set { attachments = value; }
        }

        public string DocumentNameTemplate { get; set; }
        public bool MultiDocument
        {
            get { return this.DataSetQueries.Exists(query => query.IsMaster); }
            set { /* nop */ } // пустой сеттер нужен для успешной сериализации свойства в XML (без него свойство игнорируется)

        }
        [Obsolete]
        public DocumentFormat DefaultDocumentFormat { get; set; }

    }

    public class Query
    {
        private string name = string.Empty;
        private string strquery = string.Empty;
        private List<ControlRow> queryParameters = new List<ControlRow>();
        private List<ControlRow> additionalParameters = new List<ControlRow>();
        private List<string> dependentQueries = new List<string>();

        public List<ControlRow> QueryParameters
        {
            get { return queryParameters; }
            set { queryParameters = value; }
        }

        public List<ControlRow> AdditionalParameters
        {
            get { return additionalParameters; }
            set { additionalParameters = value; }
        }

        public string Strquery
        {
            get { return strquery; }
            set { strquery = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlArrayItem(ElementName = "QueryName")]
        public List<string> DependentQueries
        {
            get { return dependentQueries; }
            set { dependentQueries = value; }
        }

        public bool IsMaster 
        { 
            get { return DependentQueries.Count > 0; }
            set { /* nop */ } // пустой сеттер нужен для успешной сериализации свойства в XML (без него свойство игнорируется)
        }

        public bool IsDetail(ReportDescription reportDescription)
        {
            bool result = false;
            foreach (var query in reportDescription.DataSetQueries)
            {
                if (query.Name != this.Name)
                {
                    if (query.DependentQueries.Contains(this.Name))
                    {
                        result = true;
                        break;
                    }                
                }
            }
            return result;
        }
    }
}
