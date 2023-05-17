using ru.novolabs.MisExchange.MainDependenceInterfaces;
using ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces;
using ru.novolabs.SuperCore;
using System;

namespace ru.novolabs.MisExchange.MainDependenceImplementation
{
    internal class Settings : ILoadSettings, IProcessRequestSettings, IProcessResultSettings, IFileLoggerSettings, ITimeFileSubjectSettings, IPDFSignerSettings
    {
        public void LoadSettings(string settingsFileName)
        {
            ProgramContext.Settings = new ProgramSettings();
            ProgramContext.Settings.LoadSettings(settingsFileName);
        }
        public void SaveSettings(string settingsFileName)
        {
            ProgramContext.Settings.SaveSettings(settingsFileName);
        }
        //Settings
        public string DefaultSampleBlankCode => (string)ProgramContext.Settings["defaultSampleBlankCode"];
        public bool UseUniqueSampleBarcoding
        {
            get
            {
                bool? setting = (bool?)ProgramContext.Settings["useUniqueSampleBarcoding", false];
                return setting != null ? setting.Value : false;
            }
        }
        public string RegistrationFormCode => (string)ProgramContext.Settings["registrationFormCode"];

        public string ServerAddress => (string)ProgramContext.Settings["serverAddress"];

        public string PatientCodePrefix => (string)ProgramContext.Settings["patientCodePrefix"];
        public string ExchangeMode => (string)ProgramContext.Settings["exchangeMode"];

        public string ExportDictionaryMode => (string)ProgramContext.Settings["exportDictionaryMode", false];

        public string ProcessStorageOperationsMode => (string)ProgramContext.Settings["processStorageOperationsMode", false];

        public string ServiceDBConnectionString => (string)ProgramContext.Settings["serviceDBConnectionString"];
        public string ExternalSystemCode => (string)ProgramContext.Settings["externalSystemCode"];
        public string ServiceAddress => (string)ProgramContext.Settings["serviceAddress"];
        public string RequestCacherSettings => (string)ProgramContext.Settings["requestsCacherSettings"];
        public bool IsAllowedToAddUserValues => (bool?)ProgramContext.Settings["isAllowedToAddUserValues", false] ?? false;
        public bool IsAltTargetCodeInRequest => (bool?)ProgramContext.Settings["IsAltTargetCodeInRequest", false] ?? false;

        #region General Settings
        public bool IsCryptedCredentialsAndConnStrs => (bool?)ProgramContext.Settings["isCryptedCredentialsAndConnStrs", false] ?? false;
        public bool IsFullConnectionStringCrypted => (bool?)ProgramContext.Settings["isFullConnectionStringCrypted", false] ?? false;
        public string LoggingLevel => (string)ProgramContext.Settings["loggingLevel", false];
        public string LogFile => (string)ProgramContext.Settings["logFile"];
        public bool RotateLogs => ((bool?)ProgramContext.Settings["rotateLogs", false]) ?? false;
        public bool VersionAfter129 => ((bool?)ProgramContext.Settings["versionAfter129", false]) ?? false;
        public int? TaskManagerWaitInterval => (int?)ProgramContext.Settings["taskManagerWaitInterval", false];
        #endregion

        #region Backward Compability
        public bool IssueResultOrganizationCode => (bool?)ProgramContext.Settings["issue_Result_OrganizationCode", false] ?? false;
        public bool IssueResultOrganizationName => (bool?)ProgramContext.Settings["issue_Result_OrganizationName", false] ?? false;
        public bool IssueResultWorkPatientGroupCode => (bool?)ProgramContext.Settings["issue_Result_Work_PatientGroupCode", false] ?? false;
        public bool IssueResultWorkPatientGroupName => (bool?)ProgramContext.Settings["issue_Result_Work_PatientGroupName", false] ?? false;
        public bool IssueResultImages => (bool?)ProgramContext.Settings["issue_Result_Images", false] ?? false;
        public bool IssueResultDepartmentNr => (bool?)ProgramContext.Settings["issue_Result_DepartmentNr", false] ?? false;
        public bool IssueResultEndDate => (bool?)ProgramContext.Settings["issue_Result_EndDate", false] ?? false;
        public bool IssueResultRegistrationDate => (bool?)ProgramContext.Settings["issue_Result_Registration_Date", false] ?? false;
        public bool IssueResultEquipmentCode => (bool?)ProgramContext.Settings["issue_Result_Equipment_Code", false] ?? false;
        public bool IssueResultEquipmentName => (bool?)ProgramContext.Settings["issue_Result_Equipment_Name", false] ?? false;
        public bool IssueResultWorkDiameter => (bool?)ProgramContext.Settings["issue_Result_Work_Diameter", false] ?? false;
        public bool IssueResultDepartmentCode => (bool?)ProgramContext.Settings["issue_Result_Department_Code", false] ?? false;
        public bool IssueResultDepartmentName => (bool?)ProgramContext.Settings["issue_Result_Department_Name", false] ?? false;
        public bool IssueResultDoctorCode => (bool?)ProgramContext.Settings["issue_Result_Doctor_Code", false] ?? false;
        public bool IssueResultDoctorName => (bool?)ProgramContext.Settings["issue_Result_Doctor_Name", false] ?? false;
        public bool IssueResultSamplingDate => (bool?)ProgramContext.Settings["issue_Result_Sampling_Date", false] ?? false;
        public bool IssueResultPriority => (bool?)ProgramContext.Settings["issue_Result_Priority", false] ?? false;
        public bool IssueResultPayCategoryCode => (bool?)ProgramContext.Settings["issue_Result_PayCategory_Code", false] ?? false;
        public bool IssueResultPayCategoryName => (bool?)ProgramContext.Settings["issue_Result_PayCategory_Name", false] ?? false;
        #endregion

