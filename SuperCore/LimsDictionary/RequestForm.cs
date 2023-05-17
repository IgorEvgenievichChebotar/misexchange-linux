using System;
using System.Collections.Generic;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class RequestFormDictionaryItem : DictionaryItem
    {
        public RequestFormDictionaryItem()
        {
            Groups = new List<RequestFormGroup>();
            Layout = new ObjectRef();
            Targets = new List<TargetDictionaryItem>();
            Biomaterials = new List<BiomaterialDictionaryItem>();
            UserFields = new List<UserFieldDictionaryItem>();
            UserGroups = new List<UserGroupDictionaryItem>();
            RequestNrTemplate = String.Empty;
            RegistrationWorklistDefRack = new ObjectRef();
            DefaultTargets = new List<TargetDictionaryItem>();
            DefaultBiomaterials = new List<BiomaterialDictionaryItem>();
            ButtonCaption = String.Empty;
        }
        [CSN("NeedBiomaterial")]
        public Boolean NeedBiomaterial { get; set; }
        [CSN("NeedPassportData")]
        public Boolean NeedPassportData { get; set; }
        [CSN("Groups")]
        public List<RequestFormGroup> Groups { get; set; }
        [CSN("Layout")]
        public ObjectRef Layout { get; set; }
        [CSN("UserFields")]
        public List<UserFieldDictionaryItem> UserFields { get; set; }
        [CSN("UserGroups")]
        public List<UserGroupDictionaryItem> UserGroups { get; set; }
        [CSN("RequestNrTemplate")]
        public String RequestNrTemplate { get; set; }
        [CSN("RegistrationWorklistDefRack")]
        public ObjectRef RegistrationWorklistDefRack { get; set; }
        [CSN("DefaultTargets")]
        public List<TargetDictionaryItem> DefaultTargets { get; set; }
        [CSN("BatchMode")]
        public Boolean BatchMode { get; set; }
        [CSN("DefaultBiomaterials")]
        public List<BiomaterialDictionaryItem> DefaultBiomaterials { get; set; }
        [CSN("PrintBatchWorklistBarcodes")]
        public Boolean PrintBatchWorklistBarcodes { get; set; }
        [CSN("PrintBatchSampleBarcodes")]
        public Boolean PrintBatchSampleBarcodes { get; set; }
        [CSN("CanSelectTargets")]
        public Boolean CanSelectTargets { get; set; }
        [CSN("ShowBatchButton")]
        public Boolean ShowBatchButton { get; set; }
        [CSN("ShowGoToButton")]
        public Boolean ShowGoToButton { get; set; }
        [CSN("ShowPrintBarcodeButton")]
        public Boolean ShowPrintBarcodeButton { get; set; }
        [CSN("ShowNewButton")]
        public Boolean ShowNewButton { get; set; }
        [CSN("ShowCopyButton")]
        public Boolean ShowCopyButton { get; set; }
        [CSN("ShowResetButton")]
        public Boolean ShowResetButton { get; set; }
        [CSN("ShowSource")]
        public Boolean ShowSource { get; set; }
        [CSN("ShowEstimatedTime")]
        public Boolean ShowEstimatedTime { get; set; }
        [CSN("ShowSamples")]
        public Boolean ShowSamples { get; set; }
        [CSN("SaveHospital")]
        public Boolean SaveHospital { get; set; }
        [CSN("PriorityMode")]
        public Int32 PriorityMode { get; set; }
        [CSN("ShowPriority")]
        public Boolean ShowPriority { get; set; }
        [CSN("Targets")]
        public List<TargetDictionaryItem> Targets { get; set; }
        [CSN("Biomaterials")]
        public List<BiomaterialDictionaryItem> Biomaterials { get; set; }

        // Макет
        [CSN("GridColumns")]
        public Int32 GridColumns { get; set; }
        [CSN("GridRows")]
        public Int32 GridRows { get; set; }
        [CSN("ButtonWidth")]
        public Int32 ButtonWidth { get; set; }
        [CSN("ButtonHeight")]
        public Int32 ButtonHeight { get; set; }
        [CSN("ButtonCaption")]
        public String ButtonCaption { get; set; }
        [CSN("ButtonIndent")]
        public Int32 ButtonIndent { get; set; }
        [CSN("UseGrouping")]
        public Boolean UseGrouping { get; set; }
        [CSN("OrderForm")]
        public Int32 OrderForm { get; set; }
        [CSN("ControlsFormLayout")]
        public Int32 ControlsFormLayout { get; set; }

        [SendToServer(false)]
        [CSN("_OrderFormLayout")]
        public OrderFormLayout _OrderFormLayout { get; set; }
        [SendToServer(false)]
        [CSN("_ControlsFormLayout")]
        public ControlsFormLayout _ControlsFormLayout { get; set; }
        [SendToServer(false)]
        [CSN("_WorkingFormsLayout")]
        public WorkingFormsLayout _WorkingFormsLayout { get; set; }

        [CSN("CheckPayment")]
        public Boolean CheckPayment { get; set; }
    }

    public class RequestFormDictionary : DictionaryClass<RequestFormDictionaryItem>
    {
        public RequestFormDictionary(String DictionaryName)
            : base(DictionaryName)
        { }

        [CSN("RequestForm")]
        public List<RequestFormDictionaryItem> RequestForm
        {
            get { return Elements; }
            set { Elements = value; }
        }
    }
}