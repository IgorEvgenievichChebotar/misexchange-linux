using System.Collections.Generic;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    [XmlType(TypeName = "LisObservationReport")]
    public class LisObservationReport : List<Observation> { }
}