using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class Statistics
    {
        public Statistics()
        {
            this.RequestsStates = new List<StateInfo>();
            this.Requests = new List<StatisticRequestInfo>();
            this.SamplesStates = new List<StateInfo>();
            this.SamplesDefects = new List<NameCountInfo>();
            this.BiomaterialsCount = new List<NameCountInfo>();
            this.BiomaterialStates = new List<StateInfo>();
            this.DepartmentCount = new List<DepartmentSampleInfo>();
        }

        [CSN("RequestsStates")]
        public List<StateInfo> RequestsStates { get; set; }

        [CSN("Requests")]
        public List<StatisticRequestInfo> Requests { get; set; }

        [CSN("SamplesStates")]
        public List<StateInfo> SamplesStates { get; set; }

        [CSN("SamplesDefects")]
        public List<NameCountInfo> SamplesDefects { get; set; }

        [CSN("BiomaterialsCount")]
        public List<NameCountInfo> BiomaterialsCount { get; set; }

        [CSN("BiomaterialStates")]
        public List<StateInfo> BiomaterialStates { get; set; }

        [CSN("DepartmentCount")]
        public List<DepartmentSampleInfo> DepartmentCount { get; set; }

        [CSN("RequestsCount")]
        public int RequestsCount { get; set; }

        [CSN("SamplesCount")]
        public int SamplesCount { get; set; }

        [CSN("DefectSamples")]
        public int DefectSamples { get; set; }
    }

    public class StateInfo
    {
        [CSN("State")]
        public int State { get; set; }

        [CSN("Count")]
        public int Count { get; set; }

        [CSN("Name")]
        [SendToServer(false)]
        public string Name { get; set; }

        [CSN("Color")]
        [SendToServer(false)]
        public string Color { get; set; }
    }

    public class NameCountInfo
    {
        [CSN("Name")]
        public string Name { get; set; }

        [CSN("Count")]
        public int Count { get; set; }

    }

    public class DepartmentSampleInfo
    {
        [CSN("Name")]
        public string Name { get; set; }

        [CSN("ClosedCount")]
        public int ClosedCount { get; set; }

        [CSN("OpenCount")]
        public int OpenCount { get; set; }
    }

    public class StatisticRequestInfo
    {
        [CSN("Id")]
        public int Id { get; set; }

        [CSN("Nr")]
        public string Nr { get; set; }

        [CSN("State")]
        public int State { get; set; }

        [CSN("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }

        [CSN("PlannedDate")]
        public DateTime PlannedDate { get; set; }

        [CSN("Color")]
        public string Color 
        {
            get { return ((RequestStateDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.RequestState, this.State)).Color; }
        }

        [CSN("StateName")]
        public string StateName
        {
            get { return ((RequestStateDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.RequestState, this.State)).Name; }
        }
    }
}
