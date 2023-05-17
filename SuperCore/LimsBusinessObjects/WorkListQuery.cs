using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.LimsDictionary;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class WorkListQueryTest
    {
        [CSN("Test")]
        public TestDictionaryItem Test { get; set; }
        [CSN("TestCode")]
        public String TestCode { get; set; }
        [CSN("Dilution")]
        public String Dilution { get; set; }
        [CSN("State")]
        public Int32 State { get; set; }
        [CSN("Value")]
        public String Value { get; set; }
    }

    public class WorkListQueryMicroorganism
    {
        [CSN("MicroOrganism")]
        public MicroOrganismDictionaryItem MicroOrganism { get; set; }
        [CSN("MicroOrganismCode")]
        public String MicroOrganismCode { get; set; }
        [CSN("Found")]
        public Boolean Found { get; set; }
    }

    public class WorkListQuerySample
    {
        public WorkListQuerySample()
        {
            Tests = new List<WorkListQueryTest>();
            Microorganisms = new List<WorkListQueryMicroorganism>();
        }
        [CSN("SampleNr")]
        public String SampleNr { get; set; }

        [CSN("PatientCode")]
        [SendToServer(false)]
        public String PatientCode { get; set; }

        [CSN("Index")]
        [SendToServer(false)]
        public int Index { get; set; }

        [CSN("Birthday")]
        [SendToServer(false)]
        public Nullable<DateTime> Birthday { get; set; }

        [CSN("FirstName")]
        [SendToServer(false)]
        public String FirstName { get; set; }

        [CSN("LastName")]
        [SendToServer(false)]
        public String LastName { get; set; }

        [CSN("MiddleName")]
        [SendToServer(false)]
        public String MiddleName { get; set; }

        [CSN("Sex")]
        [SendToServer(false)]
        public int Sex { get; set; }

        [CSN("Tests")]
        [SendToServer(false)]
        public List<WorkListQueryTest> Tests { get; set; }

        [CSN("SampleDeliveryDate")]
        [SendToServer(false)]
        public Nullable<DateTime> SampleDeliveryDate { get; set; }

        [CSN("Department")]
        [SendToServer(false)]
        public DepartmentDictionaryItem Department { get; set; }

        [CSN("Biomaterial")]
        [SendToServer(false)]
        public BiomaterialDictionaryItem Biomaterial { get; set; }

        [CSN("Cito")]
        [SendToServer(false)]
        public Boolean Cito { get; set; }

        [CSN("Patient")]
        [SendToServer(false)]
        public ObjectRef Patient { get; set; }

        [CSN("Hospital")]
        [SendToServer(false)]
        public HospitalDictionaryItem Hospital { get; set; }

        [CSN("CustDepartment")]
        [SendToServer(false)]
        public CustDepartmentDictionaryItem CustDepartment { get; set; }

        [CSN("Doctor")]
        [SendToServer(false)]
        public DoctorDictionaryItem Doctor { get; set; }

        [CSN("Microorganisms")]
        [SendToServer(false)]
        public List<WorkListQueryMicroorganism> Microorganisms { get; set; }
    }


    /// <summary>
    /// Представляет параметры запроса рабочего списка с заданиями для аназизатора. Используется при двустороннем режиме обмена данными между ЛИС и анализатором
    /// </summary>
    public class WorkListQuery
    {
        public WorkListQuery()
        {
            Samples = new List<WorkListQuerySample>();
        }

        [CSN("Equipment")]
        public EquipmentDictionaryItem Equipment { get; set; }
        [CSN("Samples")]
        public List<WorkListQuerySample> Samples { get; set; }
    }


    /// <summary>
    /// Представляет параметры запроса рабочего списка с заданиями для аназизатора. Используется при двустороннем режиме обмена данными между ЛИС и анализатором
    /// </summary>
    public class TestsForSamples2Query
    {
        public TestsForSamples2Query()
        {
        }


        [CSN("DateFrom")]
        public DateTime DateFrom { get; set; }
        [CSN("DateTill")]
        public DateTime DateTill { get; set; }

        [CSN("MaxCount")]
        public Int32 MaxCount { get; set; }
        [CSN("State")]
        public Int32 State { get; set; }

        [CSN("Department")]
        public DepartmentDictionaryItem Department { get; set; }
        [CSN("Equipment")]
        public EquipmentDictionaryItem Equipment { get; set; }
        [CSN("WorklistCode")]
        public String WorklistCode { get; set; }
    }

    /// <summary>
    /// Предназначен для заполнения рабочего списка (структуры данных, реализующей интерфейс IWorkList), передаваемого COM-компоненту драйвера анализатора
    /// </summary>
    public class WorkListQueryResponse
    {
        public WorkListQueryResponse()
        {
            Samples = new List<WorkListQuerySample>();
            Errors = new List<ResponseMessage>();
        }

        [CSN("Samples")]
        public List<WorkListQuerySample> Samples { get; set; }
        [CSN("Errors")]
        public List<ResponseMessage> Errors { get; set; }

        public Boolean isEmpty()
        {
            foreach (WorkListQuerySample sample in Samples)
                foreach (WorkListQueryTest test in sample.Tests)
                    return false;

            return true;
        }
    }
}