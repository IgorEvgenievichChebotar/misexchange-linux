using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.SOAP
{
    public class HelperSettings
    {
        public HelperSettings()
        {
            IsFilterRequestResults = false;
            IsEnabledMessageCaching = false;
            IsDisabledValidation = false;
            IsDisabledPatientSave = false;
            FillExecutorOrganizationAsHospital = false;
            UseAlternativeBiomaterialChecking = false;
            DoNotSendResultsWithEmptySamples = false;
            ReportName = "Ответ по пробе";
            ActionType = "View";
            DocumentType = "Specimen";
            IsLogSoapInFiles = false;
            UploadSampleResultReport = false;
        }
        public Boolean IsFilterRequestResults { get; set; }
        public bool IsEnabledMessageCaching { get; set; }
        public bool IsDisabledValidation { get; set; }
        public bool IsDisabledPatientSave { get; set; }
        public bool FillExecutorOrganizationAsHospital { get; set; }
        public bool UseAlternativeBiomaterialChecking { get; set; }
        public bool DoNotSendResultsWithEmptySamples { get; set; }
        public String ReportName { get; set; }
        public String ActionType { get; set; }
        public String DocumentType { get; set; }
        public bool IsLogSoapInFiles { get; set; }
        public bool UploadSampleResultReport { get; set; }
    }
}