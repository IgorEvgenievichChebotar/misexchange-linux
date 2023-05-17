using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects.Exchange
{
    public class Department
    {
        public Department()
        {
            Name = Code = Mnemonics = String.Empty;
        }

        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Mnemonics")]
        public String Mnemonics { get; set; }    
    }

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
            Department = new Department();
        }
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Mnemonics")]
        public String Mnemonics { get; set; }
        [CSN("ServiceName")]
        public String ServiceName { get; set; }
        [CSN("ServiceCode")]
        public String ServiceCode { get; set; }
        [CSN("TargetType")]
        public Int32 TargetType { get; set; }
        [CSN("TestCodes")]
        [XmlArrayItem(ElementName = "Code")]
        public List<String> TestCodes { get; set; }
        [CSN("BiomaterialCodes")]
        [XmlArrayItem(ElementName = "Code")]
        public List<String> BiomaterialCodes { get; set; }
        [XmlArrayItem(ElementName = "Code")]
        [CSN("TargetCodes")]
        public List<String> TargetCodes { get; set; }
        [CSN("Department")]
        public Department Department { get; set; }

        [XmlIgnore]
        [CSN("TargetRef")]
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

                foreach (var target in currentTarget.Targets)
                    TargetCodes.Add(target.Code);

                foreach (var test in currentTarget.Tests)
                    TestCodes.Add(test.Code);

                foreach (var biomaterial in currentTarget.Biomaterials)
                    BiomaterialCodes.Add(biomaterial.Code);

                if ((currentTarget.Department != null) && (currentTarget.Department.Id > 0))
                {
                    this.Department = new Exchange.Department()
                    {
                        Name = currentTarget.Department.Name,
                        Code = currentTarget.Department.Code,
                        Mnemonics = currentTarget.Department.Mnemonics
                    };                    
                }
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
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Items")]
        public List<ExternalTargetInfo> Items { get; set; }

    }

    [XmlType(TypeName = "TestValue")]
    public class TestValueExternalInfo
    {
        public TestValueExternalInfo()
        {
            Value = Code = String.Empty;
        }

        [CSN("Value")]
        public String Value { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
    }

    internal struct ExtTestTypes
    {
        public const string Quantitative = "QUANT";
        public const string Qualitative = "QUAL";
        public const string Antibiotic = "ANTIBIOTIC";
    }

    [XmlType(TypeName = "Item")]
    public class ExternalTestInfo
    {
        ObjectRef testRef;

        public ExternalTestInfo()
        {
            Name = Code = Mnemonics = UnitName = String.Empty;
        }
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Mnemonics")]
        public String Mnemonics { get; set; }
        [CSN("ResultType")]
        public String ResultType { get; set; } // 1 - Количественный, 2 - качественный, 3 - тест на устойчивость к антибиотику
        [CSN("UnitName")]
        public String UnitName { get; set; }
        [CSN("DecimalPlaces")]
        public int DecimalPlaces { get; set; }
        [CSN("Values")]
        public List<TestValueExternalInfo> Values { get; set; }

        [XmlIgnore]
        [CSN("TestRef")]
        public ObjectRef TestRef
        {
            get
            {
                return testRef;
            }
            set
            {
                testRef = value;
                TestDictionaryItem test = (TestDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Test, testRef.Id);

                Name = test.Name;
                Code = test.Code;
                Mnemonics = test.Mnemonics;
                if (test.ResultType == 1)
                    this.ResultType = ExtTestTypes.Quantitative;
                else if (test.ResultType == 2)
                    this.ResultType = ExtTestTypes.Qualitative;
                else if (test.ResultType == 3)
                    this.ResultType = ExtTestTypes.Antibiotic;

                if (ResultType == ExtTestTypes.Qualitative) // Качественный тест
                {
                    Values = new List<TestValueExternalInfo>();
                    if (test.Values != null)
                    {
                        foreach (TestValue testValue in test.Values)
                        {
                            Values.Add(new TestValueExternalInfo() { Value = testValue.Value, Code = testValue.Code }); 
                        }
                    }
                }

                if ((test.Unit != null) && (test.Unit.Id > 0))
                    UnitName = test.Unit.Name;

                if (!String.IsNullOrEmpty(test.Format))
                    DecimalPlaces = Int32.Parse(test.Format);

            }
        }
    }

    [XmlType(TypeName = "Item")]
    public class ExternalBiomaterialInfo
    {
        public ExternalBiomaterialInfo()
        {
        }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Mnemonics")]
        public String Mnemonics { get; set; }
        [CSN("Name")]
        public String Name { get; set; }


        ObjectRef biomaterialRef;
        [XmlIgnore]
        [CSN("BiomaterialRef")]
        public ObjectRef BiomaterialRef
        {
            get { return biomaterialRef; }
            set
            {
                BiomaterialDictionaryItem biomaterial = (BiomaterialDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.Biomaterial, value.Id];
                if (biomaterial != null)
                {
                    Code = biomaterial.Code;
                    Mnemonics = biomaterial.Mnemonics;
                    Name = biomaterial.Name;
                }
            }
        }
    }

    [XmlType(TypeName = "Dictionary")]
    public class ExternalTestDictionaryInfo
    {
        public ExternalTestDictionaryInfo()
        {
            Items = new List<ExternalTestInfo>();
            Code = "test";
        }

        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Items")]
        public List<ExternalTestInfo> Items { get; set; }
    }
}