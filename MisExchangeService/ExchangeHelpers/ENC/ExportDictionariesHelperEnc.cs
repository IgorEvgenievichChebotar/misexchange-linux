using ru.novolabs.ExchangeDTOs;
using ru.novolabs.MisExchange.Classes;
using ru.novolabs.MisExchange.ExchangeHelpers.Files;
using ru.novolabs.MisExchange.ExchangeHelpers.HL7;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.LimsBusinessObjects.Exchange;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ru.novolabs.MisExchange.ExchangeHelpers.ENC
{
    [ExportDictionariesHelperName("ExportDictionaries_ENC")]
    class ExportDictionariesHelperEnc : ExportDictionariesHelperAbstractFile.ExportDictionariesHelperAbstractFile
    {
        public ExportDictionariesHelperEnc()
            : base()
        {
        }


        protected override void ProcessDictionary(string dictionary, string exportFileName)
        {
            switch (dictionary)
            {
                case LimsDictionaryNames.Target:
                    ExportTargetDictonary(exportFileName);
                    break;
                default:
                    DictionariesExportHelper.ExportDictionaryAsNameCodeMnemonicsOld(dictionary, exportFileName);
                    break;            
            }
        }

        protected virtual void ExportTargetDictonary(string exportFileName)
        {
            var targets = (TargetDictionary)ProgramContext.Dictionaries.GetDictionary(LimsDictionaryNames.Target);

            XDocument document = new XDocument(
                new XElement("TargetDictionary",
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                        from target in targets.Elements
                        let service = target.Services.FirstOrDefault() ?? new ServiceDictionaryItem()
                        where (!target.Removed)
                        select
                            new XElement("Target",
                                new XElement("TestCodes",
                                    from test in target.Tests
                                    where (!test.Removed)
                                    select
                                      new XElement("Code", test.Code)),
                                new XElement("BiomaterialCodes",
                                    from biomaterial in target.Biomaterials
                                    where (!biomaterial.Removed)
                                    select
                                      new XElement("Code", biomaterial.Code)),
                                new XElement("TargetCodes",
                                    from target2 in target.Targets
                                    where (!target2.Removed)
                                    select
                                      new XElement("Code", target2.Code)),
                                new XElement("Name", target.Name),
                                new XElement("Code", target.Code),
                                new XElement("ServiceName", service.Name),
                                new XElement("TargetType", target.TargetType)
                                )));

            document.Save(exportFileName);
        }
    }
}
