using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.HemDictionary;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class Process : BaseObject//, IDonorHeader
    {
        // Fields
        private DateTime date = DateTime.MinValue;
        private List<Stages> stages = new List<Stages>();
        private ObjectRef activeStage = new ObjectRef();
        private List<Object> nrs = new List<Object>();
        private List<Object> outgoingDocuments = new List<Object>();
        private int state = 0;
        private List<Product> products = new List<Product>();
        private string nr = string.Empty;
        private ObjectRef department = new ObjectRef();
        private DateTime startdate = DateTime.MinValue;
        private ObjectRef template = new ObjectRef();

        private ObjectRef storeDepartment = new ObjectRef();
        private ObjectRef storage = new ObjectRef();

        private List<StateGroupItem> groupStates = new List<StateGroupItem>();

        [SendToServer(false)]
        [CSN("StoreDepartment")]
        public ObjectRef StoreDepartment
        {
            get { return storeDepartment; }
            set { storeDepartment = value; }
        }

        [SendToServer(false)]
        [CSN("Storage")]
        public ObjectRef Storage
        {
            get { return storage; }
            set { storage = value; }
        }

        [CSN("Nr")]
        public string Nr
        {
            get { return nr; }
            set { nr = value; }
        }

        [CSN("StateName")]
        public string StateName
        {
            get { return GetStateName(); }
            set { }
        }

        private string GetStateName()
        {
            return ProcessStateConst.GetStateName(state);
        }


        [LinkedDictionary("processTemplate", "Name")]
        [CSN("Template")]
        public ObjectRef Template
        {
            get { return template; }
            set { template = value; }
        }

        [CSN("Products")]
        public virtual List<Product> Products
        {
            get { return products; }
            set { products = value; }
        }

        [CSN("State")]
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        [SendToServer(false)]
        [CSN("Date")]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        [SendToServer(false)]
        [CSN("Stages")]
        public List<Stages> Stages
        {
            get { return stages; }
            set { stages = value; }
        }

        [CSN("HospitalDepartment")]
        public DepartmentDictionaryItem HospitalDepartment { get; set; }


        [SendToServer(false)]
        [CSN("GroupStates")]
        public List<StateGroupItem> GroupStates
        {
            get
            {
                return groupStates;
            }
            set
            {
                groupStates = value;
            }
        }

    }


    public class ProcessSet
    {
        private List<Process> result = new List<Process>();

        [CSN("Result")]
        public List<Process> Result
        {
            get { return result; }
            set { result = value; }
        }
    }


    #region FindProcessRequest
    public class FindProcessesRequest
    {
        public FindProcessesRequest()
        {
            DateFrom = DateTime.Today;
            DateTill = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);
        }

        public void BeforeSave()
        {

            DateFrom = DateFrom.Date;
            DateTill = DateTill.Date.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);

        }

        private DateTime dateFrom = DateTime.MinValue;
        private DateTime dateTill = DateTime.MinValue;
        private string productNr = string.Empty;
        private int terminal = 2;
        private List<ObjectRef> templates = new List<ObjectRef>();
        private List<ObjectRef> departments = new List<ObjectRef>();


        private int completedStages = 2;

        [CSN("CompletedStages")]
        public int CompletedStages
        {
            get { return completedStages; }
            set { completedStages = value; }
        }

        [CSN("Templates")]
        public List<ObjectRef> Templates
        {
            get { return templates; }
            set { templates = value; }
        }

        [CSN("Departments")]
        public List<ObjectRef> Departments
        {
            get { return departments; }
            set { departments = value; }
        }

        [CSN("Terminal")]
        public int Terminal
        {
            get { return terminal; }
            set { terminal = value; }
        }

        [CSN("ProductNr")]
        public string ProductNr
        {
            get { return productNr; }
            set { productNr = value; }
        }

        [CSN("DateTill")]
        public DateTime DateTill
        {
            get { return dateTill; }
            set { dateTill = value; }

        }

        [CSN("DateFrom")]
        public DateTime DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; }
        }
    }

    #endregion FindProductsRequest


    public class GetProcessInfoRequest
    {
        public GetProcessInfoRequest(int processId)
        {
            Ids.Add(new ObjectRef(processId));
        }


        private List<ObjectRef> ids = new List<ObjectRef>();

        [CSN("Ids")]
        public List<ObjectRef> Ids
        {
            get { return ids; }
            set { ids = value; }
        }
    }

    public class Stages : BaseObject
    {
        private List<Actions> actions = new List<Actions>();
        private ObjectRef stage = new ObjectRef();

        [CSN("Stage")]
        public ObjectRef Stage
        {
            get { return stage; }
            set { stage = value; }
        }
    }

    public class Actions : BaseObject
    {
        //private bool required = false;
        //private int actionType = 0;
        private ObjectRef transition = new ObjectRef();
    }


    public class BatchCreateProcessRequest
    {

        private List<ObjectRef> products = new List<ObjectRef>();
        private ObjectRef template = new ObjectRef();
        private ObjectRef department = new ObjectRef();

        [CSN("Products")]
        public List<ObjectRef> Products
        {
            get { return products; }
            set { products = value; }
        }

        [CSN("Department")]
        public ObjectRef Department
        {
            get { return department; }
            set { department = value; }
        }

        [CSN("Template")]
        public ObjectRef Template
        {
            get { return template; }
            set { template = value; }
        }

    }

    public class SaveProcessRequest
    {

        private List<ClientProduct> clientProducts = new List<ClientProduct>();
        private bool nrsModified = false;
        private ObjectRef department = new ObjectRef();
        private ObjectRef id = new ObjectRef();

        [CSN("ClientProducts")]
        public List<ClientProduct> ClientProducts
        {
            get { return clientProducts; }
            set { clientProducts = value; }
        }

        [CSN("Id")]
        public ObjectRef Id
        {
            get { return id; }
            set { id = value; }
        }

        [CSN("NrsModified")]
        public bool NrsModified
        {
            get { return nrsModified; }
            set { nrsModified = value; }
        }

        [CSN("Department")]
        public ObjectRef Department
        {
            get { return department; }
            set { department = value; }
        }

    }
    public class ClientProduct
    {
        private ObjectRef product = new ObjectRef();
        private bool removed = false;

        [CSN("Product")]
        public ObjectRef Product
        {
            get { return product; }
            set { product = value; }
        }

        [CSN("Removed")]
        public bool Removed
        {
            get { return removed; }
            set { removed = value; }
        }
    }

    public class ChangeStateProcessRequest
    {
        public ChangeStateProcessRequest(int processId, int val)
        {
            Ids.Add(new ObjectRef(processId));
            value = val;
        }

        private List<ObjectRef> ids = new List<ObjectRef>();
        private string comment = "1";
        private int value = 0;

        [CSN("Value")]
        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        [CSN("Comment")]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        [CSN("Ids")]
        public List<ObjectRef> Ids
        {
            get { return ids; }
            set { ids = value; }
        }
    }

    public class ChangeStageCloseRequest
    {
        public ChangeStageCloseRequest(Process process)
        {
            /*
            Parameter parameter = new Parameter(new ObjectRef(process.StoreDepartment), new ObjectRef(process.Storage));
            parameter.StoreDepartment = process.StoreDepartment;
            parameter.Storage = process.Storage;
            Parameters.Add(parameter);
            */
            foreach (Stages processStage in process.Stages)
            {

                Stages.Add(new ObjectRef(processStage.Stage));
            }
            Process = new ObjectRef(process.Id);

        }


        private ObjectRef process = new ObjectRef();
        private List<Parameter> parameters = new List<Parameter>();
        private List<ObjectRef> stages = new List<ObjectRef>();

        [CSN("Process")]
        public ObjectRef Process
        {
            get { return process; }
            set { process = value; }
        }

        [CSN("Stages")]
        public List<ObjectRef> Stages
        {
            get { return stages; }
            set { stages = value; }
        }

        [CSN("Parameters")]
        public List<Parameter> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public class Parameter
        {
            private ObjectRef storage = new ObjectRef();
            private ObjectRef storeDepartment = new ObjectRef();
            private ObjectRef product = new ObjectRef();

            [CSN("Product")]
            public ObjectRef Product
            {
                get { return product; }
                set { product = value; }
            }

            [CSN("Storage")]
            public ObjectRef Storage
            {
                get { return storage; }
                set { storage = value; }
            }

            [CSN("StoreDepartment")]
            public ObjectRef StoreDepartment
            {
                get { return storeDepartment; }
                set { storeDepartment = value; }
            }
        }
    }

    public class ChangeStageCloseResult
    {
        public List<ErrorMessage> errors = new List<ErrorMessage>();
    }
}


    


        

   