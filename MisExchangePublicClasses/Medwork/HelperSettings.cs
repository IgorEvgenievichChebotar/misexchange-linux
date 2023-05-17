using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork
{
    public class FileGroupDirectories
    {
        public string GroupCode { get; set; }
        
        public string SourcePath { get; set; }
        public string ArchivePath { get; set; }
        public string ErrorPath { get; set; }
        public string ResultsPath { get; set; }
    }

    public enum FileGroupingMode
    {
        None,
        ByHospital    
    }
    
    [System.Reflection.Obfuscation]
    public class HelperSettings
    {
        public HelperSettings()
        {
            SourcePath = "Requests";
            ArchivePath = "Archive";
            ErrorPath = "Errors";
            ResultsPath = "Results";
            RequestAcknowledgmentPath = "RequestAcknowledgment";
            ResultAcknowledgmentPath = "ResultAcknowledgment";
            BackupPath = null;
            DictionaryExportPath = null;
            IsEnabledMessageCaching = false;
            EncodingCodePage = "UTF-8";

            FileGroupDirectioriesList = new List<FileGroupDirectories>();
        }

        public string SourcePath { get; set; }
        public string ArchivePath { get; set; }
        public string ErrorPath { get; set; }
        public string ResultsPath { get; set; }
        public string RequestAcknowledgmentPath { get; set; }
        public string ResultAcknowledgmentPath { get; set; }     

        public string BackupPath { get; set; }
        public string DictionaryExportPath { get; set; }

        public FileGroupingMode FileGroupingMode { get; set; }

        public List<FileGroupDirectories> FileGroupDirectioriesList { get; set; }

        public bool IsEnabledMessageCaching { get; set; }
        public string EncodingCodePage { get; set; }
    }

    [System.Reflection.Obfuscation]
    public class ExportDirectoryHelperSettings
    {
        public ExportDirectoryHelperSettings()
        {
            SourcePath = "DirectoryExportSource";
            Output = "DirectoryExport";
            Archive = "Archive";
            Errors = "Errors";
        }

        public string SourcePath { get; set; }
        public string Output { get; set; }

        public string Archive { get; set; }
        public string Errors { get; set; }
    }
}