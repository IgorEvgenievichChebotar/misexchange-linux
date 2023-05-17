using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class WebDenyJournalItem : BaseObject
    {
        // ФИО
        private string lastName = string.Empty;
        private string firstName = string.Empty;
        private string middleName = string.Empty;
        private string birthDay = string.Empty;
        private string birthMonth = string.Empty;
        private string birthYear = string.Empty;
        private string documentType = string.Empty;
        private string documentNr = string.Empty;
        private string documentSeries = string.Empty;
        private int sex = 0;
        private string findDay = string.Empty;
        private string findMonth = string.Empty;
        private string findYear = string.Empty;
        private ObjectRef source = new ObjectRef();
        private string diagnosis = string.Empty;
        private ObjectRef deny = new ObjectRef();
        private string address = string.Empty;


        [CSN("LastName")]
        public string LastName
        {
            get { return FormatName(lastName); }
            set { lastName = value; }
        }
        [CSN("FirstName")]
        public string FirstName
        {
            get { return FormatName(firstName); }
            set { firstName = value; }
        }
        [CSN("MiddleName")]
        public string MiddleName
        {
            get { return FormatName(middleName); }
            set { middleName = value; }
        }
        [CSN("FullName")]
        public string FullName
        {
            get { return LastName + " " + FirstName + " " + MiddleName; }
        }



        //  Дата рождения
        [CSN("BirthDay")]
        public string BirthDay
        {
            get { return birthDay; }
            set { birthDay = value; }
        }
        [CSN("BirthMonth")]
        public string BirthMonth
        {
            get { return birthMonth; }
            set { birthMonth = value; }
        }
        [CSN("BirthYear")]
        public string BirthYear
        {
            get { return birthYear; }
            set { birthYear = value; }
        }
        [CSN("BirthDate")]
        public string BirthDate
        {
            get { return GetDisplayDate(birthDay, birthMonth, birthYear); }
        }

        [SendToServer(false)]
        [CSN("DisplayBirthDate")]
        public string DisplayBirthDate
        {
            get { return BirthDate; }
        }


        // Пол
        [LinkedDictionary("sex", "Name")]
        [CSN("Sex")]
        public int Sex
        {
            get { return sex; }
            set { sex = value; }
        }


        //Адрес   
        [CSN("Address")]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        [CSN("LivingAddress")]
        public string LivingAddress
        {
            get { return address; }
            set { address = value; }
        }

        // Документ
        [CSN("DocumentType")]
        public string DocumentType
        {
            get { return documentType; }
            set { documentType = value; }
        }
        [CSN("DocumentNr")]
        public string DocumentNr
        {
            get { return documentNr; }
            set { documentNr = value; }
        }
        [CSN("DocumentSeries")]
        public string DocumentSeries
        {
            get { return documentSeries; }
            set { documentSeries = value; }
        }
        [CSN("Document")]
        public string Document
        {
            get { return documentSeries + " " + documentNr; }
        }


        // Отвод 
        [CSN("Source")]
        public ObjectRef Source
        {
            get { return source; }
            set { source = value; }
        }
        [CSN("Diagnosis")]
        public string Diagnosis
        {
            get { return diagnosis; }
            set { diagnosis = value; }
        }

        [LinkedDictionary("deny", "name")]
        [CSN("Deny")]
        public ObjectRef Deny
        {
            get { return deny; }
            set { deny = value; }
        }
        [CSN("FindDay")]
        public string FindDay
        {
            get { return findDay; }
            set { findDay = value; }
        }
        [CSN("FindMonth")]
        public string FindMonth
        {
            get { return findMonth; }
            set { findMonth = value; }
        }
        [CSN("FindYear")]
        public string FindYear
        {
            get { return findYear; }
            set { findYear = value; }
        }
        [CSN("FindDate")]
        public string FindDate
        {
            get { return GetDisplayDate(findDay, findMonth, findYear); }
        }

        [SendToServer(false)]
        [CSN("DisplayBloodGroup")]
        public string DisplayBloodGroup
        {
            get { return string.Empty; }
        }

        [SendToServer(false)]
        [CSN("DisplayBloodResus")]
        public string DisplayBloodResus
        {
            get { return string.Empty; }
        }

        [SendToServer(false)]
        [CSN("DisplayAddress")]
        public string DisplayAddress
        {
            get { return address; }
        }


        [SendToServer(false)]
        [CSN("DisplayDocument")]
        public string DisplayDocument
        {
            get { return documentNr + " " + documentSeries; }
        }

        // Тип записи
        [SendToServer(false)]
        [LinkedDictionary("edcRecordType", "Name")]
        [CSN("EdcRecordType")]
        public int EdcRecordType
        {
            get { return EdcReordTypeConst.Deny; }
        }

        public void Prepare()
        {
        }

        private string GetDisplayDate(string day, string month, string year)
        {
            string dd = string.Empty;
            string mm = string.Empty;
            string yyyy = string.Empty;
            int i = 0;

            if (int.TryParse(day, out i))
            {
                dd = string.Format("{0:D2}", i);
            }
            else
            { dd = day.Trim(); }

            if (int.TryParse(month, out i))
            {
                mm = string.Format("{0:D2}", i);
            }
            else
            {
                mm = birthMonth;
            }

            if (int.TryParse(year, out i))
            {
                yyyy = string.Format("{0:D4}", i);
            }
            else
            {
                yyyy = year.Trim();
            }

            return dd + "." + mm + "." + yyyy;
        }

        public string FormatName(string StrIn)
        {
            if (StrIn.Length == 0) return string.Empty;
            return StrIn.Substring(0, 1).ToUpper() + StrIn.Substring(1).ToLower();
        }

    }



    public class WebDenyJournalItemSet
    {
        private List<WebDenyJournalItem> result = new List<WebDenyJournalItem>();
        [CSN("Result")]
        public List<WebDenyJournalItem> Result
        {
            get { return result; }
            set { result = value; }
        }
        public void Prepare()
        {
            foreach (WebDenyJournalItem Deny in result)
            {
                Deny.Prepare();
            }
        }
    }
}
