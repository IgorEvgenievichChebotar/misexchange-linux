using System;

namespace ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces
{
    public interface IProcessRequestSettings
    {
        string RegistrationFormCode { get; }
        bool IsAllowedToAddUserValues { get; }

        bool ResetSampleDeliveryDate { get; }
        DateTime? DefaultSampleDeliveryDate { get; }

        string PatientCodePrefix { get; }

        bool ResetPatientCode { get; }

        string ServiceAddress { get; }
        bool IsAltTargetCodeInRequest { get; }
    }
}