using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class BaseEmployeeMessage
    {
        public BaseEmployeeMessage() { }
        [CSN("Id")]
        public int Id { get; set; }

        [CSN("CreateDate")]
        public DateTime CreateDate { get; set; }

        [CSN("HasAttachments")]
        public bool HasAttachments { get; set; }

        [CSN("Subject")]
        public string Subject { get; set; }
    }
}
