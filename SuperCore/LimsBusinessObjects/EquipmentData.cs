using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class EquipmentAntibioticResult
    {
        public EquipmentAntibioticResult()
        {
            AntibioticCode = String.Empty;
            Value = String.Empty;
            Mic = String.Empty;
            Diameter = String.Empty;
        }

        [CSN("AntibioticCode")]
        public String AntibioticCode { get; set; }
        [CSN("Value")]
        public String Value { get; set; }
        [CSN("Mic")]
        public String Mic { get; set; }
        [CSN("Diameter")]
        public String Diameter { get; set; }
        //  [CSN("FileInfos")]
        //   public List<ru.novolabs.LisDriverWrapper.Classes.WrapperFields.FileInfo> FileInfos { get; set; }
    }

    public class EquipmentMicroResult
    {
        public EquipmentMicroResult()
        {
            MicroorganismCode = String.Empty;
            Value = String.Empty;
            AntibioticResults = new List<EquipmentAntibioticResult>();
        }

        [CSN("MicroorganismCode")]
        public String MicroorganismCode { get; set; }
        [CSN("Found")]
        public Boolean Found { get; set; }
        [CSN("Value")]
        public String Value { get; set; }
        [CSN("AntibioticResults")]
        public List<EquipmentAntibioticResult> AntibioticResults { get; set; }
    }


    public class EquipmentResult
    {
        public EquipmentResult()
        {
            TestId = String.Empty;
            Value = String.Empty;
            Comment = String.Empty;
            ExternalSystemValidationState = 0;
            FileInfos = new List<EquipmentFileInfo>();
        }

        [CSN("TestId")]
        public String TestId { get; set; }
        [CSN("Value")]
        public String Value { get; set; }
        [CSN("Comment")]
        public String Comment { get; set; }
        [CSN("ExternalSystemValidationState")]
        public int ExternalSystemValidationState { get; set; }
        [CSN("FileInfos")]
        public List<EquipmentFileInfo> FileInfos { get; set; }
        [CSN("ExecutorEquipment")]
        public ObjectRef ExecutorEquipment { get; set; }
    }

    public class EquipmentComment
    {
        [CSN("Text")]
        public string Text { get; set; }
    }

    public class EquipmentFileInfo
    {
        [CSN("FileName")]
        public string FileName { get; set; }
        [CSN("FileContent")]
        public string FileContent { get; set; }
    }

    public class EquipmentOrder
    {
        public EquipmentOrder()
        {
            Results = new List<EquipmentResult>();
            MicroResults = new List<EquipmentMicroResult>();
            Comments = new List<EquipmentComment>();
            FileInfos = new List<EquipmentFileInfo>();
        }

        [CSN("SampleId")]
        public String SampleId { get; set; }
        [CSN("LotNr")]
        public String LotNr { get; set; }
        [CSN("PositionNr")]
        public String PositionNr { get; set; }
        [CSN("BiomaterialCode")]
        public String BiomaterialCode { get; set; }
        [CSN("SortedWorkPlace")]
        public String SortedWorkPlace { get; set; }
        [CSN("SortedDate")]
        public DateTime? SortedDate { get; set; }
        [CSN("Results")]
        public List<EquipmentResult> Results { get; set; }
        [CSN("MicroResults")]
        public List<EquipmentMicroResult> MicroResults { get; set; }
        [CSN("Comments")]
        public List<EquipmentComment> Comments { get; set; }
        [CSN("FileInfos")]
        public List<EquipmentFileInfo> FileInfos { get; set; }
    }


    public class EquipmentDataPatient
    {
        public EquipmentDataPatient()
        {
            PatientId = String.Empty;
            Orders = new List<EquipmentOrder>();
        }
        [CSN("PatientId")]
        public String PatientId { get; set; }
        [CSN("Orders")]
        public List<EquipmentOrder> Orders { get; set; }
    }

    public class EquipmentResponseMessage : ResponseMessage
    {
        [CSN("SampleNr")]
        public String SampleNr { get; set; }
    }

    public class EquipmentData
    {
        public EquipmentData()
        {
            Patients = new List<EquipmentDataPatient>();
        }

        [CSN("Equipment")]
        public EquipmentDictionaryItem Equipment { get; set; }
        [CSN("Patients")]
        public List<EquipmentDataPatient> Patients { get; set; }
    }
}