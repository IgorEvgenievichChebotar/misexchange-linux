using System;

namespace ru.novolabs.SuperCore.Reporting
{
    public class ReportEvent
    {
        public ReportEvent(ErrorMessageType messageType, String message) : this (messageType, message, DateTime.Now) {}

        public ReportEvent(ErrorMessageType messageType, String message, DateTime date)
        {
            MessageType = messageType;
            Date = date;
            Message = message;
        }

        public DateTime Date { get; set; }
        public ErrorMessageType MessageType { get; private set; }
        public String Message { get; private set; }
    }
}
