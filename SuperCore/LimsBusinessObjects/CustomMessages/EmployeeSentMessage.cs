using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.SuperCore.LimsBusinessObjects
{
    public class EmployeeSentMessage : BaseEmployeeMessage
    {
        public EmployeeSentMessage() 
        {
            Recipients = new List<int>();
            RecipientsNames = new List<string>();
        }

        [CSN("Recipients")]
        public List<int> Recipients { get; set; }

        private List<string> m_RecipientsNames;

        [CSN("RecipientsNames")]
        public List<string> RecipientsNames
        {
            get 
            {
                m_RecipientsNames = new List<string>();
                foreach(int i in Recipients)
                {
                    var empl = (EmployeeDictionaryItem)ProgramContext.Dictionaries.GetDictionaryItem(LimsDictionaryNames.Employee, i);
                    m_RecipientsNames.Add(empl.Name);
                }
                return m_RecipientsNames;
            }
            set
            {
                m_RecipientsNames = value;
            }
        }
    }

    public class GetEmployeeSentMessagesResponse
    {
        public GetEmployeeSentMessagesResponse()
        {
            Messages = new List<EmployeeSentMessage>();
        }
        [CSN("Messages")]
        public List<EmployeeSentMessage> Messages { get; set; }
    }
}
