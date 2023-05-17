using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.Classes.Storage
{
    [System.Reflection.Obfuscation]
    [XmlRoot("HelperSettings")]
    public class StorageHelperSettings
    {
        public StorageHelperSettings()
        {
            ReceiptOperationsPath = "ReceiptOperations";
            ReceiptOperationsDefaultStorage = 0;
            ReceiptOperationsAcknowledgmentPath = "ReceiptOperationsAcknowledgment";
            ExpenditureInfosPath = "ExpenditureInfos";
        }

        public string ReceiptOperationsPath { get; set; }
        public int ReceiptOperationsDefaultStorage { get; set; }
        public string ReceiptOperationsAcknowledgmentPath { get; set; }
        public string ExpenditureInfosPath { get; set; }
    }
}