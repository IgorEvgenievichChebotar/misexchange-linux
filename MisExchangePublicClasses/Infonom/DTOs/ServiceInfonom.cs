using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    [XmlType("Service")]
    public class ServiceInfonom
    {
        public ServiceInfonom()
        {
            SpecimenTypes = new List<SpecimenType>();
            ServiceGroups = new List<ServiceGroup>();
        }

        public string ID { get; set; }
        public string ExternalID { get; set; }
        public string Title { get; set; }
        public string Method { get; set; }
        public List<SpecimenType> SpecimenTypes { get; set; }
        public List<ServiceGroup> ServiceGroups  { get; set; }
    }
}
