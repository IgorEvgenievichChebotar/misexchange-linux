using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using ru.novolabs.SuperCore.Controls;
using System.Reflection;
using Newtonsoft.Json;

namespace ru.novolabs.SuperCore.CommonBusinesObjects
{

    [XmlRoot("FilterSettings")]
    [Obfuscation]
    public class JournalFilterSettings
    {
        public static string lisControlComboCollection = "ComboCollection";
        public static string lisControlComboBox = "ControlComboBox";
        public static string controlEdit = "Edit";
        public static string controlDateTime = "DateTime";
        public static string controlIntEdit = "IntEdit";
        public static string controlFloatEdit = "FloatEdit";
        public static string controlCheckBox = "CheckBox";
        public static string controlNone = "None";
        public static string controlAddress = "Address";
        public static string controlBloodParameters = "BloodParameters";

        public static string cDefaultPropName = "Name";


        public JournalFilterSettings()
        {
            Rows = new List<ControlRow>();
        }

        public string JournalName { get; set; }
        public string UserJournalName { get; set; }
        public string ObjectName { get; set; }
        public List<ControlRow> Rows { get; set; }


        public ControlRow GetControlRow(string controlName)
        {
            foreach (ControlRow row in Rows)
            {
                if (row.ControlName.Equals(controlName))
                    return row;
            }
            return null;
        }
    }

    [Obfuscation]
    public class ControlRow : IComparable
    {
        private Dictionary<string, NlsControlKind> controlKinds = new Dictionary<string, NlsControlKind>();

        public ControlRow()
        {
            RelatedParameters = new List<String>();
            controlKinds.Add(NlsControlKinds.None, NlsControlKind.None);
            controlKinds.Add(NlsControlKinds.Edit, NlsControlKind.Edit);
            controlKinds.Add(NlsControlKinds.DateTime, NlsControlKind.DateTime);
            controlKinds.Add(NlsControlKinds.CheckBox, NlsControlKind.CheckBox);
            controlKinds.Add(NlsControlKinds.ComboCollection, NlsControlKind.ComboCollection);
            controlKinds.Add(NlsControlKinds.ControlComboBox, NlsControlKind.ControlComboBox);
            controlKinds.Add(NlsControlKinds.IntEdit, NlsControlKind.IntEdit);
            controlKinds.Add(NlsControlKinds.FloatEdit, NlsControlKind.FloatEdit);
            controlKinds.Add(NlsControlKinds.Address, NlsControlKind.Address);
            controlKinds.Add(NlsControlKinds.BloodParameter, NlsControlKind.BloodParameter);
            Visible = true;
        }

        [Browsable(false)]
        //[DisplayName("Порядковый номер")]
        public Int32 Order { get; set; }
     //   [DisplayName("Является категорией")]
        public bool IsCategory { get; set; }
     //   [DisplayName("Подпись")]
        public string Caption { get; set; }
     //   [DisplayName("Индекс изображения")]
        public Int32 ImageIndex { get; set; }
        [Browsable(false)]
        public string ControlKind { get; set; }

        [Browsable(false)]
        public string ControlName { get; set; }

        [CategoryAttribute("Связь со справочниками")]
       // [DisplayName("Справочник. Имя")]
#if FIELDSEDITOR
        [Editor(typeof(DictionaryNameSelector), typeof(UITypeEditor))]
#endif
        public string DictionaryName { get; set; }
        [CategoryAttribute("Связь со справочниками")]
       // [DisplayName("Справочник. Свойство")]
        public string DictionaryPropName { get; set; }

        //[DisplayName("Объект. Свойство")]
        [Description("Имя свойства в объекте, который будет редактироваться при помощи данной формы")]
#if FIELDSEDITOR
        [Editor(typeof(BusinessObjectPropertySelector), typeof(UITypeEditor))]
#endif
        public string PropertyName { get; set; }
       // [DisplayName("Код пользовательского поля")]
        public string UserFieldCode { get; set; }

        //Код параметра
        public string ParameterCode { get; set; }

        //[DisplayName("Минимальная высота")]
        public Int32 MinHeight { get; set; }
        //[DisplayName("Минимальное значение")]
        public Int32 MinValue { get; set; }
       // [DisplayName("Максимальное значение")]
        public Int32 MaxValue { get; set; }
       // [DisplayName("Максимальная длина")]
        public Int32 MaxLength { get; set; }

       // [DisplayName("Верхний регистр")]
        public bool UpperCase { get; set; }
      //  [DisplayName("Значение по умолчанию")]
        public String DefaultValue { get; set; }

        public Boolean Visible { get; set; }

        public bool ReadOnly { get; set; }

        public Int32 TabIndex { get; set; }

        public Int32 LinesCount { get; set; }

        [Browsable(false)]
        public List<String> RelatedParameters { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 0;
            if (!obj.GetType().IsInstanceOfType(this)) return 0;
            if (this.Order == 0) return 0;
            return this.Order.CompareTo(((ControlRow)obj).Order);
        }

        public override string ToString()
        {
            return String.Empty;
        }

        // Вспомогательные свойства. Нужны только для дизайна
        //  [DisplayName("Объект. Имя")]
#if FIELDSEDITOR
        [Editor(typeof(BusinessObjectSelector), typeof(UITypeEditor))]
#endif
        [XmlIgnore]
        public string BusinessObjectName { get; set; }


        [XmlIgnore]
        [JsonIgnore]
        [DisplayName("ControlKind")]
        public NlsControlKind Control_Kind
        {
            get { return ControlKind == null ? NlsControlKind.None : controlKinds[ControlKind]; }
            set
            {
                foreach (var key in controlKinds.Keys)
                {
                    if (controlKinds[key].Equals(value))
                    {
                        ControlKind = key;
                        break;
                    }
                }
            }
        }
    }
}

