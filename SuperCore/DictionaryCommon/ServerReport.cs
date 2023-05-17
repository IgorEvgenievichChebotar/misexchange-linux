using ru.novolabs.SuperCore.DictionaryCore;
using System;
using System.Collections.Generic;

namespace ru.novolabs.SuperCore.DictionaryCommon
{
    public class ParametersMapping
    {
        [CSN("PropertyName")]
        public String PropertyName { get; set; }
        [CSN("ParameterName")]
        public String ParameterName { get; set; }
    }

    public class ServerReportDictionaryItem : DictionaryItem
    {
        public ServerReportDictionaryItem()
        {
            UserGroups = new List<ObjectRef>();
            GuiElementReportExecutors = new List<GuiElementReportExecutorsDictionaryItem>();
            ParametersMapping = new List<ParametersMapping>();
        }
        [CSN("ReportName")]
        public String ReportName { get; set; }
        [CSN("UserGroups")]
        public List<ObjectRef> UserGroups { get; set; }
        [CSN("UseDefaultValues")]
        public Boolean UseDefaultValues { get; set; }
        [CSN("NotAssociatedWithObjects")]
        public Boolean NotAssociatedWithObjects { get; set; }

        [CSN("GuiElementReportExecutors")]
        public List<GuiElementReportExecutorsDictionaryItem> GuiElementReportExecutors { get; set; }
        [CSN("ParametersMapping")]
        public List<ParametersMapping> ParametersMapping { get; set; }
    }

    public class ServerReportDictionary : DictionaryClass<ServerReportDictionaryItem>
    {
        public ServerReportDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("ServerReport")]
        public List<ServerReportDictionaryItem> ServerReport
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}