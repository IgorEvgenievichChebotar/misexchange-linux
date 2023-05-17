using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ru.novolabs.ExchangeDTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace ru.novolabs.Common
{
    public enum StatusObjectCache
    {
        New = 0,
        Error = 1,
        InProcess = 2,
        Completed = 3
    }
    public class ObjectStatus
    {
        public ObjectStatus()
        {
            CreateDate = DateTime.Now;
            ModifyingDate = DateTime.Now;
        }
        public long Id { get; set; }
        //public int StatusId { get; set; }
        public StatusObjectCache StatusId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifyingDate { get; set; }
        public string Comment { get; set; }
    }
    public class RequestObjectStatus : ObjectStatus
    {
        public RequestObjectStatus()
            : base()
        { }
        public Request Request { get; set; }    
    }
    public class ResultObjectStatus : ObjectStatus
    {
        public ResultObjectStatus()
            : base()
        { }
        public Result Result { get; set; }
    
    }
}
