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
    
    public partial class RequestFilterStatus
    {
        public int Id { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public byte Status { get; set; }
        public string Comments { get; set; }
    
        public virtual RequestFilter RequestFilter { get; set; }
    }
}
