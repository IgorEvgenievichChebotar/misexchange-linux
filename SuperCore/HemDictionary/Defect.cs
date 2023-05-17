using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.HemDictionary
{
    public enum DefectTypes : int
    {
        None = 0,
        Absolute = 1,
        Relative = 2,
    }

    public class DefectDictionaryItem : DictionaryItem
    {
        [CSN("ExternalCode")]
        public new string ExternalCode { get; set; }
        [CSN("DefectType")]
        public int DefectType { get; set; }
        [CSN("NodeType")]
        public int NodeType { get; set; }
        //[SendToServer(false)]
        //[CSN("CodeName")]
        //public string CodeName
        //{
        //    get
        //    {
        //        return string.Format("{0}", base.Name);
        //    }
        //}
    }
}
