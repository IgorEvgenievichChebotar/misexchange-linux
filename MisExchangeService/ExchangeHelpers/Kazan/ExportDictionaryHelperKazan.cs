using ru.novolabs.MisExchange.Classes;
using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Kazan
{
    [ExportDictionariesHelperName("ExportDictionaries_KAZAN")]
    class ExportDictionariesHelperKAZAN : ExportDictionariesHelperAbstractFile.ExportDictionariesHelperAbstractFile
    {
        public ExportDictionariesHelperKAZAN()
            : base()
        {
        }
        protected override void ProcessDictionary(string dictionary, string exportFileName)
        {
            switch (dictionary)
            {
                case LimsDictionaryNames.Test:
                    ExportTestDictionary(exportFileName);
                    break;
                case LimsDictionaryNames.PatientGroup:
                    ExportPatientGroupDictionary(exportFileName);
                    break;
                case LimsDictionaryNames.Target:
                    ExportTargetDictionary(exportFileName);
                    break;
                case LimsDictionaryNames.RequestForm:
                    ExportRequestForm(exportFileName);
                    break;
                case LimsDictionaryNames.ServiceShort:
                    ExportServiceDictionaryV1(exportFileName);
                    ExportServiceShort(dictionary, exportFileName);
                    break;
                default:
                    ExportDictionaryInBaseFormat(dictionary, exportFileName);
                    break;
            }
        }

        private void UpdateDictionaryInfos(TargetDictionary targets, ServiceDictionary services)
        {
            foreach (TargetDictionaryItem target in targets.Elements)
                if (target.Services.Count == 0)
                {
                    foreach (ServiceDictionaryItem service in services.Elements)
                        foreach (TargetDictionaryItem serviceTarget in service.Targets)
                            if (serviceTarget.Id == target.Id)
                            {
                                target.Services.Add(service);
                                break;
                            }
                }
        }

        private void ExportTargetDictionary(string exportFileName)
        {
            var targets = (TargetDictionary)ProgramContext.Dictionaries.GetDictionary(LimsDictionaryNames.Target);
            var services = (ServiceDictionary)ProgramContext.Dictionaries.GetDictionary(LimsDictionaryNames.ServiceShort);

            UpdateDictionaryInfos(targets, services);

            XDocument document = new XDocument(
                new XElement("Dictionary",
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XElement("Code", "target"),
                    new XElement("Items",
                        from target in targets.Elements
                        let service = target.Services.FirstOrDefault() ?? new ServiceDictionaryItem()

                        where (!target.Removed)
                        select
                            new XElement("Item",
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
                                new XElement("Mnemonics", target.Mnemonics),
                                new XElement("ServiceName", service.Name),
                                new XElement("ServiceCode", service.Code),
                                new XElement("TargetType", target.TargetType)
                                ))));

            document = AdditionalProcessingXDocument(document);
            document.Save(exportFileName);
        }

        private void ExportTestDictionary(String exportFileName)
        {
            var testDictionary = (TestDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.Test];

            XDocument document = new XDocument(
                new XElement("TestDictionary",
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XElement("Name", "test"),
                    new XElement("Version", testDictionary.Version),
                    new XElement("Elements",
                        from element in testDictionary.Elements
                        where (!element.Removed)
                        select
                            GetTestElemet(element)),
                    new XElement("Test",
                        from element in testDictionary.Elements
                        where (!element.Removed)
                        select
                            GetTestElemet(element)
                                )));

            document.Save(exportFileName);
        }

        private XElement GetTestElemet(TestDictionaryItem test)
        {
            XElement element = new XElement("TestDictionaryItem",
                new XElement("Id", test.Id),
                new XElement("ExternalCode", test.ExternalCode),
                new XElement("Removed", test.Removed),
                new XElement("Name", test.Name),
                new XElement("Code", test.Code),
                new XElement("Mnemonics", test.Mnemonics),
                new XElement("Description", test.Description),
                new XElement("Format", test.Format),
                new XElement("Rank", test.Rank),
                new XElement("ResultType", test.ResultType),
                new XElement("IsSystem", test.IsSystem),
                new XElement("Unit",
                    new XElement("Id", test.Unit.Id),
                    new XElement("ExternalCode", test.Unit.ExternalCode),
                    new XElement("Removed", test.Unit.Removed),
                    new XElement("Name", test.Unit.Name),
                    new XElement("Code", test.Unit.Code),
                    new XElement("Mnemonics", test.Unit.Mnemonics),
                    new XElement("Description", test.Unit.Description)
                    ),
                new XElement("NumericRanges",
                        from numericRange in test.NumericRanges
                        select
                            new XElement("NumericRange",
                                //new XElement("PatientGroup_Name", numericRange.PatientGroup != null ? numericRange.PatientGroup.Name : null),
                                new XElement("PatientGroup_Id", numericRange.PatientGroup != null ? numericRange.PatientGroup.Id : 0),
                                new XElement("PatientGroup_Code", numericRange.PatientGroup != null ? numericRange.PatientGroup.Code : null),
                                new XElement("PatientGroup_Name", numericRange.PatientGroup != null ? numericRange.PatientGroup.Name : "по умолчанию"),
                                new XElement("Point1", numericRange.Point1),
                                new XElement("Point2", numericRange.Point2),
                                new XElement("Point3", numericRange.Point3),
                                new XElement("Point4", numericRange.Point4),
                                new XElement("Name1", numericRange.Name1),
                                new XElement("Name2", numericRange.Name2),
                                new XElement("Name3", numericRange.Name3),
                                new XElement("Name4", numericRange.Name4),
                                new XElement("Name5", numericRange.Name5),
                                new XElement("EngNormName", numericRange.EngNormName))),
                new XElement("Values",
                        from testValue in test.Values
                        select
                            new XElement("TestValue",
                                new XElement("Id", testValue.Id),
                                new XElement("Value", testValue.Value),
                                new XElement("DefaultGroup", testValue.DefaultGroup),
                                new XElement("EngValue", testValue.EngValue),
                                new XElement("Code", testValue.Code),
                                new XElement("DefaultValue", testValue.DefaultValue),
                                new XElement("Rank", testValue.Rank),
                                new XElement("SearchKeys", testValue.SearchKeys)))
                                );

            return AdditionalProcessingxElement(element);
        }

        private void ExportPatientGroupDictionary(String exportFileName)
        {
            var patientGroups = (PatientGroupDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.PatientGroup];
            IEnumerable<XElement> elements = BuildPatientGroupDictionary(patientGroups); ;
            GenerateDocumentByTemplate(patientGroups, elements, "PatientGroupDictionary", "PatientGroup", exportFileName);
        }

        private void GenerateDocumentByTemplate<T>(DictionaryClass<T> dictionary, IEnumerable<XElement> elements, string rootElementName, string collectionElementName, string exportFileName) where T : DictionaryItem
        {
            var headers = BuildHeaderTemplate(dictionary.Name, dictionary.Version.ToString());
            XDocument document = new XDocument(
                new XElement(rootElementName,
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    BuildMainElementsTemplate(elements, collectionElementName, headers)
                    )
                    );
            document.Save(exportFileName);
        }
        private List<XElement> BuildMainElementsTemplate(IEnumerable<XElement> elements, string collectionElementName, List<XElement> headers)
        {
            List<XElement> mainElements = new List<XElement>()
            {
                    new XElement("Elements", elements)
            };
            mainElements.AddRange(headers);
            mainElements.Add(new XElement(collectionElementName, elements));
            return mainElements;
        }
        private List<XElement> BuildHeaderTemplate(string dictionaryName, string dictionaryVersion)
        {
            List<XElement> headers = new List<XElement>()
            {
                new XElement("Name", dictionaryName),
                new XElement("Version", dictionaryVersion)
            };
            return headers;
        }
        /*
        private IEnumerable<XElement> BuildTestDictionaryElements(TestDictionary testDictionary)
        {

            List<XElement> elements = new List<XElement>();
            foreach (var elementDict in testDictionary.Elements)
            {
                XElement element = XElement.Parse(elementDict.Serialize(Encoding.UTF8));
                var numericRangeElements = ProduceNumericRangeElements(elementDict);
                string numericRangesName = ReflectionHelper.GetPropertyName(() => elementDict.NumericRanges);
                element.Element(numericRangesName).ReplaceNodes(numericRangeElements);
                elements.Add(element);
            }
            return elements;
        }
         */
        /// <summary>
        /// Add PatientGroup_Name to NumericRange
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        /*
        private IEnumerable<XElement> ProduceNumericRangeElements(TestDictionaryItem test)
        {
            var otherNumericRanges = new List<NumericRange>(test.NumericRanges.Where(nr => nr.PatientGroup == null));
            var numericRanges = test.NumericRanges.Where(nr => nr.PatientGroup != null)
                .Select(nr =>
                {
                    nr.PatientGroup = (PatientGroupDictionaryItem)ProgramContext.Dictionaries[LimsDictionaryNames.PatientGroup, nr.PatientGroup.Id];
                    return nr;
                });
            otherNumericRanges.AddRange(numericRanges.Where(nr => nr.PatientGroup == null));
            numericRanges = numericRanges.Where(nr => nr.PatientGroup != null);
            List<XElement> otherNRElements = otherNumericRanges.Select(nr => XElement.Parse(nr.Serialize(Encoding.UTF8))).ToList();
            var NRElementsPair = numericRanges.Select(nr => new
            {
                NRElement = XElement.Parse(nr.Serialize(Encoding.UTF8)),
                PatientGroupName = new XElement("PatientGroup_Name", nr.PatientGroup.Name)
            });
            // List<XElement> NRElements = NRElementsPair.Select(nr => { nr.NRElement.Add(nr.PatientGroupName); return nr.NRElement; }).ToList();
            string patientGroupIdName = ReflectionHelper.GetPropertyName(() => new NumericRange().PatientGroup_Id);
            List<XElement> NRElements = NRElementsPair.Select(nr => { nr.NRElement.Elements(patientGroupIdName).ElementAt(0).AddAfterSelf(nr.PatientGroupName); return nr.NRElement; }).ToList();
            return otherNRElements.Union(NRElements);
        }
        */
        private IEnumerable<XElement> BuildPatientGroupDictionary(PatientGroupDictionary patientGroups)
        {
            List<XElement> elements = new List<XElement>();
            foreach (var elementDict in patientGroups.Elements)
            {
                XElement element = AdditionalProcessingxElement(XElement.Parse(elementDict.Serialize(Encoding.UTF8)));

                elements.Add(element);
            }
            return elements;
        }

        private static XElement AdditionalProcessingxElement(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;
                if (xElement.Value == "")
                {
                    if (xmlDocument.Name.LocalName == "PregnancyStart" || xmlDocument.Name.LocalName == "PregnancyEnd")

                    {
                        xElement.Value = "0";
                    }
                    else xElement.ReplaceAll(null);
                }


                /*foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);*/
                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => AdditionalProcessingxElement(el)));

        }

        private static XDocument AdditionalProcessingXDocument(XDocument xmlDocument)
        {
            XDocument newXmlDocument = new XDocument();
            foreach (XElement xElement in xmlDocument.Elements())
            {
                newXmlDocument.Add(AdditionalProcessingxElement(xElement));
            }

            return newXmlDocument;

        }

        public static void ExportServiceDictionaryV1(string exportFileName)
        {
            var targets = (TargetDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.Target];
            var services = (ServiceDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.ServiceShort];

            XDocument document = new XDocument(
                new XElement("Dictionary",
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XElement("Items",
                        from service in services.Elements
                        where (!service.Removed)
                        let target = targets.Find(service.Targets.First().Id)
                        select
                            new XElement("Item",
                                new XElement("ServiceCode", service.Code),
                                new XElement("ServiceName", service.Name),
                                new XElement("ServiceMnemonics", service.Mnemonics),
                                new XElement("TargetCode", target != null ? target.Code : String.Empty),
                                new XElement("TargetName", target != null ? target.Name : String.Empty),
                                new XElement("TargetMnemonics", target != null ? target.Mnemonics : String.Empty)
                                ))));

            string filePath = Directory.GetParent(exportFileName).FullName;
            exportFileName = Path.Combine(filePath, "services.xml");
            document.Save(exportFileName);

            Log.WriteText(String.Format("Dictionary \"{0}\" export succeeded ", "services"));
        }

        private void ExportRequestForm(string exportFileName)
        {
            var targets = (TargetDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.Target];
            var requestForms = (RequestFormDictionary)ProgramContext.Dictionaries[LimsDictionaryNames.RequestForm];

            XDocument document = new XDocument(
                new XElement("Dictionary",
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XElement("Code", "requestForm"),
                    new XElement("Items",
                        from requestForm in requestForms.Elements
                        where (!requestForm.Removed) && (!requestForm.Code.Equals("base"))
                        select
                            new XElement("Item",
                                new XElement("Name", requestForm.Name),
                                new XElement("Code", requestForm.Code),
                                new XElement("Mnemonics", requestForm.Mnemonics),
                                new XElement("RequestFormGroups",
                                    from @group in requestForm.Groups
                                    select
                                        new XElement("RequestFormGroup",
                                            new XElement("Name", @group.Name),
                                            new XElement("TargetCodes",
                                                from targetRank in @group.Targets
                                                let target = targets.Elements.Find(t => t.Id == targetRank.Target.Id)
                                                where !target.Removed
                                                select
                                                    new XElement("Code", target.Code))))))));

            document.Save(exportFileName);
        }

        private void ExportServiceShort(string dictionaryName, string exportFileName)
        {
            var dictionary = (IBaseDictionary)ProgramContext.Dictionaries[dictionaryName];
            var items = new List<DictionaryItem>();

            foreach (var element in dictionary.DictionaryElements)
                items.Add((DictionaryItem)element);

            XDocument document = new XDocument(
                new XElement("Dictionary",
                    new XElement("Code", dictionaryName),
                    new XElement("Items",
                        from element in items
                        where (!element.Removed)
                        select
                            new XElement("Item",
                                new XElement("Name", element.Name),
                                new XElement("Code", element.Code),
                                new XElement("Mnemonics", element.Mnemonics)))));

            document.Save(exportFileName);
        }

        private void ExportDictionaryInBaseFormat(string dictionaryName, string exportFileName)
        {
            var dictionary = (IBaseDictionary)ProgramContext.Dictionaries[dictionaryName];
            var items = new List<DictionaryItem>();

            foreach (var element in dictionary.DictionaryElements)
                items.Add((DictionaryItem)element);

            XDocument document = new XDocument(
                new XElement("Dictionary",
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XElement("Code", dictionaryName),
                    new XElement("Items",
                        from element in items
                        where (!element.Removed)
                        select
                            new XElement("Item",
                                new XElement("Name", element.Name),
                                new XElement("Code", element.Code),
                                new XElement("Mnemonics", element.Mnemonics)))));

            document.Save(exportFileName);
        }
    }
}
