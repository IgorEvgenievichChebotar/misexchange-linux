using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.MisExchangeService.Classes
{
    [XmlType(TypeName = "Item")]
    public class ExternalTargetInfo
    {
        ObjectRef targetRef;

        public ExternalTargetInfo()
        {
            Name = String.Empty;
            Code = String.Empty;
            Mnemonics = String.Empty;
            ServiceName = String.Empty;
            ServiceCode = String.Empty;
            
            TestCodes = new List<String>();
            BiomaterialCodes = new List<String>();
            TargetCodes = new List<String>();
        }

        public String Name { get; set; }
        public String Code { get; set; }
        public String Mnemonics { get; set; }
        public String ServiceName { get; set; }
        public String ServiceCode { get; set; }
        public Int32 TargetType { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        public List<String> TestCodes;
        [XmlArrayItem(ElementName = "Code")]
        public List<String> BiomaterialCodes;
        [XmlArrayItem(ElementName = "Code")]
        public List<String> TargetCodes;

        [XmlIgnore]
        public ObjectRef TargetRef
        {
            get
            {
                return targetRef;
            }
            set
            {
                targetRef = value;
                TargetDictionaryItem currentTarget = (TargetDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Target, targetRef.Id);

                Name = currentTarget.Name;
                Code = currentTarget.Code;
                Mnemonics = currentTarget.Mnemonics;
                TargetType = currentTarget.TargetType;

                ServiceDictionaryItem service = null;
                if (currentTarget.Services.Count == 1)
                    service = (ServiceDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.ServiceShort, currentTarget.Services[0].Id);
                if (service != null)
                {
                    ServiceName = service.Name;
                    ServiceCode = service.Code;
                }
                else
                    ServiceName = String.Empty;

                currentTarget.Targets.ForEach(t => TargetCodes.Add(t.Code));
                currentTarget.Tests.ForEach(t => TestCodes.Add(t.Code));
                currentTarget.Biomaterials.ForEach(b => BiomaterialCodes.Add(b.Code));

                /*foreach (ObjectRef targetReference in currentTarget.Targets)
                {
                    TargetDictionaryItem target = (TargetDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LisDictionaryNames.Target, targetReference.Id);
                    if (target != null)
                    {
                        String targetCode = target.Code;
                        TargetCodes.Add(targetCode);
                    }
                }

                foreach (ObjectRef targetReference in currentTarget.Tests)
                {
                    TestDictionaryItem test = (TestDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LisDictionaryNames.Test, targetReference.Id);
                    if (test != null)
                    {
                        String testCode = test.Code;
                        TestCodes.Add(testCode);
                    }
                }

                foreach (ObjectRef biomaterialReference in currentTarget.Biomaterials)
                {
                    BiomaterialDictionaryItem biomaterial = (BiomaterialDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LisDictionaryNames.Biomaterial, biomaterialReference.Id);
                    if (biomaterial != null)
                    {
                        String biomaterialCode = biomaterial.Code;
                        BiomaterialCodes.Add(biomaterialCode);
                    }
                }*/
            }
        }
    }

    [XmlType(TypeName = "Dictionary")]    
    public class ExternalTargetDictionaryInfo
    {
        public ExternalTargetDictionaryInfo() 
            {
                Items = new List<ExternalTargetInfo>();
                Code = "target";
            }

        public String Code { get; set; }
        public List<ExternalTargetInfo> Items { get; set; }

    }
}