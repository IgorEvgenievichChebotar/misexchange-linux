using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.HL7.Files
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
            SourcePath = "Source";
            ArchivePath = "Archive";
            ErrorPath = "Errors";
            ResultsPath = "Results";
            BackupPath = null;
            DictionaryExportPath = null;
            PatientCodePrefix = "";
            FileGroupDirectioriesList = new List<FileGroupDirectories>();
        }

        public string SourcePath { get; set; }
        public string ArchivePath { get; set; }
        public string ErrorPath { get; set; }
        public string ResultsPath { get; set; }
        public string BackupPath { get; set; }
        public string DictionaryExportPath { get; set; }
        public String PatientCodePrefix { get; set; }
        public String HospitalCode { get; set; }
        public String HospitalName { get; set; }

        public FileGroupingMode FileGroupingMode { get; set; }

        public List<FileGroupDirectories> FileGroupDirectioriesList { get; set; }
    }
}
