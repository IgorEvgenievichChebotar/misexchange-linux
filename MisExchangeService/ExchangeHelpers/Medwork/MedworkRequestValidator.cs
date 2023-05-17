using ru.novolabs.SuperCore;
using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.HelperDependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.Service;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork
{
    class MedworkRequestValidator : ConfigGeneralRequestValidator
    {
        private static Lazy<HelperSettings> helperSettings = new Lazy<HelperSettings>(GetHeplerSettings);
        static string PatientPrefix { get; set; }

        static MedworkRequestValidator()
        {
            object objectPatientPrefix = ProgramContext.Settings["patientCodePrefix", false];
            PatientPrefix = objectPatientPrefix != null ? (String)objectPatientPrefix : String.Empty;
        }

        protected override void CustomizeRestricitons(ExchangeDTOs.Request request)
        {
            if (String.IsNullOrEmpty(request.Samples.First().Barcode))
            {
                errors.Add(new ErrorTriplet() { ObjectName = "Sample", PropertyName = "Barcode", ErrorValidationType = HelperDependencies.ErrorValidationType.IsMandatory });
                return;
            }

            if (request.RequestCode != request.Samples.First().Barcode)
            {
                errors.Add(new ErrorTriplet() { ObjectName = "Sample", PropertyName = "Barcode", ErrorValidationType = HelperDependencies.ErrorValidationType.Custom });
                return;
            }

            CorrectPatientCode(request.Patient);
        }

        private static void CorrectPatientCode(ExchangeDTOs.Patient patient)
        {
            patient.Code = PatientPrefix + patient.Code;
        }

        private static HelperSettings GetHeplerSettings()
        {
            return File.ReadAllText(Path.Combine(PathHelper.AssemblyDirectory, "exchangeHelperSettings.xml"), Encoding.UTF8).Deserialize<HelperSettings>(Encoding.UTF8);
        }        
    }
}