        #region Testing
        public string __fileNameIds => (string)ProgramContext.Settings["__fileNameIds", false];
        public bool __isOneRequestResultExport => (Nullable<Boolean>)ProgramContext.Settings["__isOneRequestResultExport", false] ?? false;
        public string FileNameIds => (string)ProgramContext.Settings["fileNameIds", false];
        public bool IsOneRequestResultExport => (Nullable<Boolean>)ProgramContext.Settings["isOneRequestResultExport", false] ?? false;
        #endregion

        public bool ResetSampleDeliveryDate => (bool?)ProgramContext.Settings["resetSampleDeliveryDate", false] ?? false;

        public DateTime? DefaultSampleDeliveryDate => (DateTime?)ProgramContext.Settings["defaultSampleDeliveryDate", false];

        public bool IsSynchronizeDictionary => (bool?)ProgramContext.Settings["isSynchronizeDictionary", false] ?? false;

        public bool ReplaceResultFileDotsToCommas => (bool?)ProgramContext.Settings["replaceResultFileDotsToCommas", false] ?? false;

        public bool ResetPatientCode => (bool?)ProgramContext.Settings["resetPatientCode", false] ?? false;

        public bool SortWorkResultsByTargetProfile => (bool?)ProgramContext.Settings["sortWorkResultsByTargetProfile", false] ?? false;

        public int SynchronizeDictionaryInterval
        {
            get
            {
                object setting = ProgramContext.Settings["synchronizeDictionaryInterval", false];
                if (setting == null)
                {
                    return 0;
                }

                return int.Parse(setting.ToString());
            }
        }

        public bool IsAllowParallelResults => (bool?)ProgramContext.Settings["isAllowParallelResults", false] ?? false;

        public int TimeFileBreak
        {
            get
            {
                object setting = ProgramContext.Settings["timeFileBreak", false];
                if (setting == null)
                {
                    return 0;
                }

                return int.Parse(setting.ToString());
            }
        }

        public string TimeFileName => (string)ProgramContext.Settings["timeFileName", false];

        public string TimeFileFormat => (string)ProgramContext.Settings["timeFileFormat", false];

        #region PDFSignature
        public string CryptcpFullPath => (string)ProgramContext.Settings["cryptcpFullPath", false];
        public string PdfSignerFullPath => (string)ProgramContext.Settings["pdfSignerFullPath", false];
        public bool OrganizationSignatureRequired => (bool?)ProgramContext.Settings["organizationSignatureRequired", false] ?? false;
        public string DocumentsInDirPath => (string)ProgramContext.Settings["documentsInDirPath", false];
        public string SignaturesOutDirPath => (string)ProgramContext.Settings["signaturesOutDirPath", false];
        public string EmployeeCertificateSearchCriterion => (string)ProgramContext.Settings["employeeCertificateSearchCriterion", false];
        public string EmployeeCertificatePin => (string)ProgramContext.Settings["employeeCertificatePin", false];
        public string OrganizationCertificateSearchCriterion => (string)ProgramContext.Settings["organizationCertificateSearchCriterion", false];
        public string OrganizationCertificatePin => (string)ProgramContext.Settings["organizationCertificatePin", false];
        #endregion
    }
}