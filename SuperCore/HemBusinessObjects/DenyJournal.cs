using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.HemDictionary;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{
    public class DenyJournalItem : BaseObject
    { 
        // ФИО
        private string lastName = string.Empty;
        private string firstName = string.Empty;
        private string middleName = string.Empty;
        private Int32 birthDay = 0;
        private Int32 birthMonth = 0;
        private Int32 birthYear = 0;
        public string displayBirthday = string.Empty;
        private string documentType = string.Empty;
        private string documentNr = string.Empty;
        private string documentSeries = string.Empty;
        private int sex = 0;
        private Int32 findDay = 0;
        private Int32 findMonth = 0;
        private Int32 findYear = 0;
        public string displayDenyDate = string.Empty;
        private DenySourceDictionaryItem source = null;
        private string diagnosis = string.Empty;
        private DenyDictionaryItem deny = null;
        private AddressClass address = new AddressClass();
        public string displayAddress = string.Empty;

        public string displayDenyName = string.Empty;
        public string displayDenySource = string.Empty;

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
        public Int32 BirthDay
        {
            get { return birthDay; }
            set { birthDay = value; }
        }

        [CSN("BirthMonth")]
        public Int32 BirthMonth
        {
            get { return birthMonth; }
            set { birthMonth = value; }
        }

        [CSN("BirthYear")]
        public Int32 BirthYear
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
        public AddressClass Address
        {
            get { return address; }
            set { address = value; }
        }

        [CSN("LivingAddress")]
        public AddressClass LivingAddress
        {
            get { return address; }
            set { address = value; }
        }

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
        public DenySourceDictionaryItem Source
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

        [CSN("Deny")]
        public DenyDictionaryItem Deny
        {
            get { return deny; }
            set { deny = value; }
        }

        [CSN("FindDay")]
        public Int32 FindDay
        {
            get { return findDay; }
            set { findDay = value; }
        }

        [CSN("FindMonth")]
        public Int32 FindMonth
        {
            get { return findMonth; }
            set { findMonth = value; }
        }

        [CSN("FindYear")]
        public Int32 FindYear
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
            get { return Address.DisplayName; }
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
            firstName = FormatName(firstName);
            lastName = FormatName(lastName);
            middleName = FormatName(middleName);
            displayBirthday = GetDisplayDate(birthDay, birthMonth, birthYear);
            displayDenyDate = GetDisplayDate(findDay, findMonth, findYear);
            //displayDenyName = GetDenyName();
            //displayDenySource = GetDenySorce();

            if (displayBirthday.Length > 10)
            { displayBirthday = displayBirthday.Substring(0, 10); }

            if (displayDenyDate.Length > 10)
            { displayDenyDate = displayDenyDate.Substring(0, 10); }

            //FullName = lastName + " " + firstName + " " + middleName;
            displayAddress = address.DisplayName;
        }

        private string GetDisplayDate(Int32 day, Int32 month, Int32 year)
        {
            string dd = string.Empty;
            string mm = string.Empty;
            string yyyy = string.Empty;


            if (day != 0)
             { 
                dd = string.Format("{0:D2}", day); 
            }
            else
            { dd = "  "; }

            if (month != 0)
            {
                mm = string.Format("{0:D2}", month);
            }
            else
            { 
                mm = "  "; 
            }

            if (year != 0)
            {
                 yyyy = string.Format("{0:D4}", year);
            }
            else 
            { 
                yyyy = "    "; 
            }

            return dd + "." + mm + "." + yyyy;
        }

        public string FormatName(string StrIn)
        {
            if (StrIn.Length == 0) return string.Empty;
            return StrIn.Substring(0, 1).ToUpper() + StrIn.Substring(1).ToLower();
        }
    }

    public class DenyJournalItemSet
    {
        private List<DenyJournalItem> result = new List<DenyJournalItem>();

        [CSN("Result")]
        public List<DenyJournalItem> Result
        {
            get { return result; }
            set { result = value; }
        }
        public void Prepare()
        {
            foreach (DenyJournalItem Deny in result)
            {
                Deny.Prepare();
            }
        }
    }

    public class FindDenieslRequest
    {
        private string lastName = string.Empty;
        private string firstName = string.Empty;
        private string middleName = string.Empty;
        private string birthYear = string.Empty;
        private DateTime dateFrom = DateTime.MinValue;
        private DateTime dateTill = DateTime.MinValue;
        private bool strictLastName = false;
        private int removed = 0;

        [SendEmpty()]
        [CSN("Removed")]
        public int Removed
        {
            get { return removed; }
            set { removed = value; }
        }

        [CSN("StrictLastName")]
        public bool StrictLastName
        {
            get { return CheckLastName(); }
            set { strictLastName = value; }
        }

        [CSN("DateFrom")]
        public DateTime DateFrom
        {
            get { return dateFrom; }
            set { dateFrom = value; }
        }

        [CSN("DateTill")]
        public DateTime DateTill
        {
            get { return dateTill; }
            set { dateTill = value; }
        }

        [CSN("LastName")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = Check(value); }
        }

        [CSN("FirstName")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        [CSN("MiddleName")]
        public string MiddleName
        {
            get { return middleName; }
            set { middleName = value; }
        }

        [CSN("BirthYear")]
        public string BirthYear
        {
            get { return birthYear; }
            set { birthYear = value; }
        }

        public bool CheckLastName()
        {
            if (LastName.Length < 3)
            {
                return true;
            }

            return false;
        }

        // Если поиск осуществляется по номеру, значит ищем только донора
        public string Check(string value)
        {
            if (value.Length == 0) return "/*/*";
            return value;
        }
    }

    public class GetDeniesInfoRequest
    {

        public GetDeniesInfoRequest(ObjectRef Deny)
        {
            ids.Add(Deny);
        }

        private List<ObjectRef> ids = new List<ObjectRef>();

        [CSN("Ids")]
        public List<ObjectRef> Ids
        {
            get { return ids; }
            set { ids = value; }
        }
    }

    public class FindDenyContactsRequest : FindDenieslRequest
    {
        private List<ObjectRef> denySources = new List<ObjectRef>();
        //  Адрес донора
        private string city = string.Empty;
        private string district = string.Empty;
        private string street = string.Empty;
        private string building = string.Empty;


        [SendToServer(false)]
        [CSN("Removed")]
        new public int Removed
        {
            get { return 0; }
            set { }
        }

        [SendToServer(false)]
        [CSN("StrictLastName")]
        new public bool StrictLastName
        {
            get { return false; }
            set { }
        }

        [CSN("DenySources")]
        public List<ObjectRef> DenySources
        {
            get { return denySources; }
            set { denySources = value; }
        }

        [CSN("City")]
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        [CSN("District")]
        public string District
        {
            get { return district; }
            set { district = value; }
        }

        [CSN("Street")]
        public string Street
        {
            get { return street; }
            set { street = value; }
        }

        [CSN("Building")]
        public string Building
        {
            get { return building; }
            set { building = value; }
        }
    }

    public class DenyJournalRequest
    {
        // ФИО
        public string lastName;
        public string firstName;
        public string middleName;

        public string birthYear;

        public List<ObjectRef> denySources = new List<ObjectRef>();


        //  Адрес донора
        public string city;
        public string district;
        public string street;
        public string building;



        public DenyJournalRequest()
        {
        }



        public bool BuildByCode(string Code)
        {
            int YY;
            int CurrentYear;
            if ((Code.Length != 7) && (Code.Length != 5))
            {
                return false;
            }

            this.lastName = Code.Substring(0, 3);
            this.firstName = Code.Substring(3, 1);
            this.middleName = Code.Substring(4, 1);

            if (Code.Length == 7)
            {
                try
                {
                    YY = System.Convert.ToInt32(Code.Substring(5, 2));

                    CurrentYear = System.DateTime.Now.Year;

                    // WARNING: Это будет работать только до 2100 года.
                    // Пытаемся сделать год рождения 20YY. Если получившийся год рождения больше текущего, то выставляем год 19YY.
                    // Пытаемся сделать год 19YY. Если получившийся возраст больше 80, то выставляем 20YY.
                    if (((2000 + YY) > CurrentYear) || ((CurrentYear - (YY + 1900)) < 80))
                    {
                        YY += 1900;
                    }
                    else
                    {
                        YY += 2000;
                    }
                    this.birthYear = YY.ToString();
                }
                catch
                { }

            }
            return true;
        }



        public bool IsCorrect()
        {
            return (street.Length > 0) || (lastName.Length > 0);
        }

        public bool IsEmpty()
        {
            return (lastName.Length == 0) &&
                    (firstName.Length == 0) &&
                    (middleName.Length == 0) &&
                    (birthYear.Length == 0) &&
                    (city.Length == 0) &&
                    (district.Length == 0) &&
                    (street.Length == 0) &&
                    (building.Length == 0);
        }

    }


}
