using System;
using System.Collections.Generic;
using System.Xml;
using ru.novolabs.MisExchangeService.Classes;
using ru.novolabs.SuperCore;

namespace ru.novolabs.MisExchangeService.Processors
{
    public class CancelledWork
    {
        public CancelledWork()
        {
            Request = new ObjectRef();
        }

        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Request")]
        public ObjectRef Request { get; set; }
    }

    public class CancelledWorks
    {
        public CancelledWorks()
        {
            Works = new List<CancelledWork>();
        }
        [CSN("Works")]
        public List<CancelledWork> Works { get; set; }
    }


    [ProcessorName(LisProcessorsNames.WorkCancel)]
    public class WorksCancelledProcessor : Processor
    {
        public override void Execute(XmlNode content)
        {
            CancelledWorks contentObject = new CancelledWorks();

            ObjectReader reader = new ObjectReader();
            reader.ReadContent(content, contentObject);

            List<ObjectRef> ids = new List<ObjectRef>();
            foreach (CancelledWork work in contentObject.Works)
            {
                ids.Add(work.Request);
            }

            ResultSaver.StoreRequestsResults(ids);

            RequestDone = true;
        }
    }
}
