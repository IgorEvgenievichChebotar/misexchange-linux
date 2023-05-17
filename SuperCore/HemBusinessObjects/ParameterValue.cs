using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class ParameterValue : BaseParameterValue
    {
        [CSN("State")]
        public int State { get; set; }

        [CSN("Comment")]
        public string Comment { get; set; }

        [SendToServer(false)]
        [CSN("Normality")]
        public int Normality { get; set; }

        [SendToServer(false)]
        [CSN("AutoCancelled")]
        public bool AutoCancelled { get; set; }

        [SendToServer(false)]
        [CSN("Norm")]
        public string Norm { get; set; }

        [SendToServer(false)]
        [CSN("NormComment")]
        public string NormComment { get; set; }

        [SendToServer(false)]
        [CSN("HasNormDeny")]
        public bool HasNormDeny { get; set; }

        [SendToServer(false)]
        [CSN("HasNormDefect")]
        public bool HasNormDefect { get; set; }

        [SendToServer(false)]
        [CSN("DonorNorm")]
        public string DonorNorm { get; set; }

        [SendToServer(false)]
        [CSN("ApprovingDoctor")]
        public string ApprovingDoctor { get; set; }

        [SendToServer(false)]
        [CSN("_Column")]
        public String _Column
        {
            get
            {
                return Parameter == null ? String.Empty : Parameter.Name;
            }
        
        }

    }
}