namespace ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces
{
    public interface IProcessResultSettings
    {
        string DefaultSampleBlankCode { get; }
        string ServerAddress { get; }

        string ExchangeMode { get; }

        string ExportDictionaryMode { get; }

        string ProcessStorageOperationsMode { get; }

        string ServiceDBConnectionString { get; }

        string ExternalSystemCode { get; }

        bool IsSynchronizeDictionary { get; }
        int SynchronizeDictionaryInterval { get; }

        bool IsAllowParallelResults { get; }

        bool ReplaceResultFileDotsToCommas { get; }

        bool SortWorkResultsByTargetProfile { get; }

        #region Backward Compability
        bool IssueResultOrganizationCode { get; }
        bool IssueResultOrganizationName { get; }
        bool IssueResultWorkPatientGroupCode { get; }
        bool IssueResultWorkPatientGroupName { get; }
        bool IssueResultImages { get; }
        bool IssueResultDepartmentNr { get; }
        bool IssueResultEndDate { get; }
        bool IssueResultRegistrationDate { get; }
        bool IssueResultEquipmentCode { get; }
        bool IssueResultEquipmentName { get; }
        bool IssueResultWorkDiameter { get; }
        bool IssueResultDepartmentCode { get; }
        bool IssueResultDepartmentName { get; }
        bool IssueResultDoctorCode { get; }
        bool IssueResultDoctorName { get; }
        bool IssueResultSamplingDate { get; }
        bool IssueResultPriority { get; }
        bool IssueResultPayCategoryCode { get; }
        bool IssueResultPayCategoryName { get; }
        #endregion

        #region Testing
        string __fileNameIds { get; }
        bool __isOneRequestResultExport { get; }
        string FileNameIds { get; }
        bool IsOneRequestResultExport { get; }
        #endregion
    }
}