using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class Recipient: Donor
    {
        public Recipient()
        {
            LivingAddress = new AddressClass();
            DocumentAddress = new AddressClass();
            DocumentType = new DocumentTypeDictionaryItem();
            BloodParameters = new List<BloodParameterValue>();
            Department = new DepartmentDictionaryItem();
            Diagnoses = new List<RecipientDiagnosis>();
        }

        [CSN("Date")]
        public new DateTime Date { get; set; }
        [CSN("FirstName")]
        public new String FirstName { get; set; }
        [CSN("LastName")]
        public new String LastName { get; set; }
        [CSN("MiddleName")]
        public new String MiddleName { get; set; }

        [SendToServer(false)]
        [CSN("FullName")]
        public new String FullName { get { return getFullName(); } }
        [SendToServer(false)]
        [CSN("ShortName")]
        public String ShortName { get { return getShortName(); } }

        [CSN("Sex")]
        public new Int32 Sex { get; set; }
        [CSN("LivingAddress")]
        public new AddressClass LivingAddress { get; set; }
        [CSN("DocumentAddress")]
        public new AddressClass DocumentAddress { get; set; }
        [CSN("DocumentType")]
        public new DocumentTypeDictionaryItem DocumentType { get; set; }
        [CSN("DocumentNr")]
        public new String DocumentNr { get; set; }
        [CSN("DocumentSeries")]
        public new String DocumentSeries { get; set; }
        [CSN("BirthDay")]
        public Int32? BirthDay { get; set; }
        [CSN("BirthMonth")]
        public Int32? BirthMonth { get; set; }
        [CSN("BirthYear")]
        public Int32? BirthYear { get; set; }
        [CSN("BloodParameters")]
        public new List<BloodParameterValue> BloodParameters { get; set; }

        public ObjectRef hospital = new ObjectRef();
        public String hospitalName = string.Empty;
        [CSN("Department")]
        public new DepartmentDictionaryItem Department { get; set; }
        [CSN("Comment")]
        public String Comment { get; set; }
        [CSN("Diagnoses")]
        public List<RecipientDiagnosis> Diagnoses { get; set; }



        private string getFullName()
        {
            String result = String.Empty;

            if (!String.IsNullOrEmpty(LastName))
                result = LastName;

            if (!String.IsNullOrEmpty(FirstName))
                result += FirstName + " ";

            if (!String.IsNullOrEmpty(MiddleName))
                result += MiddleName + " ";

            return result;
        }

        private string getShortName()
        {
            String result = String.Empty;

            if (!String.IsNullOrEmpty(LastName))
                result = LastName;

            if (!String.IsNullOrEmpty(FirstName))
                result += FirstName.Substring(0, 1) + ". ";

            if (!String.IsNullOrEmpty(MiddleName))
                result += MiddleName.Substring(0, 1) + ". ";

            return result;
        }


        public void Prepare()
        {
            //
        }
    }

    public class RecipientDiagnosis : BaseObject
    {
        [CSN("Date")]
        public DateTime Date { get; set; }
        [CSN("Diagnosis")]
        public String Diagnosis { get; set; }
        [CSN("RemoveDate")]
        public DateTime RemoveDate { get; set; }
        [CSN("RemoveReason")]
        public String RemoveReason { get; set; }
        [CSN("Comment")]
        public String Comment { get; set; }
    }


    public class FindRecipientRequest
    {
        public FindRecipientRequest()
        {
            Address = new AddressClass();
            DocumentTypes = new List<DocumentTypeDictionaryItem>();
            Departments = new List<DepartmentDictionaryItem>();
        }

        private Int32 birthDay;
        private Int32 birthYear;
        private Int32 birthMonth;

        [CSN("FirstName")]
        public String FirstName { get; set; }
        [CSN("LastName")]
        public String LastName { get; set; }
        [CSN("MiddleName")]
        public String MiddleName { get; set; }
        [CSN("Sex")]
        public Int32 Sex { get; set; }
        [CSN("Address")]
        public AddressClass Address { get; set; }

        [CSN("BirthDay")]
        public Int32? BirthDay { get { return birthDay == 0 ? null : (Int32?)birthDay; } set { birthDay = value == null ? 0 : value.Value; } }
        [CSN("BirthYear")]
        public Int32? BirthYear { get { return birthYear == 0 ? null : (Int32?)birthYear; } set { birthYear = value == null ? 0 : value.Value; } }
        [CSN("BirthMonth")]
        public Int32? BirthMonth { get { return birthMonth == 0 ? null : (Int32?)birthMonth; } set { birthMonth = value == null ? 0 : value.Value; } }

        [CSN("DocumentTypes")]
        public List<DocumentTypeDictionaryItem> DocumentTypes { get; set; }
        [CSN("DocumentNr")]
        public String DocumentNr { get; set; }
        [CSN("DocumentSeries")]
        public String DocumentSeries { get; set; }
        [CSN("Departments")]
        public List<DepartmentDictionaryItem> Departments { get; set; }

        public List<ObjectRef> hospitals = new List<ObjectRef>(); // ЛПУ, создавшие реципиента
        public string hospitalName = string.Empty;  // название ЛПУ, создавшее запись о пациенте, если оно не найдено в справочнике ЛПУ
    }

    public class FindRecipientResponse
    {
        public List<Recipient> result = new List<Recipient>();
    }


    public class GetRecipientRequest
    {
        public GetRecipientRequest()
        {
        }

        public GetRecipientRequest(int id)
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


    public class RecipientSet
    {
        private List<Recipient> result = new List<Recipient>();

        [CSN("Result")]
        public List<Recipient> Result
        {
            get { return result; }
            set { result = value; }
        }


        public void Prepare()
        {
            foreach (Recipient recipient in result)
            {
                recipient.Prepare();
            }
        }
    }

}
