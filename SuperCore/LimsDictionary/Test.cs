using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public struct TestTypes
    {
        public const Int32 VALUE = 1; // Количественный тест
        public const Int32 ENUM = 2; // Качественный тест
        public const Int32 ANTIBIOTIC = 3; // Тест на устойчивость к антибиотику
    }

    public class TestValue : BaseObject
    {
        [CSN("Value")]
        public String Value { get; set; }
        [CSN("PatientGroups")]
        public List<PatientGroupDictionaryItem> PatientGroups { get; set; }
        [CSN("Test")]
        public TestDictionaryItem Test { get; set; }
        [CSN("DefaultGroup")]
        public Boolean DefaultGroup { get; set; }
        [CSN("EngValue")]
        public String EngValue { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("DefaultValue")]
        public Boolean DefaultValue { get; set; }
        [CSN("Rank")]
        public Int32 Rank { get; set; }
        [CSN("SearchKeys")]
        public String SearchKeys { get; set; }
    }

    public class TestDictionaryItem : DictionaryItem
    {
        private List<TestValue> values = new List<TestValue>();
        private List<TargetDictionaryItem> targets = new List<TargetDictionaryItem>();
        private UnitDictionaryItem unit = new UnitDictionaryItem();
        private List<NumericRange> numericRanges = new List<NumericRange>();


        [CSN("Format")]
        public String Format { get; set; }
        [CSN("Rank")]
        public Int32 Rank { get; set; }
        [CSN("ResultType")]
        public Int32 ResultType { get; set; }
        [CSN("Unit")]
        public UnitDictionaryItem Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        [CSN("Values")]
        public List<TestValue> Values
        {
            get { return values; }
            set { values = value; }
        }

        [JsonIgnore]
        [XmlIgnore]
        [CSN("Targets")]
        public List<TargetDictionaryItem> Targets
        {
            get { return targets; }
            set { targets = value; }
        }
        [CSN("IsSystem")]
        public Boolean IsSystem { get; set; }

        [CSN("NumericRanges")]
        public List<NumericRange> NumericRanges
        {
            get { return numericRanges; }
            set { numericRanges = value; }
        }

        [CSN("IsMicrobiology")]
        public Boolean IsMicrobiology { get; set; }

        [CSN("EngName")]
        public String EngName { get; set; }
    }

    public class TestDictionary : DictionaryClass<TestDictionaryItem>
    {
        public TestDictionary(String dictionaryName) : base(dictionaryName) { }
        public TestDictionary() { }

        [CSN("Test")]
        public List<TestDictionaryItem> Test
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }

    public class NumericRange
    {
        [CSN("PatientGroup")]
        [XmlIgnore]
        public PatientGroupDictionaryItem PatientGroup { get; set; }
        [CSN("PatientGroup_Id")]
        public Int32 PatientGroup_Id { get { return null == PatientGroup ? 0 : PatientGroup.Id; } set { } }
        [CSN("Point1")]
        public Double? Point1 { get; set; }
        [CSN("Point2")]
        public Double? Point2 { get; set; }
        [CSN("Point3")]
        public Double? Point3 { get; set; }
        [CSN("Point4")]
        public Double? Point4 { get; set; }
        [CSN("Name1")]
        public String Name1 { get; set; }
        [CSN("Name2")]
        public String Name2 { get; set; }
        [CSN("Name3")]
        public String Name3 { get; set; }
        [CSN("Name4")]
        public String Name4 { get; set; }
        [CSN("Name5")]
        public String Name5 { get; set; }
        [CSN("EngNormName")]
        public String EngNormName { get; set; }
    }


}