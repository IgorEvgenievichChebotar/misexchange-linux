//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MisExchangeEntities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Patient
    {
        public Patient()
        {
            this.Sex = 0;
            this.UserFields = new HashSet<UserField>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int BirthDay { get; set; }
        public int BirthMonth { get; set; }
        public int BirthYear { get; set; }
        public int Sex { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Flat { get; set; }
        public string InsuranceCompany { get; set; }
        public string PolicySeries { get; set; }
        public string PolicyNumber { get; set; }
    
        public virtual PatientCard PatientCard { get; set; }
        public virtual ICollection<UserField> UserFields { get; set; }
    }
}
