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
    
    public partial class MicroResult
    {
        public MicroResult()
        {
            this.Antibiotics = new HashSet<Work>();
        }
    
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Comments { get; set; }
        public int SampleResultId { get; set; }
    
        public virtual SampleResult SampleResult { get; set; }
        public virtual ICollection<Work> Antibiotics { get; set; }
    }
}
