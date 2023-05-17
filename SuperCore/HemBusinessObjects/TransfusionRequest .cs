using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{

    public struct TransfusionRequestState
    {
        /// <summary>
        /// Новая
        /// </summary>
        public const int New = 1;

        /// <summary>
        /// одобрена зав. отделением
        /// </summary>
        public const int ApprovedBySubDep = 2;

        /// <summary>
        /// отклонена заведующим
        /// </summary>
        public const int DeclinedBySubDep = 3;

        /// <summary>
        /// одобрена трансфузиологом
        /// </summary>
        public const int ApprovedByTrans = 4;

        /// <summary>
        /// отклонена трансфузиологом
        /// </summary>
        public const int DeclinedByTrans = 5;

        /// <summary>
        /// передана на подбор
        /// </summary>
        public const int MovedToSearching = 6;

        /// <summary>
        /// подбор выполнен
        /// </summary>
        public const int CompleteSearching = 7;

        /// <summary>
        /// подбор не выполнен
        /// </summary>
        public const int FailedSearching = 8;

        /// <summary>
        /// компонент передан в экспедицию
        /// </summary>
        public const int MovedToExpedition = 9;

        /// <summary>
        /// компонент выдан
        /// </summary>
        public const int Accepted = 10;

        /// <summary>
        /// отменена
        /// </summary>
        public const int Removed = 100;
    }

    public class TransfusionRequest: BaseObject
    {
        public TransfusionRequest()
        {
            BloodParameters = new List<BloodParameterValue>();
            AnamnesisParamValues = new List<ParameterValue>();
            PhysioIndicatorValues = new List<ParameterValue>();
            LabParameterValues = new List<ParameterValue>();
            Candidates = new List<CandidateProduct>();
            Transfusions = new List<Transfusion>();

            Recipient = new Donor();
            Doctor = new EmployeeDictionaryItem();
            Template = new TransfusionRequestTemplateDictionaryItem();
            HospitalDepartment = new DepartmentDictionaryItem();
            
            GroupStates = new List<StateGroupItem>();

        }

        [SendAsRef(true)]
        [CSN("Treatment")]
        public Treatment Treatment { get; set; }

        [CSN("Template")]
        public TransfusionRequestTemplateDictionaryItem Template { get; set; }

        [SendToServer(false)]
        [CSN("State")]
        public Int32 State { get; set; }

        [SendToServer(false)]
        [CSN("Doctor")]
        public EmployeeDictionaryItem Doctor { get; set; }

        [CSN("HospitalDepartment")]
        public DepartmentDictionaryItem HospitalDepartment { get; set; }

        [CSN("Date")]
        public DateTime? Date { get; set; }

        [CSN("PlannedDate")]
        public DateTime? PlannedDate { get; set; }

        [SendAsRef(true)]
        [CSN("Recipient")]
        public Donor Recipient { get; set; }
        
        [CSN("ProductClassification")]
        public ProductClassificationDictionaryItem ProductClassification { get; set; }

        [CSN("Volume")]
        public Int32 Volume { get; set; }

        [CSN("UnitType")]
        public Int32 UnitType { get; set; }

        [SendToServer(false)]
        [CSN("BloodParameters")]
        public List<BloodParameterValue> BloodParameters { get; set; }

        //[SendToServer(false)]
        [CSN("AnamnesisParamValues")]
        public List<ParameterValue> AnamnesisParamValues { get; set; }

        //[SendToServer(false)]
        [CSN("PhysioIndicatorValues")]
        public List<ParameterValue> PhysioIndicatorValues { get; set; }

        //[SendToServer(false)]
        [CSN("LabParameterValues")]
        public List<ParameterValue> LabParameterValues { get; set; }

        [CSN("Comments")]
        public String Comments { get; set; }

        [SendToServer(false)]
        [CSN("ApprovedVolume")]
        public Int32 ApprovedVolume { get; set; }

        [SendToServer(false)]
        [CSN("ApprovingDoctor")]
        public EmployeeDictionaryItem ApprovingDoctor { get; set; }

        [SendToServer(false)]
        [CSN("ApproveDate")]
        public DateTime? ApproveDate { get; set; }

        [SendToServer(false)]
        [CSN("Transfusiologist")]
        public EmployeeDictionaryItem Transfusiologist { get; set; }

        [SendToServer(false)]
        [CSN("TransfusiologistDate")]
        public DateTime? TransfusiologistDate { get; set; }

        [SendToServer(false)]
        [CSN("CancelDoctor")]
        public EmployeeDictionaryItem CancelDoctor { get; set; }

        [SendToServer(false)]
        [CSN("CancelDate")]
        public DateTime? CancelDate { get; set; }

        [SendToServer(false)]
        [CSN("Candidates")]
        public List<CandidateProduct> Candidates { get; set; }

        [SendToServer(false)]
        [CSN("TransferProcess")]
        public ObjectRef TransferProcess { get; set; }

        [SendToServer(false)]
        [CSN("ReceptProcess")]
        public ObjectRef ReceptProcess { get; set; }

        [SendToServer(false)]
        [CSN("Transfusions")]
        public List<Transfusion> Transfusions { get; set; }

        [SendToServer(false)]
        [CSN("Transfusion")]
        public Transfusion Transfusion { get; set; }

        [CSN("Nr")]
        public String Nr { get; set; }


        [SendToServer(false)]
        [CSN("GroupStates")]
        public List<StateGroupItem> GroupStates { get; set; }

        [CSN("HighPriority")]
        public bool HighPriority { get; set; }

        [CSN("RequestType")]
        public int RequestType { get; set; }

        [CSN("ParentTransfusionRequest")]
        public TransfusionRequest ParentTransfusionRequest { get; set; }
    }

    public class CandidateProduct
    {
        public CandidateProduct()
        {
            Product = new Product();
        }

        [CSN("Product")]
        public Product Product { get; set; }

        //0 - новый, 1 - одобрен, 2 - отклонен
        [CSN("State")]
        public Int32 State { get; set; }

        [CSN("ApprovingDoctor")]
        public EmployeeDictionaryItem ApprovingDoctor { get; set; }

        [CSN("ApproveDate")]
        public DateTime ApproveDate { get; set; }

        [CSN("ReagentType")]
        public ObjectRef ReagentType { get; set; }

        [CSN("ReagentLot")]
        public String ReagentLot { get; set; }

        [CSN("ReagentExpire")]
        public DateTime ReagentExpire { get; set; }

        [CSN("Comment")]
        public String Comment { get; set; }


    }

    public class TransfusionRequestNrResponse
    {
        public TransfusionRequestNrResponse()
        {
            TransfusionRequestNr = "";
            Errors = new List<ErrorMessage>();
        }
        [CSN("TransfusionRequestNr")]
        public String TransfusionRequestNr { get; set; }
        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
    }
}
