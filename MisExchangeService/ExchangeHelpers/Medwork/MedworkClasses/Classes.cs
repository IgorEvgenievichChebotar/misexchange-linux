using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses
{
    public class Acknowledgment
    {
        public string RequestCode { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }

    public class StatusTypes
    {
        public const string STATUS_SUCCESS = "Success";
        public const string STATUS_ERROR = "Error";
    }

    public class ResearchesData
    {
        public ResearchesData()
        {
            Researches = new List<ResearchInfo>();
        }

        public List<ResearchInfo> Researches { get; set; }
    }

    public class ResearchInfo
    {
        public ResearchInfo()
        {
            Methods = new List<MethodInfo>();
            Tests = new List<TestInfo>();
        }

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }

        [XmlAttribute("GroupId")]
        public string GroupId { get; set; }

        [XmlAttribute("GroupName")]
        public string GroupName { get; set; }

        [XmlAttribute("GroupDescription")]
        public string GroupDescription { get; set; }

        [XmlAttribute("BiomaterialCode")]
        public string BiomaterialCode { get; set; }

        [XmlAttribute("BiomaterialName")]
        public string BiomaterialName { get; set; }

        public List<MethodInfo> Methods { get; set; }
        public List<TestInfo> Tests { get; set; }
    }

    public class MethodInfo
    {
        public MethodInfo()
        {
            TestRefValues = new List<TestRefValuesInfo>();
        }

        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Description")]
        public string Description { get; set; }

        public List<TestRefValuesInfo> TestRefValues { get; set; }
    }

    public class TestInfo
    {
        [XmlAttribute("Id")]
        public string Id { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("TestType")]
        public string TestType { get; set; }

        [XmlAttribute("BaseMU")]
        public string BaseMU { get; set; }

        [XmlAttribute("BaseMUId")]
        public string BaseMUId { get; set; }

        public List<string> Enum { get; set; }
    }

    public class TestRefValuesInfo
    {
        public TestRefValuesInfo()
        {
            RefChars = new List<TestRefInterval>();
        }

        [XmlAttribute("TestId")]
        public string TestId { get; set; }

        [XmlAttribute("IntervalToDisplay")]
        public string IntervalToDisplay { get; set; }

        [XmlAttribute("Interval")]
        public string Interval { get; set; }

        public List<TestRefInterval> RefChars { get; set; }
    }

    public class TestRefInterval
    {
        [XmlAttribute("RefChar")]
        public string RefChar { get; set; }

        [XmlAttribute("RefCharType")]
        public string RefCharType { get; set; }

        [XmlAttribute("IntervalToDisplay")]
        public string IntervalToDisplay { get; set; }

        [XmlAttribute("Interval")]
        public string Interval { get; set; }

        [XmlAttribute("RefCharBaseMU")]
        public string RefCharBaseMU { get; set; }

        [XmlAttribute("RefCharBaseMUId")]
        public string RefCharBaseMUId { get; set; }
    }

}