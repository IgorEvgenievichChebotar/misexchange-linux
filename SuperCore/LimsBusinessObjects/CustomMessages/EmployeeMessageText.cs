using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class EmployeeMessageText
    {
        public EmployeeMessageText() 
        {
            this.Attachments = new List<int>();
        }

        [CSN("Text")]
        public string Text { get; set; }

        [CSN("Sender")]
        public string Sender { get; set; }

        [CSN("Subject")]
        public string Subject { get; set; }

        [CSN("CreateDate")]
        public DateTime CreateDate { get; set; }

        [CSN("Attachments")]
        public List<int> Attachments { get; set; }
    }
}
