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
    
    public partial class PatientCard
    {
        public PatientCard()
        {
            this.UserFields = new HashSet<UserField>();
        }
    
        public int Id { get; set; }
        public string CardNr { get; set; }
    
        public virtual ICollection<UserField> UserFields { get; set; }
    }
}