using Newtonsoft.Json;
using ru.novolabs.SuperCore.DictionaryCore;
using System.Collections.Generic;
using System.Xml.Serialization;

public enum TargetTypes : int
{
    Simple = 1, // Простое исследование
    Group = 2,  // Пакет исследований
    Profile = 3 // Профиль исследований
}

namespace ru.novolabs.SuperCore.LimsDictionary
{
    [OldSaveMethod]
    public class TargetDictionaryItem : DictionaryItem
    {
        public TargetDictionaryItem()
        {
            Tests = new List<TestDictionaryItem>();
            Services = new List<ServiceDictionaryItem>();
            Biomaterials = new List<BiomaterialDictionaryItem>();
            Targets = new List<TargetDictionaryItem>();
            CommentSources = new List<CommentSourceDictionaryItem>();
            Samples = new List<ProfileSample>();
            MandatoryFields = new List<string>();
            MandatoryUserFields = new List<string>();
            Hospitals = new List<ObjectRef>();
            BiomaterialContainers = new List<BiomaterialContainerDictionaryItem>();
        }

        [XmlIgnore]
        [CSN("EngName")]
        public string EngName { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        [CSN("Tests")]
        public List<TestDictionaryItem> Tests { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        [CSN("Services")]
        public List<ServiceDictionaryItem> Services { get; set; }
        [XmlIgnore]
        [CSN("Biomaterials")]
        public List<BiomaterialDictionaryItem> Biomaterials { get; set; }
        [XmlIgnore]
        [JsonIgnore]
        [CSN("Targets")]
        public List<TargetDictionaryItem> Targets { get; set; }
        [CSN("Samples")]
        [JsonIgnore]
        public List<ProfileSample> Samples { get; set; }
        [CSN("TargetType")]
        public int TargetType { get; set; }
        [CSN("Department")]
        public DepartmentDictionaryItem Department { get; set; }
        [CSN("Comment")]
        public string Comment { get; set; }
        [CSN("CommentSources")]
        public List<CommentSourceDictionaryItem> CommentSources { get; set; }
        [CSN("MandatoryFields")]
        public List<string> MandatoryFields { get; set; }
        [CSN("MandatoryUserFields")]
        public List<string> MandatoryUserFields { get; set; }
        [CSN("Hospitals")]
        public List<ObjectRef> Hospitals { get; set; }
        [CSN("AdditionalTube")]
        public bool AdditionalTube { get; set; }
        [CSN("AdditionalTubeCode")]
        public string AdditionalTubeCode { get; set; }
        [CSN("SampleNrSuffix")]
        public string SampleNrSuffix { get; set; }
        [CSN("DefaultBiomaterial")]
        public BiomaterialDictionaryItem DefaultBiomaterial { get; set; }
        [CSN("ResearchPreparationText")]
        public string ResearchPreparationText { get; set; }

        [CSN("BiomaterialContainers")]
        public List<BiomaterialContainerDictionaryItem> BiomaterialContainers { get; set; }
        /// <summary>
        /// Возвращает признак того, что исследование является простым
        /// </summary>
        /// <returns></returns>
        public bool IsSimple()
        {
            return TargetType.Equals((int)TargetTypes.Simple);
        }

        /// <summary>
        /// Возвращает признак того, что исследование является пакетом исследований
        /// </summary>
        /// <returns></returns>
        public bool IsGroup()
        {
            return TargetType.Equals((int)TargetTypes.Group);
        }

        /// <summary>
        /// Возвращает признак того, что исследование является профилем исследований
        /// </summary>
        /// <returns></returns>
        public bool IsProfile()
        {
            return TargetType.Equals((int)TargetTypes.Profile);
        }
    }

    public class ProfileSample
    {
        public ProfileSample()
        {
            {
                Biomaterial = new BiomaterialDictionaryItem();
                Targets = new List<TargetDictionaryItem>();
            }
        }

        [CSN("Biomaterial")]
        public BiomaterialDictionaryItem Biomaterial { get; set; }
        [XmlIgnore]
        [CSN("Targets")]
        public List<TargetDictionaryItem> Targets { get; set; }
    }

    /// <summary>
    /// Представляет справочник "Исследования"
    /// </summary>
    public class TargetDictionary : DictionaryClass<TargetDictionaryItem>
    {
        public TargetDictionary(string DictionaryName) : base(DictionaryName) { }

        [CSN("Target")]
        public List<TargetDictionaryItem> Target
        {
            get => Elements;
            set => Elements = value;
        }
    }
}