using ru.novolabs.SuperCore;
using ru.novolabs.SuperCore.Controls;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ru
{    
    public struct NlsControlKinds
    {
        public const String None = "None";
        public const String ComboCollection = "ComboCollection";
        public const String ControlComboBox = "ControlComboBox";
        public const String Edit = "Edit";
        public const String Date = "Date";
        public const String DateTime = "DateTime";
        public const String IntEdit = "IntEdit";
        public const String FloatEdit = "FloatEdit";
        public const String CheckBox = "CheckBox";
        public const String Address = "Address";
        public const String BloodParameter = "BloodParameter";
        public const String Parameter = "Parameter";
        public const String UserValue = "UserValue";
        public const String Image = "Image";
        public const String Label = "Label";
        public const String AttrValue = "AttrValue";
        public const String Button = "Button";
        public const String Grid = "Grid";
        public const String AnamnesisParameter = "AnamnesisParameter";
        public const String PhysioIndicator = "PhysioIndicator";
    }


    public enum ObjectType
    {
        None = 0,
        Patient = 1,
        Request = 2,
        Sample = 3,
        Hem_Donor = 4,
        Hem_Product = 5,
        Hem_Transfusion = 6,
        Hem_Transfusion_request = 7,
        Hem_Donor_Comparation = 8,
        Hem_Recipient = 9,
        Hem_Treatment_request = 10,
        Hem_Treatment = 11
    }

    public enum ListTypes
    {
        Анкета = 1,
        Предварительное_лабораторное_обследование = 2, 
        Врачебный_осмотр = 3,
        Лабораторное_обследование = 4,
        Финиш = 5,
        Физиологические_показатели = 6,
        Параметры_анамнеза = 8

    }

    public class ControlField
    {
        private void Init()
        {
            Appearance = new TableFormCellAppearance();
            Appearance.FontFamily = "Verdana";
            Appearance.FontSize = 9;
            VisibleCondition = "";
            FilterCondition = "";
            ReadOnlyCondition = "";
            DefaultValueCondition = "";
            NotNullValidationCondition = "";
            Format = "";
            NowButton = false;
            PropertyName = ""; 
        }
        public ControlField()
        {
            Init();
        }

        public ControlField(WorkingForm form)
        {
            Init();
            Parent = form;
        }

        public String FieldType { get; set; }
        public Boolean NowButton { get; set; }
        public String DefaultValue { get; set; }
        public Boolean Placeholder { get; set; }
        public String PlaceholderText { get; set; }
        public Int32? PlaceholderActiveColor { get; set; }
        public Int32? PlaceholderPassiveColor { get; set; }
        public Int32 Width { get; set; }
        public Int32 Height { get; set; }
        public Int32 Left { get; set; }
        public Int32 Top { get; set; }
#if FIELDSEDITOR
        [Editor(typeof(PropertiesSelector), typeof(UITypeEditor))]
#endif
        public String PropertyName { get; set; }
        public String DictionaryName { get; set; }
        public String DictionaryPropertyName { get; set; }
        public String OnDblClick { get; set; }
        public String OnClick { get; set; }
        public String UserParameterCode { get; set; }
        public Boolean ShowAll { get; set; }
#if FIELDSEDITOR
        [Editor(typeof(ListTypeSelector), typeof(UITypeEditor))]
#endif
        public Int32 ListType { get; set; }
        public Boolean ReadOnly { get; set; }
        public Int32 TabIndex { get; set; }
        public Int32 LinesCount { get; set; }
#if FIELDSEDITOR
        [Editor(typeof(ScriptEditor), typeof(UITypeEditor))]
#endif
        public String VisibleCondition { get; set; }
#if FIELDSEDITOR
        [Editor(typeof(ScriptEditor), typeof(UITypeEditor))]
#endif
        public String FilterCondition { get; set; }
#if FIELDSEDITOR
        [Editor(typeof(ScriptEditor), typeof(UITypeEditor))]
#endif
        public String ReadOnlyCondition { get; set; }
#if FIELDSEDITOR
        [Editor(typeof(PowerScriptEditor), typeof(UITypeEditor))]
#endif
        public String DefaultValueCondition { get; set; }

        [Category("Проверка значения")]
#if FIELDSEDITOR
        [Editor(typeof(ScriptEditor), typeof(UITypeEditor))]
#endif
        public String NotNullValidationCondition { get; set; }

        [Category("Проверка значения")]
        [Description("True если пустое значение запрещено")]
        public Boolean NotNull { get; set; }

        [Category("Проверка значения")]
        [Description("Максимально допустимое значение (для числовых полей)")]
        public String MaxValue { get; set; }

        [Category("Проверка значения")]
        [Description("Минимальное допустимое значение (для числовых полей)")]
        public String MinValue { get; set; }

        [Category("Проверка значения")]
        [Description("Минимальное допустимая длина строки (для строковых полей)")]
        public String MinLength { get; set; }

        [Category("Проверка значения")]
        [Description("Максимально допустимая длина строки (для строковых полей)")]
        public String MaxLength { get; set; }

        [Category("Проверка значения")]
        [Description("Маска ввода. Возможные значения: a (англ.) - любая буква, 9 - любая цифра, * - буква или цифра, ~ - любой символ или его отсутствие, ? - всё что находится правее будет опциональным для ввода (необязательным)")]
        public String Mask { get; set; }

        [Category("Проверка значения")]
        [Description("Связанный Label, имя которого будет оботражаться в суммарном окне с ошибками при валидации и к которому будет добавляться звёздочка")]
        [TypeConverter(typeof(BindedLabelStringConverter))]
        public string BindedLabel { get; set; }

        [Category("Проверка значения")]
        [Description("Регулярное выражение, которому должно соответствовать значение поля. Не должно иметь знака / и флагов на конце.")]
        public string RegEx { get; set; }

        [CategoryAttribute("Внешний вид")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TableFormCellAppearance Appearance { get; set; }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ControlGridOptions ControlGrid { get; set; }

        [XmlIgnore]
        public Color PActiveColor
        {
            get { return PlaceholderActiveColor != null ? PlaceholderActiveColor.Value.ToRgbColor() : SystemColors.Control; }
            set { PlaceholderActiveColor = value.ToRgb(); }
        }

        [XmlIgnore]
        public Color PPassiveColor
        {
            get { return PlaceholderPassiveColor != null ? PlaceholderPassiveColor.Value.ToRgbColor() : SystemColors.Control; }
            set { PlaceholderPassiveColor = value.ToRgb(); }
        }

        public String Format { get; set; }


        private ObjectType buisnessObjectType;
        [Browsable(false)]
        [XmlIgnore]
        public ObjectType BuisnessObjectType
        {
            get { return buisnessObjectType; }
            set { buisnessObjectType = value; }
        }

        [XmlIgnore]
        [Browsable(false)]
        public WorkingForm Parent { get; set; }

    }

    public class WorkingForm
    {
        public WorkingForm()
        {
            Controls = new ControlCellList();
            Width = 500;
            Height = 500;
            PlaceholderAppearance = new TableFormCellAppearance();
        }

        public String Caption { get; set; }
        public Int32 PageIndex { get; set; }
        public ControlCellList Controls { get; set; }
        public Int32 Width { get; set; }
        public Int32 Height { get; set; }


        private String backgroundBase64;

        public String BackgroundBase64
        {
            get { return backgroundBase64; }
            set
            {
                if (value != null)
                {
                    backgroundBase64 = value;
                    byte[] imageBytes = Convert.FromBase64String(backgroundBase64);
                    MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    background = Image.FromStream(ms, true);
                }
                
            }
        }
        private Image background;
        [XmlIgnore]
        public Image Background
        {
            get
            {
                return background;
            }
            set
            {
                background = value;
                MemoryStream ms = new MemoryStream();
                background.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                byte[] data = new byte[ms.Length];
                data = ms.ToArray();
                backgroundBase64 = Convert.ToBase64String(data);
            }

        }

        public Int32? BgColor { get; set; }

        [XmlIgnore]
        public Color BackgroundColor
        {
            get { return BgColor != null ? BgColor.Value.ToRgbColor() : SystemColors.Control; }
            set { BgColor = value.ToRgb(); }
        }

        [CategoryAttribute("Внешний вид заглушки")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public TableFormCellAppearance PlaceholderAppearance { get; set; }

        //public Image Background { get; set; }

        private ObjectType buisnessObjectType;
        [Browsable(false)]
        [XmlIgnore]
        public ObjectType BuisnessObjectType
        {
            get { return buisnessObjectType; }
            set
            {
                buisnessObjectType = value;
                foreach (ControlField cell in Controls)
                    cell.BuisnessObjectType = buisnessObjectType;
            }
        }
    }

    public class ControlCellList : List<ControlField>
    {
        public ControlField this[Int32 x, Int32 y]
        {
            get 
            {
                foreach (ControlField control in this)
                {
                    if (control.Top == y && control.Left == x)
                        return control;
                }
                return null;
            }
        }

        public ControlField this[String PropertyName]
        {
            get
            {
                foreach (ControlField control in this)
                {
                    if (control.UserParameterCode != null && control.UserParameterCode != "")
                    {
                        if (control.UserParameterCode == PropertyName)
                            return control;
                    }
                    else
                    if (control.PropertyName == PropertyName)
                        return control;
                }
                return null;
            }
        }
    }


    public class WorkingFormsLayout
    {
        public List<WorkingForm> Pages = new List<WorkingForm>();

        private ObjectType buisnessObjectType;
        public ObjectType BuisnessObjectType {
            get
            {
                return buisnessObjectType;
            }
            set
            {
                buisnessObjectType = value;
                foreach (WorkingForm page in Pages)
                    page.BuisnessObjectType = buisnessObjectType;
            }
        }
        


        public ControlField GetControlFieldByPropertyName(String PropertyName)
        {
            foreach (WorkingForm page in Pages)
            {
                ControlField control = page.Controls[PropertyName];
                if (control != null)
                    return control;
            }
            return null;
        }

        public static Type GetBuisnessObjectType(Object BuisnessObject)
        {
            return GetBuisnessObjectType(((ObjectType)BuisnessObject.GetType().GetProperty("BuisnessObjectType").GetValue(BuisnessObject, null)));
        }

        public static Type GetBuisnessObjectType(ObjectType BuisnessObject)
        {
            switch (BuisnessObject)
            {
                case ObjectType.Patient:
                    return typeof(Patient);
                case ObjectType.Request:
                    return typeof(CreateRequest3Request);
                case ObjectType.Sample:
                    return typeof(BaseSample);
                case ObjectType.Hem_Donor:
                    return typeof(ru.novolabs.SuperCore.HemBusinessObjects.Donor);
                case ObjectType.Hem_Product:
                    return typeof(ru.novolabs.SuperCore.HemBusinessObjects.Product);
                case ObjectType.Hem_Transfusion:
                    return typeof(ru.novolabs.SuperCore.HemBusinessObjects.Transfusion);
                case ObjectType.Hem_Transfusion_request:
                    return typeof(ru.novolabs.SuperCore.HemBusinessObjects.TransfusionRequest);
                case ObjectType.Hem_Donor_Comparation:
                    return typeof(ru.novolabs.SuperCore.HemBusinessObjects.DonorComparation);
                case ObjectType.Hem_Recipient:
                    return typeof(ru.novolabs.SuperCore.HemBusinessObjects.Recipient);
                case ObjectType.Hem_Treatment_request:
                    return typeof(ru.novolabs.SuperCore.HemBusinessObjects.TreatmentRequest);
                case ObjectType.Hem_Treatment:
                    return typeof(ru.novolabs.SuperCore.HemBusinessObjects.Treatment);
            }
            return null;
        }
    
    }

    public class ControlGridOptions
    {
        public ControlGridOptions()
        {
            Columns = new ControlColumnList();
        }

        public ControlColumnList Columns
        {
            get;
            set; 
        }
        public Boolean CheckBoxes { get; set; }
        public String JournalLayoutCode { get; set; }
    }

    public class ControlColumnList : ObservableCollection<ControlColumn>
    {
    }

    public class ControlColumn
    {
        public ControlColumn()
        {
            Width = 50;
        }

        public String Header { get; set; }
        public String FieldName { get; set; }
        public String ParameterCode { get; set; }
        public String DictionaryName { get; set; }
        public String DictionaryPropName { get; set; }
        public Boolean ReadOnly { get; set; }
        public Int32 Width { get; set; }
        public Boolean FixedSize { get; set; }

        public override string ToString()
        {
            return this.Header;
        }
    }

    public class BindedLabelStringConverter : StringConverter
    {
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<String> list = new List<String>() { "" };
            var control = context.Instance as ControlField;
            if(control.FieldType == NlsControlKinds.Label)
                return new StandardValuesCollection(list);

            if(control.Parent == null)
                return new StandardValuesCollection(list);
            
            foreach(var curControl in control.Parent.Controls.Where(x => x.FieldType == NlsControlKinds.Label).OrderBy(x => x.DefaultValue))
                list.Add(curControl.DefaultValue);
            
            return new StandardValuesCollection(list);
        }
    }

}
