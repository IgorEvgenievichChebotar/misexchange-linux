using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ru.novolabs.MisExchange.ExchangeHelpers.Medwork.MedworkClasses.DictionaryExport
{
    public class ExportedDictionaryItem
    {
        public ExportedDictionaryItem()
        {
            Name = String.Empty;
            Code = String.Empty;
            Mnemonics = String.Empty;
            Removed = false;
        }

        [XmlAttribute(AttributeName = "Name")]
        public String Name { get; set; }
        [XmlAttribute(AttributeName = "Code")]
        public String Code { get; set; }
        [XmlAttribute(AttributeName = "Mnemonics")]
        public String Mnemonics { get; set; }
        [XmlAttribute(AttributeName = "Removed")]
        public Boolean Removed { get; set; }
    }

    public class Biomaterial : ExportedDictionaryItem
    {
        //
    }

    public abstract class ExportedDictionary
    {
        public abstract void AddItem(DictionaryItem item);
    }

    public class BiomaterialDictionary : ExportedDictionary
    {
        public BiomaterialDictionary()
        {
            Biomaterials = new List<Biomaterial>();
        }

        public override void AddItem(DictionaryItem item)
        {
            Biomaterial biomaterial = new Biomaterial() { Name = item.Name, Code = item.Code, Mnemonics = item.Mnemonics, Removed = item.Removed };
            Biomaterials.Add(biomaterial);
        }

        public List<Biomaterial> Biomaterials { get; set; }
    }

    public class DefectType : ExportedDictionaryItem
    {
        //
    }

    public class DefectTypeDictionary : ExportedDictionary
    {
        public DefectTypeDictionary()
        {
            DefectTypes = new List<DefectType>();
        }

        public override void AddItem(DictionaryItem item)
        {
            DefectType defectType = new DefectType() { Name = item.Name, Code = item.Code, Mnemonics = item.Mnemonics, Removed = item.Removed };
            DefectTypes.Add(defectType);
        }

        public List<DefectType> DefectTypes { get; set; }
    }

    public class Hospital : ExportedDictionaryItem
    {
        //
    }

    public class HospitalDictionary : ExportedDictionary
    {
        public HospitalDictionary()
        {
            Hospitals = new List<Hospital>();
        }

        public override void AddItem(DictionaryItem item)
        {
            Hospital hospital = new Hospital() { Name = item.Name, Code = item.Code, Mnemonics = item.Mnemonics, Removed = item.Removed };
            Hospitals.Add(hospital);
        }

        public List<Hospital> Hospitals { get; set; }
    }

    public class UserField : ExportedDictionaryItem
    {
        //
    }

    public class UserFieldDictionary : ExportedDictionary
    {
        public UserFieldDictionary()
        {
            UserFields = new List<UserField>();
        }

        public override void AddItem(DictionaryItem item)
        {
            UserField userField = new UserField() { Name = item.Name, Code = item.Code, Mnemonics = item.Mnemonics, Removed = item.Removed };
            UserFields.Add(userField);
        }

        public List<UserField> UserFields { get; set; }
    }

    public class Test : ExportedDictionaryItem
    {
        //
    }

    public class Target : ExportedDictionaryItem
    {
        public Target()
        {
            Tests = new List<Test>();
        }

        public void AddTest(TestDictionaryItem item)
        {
            Test test = new Test() { Name = item.Name, Code = item.Code, Mnemonics = item.Mnemonics, Removed = item.Removed };
            Tests.Add(test);
        }

        public List<Test> Tests { get; set; }
    }

    public class TargetDictionary : ExportedDictionary
    {
        public TargetDictionary()
        {
            Targets = new List<Target>();
        }

        public override void AddItem(DictionaryItem item)
        {
            Target target = new Target() { Name = item.Name, Code = item.Code, Mnemonics = item.Mnemonics, Removed = item.Removed };
            ((TargetDictionaryItem)item).Tests.ForEach(test => target.AddTest(test));
            Targets.Add(target);
        }

        public List<Target> Targets { get; set; }
    }

    public class TestDictionary : ExportedDictionary
    {
        public TestDictionary()
        {
            Tests = new List<Test>();
        }

        public override void AddItem(DictionaryItem item)
        {
            Test test = new Test() { Name = item.Name, Code = item.Code, Mnemonics = item.Mnemonics, Removed = item.Removed };
            Tests.Add(test);
        }

        public List<Test> Tests { get; set; }
    }

    static class ExportedDictionaries
    {
        internal const String BiomaterialDictionary = "bioMaterial"; // биоматериалы
        internal const String DefectTypeDictionary = "defectType";  // браки
        internal const String HospitalDictionary = "hospital";    // заказчики
        internal const String UserFieldDictionary = "userField";   // пользовательские поля
        internal const String TargetDictionary = "target";      // исследования
        internal const String TestDictionary = "test";        // тесты
    }

    [XmlType(TypeName = "Dictionary")]
    public class DictionaryVersion
    {
        public DictionaryVersion()
        {
            Name = String.Empty;
            Version = 0;
        }

        [XmlAttribute(AttributeName = "Name")]
        public String Name { get; set; }
        [XmlAttribute(AttributeName = "Version")]
        public Int64 Version { get; set; }
    }

    public class DictionaryVersions
    {
        public DictionaryVersions()
        {
            Dictionaries = new List<DictionaryVersion>();
        }

        public void Initialize()
        {
            AddDictionary(ExportedDictionaries.BiomaterialDictionary);
            AddDictionary(ExportedDictionaries.DefectTypeDictionary);
            AddDictionary(ExportedDictionaries.HospitalDictionary);
            AddDictionary(ExportedDictionaries.UserFieldDictionary);
            AddDictionary(ExportedDictionaries.TargetDictionary);
            AddDictionary(ExportedDictionaries.TestDictionary);
        }

        private void AddDictionary(String dictionaryName)
        {
            DictionaryVersion item = new DictionaryVersion() { Name = dictionaryName };
            Dictionaries.Add(item);
        }

        public Boolean FindDictionary(String dictionaryName, out Int32 index)
        {
            Boolean result = false;
            index = -1;

            for (Int32 i = 0; i < Dictionaries.Count; i++)
                if (Dictionaries[i].Name == dictionaryName)
                {
                    result = true;
                    index = i;
                    break;
                }

            return result;
        }

        public void UpdateVersion(String dictionaryName, Int64 value)
        {
            Int32 index;
            if (FindDictionary(dictionaryName, out index))
                Dictionaries[index].Version = value;
        }

        public List<DictionaryVersion> Dictionaries { get; set; }
    }
}