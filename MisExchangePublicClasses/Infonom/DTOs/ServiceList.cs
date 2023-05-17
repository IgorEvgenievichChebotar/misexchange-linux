using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchangeService.ExchangeHelpers.Infonom.DTOs
{
    [XmlType("ServiceList")]
    public class ServiceList : List<ServiceInfonom>
    {
    }
}
