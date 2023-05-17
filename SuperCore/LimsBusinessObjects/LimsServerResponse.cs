using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class LimsServerResponse : BaseObject
    {
        public LimsServerResponse()
        {
            Errors = new List<ErrorMessage>();
        }
        [CSN("Errors")]
        public List<ErrorMessage> Errors { get; set; }
    }

    public class CreateRequest3Response : LimsServerResponse
    {
        public CreateRequest3Response()
        {
            Ids = new List<Ids>();
        }
        [CSN("Id")]
        public int Id { get; set; }
        [CSN("Ids")]
        public List<Ids> Ids { get; set; }
    }

    public class Ids
    {
        [CSN("Id")]
        public int Id { get; set; }
        [CSN("InternalNr")]
        public string InternalNr { get; set; }
    }
}