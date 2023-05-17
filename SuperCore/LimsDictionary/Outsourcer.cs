using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    public class OutsourceDefectMapping : BaseObject
    {
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("DefectType")]
        public ObjectRef DefectType { get; set; }
    }
    public class OutsourceUserFieldMapping : BaseObject
    {
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("UserField")]
        public ObjectRef UserField { get; set; }

    }

    public class OutsourceTestMapping : BaseObject
    {
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("Test")]
        public ObjectRef Test { get; set; }
    }

    public class OutsourceBiomaterialMapping : BaseObject
    {
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("ContainerType")]
        public String ContainerType { get; set; }

        [CSN("Biomaterial")]
        public BiomaterialDictionaryItem Biomaterial { get; set; }
    }

    public class OutsourceTargetMapping : BaseObject
    {
        public OutsourceTargetMapping()
        {
            Biomaterials = new List<ObjectRef>();
        }
        [CSN("Name")]
        public String Name { get; set; }
        [CSN("Code")]
        public String Code { get; set; }
        [CSN("SingleTest")]
        public Boolean SingleTest { get; set; }
        [CSN("Biomaterials")]
        public List<ObjectRef> Biomaterials { get; set; }//{ get; private set; }
        [CSN("Target")]
        public ObjectRef Target { get; set; }
    }

    public class OutsourceMicroOrganismMapping : BaseObject
    {
        public OutsourceMicroOrganismMapping()
        {
        }

        [CSN("Name")]
        public String Name { get; set; }

        [CSN("Code")]
        public String Code { get; set; }

        [CSN("MicroOrganism")]
        public ObjectRef MicroOrganism { get; set; }
    }

    public class OutsourcerDictionaryItem : DictionaryItem
    {
        public OutsourcerDictionaryItem()
        {
            Targets = new List<OutsourceTargetMapping>();
            Defects = new List<OutsourceDefectMapping>();
            Biomaterials = new List<OutsourceBiomaterialMapping>();
            Tests = new List<OutsourceTestMapping>();
            MicroOrganisms = new List<OutsourceMicroOrganismMapping>();
            UserFields = new List<OutsourceUserFieldMapping>();
        }
        [CSN("Account_Id")]
        public String Account_Id { get; set; }
        [CSN("Contract_Id")]
        public String Contract_Id { get; set; }
        [CSN("Targets")]
        public List<OutsourceTargetMapping> Targets { get; set; }//private set; }
        [CSN("Defects")]
        public List<OutsourceDefectMapping> Defects { get; set; }//private set; }
        [CSN("Biomaterials")]
        public List<OutsourceBiomaterialMapping> Biomaterials { get; set; }//private set; }
        [CSN("Tests")]
        public List<OutsourceTestMapping> Tests { get; set; }//{ get; private set; }
        [CSN("ExternalSystem")]
        public ExternalSystemDictionaryItem ExternalSystem { get; set; }//{ get; private set; }
        [CSN("ContractNr")]
        public String ContractNr { get; set; }
        [CSN("PartnerId")]
        public String PartnerId { get; set; }
        [CSN("RequestAutoSend")]
        public String RequestAutoSend { get; set; }
        [CSN("MicroOrganisms")]
        public List<OutsourceMicroOrganismMapping> MicroOrganisms { get; set; }
        [CSN("UserFields")]
        public List<OutsourceUserFieldMapping> UserFields { get; set; }

    }

    public class OutsourcerDictionary : DictionaryClass<OutsourcerDictionaryItem>
    {
        public OutsourcerDictionary(String DictionaryName) : base(DictionaryName) { }

        [CSN("OutsourcerNew")]
        public List<OutsourcerDictionaryItem> OutsourcerNew
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}
