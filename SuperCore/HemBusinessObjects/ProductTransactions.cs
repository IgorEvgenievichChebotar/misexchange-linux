using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.SuperCore.HemDictionary;



namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class ProductTransactionsFilter
    {
        public ProductTransactionsFilter()
        {
            Departments = new RefSet();
            Classifications = new RefSet();
            BloodParameters = new List<RequestBloodParameter>();
            DateFrom = DateTime.Today;
            DateTill = DateTime.Today;
            PermissionDateFrom = DateTime.Today;
            PermissionDateTill = DateTime.Today;
            State = new List<Int32>();
            RequestNr = String.Empty;
            TransactionNr = String.Empty;
            ProductNr = String.Empty;
            TransactionDateFrom = DateTime.Now;
            TransactionDateTill = DateTime.Now;
        }

        [CSN("Departments")]
        public RefSet Departments { get; set; }
        [CSN("Classifications")]
        public RefSet Classifications { get; set; }
        [CSN("BloodParameters")]
        public List<RequestBloodParameter> BloodParameters { get; set; }
        [CSN("DateFrom")]
        public DateTime DateFrom { get; set; }
        [CSN("DateTill")]
        public DateTime DateTill { get; set; }
        [CSN("PermissionDateFrom")]
        public DateTime PermissionDateFrom { get; set; }
        [CSN("PermissionDateTill")]
        public DateTime PermissionDateTill { get; set; }
        [CSN("State")]
        public List<Int32> State { get; set; }
        [CSN("RequestNr")]
        public String RequestNr { get; set; }
        [CSN("TransactionNr")]
        public String TransactionNr { get; set; }
        [CSN("ProductNr")]
        public String ProductNr { get; set; }
        [CSN("TransactionDateFrom")]
        public DateTime TransactionDateFrom { get; set; }
        [CSN("TransactionDateTill")]
        public DateTime TransactionDateTill { get; set; }
    }

    public class FindProductTransactionRequestClass
    {
        public List<ObjectRef> departments = new List<ObjectRef>(); // ОПК, разместившее заявку
        public List<ObjectRef> classifications = new List<ObjectRef>();  // компонента крови, на которую была размещена заявка
        public List<BloodParameterValue> bloodParameters = new List<BloodParameterValue>();

        public DateTime dateFrom; //  дата+время создания заявки
        public DateTime dateTill; // дата+время создания заявки
        public DateTime permissionDateFrom; // дата+время вынесения решения по заявке.
        public DateTime permissionDateTill;  // дата+время вынесения решения по заявке.
        public List<int> state = new List<int>(); // состояние заявок

        public string requestNr = string.Empty;  // входящий номер заявки от ОПК

        public string transactionNr = string.Empty;  // Номер разрешения на продажу

        public string productNr = string.Empty;  // номер продукта, выданного по заявке.


        public DateTime transactionDateFrom;  // дата+время продажи компонентов крови
        public DateTime transactionDateTill;  // дата+время продажи компонентов крови


        public string recipientFirstName = string.Empty; // имя реципиента
        public string recipientMiddleName = string.Empty; // фамилия реципиента
        public string recipientLastName = string.Empty; // отчество реципиента
        public int recipientSex = 0;  // пол реципиента 1 - только М, 2 - только Ж, 3 - без учёта пола
        public AddressClass recipientAddress = new AddressClass(); // адрес реципиента

        public List<ObjectRef> recipientDocumentTypes = new List<ObjectRef>();  // тип документа реципиента
        public string recipientDocumentNr = string.Empty; // номер документа реципиента
        public string recipientDocumentSeries = string.Empty; // серия документа реципиента

        public int recipientBirthDay = 0;  // день рождения реципиента
        public int recipientBirthYear = 0;  // год рождения реципиента
        public int recipientBirthMonth = 0;  // месяц рождения реципиента

        public List<ObjectRef> hospitals = new List<ObjectRef>(); // ЛПУ, которым выдавались продукты по заявке.
        public string hospitalName = string.Empty; // название ЛПУ, которому выдавались продукты по заявке. 
    }

    public class FindProductTransactionResponse
    {
        public List<ProductTransaction> result = new List<ProductTransaction>();
    }

   
    
    public class GetProductTransactionRequest
    {
        public GetProductTransactionRequest()
        {
        }

        public GetProductTransactionRequest(int id)
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

    public class DeleteProductTransactionRequest
    {
        public int id = 0;
    }


    public class ProductTransactionChangeStateRequest : BaseObject
    {

        public ProductTransactionChangeStateRequest(Int32 Id, Int32 State)
        {
            this.Id = Id;
            this.State = State;
        }

        [CSN("State")]
        public Int32 State { get; set; }
    }


    public class ProductTransaction: BaseObject
    {
        public static int STATE_CREATED = 1;
        public static int STATE_PERMITTED = 2;
        public static int STATE_DENY = 3;
        public static int STATE_DONE = 4;


        public ProductTransaction()
        {
        }

        [CSN("Department")]
        public DepartmentDictionaryItem Department { get; set; }
        [CSN("Classification")]
        public ProductClassificationDictionaryItem Classification { get; set; }
        [CSN("BloodParameters")]
        public List<BloodParameterValue> BloodParameters { get; set; }
        [CSN("RequestQuantity")]
        public Int32 RequestQuantity { get; set; }
        [CSN("PermittedQuantity")]
        public Int32 PermittedQuantity { get; set; }
        [CSN("Date")]
        public DateTime Date { get; set; }
        [CSN("PermissionDate")]
        public DateTime PermissionDate { get; set; }
        [CSN("State")]
        public Int32 State { get; set; }
        [CSN("RequestNr")]
        public String RequestNr { get; set; }
        [CSN("TransactionNr")]
        public String TransactionNr { get; set; }
        [CSN("ProductNr")]
        public String ProductNr { get; set; }
        [CSN("TransactionDate")]
        public DateTime TransactionDate { get; set; } // дата+время продажи компонентов крови
        public ObjectRef recipient = new ObjectRef(); // ссылка на реципиента, для которого выдаются компоненты крови.
        public ObjectRef hospital = new ObjectRef();// ссылка на ЛПУ, котормjу были проданы компоненты. Может быть не заполнена, если ЛПУ не нашлось в спроавочнике  
        public string hospitalName = string.Empty; //  название ЛПУ, котормjу были проданы компоненты. Если не удалось выбрать ЛПУ из справочника, то заполняется в ручную, в противном случае копируется название ЛПУ из справочника.
        public List<string> IgnorList = new List<string>(new string[] { "date", "permissionDate", "permittedQuantity", "state", "requestNr", "transactionNr", "transactionDate" });
        [CSN("hospitalDepartment")]
        public String hospitalDepartment { get; set; }
        [CSN("hospitalDoctor")]
        public String hospitalDoctor { get; set; }
        [CSN("comment")]
        public String comment { get; set; }

        private List<Product> products = new List<Product>(); // список номеров компонентов крови, выданных по заявке.
        [CSN("Products")]
        public List<Product> Products
        {
            get { return products; }
            set { products = value; }
        }
    }
}
