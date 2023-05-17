using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    /// <summary>
    /// Переливание крови
    /// </summary>
    public class Transfusion: BaseObject
    {
        public Transfusion()
        {
            Products = new List<ObjectRef>();
            Observations = new List<MedicalObservation>();
            Recipient = new Donor();
            Request = new TransfusionRequest();
            Product = new Product();
            GroupStates = new List<StateGroupItem>();
        }
        [SendToServer(false)]
        [SendAsRef(true)]
        [CSN("Template")]
        public TransfusionTemplateDictionaryItem Template { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [SendToServer(false)]
        [CSN("Date")]
        public DateTime Date { get; set; }

        [SendToServer(false)]
        [CSN("TransfusionDate")]
        public DateTime TransfusionDate { get; set; }

        [SendToServer(false)]
        [SendAsRef(true)]
        [CSN("Employee")]
        public EmployeeDictionaryItem Employee { get; set; }

        [SendToServer(false)]
        [SendAsRef(true)]
        [CSN("Request")]
        public TransfusionRequest Request { get; set; }

        [SendToServer(false)]
        [SendAsRef(true)]
        [CSN("Recipient")]
        public Donor Recipient { get; set; }

        [SendToServer(false)]
        [CSN("Observations")]
        public List<MedicalObservation> Observations { get; set; }

        /// <summary>
        /// Номер трансфузии
        /// </summary>
        [SendToServer(false)]
        [CSN("Nr")]
        public String Nr { get; set; }
        /// <summary>
        /// Отделение больницы
        /// </summary>
        /// 
        [SendToServer(false)]
        [SendAsRef(true)]
        [CSN("HospitalDepartment")]
        public DepartmentDictionaryItem HospitalDepartment { get; set; }
        /// <summary>
        /// ЛПУ
        /// </summary>
        /// 
        [SendToServer(false)]
        [SendAsRef(true)]
        [CSN("Hospital")]
        public HospitalDictionaryItem Hospital { get; set; }
        /// <summary>
        /// Врач, выполнивший трансфузию
        /// </summary>
        [SendToServer(false)]
        [SendAsRef(true)]
        [CSN("Doctor")]
        public EmployeeDictionaryItem Doctor { get; set; }
        /// <summary>
        /// Реципиент
        /// </summary>
        [SendToServer(false)]
        [SendAsRef(true)]
        [CSN("Patient")]
        public Donor Patient { get; set; }

        /// <summary>
        /// Карта реципиента
        /// </summary>
        [SendToServer(false)]
        [SendAsRef(true)]
        [CSN("PatientCard")]
        public ObjectRef PatientCard { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        [SendToServer(false)]
        [CSN("Volume")]
        public Int32 Volume { get; set; }
        /// <summary>
        /// Продукт
        /// </summary>
        [SendToServer(false)]
        [SendAsRef(true)]
        [CSN("Products")]
        public List<ObjectRef> Products { get; set; }


        [SendToServer(false)]
        [CSN("ProductsFull")]
        public List<Product> ProductsFull { get; set; }
        /// <summary>
        /// Результаты трансфузии
        /// </summary>
        
        [SendAsRef(true)]
        [CSN("TransfusionResult")]
        public TransfusionResultDictionaryItem TransfusionResult { get; set; }


        [SendToServer(false)]
        [CSN("UnitType")]
        public Int32 UnitType { get; set; }

        [CSN("Comments")]
        public String Comments { get; set; }

        [SendToServer(false)]
        [CSN("RecipientBloodParams")]
        public List<BloodParameterValue> RecipientBloodParams
        {
            get
            {
                if (Recipient != null)
                    return Recipient.BloodParameters;
                return null;
            }
            set
            {
                if (Recipient != null)
                    Recipient.BloodParameters = value;
            }
        }
        [SendAsRef(true)]
        [SendToServer(false)]
        [CSN("Product")]
        public Product Product { get; set; }

        [CSN("MatchBio")]
        public Int32? MatchBio { get; set; }
        [CSN("MatchLab")]
        public Int32? MatchLab { get; set; }
        [CSN("PolyglucinumReaction")]
        public Int32 PolyglucinumReaction { get; set; }

        [SendToServer(false)]
        [CSN("Status")]
        public Int32 Status { get; set; }

        //[SendToServer(false)]
        //[CSN("DonorBloodParams")]
        //public List<BloodParameterValue> DonorBloodParams
        //{
        //    get
        //    {
        //        if (Donor != null)
        //            return Donor.BloodParameters;
        //        return null;
        //    }
        //    set
        //    {
        //        if (Donor != null)
        //            Donor.BloodParameters = value;
        //    }
        //}


        [SendToServer(false)]
        [CSN("GroupStates")]
        public List<StateGroupItem> GroupStates { get; set; }
    }

    public class FindTransfusionRequest : BaseObject
    {
        public FindTransfusionRequest()
        {
            BloodParameters = new List<BloodParameterValue>();
        }

        [CSN("DateFrom")]
        public DateTime DateFrom { get; set; }
        [CSN("DateTill")]
        public DateTime DateTill { get; set; }
        [CSN("ProductNr")]
        public String ProductNr { get; set; }
        [CSN("HospitalDepartment")]
        public String HospitalDepartment { get; set; }
        [CSN("HospitalDoctor")]
        public String HospitalDoctor { get; set; }
        [CSN("BloodParameters")]
        public List<BloodParameterValue> BloodParameters { get; set; }
        [CSN("FirstName")]
        public String FirstName { get; set; }
        [CSN("LastName")]
        public String LastName { get; set; }
        [CSN("MiddleName")]
        public String MiddleName { get; set; }   
    }


    public class GetTransfusionRequest
    {
        public GetTransfusionRequest()
        {
        }

        public GetTransfusionRequest(int id)
        {
            this.id = id;
        }

        private int id = 0;

        [CSN("Id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }

    public class MedicalObservation
    {
        public MedicalObservation()
        {
            PhysioIndicatorValues = new List<ParameterValue>();
        }
        [CSN("Id")]
        public int? Id { get; set; }
        [CSN("Date")]
        public Int32 Date { get; set; }
        [CSN("PhysioIndicatorValues")]
        public List<ParameterValue> PhysioIndicatorValues { get; set; }

        [CSN("Template")]
        public ObservationTemplate Template { get; set; }
    }
}
