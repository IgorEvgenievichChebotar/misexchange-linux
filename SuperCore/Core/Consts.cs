using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Reflection;

public enum ErrorMessageType : int
{
    None = 0,
    Error = 1,
    Warning = 2,
    Info = 3
}

public enum LisErrorMessageTypes : int
{
    ERROR = 0,
    WARNING = 1,
    INFO = 2
}

namespace ru.novolabs.SuperCore
{
    public class BaseMergeRules
    {
        public virtual string NameRule1(string ColumnName)
        {
            return ColumnName;
        }

        public virtual string NameRule2(string ColumnName)
        {
            return ColumnName;
        }

        public virtual bool DuplicateRule(DataTable Table, DataRow Row)
        {
            return false;
        }
    }

    public class DonorToEDCRule : BaseMergeRules
    {
        public override string NameRule2(string ColumnName)
        {
            return ColumnName;
        }

        public override bool DuplicateRule(DataTable Table, DataRow Row)
        {
            foreach (DataRow TableRow in Table.Rows)
            {
                if (TableRow["donorId"].ToString().Equals(Row["id"].ToString()))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public static class ConstServerOptions
    {
        public const String ENABLE_LATE_BIOMATERIAL_DELIVERY = "enableLateBiomaterialDelivery";
        public const String PERSONAL_TUBE_NR = "personalTubeNr";
        public const String LOCK_DELIVERED_WORKS = "lockDeliveredWorks";
        public const String EDIT_PATIENT_FROM_REQUEST = "editPatientFromRequest";
        public const String OUTSOURCE_MICROBIOLOGY_WORK = "outsourceMicrobiologyWork";
    }

    public static class XMLConst
    {
        private static Boolean isPhox = false;

        internal const String DTDPhox = "\"phox.dtd\"";

        internal static Boolean IsPhox
        {
            get { return isPhox; }
            set { isPhox = value; }
        }

        internal static String XML_Node_Response
        {
            get
            {
                return IsPhox ? "phox-response" : "response";
            }
        }

        internal static String XML_DOCTYPE_NAME
        {
            get
            {
                return IsPhox ? "phox-request" : "request";
            }
        }

        internal static String XML_NODE_EVENT
        {
            get
            {
                return IsPhox ? "phox-event" : "event";
            }
        }

        internal static String XML_DocType_SysId
        {
            get
            {
                return IsPhox ? "phox.dtd" : "lims.dtd";
            }
        }

        internal static String XML_Request
        {
            get
            {
                return IsPhox ? "phox-request" : "request";
            }
        }

        internal static String DTDString
        {
            get
            {
                return IsPhox ? "\"phox.dtd\"" : "\"lims.dtd\"";
            }
        }

        // Старые константы
        /*  
          internal const String XML_Node_Response = "phox-response";
          internal const String XML_DOCTYPE_NAME = "phox-request";
          internal const String XML_NODE_EVENT = "phox-event";
          internal static String XML_DocType_SysId = "phox.dtd";
          internal const String XML_Request = "phox-request";
          internal const String DTDString = "\"phox.dtd\"";
        */
        // Новые константы
        /*
        internal const String XML_Node_Response = "response";
        internal const String XML_DOCTYPE_NAME = "request";
        internal const String XML_NODE_EVENT = "event";
        internal const String XML_Request = "request";
        internal const String XML_DocType_SysId = "lims.dtd";
        internal const String DTDString = "\"lims.dtd\"";
        */

        internal const String XML_Node_Mapping = "mapping";
        internal const String XML_Node_Map = "map";
        internal const String XML_Node_Property = "property";
        internal const String XML_Node_Filter = "filter";
        internal const String XML_Node_Id = "id";
        internal const String XML_Node_Class = "class";
        internal const String XML_Attribute_Column = "column";
        internal const String XML_Attribute_Class = "class";
        internal const String XML_Attribute_Table = "table";
        internal const String XML_Attribute_NAME = "name";
        internal const String XML_Attribute_TYPE = "type";
        internal const String XML_Attribute_Length = "length";
        internal const String XML_Attribute_Not_Null = "not_null";
        internal const String XML_Attribute_DefaultValue = "defaultValue";
        internal const String XML_Attribute_Property = "property";
        internal const String XML_Attribute_Equals = "equals";
        internal const String XML_Attribute_Not_Equals = "not_equals";
        internal const String XML_Attribute_Less_Then = "less_then";
        internal const String XML_Attribute_Grater_Then = "grater_then";
        internal const String XML_Attribute_In_Range = "range";
        internal const String XML_Attribute_Callback_Address = "callback-address";
        internal const String XML_Attribute_Async = "async";
        internal const String XML_Node_Content = "content";
        public const String XML_Attribute_Source = "source";
        public const String XML_Attribute_Encoding = "encoding";
        public const String XML_Attribute_Encoding_866 = "866";
        public const String XML_Attribute_Encoding_1251 = "1251";
        public const String XML_Attribute_Encoding_none = "none";
        public const String XML_Attribute_sql = "sql";


        internal const String XML_Node_ServerRequest = "ServerRequest";
        internal const String XML_Node_RequestType = "RequestType";
        internal const String XML_Node_Async = "Async";
        internal const String XML_Node_CallbackAddress = "CallbackAddress";
        internal const String XML_Node_ContentUpperCase = "Content";

        internal const String XML_Node_Object = "o";
        internal const String XML_Node_Reference = "r";
        internal const String XML_Node_Field = "f";
        internal const String XML_Node_Set = "s";
        internal const String XML_Node_Error = "error";
        internal const String XML_Error_Code = "code";
        internal const String XML_Error_Message = "description";
        internal const String XML_SessionID = "sessionid";
        internal const String XML_Attribute_ID = "i";
        internal const String XML_Attribute_Name = "n";
        internal const String XML_Attribute_Obejct_ID = "id";
        internal const String XML_Attribute_Value = "v";
        internal const String XML_Attribute_Type = "t";
        internal const String XML_Attribute_Item_Class = "item-class";
        internal const String XML_FieldType_String = "s";
        internal const String XML_FieldType_Date = "d";
        internal const String XML_FieldType_Int = "i";
        internal const String XML_FieldType_Bool = "b";
        internal const String XML_FieldType_Ref = "r";
        internal const String XML_FieldType_Color = "clr";
        internal const String XML_FieldType_Float = "f";
        internal const String XML_FieldType_Long = "l";
        internal const String XML_Bool_Value_True = "true";
        internal const String XML_Bool_Value_False = "false";

        internal const String XML_DocType_PubId = "";

        internal const String XML_Request_Type = "type";
        internal const String XML_Request_Version = "version";
        internal const String XML_Request_VersionId = "2.0";
        internal const String XML_Request_BuildNumber = "buildnumber";
        public const String XML_Request_BuildNumberId = "@buildnumber@";
        internal const String DTDSysString = "SYSTEM";
        internal const String DTDPubString = "internal \"\"";

        internal const String XML_METHOD_LOGIN = "login";
        internal const String XML_METHOD_SYSTEM_LOGIN = "system-login";
        internal const String XML_METHOD_SYSTEM_LOGOUT = "logout";
        internal const String XML_METHOD_LOGIN_OFFICE = "login-office";

        internal const String XML_METHOD_DIRECTORY = "directory";
        internal const String XML_METHOD_DIRECTORY_SAVE_NEW = "directory-save-new";
        internal const String XML_METHOD_DIRECTORY_SAVE = "directory-save";
        internal const String XML_METHOD_GET_DENIES = "find-deny-elements";
        internal const String XML_METHOD_FIND_DONORS = "find-donors";
        internal const String XML_METHOD_FIND_PRODUCTS = "find-products";
        internal const String XML_METHOD_LOGIN_DEPARTMENT = "login-department";
        internal const String XML_METHOD_GET_VISITS = "visit-journal";
        internal const String XML_METHOD_GET_DONOR_INFO = "donor-info";
        internal const String XML_METHOD_GET_DONOR_DENY_INFO = "donor-deny-single-info";
        internal const String XML_METHOD_GET_VISIT_INFO = "visit-info";
        internal const String XML_METHOD_SAVE_VISIT = "visit-save";
        internal const String XML_METHOD_REMOVE_DICTIONARY = "directory-remove";
        internal const String XML_METHOD_SAVE_DONOR = "donor-save";
        internal const String XML_METHOD_VISIT_CHANGE_STATE = "visits-change-state";
        internal const String XML_METHOD_GET_DONATIONS_INFO = "donations-info";
        internal const String XML_METHOD_DONATION_CHANGE_STATE = "donations-change-state";
        internal const String XML_METHOD_DONATION_SAVE = "donation-save";
        internal const String XML_METHOD_DONOR_DENY_SAVE = "donor-deny-save";
        internal const String XML_METHOD_DAILY_REPORT_SAVE = "daily-stock-report-save";
        internal const String XML_METHOD_GET_PRODUCTS_INFO = "products-info";
        internal const String XML_METHOD_GET_PRODUCTS_SHORT_INFO = "products-short-info";
        internal const String XML_METHOD_FIND_PRODUCT_TRANSACTIONS = "find-product-transactions";
        internal const String XML_METHOD_PRODUCT_TRANSACTION_SAVE = "product-transaction-save";
        internal const String XML_METHOD_PRODUCT_TRANSACTION_GET = "product-transaction-get";
        internal const String XML_METHOD_PRODUCT_TRANSACTION_DELETE = "product-transaction-delete";
        internal const String XML_METHOD_PRODUCT_TRANSACTION_CHANGE_STATE = "product-transaction-change-state";
        internal const String XML_METHOD_RECIPIENT_GET = "recipient-get";
        internal const String XML_METHOD_FIND_RECIPIENT = "find-recipient";
        internal const String XML_METHOD_RECIPIENT_SAVE = "recipient-save";
        internal const String XML_METHOD_GENERATE_REPORT = "generate-report";
        internal const String XML_METHOD_SAVE_BATCH = "batch-save";
        internal const String XML_METHOD_BATCH_CHANGE_STATE = "batches-change-state";
        internal const String XML_METHOD_GET_EXPRESS_DONATIONS = "express-donation-journal";
        internal const String XML_METHOD_GET_EXPRESS_DONATION_INFO = "express-donation-info";
        internal const String XML_METHOD_SAVE_EXPRESS_DONATION = "express-donation-save";
        internal const String XML_METHOD_EXEC_EXPRESS_DONATION = "express-donation-execute";
        internal const String XML_METHOD_GET_DONOR_SHORT_HISTORY = "donor-get-short-history";
        internal const String XML_METHOD_GET_WEB_DENIES = "web-service-get-denies";
        internal const String XML_METHOD_GET_DONATIONS = "donation-journal";
        internal const String XML_METHOD_GET_DENIES_INFO = "deny-elements-info";
        internal const String XML_METHOD_LOAD_RECIPIENT_JOURNAL = "find-recipient";
        internal const String XML_METHOD_SAVE_PROCESS = "process-save";
        internal const String XML_METHOD_DEFECT_SAVE = "defect-save";
        internal const String XML_METHOD_DEFECTS_REMOVE = "defects-remove";
        internal const String XML_METHOD_PROCESS_JOURNAL = "process-journal";
        internal const String XML_METHOD_PROCESSES_INFO = "processes-info";
        internal const String XML_METHOD_PRODUCT_DEFECTS = "product-defects";
        internal const String XML_METHOD_BATCH_CREATE_PROCESS = "batch-create-process";
        internal const String XML_METHOD_PROCESSES_CHANGE_STATE = "processes-change-state";
        internal const String XML_METHOD_PROCESSES_STAGE_CLOSE = "processes-stage-close";
        internal const String XML_METHOD_GET_TRANSFUSION = "transfusion-get";
        internal const String XML_METHOD_SAVE_TRANSFUSION = "transfusion-save";
        internal const String XML_METHOD_CREATE_TRANSFUSION = "create-transfusion";
        internal const String XML_METHOD_LOAD_TRANSFUSION_JOURNAL = "transfusion-journal";
        internal const String XML_METHOD_SAVE_PATIENT_CARD = "patient-card-save";
        internal const String XML_METHOD_GET_PATIENT_CARD = "patient-card-get";
        internal const String XML_METHOD_DONOR_MANAGE_RECIPIENTS = "donor_manage_recipients";
        internal const String XML_METHOD_DONOR_MANAGE_PERSONAL_DONORS = "donor-manage-personal-donors ";
        internal const String XML_METHOD_LOAD_TRANSFUSION_REQUEST_JOURNAL = "transfusion-request-journal";
        internal const String XML_METHOD_SAVE_TRANSFUSION_REQUEST = "transfusion-request-save";
        internal const String XML_METHOD_GET_TRANSFUSION_REQUEST = "transfusion-request-get";
        internal const String XML_METHOD_DOCTOR_APPROVE_TRANSFUSION_REQUEST = "transfusion-request-doctor-approve";
        internal const String XML_METHOD_CANCEL_TRANSFUSION_REQUEST = "transfusion-request-cancel";
        internal const String XML_METHOD_OBSERVATION_SAVE = "observation-save";
        internal const String XML_METHOD_PRODUCT_RECEIVE = "receive-product";
        internal const String XML_METHOD_PRODUCTS_RECEIVE = "receive-products";
        internal const String XML_METHOD_PRODUCTS_RETURN = "return-products";
        internal const String XML_METHOD_TRANSFUSION_EXECUTE = "transfusion-execute";
        internal const String XML_METHOD_VACANT_PRODUCTS_GET = "get-vacant-products";
        internal const String XML_METHOD_TRANSFUSION_ADD_PRODUCT = "transfusion-add-product";
        internal const String XML_METHOD_TRANSFUSION_REMOVE_PRODUCT = "transfusion-remove-product";
        internal const String XML_METHOD_PRODUCT_JOURNAL = "product-journal";
        internal const String XML_METHOD_LOAD_SERVER_OPTIONS = "system-params-get";
        internal const String XML_METHOD_NOTIFY = "notify";
        internal const String XML_METHOD_GET_FREE_TRANSFUSION_REQUEST_NR = "get-next-transfusion-request-nr";
        internal const String XML_METHOD_GET_TREATMENT_REQUEST_JOURNAL = "treatment-request-journal";
        internal const String XML_METHOD_GET_TREATMENT_REQUEST = "treatment-request-get";
        internal const String XML_METHOD_SAVE_TREATMENT_REQUEST = "treatment-request-save";
        internal const String XML_METHOD_CANCEL_TREATMENT_REQUEST = "treatment-request-cancel";
        internal const String XML_METHOD_CREATE_TREATMENT = "treatment-create";
        internal const String XML_METHOD_START_TREATMENT = "treatment-start";
        internal const String XML_METHOD_CANCEL_TREATMENT = "treatment-cancel";
        internal const String XML_METHOD_FINISH_TREATMENT = "treatment-finish";
        internal const String XML_METHOD_GET_TREATMENT_JOURNAL = "treatment-journal";
        internal const String XML_METHOD_GET_TREATMENT = "treatment-get";
        internal const String XML_METHOD_SAVE_TREATMENT = "treatment-save";
        internal const String XML_METHOD_TRANSFUSION_REQUEST_ADDITIONAL_SAVE = "transfusion-request-additional-save";

        internal const String XML_METHOD_FIND_SAMPLES_BY_NR = "find-samples-by-nr";
        internal const String XML_METHOD_GET_SAMPLES_SHORT_INFO = "get-samples-short-info";

        internal const String XML_METHOD_DIRECTORY_VERSIONS = "directory-versions";
        internal const String XML_METHOD_CREATE_REQUESTS = "create-requests";
        internal const String XML_METHOD_CREATE_REQUEST_XXX = "create-requests-xxx";
        internal const String XML_METHOD_REGISTRATION_JOURNAL = "registration-journal";
        internal const String XML_METHOD_REQUEST_INFO = "request-info";
        internal const String XML_METHOD_PATIENT_INFO = "patient-info";
        internal const String XML_METHOD_GET_NEXT_REQUEST_NR = "get-next-request-nr";
        internal const String XML_METHOD_GET_REQUEST_NR_RANGE = "request-nr-get-range";
        internal const String XML_METHOD_REQUEST_NR_UNLOCK = "request-nrs-unlock";
        internal const String XML_METHOD_REQUEST_NR_CHECK_AND_RESERVE = "request-nr-check-and-reserve";
        internal const String XML_METHOD_REQUEST_SHORT_INFO = "requests-short-info";
        internal const String XML_METHOD_REQUEST_PAYMENT = "request-payment";
        internal const String XML_METHOD_REQUEST_PAYMENT_CANCEL = "request-payment-cancel";
        internal const String XML_METHOD_CHECK_REQUEST_NRS = "check-request-nrs";
        internal const String XML_METHOD_REQUEST_DELETE = "remove-requests";
        internal const String XML_METHOD_PATIENT_SAVE = "patient-save";
        internal const String XML_METHOD_OPERATION_TEMPLATE_SAVE = "operation-template-save";
        internal const String XML_METHOD_OPERATION_ARRIVAL = "operation-arrival";
        internal const String XML_METHOD_OPERATION_INFO = "operation-info";
        internal const String XML_METHOD_SYSTEM_OPERATION_INFO = "system-operation-info";
        internal const String XML_METHOD_SYSTEM_NEXT_MATERIAL_CATALOG_NR = "system-next-material-catalog-nr";
        internal const String XML_METHOD_CREATE_REQUEST_2 = "create-requests-2";
        internal const String XML_METHOD_GET_REQUESTS_RESULTS_2 = "get-requests-results-2";
        internal const String XML_METHOD_REQUESTS_SAMPLES = "request-samples";
        internal const String XML_METHOD_EQUIPMENT_DATA = "equipment-data";
        internal const String XML_METHOD_APPARATUS_DATA = "apparatus-data";
        internal const String XML_METHOD_TESTS_FOR_SAMPLES = "tests-for-samples";
        internal const String XML_METHOD_TESTS_FOR_SAMPLES_2 = "tests-for-samples2";
        internal const String XML_METHOD_WORKLIST_JOURNAL = "worklist-journal";
        internal const String XML_METHOD_WORKLIST_INFOS = "worklist-infos";
        internal const String XML_METHOD_WORKLIST_SAVE = "worklist-save";
        internal const String XML_METHOD_OUTSOURCE_REQUEST_JOURNAL = "outsource-journal";
        internal const String XML_METHOD_OUTSOURCE_REQUEST_INFO = "outsource-request-info";
        internal const String XML_METHOD_OUTSOURCE_REQUEST_SAVE_RESULTS = "outsource-request-save-results";
        internal const String XML_METHOD_OUTSOURCE_REQUEST_CHANGE_STATE = "outsource-request-change-state";
        internal const String XML_METHOD_LOAD_SERVICE_PRICE = "load-service-prices";
        internal const String XML_METHOD_WORK_JOURNAL = "work-journal";
        internal const String XML_METHOD_REQUEST_WORKS = "request-works";
        internal const String XML_METHOD_TEAM_JOURNAL = "team-journal";
        internal const String XML_METHOD_TEAM_SAVE = "team-save";
        internal const String XML_METHOD_TEAM_GET = "team-get";
        internal const String XML_METHOD_SAMPLE_INFO = "sample-info";
        internal const String XML_METHOD_SAVE_SAMPLE = "save-sample";
        internal const String XML_METHOD_USER_OPTIONS = "options-get";
        internal const String XML_METHOD_ARCHIVE_RACK_JOURNAL = "archive-rack-journal";
        internal const String XML_METHOD_ARCHIVE_RACK_CREATE = "archive-rack-create";
        internal const String XML_METHOD_ARCHIVE_RACK_SAVE = "archive-rack-save";
        internal const String XML_METHOD_ARCHIVE_RACK_GET = "archive-rack-get";
        internal const String XML_METHOD_ARCHIVE_RACK_PUT_TUBES = "archive-rack-put-tubes";
        internal const String XML_METHOD_ARCHIVE_RACK_EXTRACT_TUBES = "archive-rack-extract-tubes";
        internal const String XML_METHOD_ARCHIVE_RACK_CLEAR = "archive-rack-clear";
        internal const String XML_METHOD_ARCHIVE_RACK_DELETE = "archive-rack-delete";
        internal const String XML_METHOD_PATIENT_JOURNAL = "patient-journal";
        internal const String XML_METHOD_PATIENT_QUICK_SEARCH = "patient-quick-search";
        internal const String XML_METHOD_ARCHIVE_RACK_CHANGE_STORAGE = "archive-rack-change-storage";
        internal const String XML_METHOD_ARCHIVE_RACK_FIND = "archive-rack-find";
        internal const String XML_METHOD_WAYBILL_JOURNAL = "waybill-journal";
        internal const String XML_METHOD_WAYBILL_GET_NEXT_NR = "get-next-waybill-nr";
        internal const String XML_METHOD_WAYBILL_SAVE = "waybill-save";
        internal const String XML_METHOD_WAYBILL_GET = "waybill-get";
        internal const String XML_METHOD_WAYBILL_REMOVE = "waybill-remove";
        internal const String XML_METHOD_ARCHIVE_RACK_SLOTS_FIND = "archive-rack-slots-find";
        internal const String XML_METHOD_PATIENTS_BY_CODE = "patients-by-code";
        internal const String XML_METHOD_TARGET_MAP_GET = "target-map-get";
        internal const String XML_METHOD_TARGET_MAP_VERSION = "target-map-version";
        internal const String XML_METHOD_AUDIT_JOURNAL = "audit-journal";
        internal const String XML_METHOD_PROCESS_BARCODE = "process-barcode";
        internal const String XML_METHOD_STORAGE_OPERATION_SAVE = "storage-operation-save";
        internal const String XML_METHOD_PATIENT_HISTORY = "patient-history";
        internal const String XML_METHOD_FIND_AUTO_WORKLISTS = "find-autoworklists";
        internal const String XML_METHOD_FIND_MICROBYOLOGY_AUTO_WORKLISTS = "find-microbyology-autoworklists";
        internal const String XML_METHOD_AUTO_WORKLISTS_CHANGE_STATE = "autoworklists-change-state";
        internal const String XML_METHOD_MICROBYOLOGY_AUTO_WORKLISTS_CHANGE_STATE = "microbyology-autoworklists-change-state";
        internal const String XML_METHOD_GET_PRICES = "get-prices";
        internal const String XML_METHOD_GET_STATISTICS = "get-statistics";
        internal const String XML_METHOD_QUOTA_CREATE = "create-quota";
        internal const String XML_METHOD_QUOTA_GET = "quota-get";
        internal const String XML_METHOD_QUOTA_JOURNAL = "quota-journal";
        internal const String XML_METHOD_QUOTA_CHANGE_ACTIVE_STATE = "quota-change-active-state";
        internal const String XML_METHOD_QUOTA_REMOVE = "quota-remove";
        internal const String XML_METHOD_LOGGING_JOURNAL = "logging-journal";
        internal const String XML_METHOD_FIND_QUOTAS_FOR_HOSPITAL = "find-quotas-for-hospital";
        internal const String XML_METHOD_RESEND_CLOSE_EVENTS = "resend-close-events";
        internal const String XML_METHOD_PERSON_HISTORY = "person-history";
        internal const String XML_METHOD_GET_PATIENTLK_REQUEST = "get-patientlk-request";
        internal const String XML_METHOD_GET_REQUEST_TEMP_PASSWORD = "get-request-temp-password";
        internal const String XML_METHOD_GENERATE_REQUEST_TEMP_PASSWORD = "generate-request-temp-password";
        internal const String XML_METHOD_GET_PATIENTLK_ALL_REQUESTS = "get-patientlk-all-requests";
        internal const String XML_METHOD_CREATE_EMPLOYEES_MESSAGE = "create-employees-message";
        internal const String XML_METHOD_GET_EMPLOYEE_MESSAGES = "get-employee-messages";
        internal const String XML_METHOD_GET_EMPLOYEE_MESSAGE_TEXT = "get-employee-message-text";
        internal const String XML_METHOD_GET_EMPLOYEES_NEW_MESSAGE = "get-employees-new-messages";
        internal const String XML_METHOD_REQUEST_TARGETS_WITH_PRICES = "request-targets-with-prices";
        internal const String XML_METHOD_GET_EMPLOYEE_SENT_MESSAGES = "get-employee-sent-messages";
        internal const String XML_METHOD_GET_SAMPLES_TARGETINFO = "get-samples-targetinfo";
        internal const String XML_METHOD_GET_TARGETS_DETAILS = "get-targets-details";
        internal const String XML_METHOD_CREATE_PAYMENT = "create-payment";
        internal const String XML_METHOD_PAYMENT_NFO = "payment-info";
        internal const String XML_METHOD_COMPLETE_PAYMENT = "complete-payment";
        internal const String XML_METHOD_GET_REQUEST_PAYMENTS = "get-request-payments";
        internal const String XML_METHOD_REJECT_PAYMENT_SERVICES = "reject-payment-services";
        internal const String XML_METHOD_COMPLETE_REJECTED_PAYMENT = "complete-rejected-payment";
        internal const String XML_METHOD_CANCEL_REJECTED_PAYMENT_SERVICES = "cancel-rejected-payment-services";
        internal const String XML_METHOD_OPERATION_JOURNAL = "operation-journal";
        internal const String XML_METHOD_UPDATE_PAYMENT = "update-payment";
    }

    // Event identifiers of Blood Service
    internal struct BloodProcessorsNames
    {
        // Специальное имя процессора, обозначающее процессор используемый для наследования и
        // не участвующий в обслуживании запросов.
        internal const String Stub = "BC82ECB7-88E1-4C89-BB5F-B443E5AA3663";

        internal const String DonorSave = "donor-save";
        internal const String DenySave = "deny-save";
        internal const String DonorDenySave = "donor-deny-save";
        internal const String DonationSave = "donation-save";
    }

    // Event identifiers of LIS
    public struct LisProcessorsNames
    {
        // Специальное имя процессора, обозначающее процессор используемый для наследования и
        // не участвующий в обслуживании запросов.
        public const String Stub = "EF79A339-29C5-466F-B3E9-5B8D503FCE6B";

        public const String RequestSave = "request-save";
        public const String RequestResults = "request-results";
        public const String RequestClose = "request-close";
        public const String DirectorySync = "directory-sync";
        public const String WorkApprove = "work-approve";
        public const String WorkCancel = "work-cancel";
        public const String WarehouseUndoArrival = "warehouse-undo-arrival";
        public const String WarehouseExpense = "warehouse-expense";
        public const String RetirementExpense = "warehouse-retirement";
        public const String SampleSave = "sample-save";
        public const String SampleClose = "sample-close";
        public const String RemoteActivate = "remote-activate";
        public const String ImportDictionaries = "import-dictionaries";
        public const String PatientSyncrhonize = "patient-synchronize";
        // public const String OutsourceRequestsSent = "outsource-requests-sent";
        public const String BiomaterialDelivered = "biomaterial-delivered";
    }

    public struct ReportServerProcessorsNames
    {
        public const String ExecuteReport = "execute-report";
        public const String GetReportDescription = "get-report-description";
        public const String GetReportList = "get-report-list";
        public const String GetDictionary = "get-dictionary";
        public const String DebugGarbageCollect = "debug-garbage-collect";
    }

    internal struct JavaLinkClasses
    {
        internal const Int32 TYPE_ARRIVAL = 1;
        internal const Int32 TYPE_TRANSFER = 2;
        internal const Int32 TYPE_RETIREMENT = 3;
        internal const Int32 TYPE_RETURN = 4;
        internal const Int32 TYPE_EXPENSE = 5;
        internal const Int32 TYPE_EXPENDITURE = 6;
    }

    internal struct ServerConst
    {
        internal const String HTTP_Method_Post = "POST";
        internal const String HTTP_Method_Get = "GET";
        internal const String HTTP_Method_List = "LIST";
        internal const String HTTP_User_Agent = "Mozilla/3.0";
        internal const String HTTP_Accept = "text/html, */*";
        internal const String HTTP_File_Accept = "*/*";
        internal const String HTTP_Content_Type = "text/html";
        internal const String HTTP_File_Content_Type = "";
        internal const String HTTP_http_Prefix = "http://";
        internal const String HTTP_Lis_Id = "LIS-Id: ";
        internal const String HTTP_Lis_Path = "LIS-Path: ";
        internal const String HTTP_Lis_Description = "LIS-Description: ";
        internal const String HTTP_FileServer_URL = "storage?sessionid=";
        public const String HTTP_Header_Client_Id = "CLID";
        public const String HTTP_Login_Code = "19901";
        internal const String LOGIN_Machine_Name = "WebService";
    }


    public struct RequestHeaderConst
    {
        public const String Header_CLID = "CLID";
        public const String Header_REQID = "REQID";
    }
    internal struct RightsConst
    {
        internal const Int32 Rights_Web_Login = 60;
        internal const Int32 Approve_Product_Transaction = 101;
    }

    public struct SettingsConst
    {
        public const String Settings_File_Name = "settings.xml";
        internal const String Mapping_File_Name = "/Mapping.xml";
        public const String Lims_Dictionary_File_Name = "/LimsStaticDictionary.xml";
        public const String Hem_Dictionary_File_Name = "/HemStaticDictionary.xml";
        internal const String Settings_WayBill_FileName = "waybill.frx";
        public const String Dictionary_Mapping_FileName = "DictionaryMapping.xml";
    }


    public struct AndOrListContst
    {
        public const String AndOperator = "AND";
        public const String OrOperator = "OR";
    }

    internal struct SQL_Queries
    {

        internal static class KLADR_FIC_Queries
        {

            internal static string Find_Object = "SELECT * " +
                "FROM {0} " +
                "WHERE {1} = @{1}";


            internal static string Insert_Object = "INSERT INTO {0} " +
                "({1})  values ({2})";

            internal static string Update_Object = "UPDATE {0} " +
                "SET {1}  WHERE {2} = @{2}";

            internal static string Mark_As_Removed_Objects = "UPDATE {0} " +
                "SET STATE = 10";
        }

        internal struct KLADR_SQL_Queries
        {

            internal static string Select_Region = "SELECT " +
                "ADDRESSREGION_ID,ABBREV_SHNAME, ABBREV_NAME, ADDRESS_NAME, " +
                "ADDRESS_NAME || ' '  || ABBREV_SHNAME || ' [' ||  ADDRESSREGION_ID || ']' as REGION_ADDRESS_NAME " +
                "FROM VW_REGIONS_2 ORDER BY ADDRESS_NAME";

            internal static string Select_City = "SELECT " +
                "ADDRESS_ID, ADDRESS_NAME, ABBREV_SHNAME, ABBREV_NAME, ADDRESS_PAR, " +
                "ADDRESS_NAME || '  ' || ABBREV_SHNAME as FULL_ADDRESSNAME " +
                "FROM VW_ADDRESS_2 WHERE " +
                "ADDRESSREGION_ID = {0} " +
                "AND ADDRESS_PAR=0 " +
                "ORDER BY ADDRESS_NAME";

            internal static string Select_Town = "SELECT  " +
                "ADDRESS_ID,ADDRESS_NAME,ABBREV_SHNAME, ABBREV_NAME,ADDRESS_PAR, " +
                "ADDRESS_NAME || '  ' || ABBREV_SHNAME as FULL_ADDRESSNAME " +
                "FROM VW_ADDRESS_2 WHERE " +
                "ADDRESSREGION_ID = {0} " +
                "AND ADDRESS_PAR={1} " +
                "ORDER BY ADDRESS_NAME";

            internal static string Select_Street = "SELECT  " +
                "ADDRESS_ID, ADDRESS_NAME, ABBREV_SHNAME, ABBREV_NAME, ADDRESS_PAR, " +
                "ADDRESS_NAME || '  ' || ABBREV_SHNAME as FULL_ADDRESSNAME " +
                "FROM VW_STREET_2 WHERE " +
                "ADDRESSREGION_ID = {0} " +
                "AND ADDRESS_PAR={1} " +
                "ORDER BY ADDRESS_NAME";

            internal static string Parent_Info = "SELECT   " +
                "CITY.ADDRESS_ID  PARENT_ID   " +
                "FROM vw_address_2  TOWN  " +
                "left JOIN  vw_address_2  CITY ON CITY.ADDRESS_ID = TOWN.ADDRESS_PAR AND  CITY.ADDRESSREGION_ID = TOWN.ADDRESSREGION_ID   " +
                "WHERE TOWN.ADDRESS_ID = {0}  AND TOWN.ADDRESSREGION_ID = {1}";

            internal static string Select_District = "SELECT ID AS DISTRICT_ID, DISTRICT_NAME " +
                            "FROM  VW_DISTRICT " +
                            "WHERE  " +
                            "ADDRESSREGION_ID = {0} " +
                            "AND ADDRESS_ID = {1}";

            internal static string Region_Info = "SELECT " +
                     "ADDRESSREGION_ID,ABBREV_SHNAME, ABBREV_NAME,ADDRESS_NAME " +
                     "FROM VW_REGIONS_2 " +
                     "WHERE ADDRESSREGION_ID = {0}";

            internal static string Select_Town_Types = "SELECT DISTINCT " +
                     "ABBREV_NAME, ABBREV_SHNAME " +
                     "FROM KLADR_ABBREV " +
                      "WHERE ABBREV_LEVEL > 2 and ABBREV_LEVEL < 5";

            internal static string Select_Street_Types = "SELECT DISTINCT " +
                "ABBREV_NAME, ABBREV_SHNAME FROM KLADR_ABBREV WHERE " +
                "ABBREV_LEVEL = 5";
        }

        internal struct KLADR_SQL_Fields
        {
            internal static string Region_Name = "REGION_ADDRESS_NAME";
            internal static string Region_Id = "ADDRESSREGION_ID";

            internal static string Address_Full_Name = "FULL_ADDRESSNAME";
            internal static string Address_Id = "ADDRESS_ID";

            internal static string Address_Name = "ADDRESS_NAME";
            internal static string Address_Short_Name = "ABBREV_SHNAME";

            internal static string District_Name = "DISTRICT_NAME";
            internal static string District_Id = "DISTRICT_ID";

            internal static string Abbrev_Name = "ABBREV_SHNAME";
            internal static string PARENT_ID = "PARENT_ID";
        }
    }

    public struct HemDictionaryNames
    {
        public const string DefaultDictionaryProperty = "Name";
        public const string DepartmentsProperty = "Departments";

        // Динамические справочники
        public const String AccessRight = "accessRight";
        public const String Attribute = "attribute";
        public const string Deny = "deny";
        public const string Department = "department";
        public const string UserDirectory = "userDirectory";
        public const string Employee = "employee";
        public const string DonorCategory = "donorCategory";
        public const string DonationTemplate = "donationTemplate";
        public const string PaymentCategory = "paymentCategory";
        public const string UserGroup = "userGroup";
        public const string DonationType = "donationType";
        public const string DocumentType = "documentType";
        public const string ProductClassification = "productClassification";
        public const string Hospital = "hospital";
        public const string BloodParameterGroup = "bloodParameterGroup";
        public const string AnamnesisParameterGroup = "anamnesisParameterGroup";
        public const string PhysioIndicatorGroup = "physioIndicatorGroup";
        public const string DenySource = "denySource";
        public const string ParameterGroup = "parameterGroup";
        public const string DenyDurationUnits = "denyDurationUnits";
        public const string Defect = "defect";
        public const string ContainerType = "containerType";
        public const string AdditiveType = "additiveType";
        public const string ProductJournal = "productJournal";
        public const string ProductType = "productType";
        public const string Storage = "storage";
        public const string ProcessTemplate = "processTemplate";
        public const string ProductFilter = "productFilter";
        public const string ProductState = "productState";
        public const string UserProfile = "userProfile";
        public const string Manufacturer = "manufacturer";
        public const string ProductTariff = "productTariff";
        public const string ProductionTemplate = "productionTemplate";
        public const string QuarantineTemplate = "quarantineTemplate";
        public const string TransfusionResult = "transfusionResult";
        public const string TransfusionRequestTemplate = "transfusionRequestTemplate";
        public const string TransfusionTemplate = "transfusionTemplate";
        public const string Doctor = "doctor";
        public const string ReagentType = "reagentType";
        public const string TreatmentRequestTemplate = "treatmentRequestTemplate";
        public const string TreatmentTemplate = "treatmentTemplate";
        public const string VeinAccessType = "veinAccessType";
        public const string Apparatus = "apparatus";
        public const string Diagnosis = "diagnosis";
        public const string OperatingRoom = "operatingRoom";
        public const string AllergicReactionType = "allergicReactionType";
        public const string ComplicationType = "complicationType";
        public const string ComplicationReason = "complicationReason";
        public const string TherapyType = "therapyType";
        public const string AdditionalComponent = "additionalComponent";
        public const string StorageOperationType = "storageOperationType";
        public const string Contractor = "contractor";
        public const string GoodsType = "goodsType";

        // Статические справочники
        public const string Sex = "sSex";
        public const string FSex = "fSex"; //справочник "Пол" для фильтра (включает дополнительные поля вроде "да", "нет", "не знаю")
        public const string DonorStatus = "sDonorStatus";
        public const string DonorType = "donorType";
        public const string EdcRecordType = "edcRecordType";
        public const string VisitReserveType = "visitReserveType";
        public const string ExpressDonationResult = "expressDonationResult";
        public const string DenyType = "sDenyType";
        public const string Occupation = "sDononorOccupation";
        public const string TransfusionRequestState = "sTransfusionRequestState";
        public const string ListType = "sListType";
        public const string ProductUnitType = "sProductUnitType";
        public const string FBoolean = "fBoolean";
        public const string TransfusionState = "sTransfusionState";
        public const string TreatmentRequestState = "sTreatmentRequestState";
        public const string TreatmentState = "sTreatmentState";
        public const String LimsObjectType = "static-audit";
        public const string BooleanValue = "sBooleanValue";
        public const string WorkState = "static-workState";
        public const string TransfusionRequestType = "sTransfusionRequestType";
    }


    public struct LimsDictionaryNames
    {
        // Динамические справочники
        public const String Department = "department";
        public const String CustDepartment = "custDepartment";
        public const String UserDirectory = "userDirectory";
        public const String Employee = "employee";
        public const String UserGroup = "userGroup";
        public const String Hospital = "hospital";
        public const String IncomingMaterialType = "incomingMaterialType";
        public const String ContainerType = "containerType";
        public const String Biomaterial = "bioMaterial";
        public const String Target = "target";
        public const String RequestForm = "requestForm";
        public const String ContainerGroup = "containerGroup";
        public const String UserField = "userField";
        public const String RequestCustomState = "requestCustomState";
        public const String Doctor = "doctor";
        public const String PayCategory = "payCategory";
        public const String Test = "test";
        public const String SampleBlank = "sampleBlank";
        public const String ExternalSystem = "externalSystem";
        public const String ServiceShort = "serviceShort";
        public const String Equipment = "equipment";
        public const String WorkPlace = "workPlace";
        public const String WorklistDef = "worklistDef";
        public const String OutsourcerNew = "outsourcerNew";
        public const String UserProfile = "userProfile";
        public const String Organization = "organization";
        public const String TestProfile = "profile";
        public const String ServerReport = "serverReport";
        public const String PricelistShort = "pricelistShort";
        public const String Unit = "unit";
        public const String PatientGroup = "patientGroup";
        public const String EmployeeRole = "employeeRole";
        public const String TeamTemplate = "teamTemplate";
        public const String DefectType = "defectType";
        public const String CommentSource = "commentSource";
        public const String Office = "office";
        public const String StorageRule = "storageRule";
        public const String ArchiveStorage = "archiveStorage";
        public const String ArchiveRackType = "archiveRackType";
        public const String TestPrintGroup = "testPrintGroup";
        public const String TargetGroup = "targetGroup";
        public const String TreeViewLayout = "treeViewLayout";
        public const String AccessRight = "accessRight";
        public const String Courier = "courier";
        public const String Antibiotic = "antibiotic";
        public const String MicroOrganism = "microOrganism";
        public const String UserDirectoryValue = "userDirectoryValue";
        public const String BiomaterialContainer = "biomaterialContainer";
        public const String FundingSource = "fundingSource";
        public const String Supplier = "supplier";
        public const String Manufacturer = "manufacturer";
        public const String MaterialUnit = "materialUnit";
        public const String Material = "material";
        public const String UnitTreeNode = "unitTreeNode";
        public const String PaymentReturnReason = "paymentReturnReason";
        public const String ColonyFormingUnit = "colonyFormingUnit";
        public const String LegalEntity = "legalEntity";

        // Статические справочники
        public const String Sex = "sex";
        public const String SexFilter = "sex-filter";
        public const String YesNoIgnore = "yes-no-ignore";
        public const String Billed = "billed";
        public const String RequestState = "requestState";
        public const String GuiElementReportExecutors = "guiElementReportExecutors";
        public const String WorkState = "static-workState";
        public const String SampleState = "static-sampleState";
        public const String CyclePeriod = "cyclePeriod";
        public const String NormalityState = "normalityState";
        public const String BiomaterialStateEx = "biomaterialStateEx";
        public const String Priority = "priority";
        public const String HasDefects = "has-defects";
        public const String Normality = "static-normality";
        public const String Removed = "removed";
        public const String DocumentState = "document-state";
        public const String FillingDirection = "static-fillingDirection";
        public const String AxisNumerationType = "static-axisNumerationType";
        public const String DefectState = "sDefectState";
        public const String QuotaState = "static-quota-states";
        public const String RequestFilterState = "static-requestFilterState";
        public const String DateType = "date-type";
        public const String QuotaControlType = "static-quota-controlTypes";
        public const String LogObjectType = "static-log-objectTypes";
        public const String LogOperationType = "static-log-operationTypes";
        public const String TargetInfoState = "static-targetInfoState";
        public const String PaymentState = "static-paymentState";

    }

    internal struct DictionaryPropertyConst
    {
        internal const String Name = "Name";
        internal const String Fullname = "Fullname";
    }

    //public struct ValueStateConst
    //{
    //    public static int New = 1;
    //    public static int Done = 2;
    //    public static int Approved = 3;
    //    public static int Cancelled = 4;
    //    public static int Loaded = 5;
    //    public static int Delayed = 6;
    //}

    internal struct RedirectConst
    {
        internal static string MainPage = "~/MainPage.aspx";
        internal static string DonorPrintForm = "~/DonorPrintForm.aspx";
        internal static string LoginPage = "~/LoginPage.aspx";
        internal static string VisitStartedPage = "~/VisitStartedPage.aspx";
        internal static string UserSavedPage = "~/UserSavedPage.aspx";
        internal static string DenySearchResultPage = "~/DenySearchResultPage.aspx";
        internal static string DenySavedPage = "~/DenySavedPage.aspx";
        internal static string VisitCompletedPage = "~/VisitCompletedPage.aspx";
        internal static string DonorCard_w_Params = "DonorCard.aspx?";
        internal static string VisitResultPage_w_Params = "VizitResultPage.aspx?";
        internal static string VisitNrUpdatePage = "VizitNrUpdatePage.aspx?";
        internal static string UserPage_w_Params = "UserPage.aspx?";
        internal static string UserDeletedPage_w_Params = "UserDeltedPage.aspx?";
        internal static string ContactSearchResultPage = "ContactSearchResultPage.aspx";
        internal static string VisitNrUpdatedPage = "VisitNrUpdatedPage.aspx";
        internal static string DonationResultPage = "DonationClosePage.aspx?";
        internal static string MainLabPage = "MainLabPage.aspx?";
        internal static string NoUpdate = "NoUpdate";
    }

    internal struct LisRequestPriorities
    {
        internal const Int32 LIS_PRIORITY_LOW = 10;
        internal const Int32 LIS_PRIORITY_HIGH = 20;
    }

    internal struct ExternalPriorities
    {
        internal const Int32 PRIORITY_LOW = 0;
        internal const Int32 PRIORITY_HIGH = 1;
    }

    public struct SexConst
    {
        public const Int32 LIS_GENDER_NONE = 0;
        public const Int32 LIS_GENDER_MALE = 1;
        public const Int32 LIS_GENDER_FEMALE = 2;
        public const Int32 LIS_GENDER_ALL = 3;
    }

    internal struct EventTypes
    {
        internal const String ImportDictionaries = "import-dictionaries";
    }

    internal struct EdcReordTypeConst
    {
        internal static int Donor = 1;
        internal static int Deny = 2;
    }

    internal struct OrmMappingTypes
    {
        public const String NUMERIC = "numeric";
        public const String VARCHAR = "varchar";
        public const String DATE = "date";
        public const String UNDEFINED = "undefined";
    }

    internal class NumValues
    {
        internal const Int32 F029E319DB784CBC9F3FD12A7C4A13AF = 156875;
        internal const Int32 A8CF8EE3B08B4392B40EDFBD442B60C7 = 789546;
        internal const Int32 CEE7989355E64B0CBEE0AD6B7C46B07E = 568794;
        internal const Int32 E800E8228C8F4EB1A154AB11CD7C2C3D = 823497;
    }

    internal struct ParameterTypes
    {
        internal const int TYPE_STRING = 1;
        internal const int TYPE_BOOLEAN = 3;
        internal const int TYPE_DATETIME = 4;
        internal const int TYPE_ENUM = 5;
        internal const int TYPE_SET = 6;
        internal const int TYPE_SEX = 8;
    }

    public struct ErrorMessageTypes
    {
        public const Int32 ERROR = 0;
        public const Int32 WARNING = 1;
        public const Int32 INFO = 2;
    }

    public struct TreatmentStates
    {
        public const Int32 NEEDSPRODUCTSSELECTION = 1;
        public const Int32 PRODUCTSORDERED = 2;
        public const Int32 ACCEPTED = 5;
        public const Int32 STARTED = 10;
        public const Int32 CANCELED = 15;
        public const Int32 FINISHED = 20;
    }

    internal struct ParameterValueState
    {
        internal const int STATE_NEW = 1;
        internal const int STATE_ENTERED = 2;
        internal const int STATE_APPROVED = 3;
        internal const int STATE_CANCELLED = 4;
        internal const int STATE_LOADED = 5;
        internal const int STATE_DELAYED = 6;
    }

    internal struct StageTypes
    {
        internal const int STAGE_TYPE_QUESTIONNAIRE = 1;
        internal const int STAGE_TYPE_EXPRESS_LAB = 2;
        internal const int STAGE_TYPE_DOCTOR = 3;
        internal const int STAGE_TYPE_MAIN_LAB = 4;
        internal const int STAGE_TYPE_FINISH = 5;
    }

    public struct SystemLoggingLevels
    {
        public const String LOGIN_LEVEL_ALL = "ALL";
        public const String LOGIN_LEVEL_FATAL = "FATAL";
        public const String LOGIN_LEVEL_ERROR = "ERROR";
        public const String LOGIN_LEVEL_WARN = "WARN";
        public const String LOGIN_LEVEL_INFO = "INFO";
        public const String LOGIN_LEVEL_DEBUG = "DEBUG";
        public const String LOGIN_LEVEL_TRACE = "TRACE";
        public const String LOGIN_LEVEL_OFF = "OFF";
    }

    public struct Rights
    {
        public const int non_existent = -1000;
    }

    public struct HemRightsConst
    {
        public const int Rights_Web_Login = 0;
        public const int donor_journal = 39;
        public const int recipient_journal = 66;
        public const int treatment_request_journal = 137;
        public const int treatment_journal = 138;
        public const int edit_transfusion_request_card = 142;
    }

    public struct LisRightsConst
    {
        public const int man_request_template_journal = 0;
        public const int man_registration_journal = 91;
        public const int team_journal = 186;
        public const int waybill_journal = 188;
        public const int archive_journal = 91;//190;
        public const int patient_journal = 135;
        public const int request_pay = 200;
        public const int request_pament_cancel = 201;
        public const int quota_journal = 205;
        public const int quota_change_active_state = 206;
        public const int quota_remove = 207;
        public const int target_stage_template = 208;
        public const int send_messages = 209;
        public const int access_payment = 213;
    }

    public enum RequestState : int
    {
        Registration = 1, // Регистрация
        Opened = 2,       // Открыта
        Closed = 3,       // Закрыта
        Deleted = 4,      // Удалена
        Delayed = 5       // Просрочена
    }

    //public struct LimsSampleStates
    //{
    //    public const int STATE_NEW = 1;
    //    public const int STATE_CLOSED = 2;
    //    public const int STATE_LOADED = 3;
    //    public const int STATE_DONE = 4;
    //    public const int STATE_APPROVED = 5;
    //    public const int STATE_CANCELLED = 6;
    //}

    public struct LimsWorkStates
    {
        public const int STATE_NEW = 1;
        public const int STATE_LOADED = 2;
        public const int STATE_DONE = 3;
        public const int STATE_APPROVED = 4;
        public const int STATE_CANCELLED = 5;
    }

    public struct HemTreeTypes
    {
        public const int DonorInDonorJournal = 1;
        //BC_TREE_TYPE_DENY_ELEMENT_IN_DENY_JOURNAL = 2;
        //BC_TREE_TYPE_DONOR_DENY = 3;
        //BC_TREE_TYPE_VISIT_IN_VISIT_JOURNAL = 4;
        //BC_TREE_TYPE_DONATION_IN_DONATION_JOURNAL = 5;
        //BC_TREE_TYPE_EXCHANGE_LOG_IN_JOURNAL = 6;
        public const int ProductInProductJournal = 7;
        public const int PatientInPatientJournal = 68; //  Журнал пациентов (реципиентов)
        public const int TreatmentRequestInTreatmentRequestJournal = 75;
        public const int TreatmentInTreatmentJournal = 76;
    }

    public struct LimsTreeTypes
    {
        public const int LogItemInLogJournal = -1;
        public const int RequestInRegistrationJournal = 1;
        public const int SampleInRegistrationJournal = 2;
        public const int SampleInRegistrationForm = 3;
        public const int SampleInWorkJournal = 4;
        public const int WorkInWorkJournal = 5; // Работы в рабочем журнале
        public const int WorkInSampleForm = 6;  // Работы в карточке пробы
        public const int MaterialInOperationArrival = 7;
        public const int BatchInOperationExpense = 8;
        public const int BatchInStorageJournal = 9;
        public const int OperationInOperationJournal = 10;
        public const int WorkInPatientHistory = 11;
        public const int PatientInPatientJournal = 12;
        public const int BillInBillJournal = 13;
        public const int ServiceInBillJournal = 14;
        public const int WorklistInWorlistJournal = 15;
        public const int SampleInWorklist = 16;
        public const int WorklistForSample = 17;
        public const int MaterialInStrorageJournal = 18;
        public const int TestseriesInQCJournal = 19;
        public const int ErrorInQCErrorJournal = 20;
        public const int RemovedPointsInQCJournal = 21;
        public const int EventInQCEventJournal = 22;
        public const int SampleInDefectList = 23;
        public const int SampleInDefectJournal = 24;
        public const int SamplesForForklist = 25;
        public const int ResponsepackageInResponsepackageJournal = 26;
        public const int ResponseInResponsepackage = 27;
        public const int CreatedWorklist = 28;
        public const int ExternalMessageInWorklist = 29;
        public const int SampleInAssignDepartmentNr = 30;
        //   public const int ContainerInContainerJournal = 31;
        public const int ConvergenceSeriesInQCJournal = 32;
        public const int DailyAverageSeriesInQCJournal = 33;
        public const int DuplicatesSeriesInQCJournal = 34;
        public const int PatientCardInCardlist = 35;
        public const int OperationArrivalTemplateInOATJournal = 36;
        public const int SixSigmaSeriesInQCJournal = 37;
        public const int ReagentInReagentJournal = 38;
        public const int TubeInTubeJournal = 39;
        public const int RequestTemplateInRequestTemplateJournal = 40;
        public const int SampleTemplateInRequestTemplateJournal = 41;
        public const int OutsourceRequestInOutsourceJournal = 42;
        public const int SampleInOutsourceRequestForm = 43;
        public const int WorkInOutsourceRequestForm = 44;
        public const int TeamInTeamJournal = 45;
        public const int WaybillInWaybillJournal = 46;
        // 47        
        public const int ArchiveRackInArchiveJournal = 48;
        public const int ArchiveRackSlotInArchiveJournal = 49;
        public const int ArchiveRackSlotInArchiveRackEditor = 50;
        // 51;
        // 52;
        // 53;
        // 54;
        public const int QuotaInQuotaJournal = 55;
    }

    public enum LimsSampleCardExceptions
    {
        Comment,
        Value,
        State,
        TestId
    }

    public enum LimsRequestJournalExceptions
    {
        State
    }

    public static class ProcessStateConst
    {
        public const int New = 1;
        public const int InProgress = 2;
        public const int Done = 3;
        public const int Cancelled = 4;


        public static string GetStateName(int State)
        {
            return Values[State].ToString();
        }

        private static Hashtable Values = new Hashtable();

        static ProcessStateConst()
        {
            InitValues();
        }

        private static void InitValues()
        {
            Values.Add(New, "Начат");
            Values.Add(InProgress, "Начат");
            Values.Add(Done, "Завершен");
            Values.Add(Cancelled, "Отменен");
        }
    }



    public static class DonationPathologyConst
    {

        public static string displayPatology = "Патология";
        public static string displayNorm = "Норма";
        public static string displayDelay = "ЗАДЕРЖАН!";

        public static string GetPathologyName(bool hasPathology, int state)
        {
            if (hasPathology)
            {
                if (state == 4)
                {
                    return displayPatology;
                }
                if (state == 3)
                {
                    return displayDelay;
                }
            }
            else if (!hasPathology)
            {
                if (state == 4)
                {
                    return displayNorm;
                }
            }

            return string.Empty;
        }

        /*public static bool pathology = true;
        public static bool normal = false;
        

        public static string GetPathologyName(bool hasPathology)
        {
            return Values[hasPathology].ToString();
        }

        
        private static Hashtable Values = new Hashtable();

        static DonationPathologyConst()
        {
            InitValues();
        }

        private static void InitValues()
        {
            Values.Add(pathology, "Патология");
            Values.Add(normal, "Норма");
        }*/
    }



    public static class DonationStateConst
    {
        public static int New = 1;
        public static int InProgress = 2;
        public static int Done = 3;
        public static int Closed = 4;
        public static int Deny = 5;
        public static int Cancelled = 6;

        public static string GetStateName(int State)
        {
            return Values[State].ToString();
        }

        private static Hashtable Values = new Hashtable();

        static DonationStateConst()
        {
            InitValues();
        }

        private static void InitValues()
        {
            Values.Add(New, "Ожидание");
            Values.Add(InProgress, "Донация");
            Values.Add(Done, "Анализы");
            Values.Add(Closed, "Закрыта");
            Values.Add(Deny, "Отвод");
            Values.Add(Cancelled, "Отменена");
        }
    }

    public static class LimsFieldNamesConst
    {
        public const String UserValues = "UserValues";
    }

    public static class LimsUserFieldType
    {
        public const Int32 LIS_UERS_FIELD_TYPE_STRING = 1;
        public const Int32 LIS_UERS_FIELD_TYPE_NUMERIC = 2;
        public const Int32 LIS_UERS_FIELD_TYPE_BOOLEAN = 3;
        public const Int32 LIS_UERS_FIELD_TYPE_DATETIME = 4;
        public const Int32 LIS_UERS_FIELD_TYPE_ENUMERATION = 5;
        public const Int32 LIS_UERS_FIELD_TYPE_SET = 6;
        public const Int32 LIS_UERS_FIELD_TYPE_TEST = 7;
        public const Int32 LIS_UERS_FIELD_TYPE_SEX = 8;
        public const Int32 LIS_UERS_FIELD_TYPE_EXPRESSION = 9;
        public const Int32 LIS_UERS_FIELD_TYPE_PICTURE = 10;
    }

    public static class LimsCommentSourceType
    {
        public const Int32 INFO = 1;
        public const Int32 MANUAL = 2;
    }

    public struct LimsServerReportsElementsMapping
    {
        public const string RegistrationJournal = "TfrRegistrationJournal";
        public const string RequestEditor = "TfRequestEditor";
        public const string PatientJournal = "TfrPatientJournal";
        public const string PatientEditor = "TfPatientEditor";
        public const string WorkJournal = "TfrWorkJournal";
        public const string SampleEditor = "TfSampleEditor";
        public const string WaybillJournal = "TfrWaybillJournal";
        public const string WaybillEditor = "TfWaybillEditor";
    }

    public struct HemServerReportsElementsMapping
    {
        public const string RegistrationJournal = "TfrRegistrationJournal";
        public const string RequestEditor = "TfRequestEditor";
        public const string PatientJournal = "TfrPatientJournal";
        public const string PatientEditor = "TfPatientEditor";
        public const string DonorJournal = "TfrDonorJournal";
        public const string DonorEditor = "TfDonorEditor";
        public const string TransfusionJournal = "TfrTransfusionJournal";
        public const string TransfusionViewer = "TfTransfusionViewer";
        public const string TransfusionRequestJournal = "TfrTransfusionRequestJournal";
        public const string TransfusionRequestCard = "TfTransfusionRequestCard";
        public const string TreatmentRequestJournal = "TfrTreatmentRequestJournal";
        public const string TreatmentRequestCard = "TfTreatmentRequestCard";
    }

    public struct LimsServerReportsElementsHeaders
    {
        public const string RegistrationJournal = "Журнал заявок";
        public const string RequestEditor = "Карта заявки";
        public const string PatientJournal = "Журнал пациентов";
        public const string PatientEditor = "Карта пациента";
        public const string WorkJournal = "Рабочий журнал";
        public const string SampleEditor = "Карта пробы";
        public const string WaybillJournal = "Журнал инвентарных ведомостей";
        public const string WaybillEditor = "Карта инвентарной ведомости";
    }

    public struct HemServerReportsElementsHeaders
    {
        public const string RegistrationJournal = "Журнал заявок";
        public const string RequestEditor = "Карта заявки";
        public const string PatientJournal = "Журнал пациента";
        public const string PatientEditor = "Карта пациента";
        public const string DonorJournal = "Журнал пациента";
        public const string DonorEditor = "Карта пациента";
        public const string TransfusionJournal = "Журнал трансфузии";
        public const string TransfusionViewer = "Карта трансфузии";
        public const string TransfusionRequestJournal = "Журнал заявок на трансфузию";
        public const string TransfusionRequestCard = "Карта заявки на трансфузию";
        public const string TreatmentRequestJournal = "Журнал заявок на ЛП";
        public const string TreatmentRequestCard = "Карта заявки на ЛП";
    }

    public struct ProcedureParameterTypes
    {
        public const int PARAMETER_TYPE_LABORATORY = 4;
        public const int PARAMETER_TYPE_PHYSIO = 6;
        public const int PARAMETER_TYPE_TECHNICAL = 9;
        public const int PARAMETER_TYPE_LIQUID_BALANCE = 10;
        public const int PARAMETER_TYPE_TOTAL_VOLUME = 11;
    }

    public struct TreatmentRequestParameterTypes
    {
        public const int PARAMETER_TYPE_LABORATORY = 1;
        public const int PARAMETER_TYPE_PHYSIO = 2;
    }

    public struct ProductTypes
    {
        public const int PRODUCT_TYPE_INPUT = 1;
        public const int PRODUCT_TYPE_OUTPUT = 2;
    }

    public struct LogObjectTypes
    {
        public const int LOG_OBJECT_TYPE_REQUEST = 1;
        public const int LOG_OBJECT_TYPE_SAMPLE = 2;
        public const int LOG_OBJECT_TYPE_WORK = 3;
        public const int LOG_OBJECT_TYPE_OPERATION = 4;
        public const int LOG_OBJECT_TYPE_BILL = 5;
        public const int LOG_OBJECT_TYPE_WORKLIST = 6;
        public const int LOG_OBJECT_TYPE_RESPONSEPACKAGE = 7;
        public const int LOG_OBJECT_TYPE_QUOTA = 8;
    }

    public static class LimsSystemVariables
    {
        public const string CurEmployeeLastName = "%CurrentEmployeeLastName%";
        public const string CurEmployeeFirstNamee = "%CurrentEmployeeFirstName%";
        public const string CurEmployeeMiddleNamee = "%CurrentEmployeeMiddleName%";
        public const string CurEmployeeCode = "%CurrentEmployeeCode%";
        public const string CurEmployeeProfession = "%CurrentEmployeeProfession%";
        public const string CurOfficeName = "%CurrentOfficeName%";
        public const string CurOfficeId = "%CurrentOfficeId%";
        public const string CurOrganizationName = "%CurrentOrganizationName%";
        public const string CurOrganizationId = "%CurrentOrganizationId%";
    }

    /*
    // 63 // Внутри enum SampleStates явно не хватало значения для Частично доставленного v="7" "name" v="Частично доставлен" => PART_DELIVERED
    biomaterial-state
    SampleStates
        <f t = "I" n="id" v="0" n="name" v="Нет" "code" v="None"/>
        <f t = "I" n="id" v="1" "name" v="Регистрация" "code" v="Registration"/>
        <f t = "I" n="id" v="2" "name" v="Доставлен" "code" v="Delivered"/>
        <f t = "I" n="id" v="3" "name" v="В работе" "code" v="InPriogress"/>
        <f t = "I" n="id" v="4" "name" v="Закрыт" "code" v="Closed"/>
        <f t = "I" n="id" v="5" "name" v="Удален" "code" v="Deleted"/>
        <f t = "I" n="id" v="6" "name" v="Задержан" "code" v="Dalayed"/>
        <f t = "I" n="id" v="7" "name" v="Частично доставлен" "code" v="PartDelivered"/>
    */

    public enum SampleStates
    {
        NONE = 0,
        REGISTRATION = 1,
        DELIVERED = 2,
        IN_PROCESS = 3,
        CLOSED = 4,
        DELETED = 5,
        DELAYED = 6,
        PART_DELIVERED = 7,
    }

    public static class IncorrectConfiguration
    {
        public const string Error = "Внедренец, прочти это!!!:\r\n";
    }

    public static class DocumentSates
    {
        public const int LIS_REQUEST_DOCUMENT_MASK_NON = 0x00; // 0 (dec) = 0000 0000 (bin) // "" / "Не печат. или отправл."
        public const int LIS_REQUEST_DOCUMENT_MASK_PRINTED = 0x01; // 1 (dec) = 0000 0001 (bin) // "Распечатан"
        public const int LIS_REQUEST_DOCUMENT_MASK_REPRINTED = 0x02; // 2 (dec) = 0000 0010 (bin) // "Распеч. повт."
        public const int LIS_REQUEST_DOCUMENT_MASK_EMAIL_SENT = 0x04; // 4 (dec) = 0000 0100 (bin) // "Отправлен"
        public const int LIS_REQUEST_DOCUMENT_MASK_EMAIL_RESENT = 0x08; // 8 (dec) = 0000 1000 (bin) // "Отпр. повт."
        public const int LIS_REQUEST_DOCUMENT_MASK_EMAIL_NOT_SENT = 0x10; // 16 (dec) = 0001 0000 (bin) // "Не отправ."
        public const int LIS_REQUEST_DOCUMENT_MASK_SMS_SENT = 0x20; // 32 (dec) = 0010 0000 (bin) // "Отправлена СМС" 
        public const int LIS_REQUEST_DOCUMENT_MASK_SMS_ERROR = 0x40; // 64 (dec) = 0100 0000 (bin) // "Ошибка СМС"
        public const int LIS_REQUEST_DOCUMENT_MASK_EXTERNAL_SYSTEM_SENT = 0x10000; // 65536 (dec) = 0001 0000 0000 0000 0000 (bin) // "Отправлен в МИС"
    }

    public class PathHelper
    {
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}