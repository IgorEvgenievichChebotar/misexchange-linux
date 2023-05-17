using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.DictionaryCommon;
using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class AccessAddressClass
    {
        public string ip = string.Empty;
    }

    public class EmployeeDictionaryItem : DictionaryItem, IEmployee
    {
        public EmployeeDictionaryItem()
        {
            FirstName = String.Empty;
            MiddleName = String.Empty;
            LastName = String.Empty;
            Login = String.Empty;
            Password = String.Empty;
            Profession = String.Empty;
            CertificateSearchCriterion = String.Empty;
            CertificatePin = String.Empty;
            Snils = String.Empty;
            OrganizationPositionCode = String.Empty;
            SpecialtyCode = String.Empty;
            PositionCode = String.Empty;
            PositionTitle = String.Empty;
            EmployeeRole = String.Empty;
            UserGroups = new List<UserGroupDictionaryItem>();
            Departments = new List<DepartmentDictionaryItem>();
            Rights = new List<AccessRightDictionaryItem>();
            Hospitals = new List<HospitalDictionaryItem>();
            Offices = new List<OfficeDictionaryItem>();
            SendMessagesUserGroups = new List<UserGroupDictionaryItem>();
            Doctors = new List<DoctorDictionaryItem>();
            CustDepartments = new List<CustDepartmentDictionaryItem>();
        }

        public Boolean HasRight(Int32 right)
        {
            return Rights.Exists(r => r.Id1 == right) || UserGroups.Exists(g => g.Rights.Exists(r => r.Id1 == right));
        }

        [CSN("FirstName")]
        public String FirstName { get; set; }
        [CSN("MiddleName")]
        public String MiddleName { get; set; }
        [CSN("LastName")]
        public String LastName { get; set; }
        [CSN("Login")]
        public String Login { get; set; }
        [CSN("Password")]
        public String Password { get; set; }
        [CSN("Profession")]
        public String Profession { get; set; }
        [CSN("CertificateSearchCriterion")]
        public String CertificateSearchCriterion { get; set; }
        [CSN("CertificatePin")]
        public String CertificatePin { get; set; }
        [CSN("Snils")]
        public String Snils { get; set; }
        [CSN("OrganizationPositionCode")]
        public String OrganizationPositionCode { get; set; }
        [CSN("SpecialtyCode")]
        public String SpecialtyCode { get; set; }
        [CSN("PositionCode")]
        public String PositionCode { get; set; }
        [CSN("PositionTitle")]
        public String PositionTitle { get; set; }
        [CSN("EmployeeRole")]
        public String EmployeeRole { get; set; }
        [CSN("BirthDay")]
        public Int32 BirthDay { get; set; }
        [CSN("BirthMonth")]
        public Int32 BirthMonth { get; set; }
        [CSN("BirthYear")]
        public Int32 BirthYear { get; set; }

        [CSN("UserGroups")]
        public List<UserGroupDictionaryItem> UserGroups { get; set; }
        [CSN("Departments")]
        public List<DepartmentDictionaryItem> Departments { get; set; }
        [CSN("UserProfile")]
        public UserProfileDictionaryItem UserProfile { get; set; }
        [CSN("Rights")]
        public List<AccessRightDictionaryItem> Rights { get; set; }
        [CSN("Hospitals")]
        public List<HospitalDictionaryItem> Hospitals { get; set; }
        [CSN("CustDepartments")]
        public List<CustDepartmentDictionaryItem> CustDepartments { get; set; }
        [CSN("Doctors")]
        public List<DoctorDictionaryItem> Doctors { get; set; }

        [CSN("Office")]
        public OfficeDictionaryItem Office { get; set; }
        [CSN("Offices")]
        public List<OfficeDictionaryItem> Offices { get; set; }
        [CSN("Organization")]
        public OrganizationDictionaryItem Organization { get; set; }
        [CSN("SendMessagesUserGroups")]
        public List<UserGroupDictionaryItem> SendMessagesUserGroups { get; set; }

        [SendToServer(false)]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1} {2}", LastName, FirstName, MiddleName);
            }
        }
    }

    public class EmployeeDictionary : DictionaryClass<EmployeeDictionaryItem>
    {
        public EmployeeDictionary(String dictionaryName) : base(dictionaryName) { }

        [CSN("Employee")]
        public List<EmployeeDictionaryItem> Employee
        {
            get { return Elements; }
            set { Elements = value; }
        }

        protected override Int32 Compare(EmployeeDictionaryItem a, EmployeeDictionaryItem b)
        {
            return a.LastName.CompareTo(b.LastName);
        }
    }
}
