using ru.novolabs.MisExchange.Classes;
using ru.novolabs.SuperCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ru.novolabs.MisExchange.Processors
{
    public class Patient4Synchronize
    {
        public Patient4Synchronize ()
        {

        }

        [CSN("PatientCode")]
        public String PatientCode { get; set; }
        [CSN("PatientId")]
        public Int32 PatientId { get; set; }
    }



    [ProcessorName(LisProcessorsNames.PatientSyncrhonize)]
    public class PatientSyncrhonizeProcessor : Processor
    {
        public override void Execute(XmlNode content)
        {
            Patient4Synchronize contentObject = new Patient4Synchronize();

            ObjectReader reader = new ObjectReader();
            reader.ReadContent(content, contentObject);
            PatientSynchronizer.Synchronize(contentObject);

            RequestDone = true;
        }

    }
}
