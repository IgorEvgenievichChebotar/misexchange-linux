using System;
using System.Reflection;
using ru.novolabs.SuperCore;

namespace ru.novolabs.MisExchangeService.Classes
{
    public struct UserObjectTypes
    {
        public const string Request = "Request";
        public const string Patient = "Patient";
        public const string PatientCard = "PatientCard";
    }

    [Obfuscation(ApplyToMembers = false, Exclude = true, StripAfterObfuscation = true)]
    public class DefaultUserFieldValue
    {
        public DefaultUserFieldValue()
        {
            UserObjectType = UserObjectTypes.Request;
            UserFieldCode = String.Empty;
            DefaultValue = String.Empty;
        }

        [CSN("UserObjectType")]
        public string UserObjectType { get; set; }
        [CSN("UserFieldCode")]
        public string UserFieldCode { get; set; }
        [CSN("DefaultValue")]
        public string DefaultValue { get; set; }
    }
}