using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.HemBusinessObjects
{

    public class BaseSaveResult : BaseObject
    {
        private List<ErrorMessage> errors = new List<ErrorMessage>();
        [CSN("Errors")]
        public List<ErrorMessage> Errors
        {
            get { return errors; }
            set { errors = value; }
        }
    }
}