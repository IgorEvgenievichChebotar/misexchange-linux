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
    
    public partial class Norm
    {
        public Norm()
        {
            this.Norms = "\"\"";
        }
    
        public int Id { get; set; }
        public Nullable<double> CriticalLowLimit { get; set; }
        public Nullable<double> LowLimit { get; set; }
        public Nullable<double> CriticalHighLimit { get; set; }
        public Nullable<double> HighLimit { get; set; }
        public string Norms { get; set; }
        public string NormComment { get; set; }
        public string UnitName { get; set; }
    
        public virtual Work Work { get; set; }
    }
}
