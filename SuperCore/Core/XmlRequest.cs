using System.Xml.Serialization;

namespace ru.novolabs.SuperCore.Reporting
{
    [XmlType("ServerRequest")]
    public class XmlRequest<T>
        where T : class
    {
        public string RequestType { get; set; }
        public bool Async { get; set; }
        public string CallbackAddress { get; set; }
        public T Content { get; set; }
    }
}