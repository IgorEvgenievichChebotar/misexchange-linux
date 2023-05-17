using ru.novolabs.SuperCore.Controls;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.HemBusinessObjects;
using ru.novolabs.SuperCore.HemDictionary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ru.novolabs.SuperCore.CommonBusinesObjects.Forms
{
    public class WorkFormHelper
    {
        ScriptParser parser;
        Panel MainPanel;


        #region Control building
        public ScriptParser BuildWorkForm(ScrollableControl parent, WorkingFormsLayout form)
        {
            parent.AutoScroll = true;
            //Создаем Главную Панель 
            //(YARLY)
            Panel main_panel = new Panel();
            MainPanel = main_panel;
            //Вставить множество страниц
            WorkingForm page = form.Pages[0];
            //Создаем панель-страницу
            Panel page_panel = new Panel();

            //Создаем парсер, хранящий значения и определяющий состояние полей
            ScriptParser scr = CreateParser(form); 
            parser = scr;
            //Устанавливаем значения свойств панели
            main_panel.Size = page_panel.Size = new System.Drawing.Size(page.Width, page.Height);
            if (page.BackgroundBase64 != null && page.BackgroundBase64 != "")
            {
                page_panel.BackgroundImage = page.Background;
            }

            if (page.BackgroundColor != null)
            {
                page_panel.BackColor = page.BackgroundColor;
            }
            //Создаем все контролы на странице
            foreach (ControlField field in page.Controls)
            {
                Control control = BuildControl(field);
                page_panel.Controls.Add(control);
            }

            //После того, как все контролы созданы, назначаем им события
            foreach (Control control in page_panel.Controls)
            {
                ControlField field = page.Controls[control.Name];
                
                SetEvents(page_panel, field, scr);
                if (field.VisibleCondition != null && field.VisibleCondition != "")
                {
                    Boolean visible = parser.GetFieldVisibility(control.Name);
                    if (!visible)
                        control.Visible = false;
                }

                if (field.FilterCondition != null && field.FilterCondition != "")
                {
                    List<Object> items = parser.GetDictionaryField(control.Name, field.DictionaryName);
                    ((ComboBox)control).Items.Clear();
                    foreach (Object item in items)
                    {
                        ((ComboBox)control).Items.Add(new { Text = ((DictionaryItem)item).Name, Value = ((DictionaryItem)item).Id });
                    }
                }
            }

            //Крепим страницу к главной панели
            main_panel.Controls.Add(page_panel);
            main_panel.AutoSize = true;
            parent.Controls.Add(main_panel);
            return scr;
        }

        /// <summary>
        /// Метод создания парсера
        /// </summary>
        /// <param name="form">Сама форма</param>
        /// <returns></returns>
        private ScriptParser CreateParser(WorkingFormsLayout form)
        {
            ScriptParser scr = new ScriptParser(form);
            scr.ParseForm();
            return scr;
        }

        /// <summary>
        /// Создать контрол
        /// </summary>
        /// <param name="field">Описание контрола</param>
        /// <returns></returns>
        private Control BuildControl(ControlField field)
        {
            Control control;
            switch (field.Type)
            {
                case NlsControlKinds.CheckBox:
                    control = CreateCheckBox(field);
                    break;
                case NlsControlKinds.ComboCollection:
                    control = CreateComboCollection(field);
                    break;
                case NlsControlKinds.ControlComboBox:
                    control = CreateComboBox(field);
                    break;
                case NlsControlKinds.Date:
                    control = CreateDate(field);
                    break;
                case NlsControlKinds.DateTime:
                    control = CreateDateTime(field);
                    break;
                case NlsControlKinds.Edit:
                    control = CreateEdit(field);
                    break;
                case NlsControlKinds.IntEdit:
                    control = CreateIntEdit(field);
                    break;
                case NlsControlKinds.FloatEdit:
                    control = CreateFloatEdit(field);
                    break;
                case NlsControlKinds.Image:
                    control = CreateImage(field);
                    break;
                case NlsControlKinds.BloodParameterValue:
                    control = CreateBloodParameterValue(field);
                    break;
                case NlsControlKinds.None:
                case NlsControlKinds.Label:
                default:
                    control = CreateLabel(field);
                    break;
                
            }
            control = SetAttr(control, field);
            return control;
        }

        private Control CreateCheckBox(ControlField field)
        {
            CheckBox control = new CheckBox();
            if (field.DefaultValue != null && field.DefaultValue != "")
                control.Checked = true;
            return control;
        }

        private Control CreateComboCollection(ControlField field)
        {
            ComboCollection control = new ComboCollection();
            control.Items.AddRange(parser.GetDictionaryField(field.PropertyName, field.DictionaryName).ToArray());
            return control;
        }

        private Control CreateComboBox(ControlField field)
        {
            ComboBox control = new ComboBox();
            control.Items.AddRange(parser.GetDictionaryField(field.PropertyName, field.DictionaryName).ToArray());

            ControlTag tag;
            if (control.Tag == null)
            {
                tag = new ControlTag();
                control.Tag = tag;
            }
            else
            {
                
                tag = (ControlTag)control.Tag;
            }
            tag.dictionaryName = field.DictionaryName;
            
            return control;
        }

        private Control CreateDate(ControlField field)
        {
            DateTimePicker control = new DateTimePicker();
            if (field.Format == "")
                control.Format = DateTimePickerFormat.Short;
            else
            {
                control.Format = DateTimePickerFormat.Custom;
                control.CustomFormat = field.Format;
            }

            return control;
        }

        private Control CreateDateTime(ControlField field)
        {
            DateTimePicker control = new DateTimePicker();
            control.Format = DateTimePickerFormat.Custom;
            if (field.Format == "")
                control.CustomFormat = "dd.MM.yyyy HH:mm";
            else
                control.CustomFormat = field.Format;
            return control;
        }

        private Control CreateEdit(ControlField field)
        {
            TextBox control = new TextBox();
            SetFormat(control, field);
            return control;
        }

        private Control CreateIntEdit(ControlField field)
        {
            TextBox control = new TextBox();
            SetFormat(control, field);
            return control;
        }

        private Control CreateFloatEdit(ControlField field)
        {
            TextBox control = new TextBox();
            SetFormat(control, field);
            return control;
        }

        private Control CreateImage(ControlField field)
        {
            PictureBox control = new PictureBox();
            return control;
        }

        private Control CreateLabel(ControlField field)
        {
            Label control = new Label();
            SetFormat(control, field);
            return control;
        }

        private Control CreateBloodParameterValue(ControlField field)
        {
            BloodParameterValueControl control = new BloodParameterValueControl();
            control.BloodParameterCode = field.UserParameterCode;
            return control;
        }

        private void SetFormat(Control control, ControlField field)
        {
            if (field.Format != "")
            {
                ControlTag tag;
                if (control.Tag == null)
                {
                    tag = new ControlTag();
                    control.Tag = tag;
                }
                else
                {

                    tag = (ControlTag)control.Tag;
                }
                tag.format = field.Format;
            }
        }

        /// <summary>
        /// Проставить настройки контрола (обобщенный метод, более тонкие настройки задаются внутри соответствующих методов)
        /// </summary>
        /// <param name="control">Ссылка на контрол</param>
        /// <param name="field">Описание контрола</param>
        /// <returns>Измененный контрол (в целом, не нужен)</returns>
        private Control SetAttr(Control control, ControlField field)
        {
            if (field.ReadOnly)
                control.Enabled = false;
            if (field.UserParameterCode != null && field.UserParameterCode != "")
                control.Name = field.UserParameterCode;
            else
                control.Name = field.PropertyName;
            control.Text = field.DefaultValue;
            
            control.Location = new System.Drawing.Point(field.Left, field.Top);
            //Get Color
            //Поддержать прозрачность?
            Color color = field.Appearance._BgColor;
            color = Color.FromArgb(255, color.R, color.G, color.B);
            control.BackColor = color;
            if (field.Appearance.FontColor != null)
            {
                color = field.Appearance.FontColor.Value.ToARgbColor();
                color = Color.FromArgb(255, color.R, color.G, color.B);
                control.ForeColor = color;
            }

            FontStyle fs = new FontStyle();
            if (field.Appearance.Italic != null && (bool)field.Appearance.Italic)
                fs = fs | FontStyle.Italic;
            if (field.Appearance.Bold != null && (bool)field.Appearance.Bold)
                fs = fs | FontStyle.Bold;
            if (field.Appearance.Underline != null && (bool)field.Appearance.Underline)
                fs = fs | FontStyle.Underline;

            if (field.Appearance.FontSize == null)
                control.Font = new System.Drawing.Font(field.Appearance.FontFamily, 8F, fs);
            else
                control.Font = new System.Drawing.Font(field.Appearance.FontFamily, field.Appearance.FontSize.Value, fs);

            
            control.Width = field.Width;
            control.Height = field.Height;
            //control.Size = new System.Drawing.Size(field.Width, field.Height);
            return control;
        }

        /// <summary>
        /// Задать такие события для контрола, как изменение видимости или фильтрации.
        /// </summary>
        /// <param name="field">Описание контрола</param>
        /// <param name="scr">Парсер</param>
        private void SetEvents(Panel page, ControlField field, ScriptParser scr)
        {
            String FieldName;
            if (field.UserParameterCode != null && field.UserParameterCode != "")
                FieldName = field.UserParameterCode;
            else
                FieldName = field.PropertyName;
            List<String> filterFields = scr.GetRelatedFilterFields(FieldName);
            List<String> visibleFields = scr.GetRelatedVisibleFields(FieldName);


            Control control = page.Controls[FieldName];
            if (control != null)
            {
                ControlTag tag;
                if (control.Tag == null)
                    tag = new ControlTag();
                else
                    tag = (ControlTag)control.Tag;

                foreach (String filter in filterFields)
                {
                    Control subctrl = page.Controls[filter];
                    if (subctrl != null)
                    {
                        tag.filter.Add(subctrl);
                    }
                }

                foreach (String visible in visibleFields)
                {
                    Control subctrl = page.Controls[visible];
                    if (subctrl != null)
                    {
                        tag.visible.Add(subctrl);
                    }
                }

                control.Tag = tag;
                if (control.GetType() == typeof(CheckBox))
                    ((CheckBox)control).CheckedChanged += control_TextChanged;
                else
                    control.TextChanged += control_TextChanged;
                
            }
        }

        public event EventHandler DataChanged;

        private void control_TextChanged(object sender, EventArgs e)
        {
            if (DataChanged != null)
                DataChanged(sender, e);    

            //Если мы в данный момент загружаем объект - приостанавливаем обновление данных
            if (StopEvents) return;

            ControlTag tag = (ControlTag)((Control)sender).Tag;
            String value;
            if (tag.dictionaryName == "")
            {
                if (sender.GetType() == typeof(CheckBox))
                    value = ((CheckBox)sender).Checked ? true.ToString() : false.ToString();
                else
                    value = ((Control)sender).Text;
            }
            else
            {
                value = "";
                if (sender.GetType() == typeof(ComboBox))
                {
                    if (((ComboBox)sender).SelectedIndex == -1)
                        value = "0";
                    else
                        value = ((BaseObject)((ComboBox)sender).SelectedItem).Id.ToString();
                }
                else
                    if (sender.GetType() == typeof(ComboCollection))
                    {
                        foreach (DictionaryItem item in ((ComboCollection)sender).CheckedItems)
                        {
                            value += item.Id + ",";
                        }
                    }
            }

            String name = ((Control)sender).Name;
            if (parser.values.Keys.Contains(name))
                parser.values[name] = value;
            else
                parser.values.Add(name, value);

            foreach (Control control in tag.visible)
            {
                Boolean visibility = parser.GetFieldVisibility(control.Name);
                if (visibility)
                    control.Visible = true;
                else
                    control.Visible = false;
            }

            foreach (Control control in tag.filter)
            {
                ControlTag subtag = (ControlTag)control.Tag;
                ((ComboBox)control).SelectedIndex = -1;
                ((ComboBox)control).Items.Clear();
                ((ComboBox)control).Items.AddRange(parser.GetDictionaryField(control.Name, subtag.dictionaryName).ToArray());
            }

        }

        
        #endregion

        #region Control management

        private void control_UpdateField()
        {

        }

        public Object GetResult(Type ObjectType)
        {
            return parser.GetResult(ObjectType);
        }

        private Boolean StopEvents = false;

        public void ReadFromObject(Object obj) 
        {
            StopEvents = true;
            //Листаем страницы
            foreach (Control page in MainPanel.Controls)
            {
                foreach (Control control in page.Controls)
                {
                    //Листаем контролы
                    ControlTag tag = (ControlTag)control.Tag;
                    String name = control.Name;

                    PropertyInfo propInfo = obj.GetType().GetProperty(name);

                    if (propInfo != null)
                    {
                        String value;
                        if (propInfo.PropertyType.IsSubclassOf(typeof(BaseObject)))
                        {
                            Object subobj = propInfo.GetValue(obj, null);
                            if (tag == null || tag.format == "")
                            {
                                value = (subobj != null) ? subobj.GetType().GetCustomProperty("Id").GetValue(subobj, null).ToString() : "0";
                            }
                            else
                            {
                                subobj = ProgramContext.Dictionaries.GetDictionaryItem(subobj.GetType(), (Int32)subobj.GetType().GetCustomProperty("Id").GetValue(subobj, null));
                                if (subobj != null)
                                    value = GetPropertyByFormat(tag.format, subobj);
                                else
                                    value = "null";
                            }

                        }
                        else
                        {
                            object valueObj = propInfo.GetValue(obj, null);
                            value = valueObj != null ? valueObj.ToString() : String.Empty;
                        }

                        if (tag.dictionaryName == "")
                        {
                            if (control.GetType() == typeof(CheckBox))
                            {
                                ((CheckBox)control).Checked = Boolean.Parse(value);
                            }
                            else
                                control.Text = value;
                        }
                        else
                        {
                            if (control.GetType() == typeof(ComboBox))
                            {
                                if (value == "0")
                                    ((ComboBox)control).SelectedIndex = -1;
                                else
                                {
                                    foreach (Object item in ((ComboBox)control).Items)
                                    {
                                        if (((BaseObject)item).Id == Int32.Parse(value))
                                        {
                                            ((ComboBox)control).SelectedItem = item;
                                            break;
                                        }
                                    }
                                }

                            }
                            else
                                if (control.GetType() == typeof(ComboCollection))
                                {
                                    List<String> values = value.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                                    List<DictionaryItem> checkedItems = new List<DictionaryItem>();
                                    foreach (String val in values)
                                    {
                                        foreach (Object item in ((ComboCollection)control).Items)
                                        {
                                            if (((BaseObject)item).Id.ToString() == val.ToString())
                                            {
                                                checkedItems.Add(new DictionaryItem() { Id = (((BaseObject)item).Id) });
                                            }
                                        }
                                    }
                                    ((ComboCollection)control).CheckedItems = checkedItems;
                                }
                        }

                        if (parser.values.Keys.Contains(name))
                            parser.values[name] = value;
                        else
                            parser.values.Add(name, value);
                    }
                    else
                    {
                        if (control.GetType().Equals(typeof(BloodParameterValueControl)))
                        {
                            List<BloodParameterValue> paramValues = (List<BloodParameterValue>)obj.GetType().GetCustomProperty(UserParamNames.BloodParameters).GetValue(obj, null);
                            foreach (BloodParameterValue paramValue in paramValues)
                            {
                                BloodParameterItem bloodParameter = (BloodParameterItem)((BloodParameterGroupDictionaryClass)ProgramContext.Dictionaries[HemDictionaryNames.BloodParameterGroup]).GetBloodParameterById(paramValue.Parameter.Id);
                                if (bloodParameter != null)
                                {
                                    if (bloodParameter.Code == control.Name)
                                    {
                                        ((BloodParameterValueControl)control).SetValue(paramValue);
                                        String value = GetStringParameterValue(paramValue);
                                        if (parser.values.Keys.Contains(name))
                                            parser.values[name] = value;
                                        else
                                            parser.values.Add(name, value);
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            
            
            StopEvents = false;

            foreach (Control control in MainPanel.Controls)
            {
                ControlTag tag = (ControlTag)control.Tag;
                if (tag != null)
                {
                    InnerFilter(tag);
                    InnerVisibility(tag);
                }
            }
        }

        private String GetStringParameterValue(BloodParameterValue paramValue)
        {
            BloodParameterItem param = (BloodParameterItem)((BloodParameterGroupDictionaryClass)ProgramContext.Dictionaries[HemDictionaryNames.BloodParameterGroup]).GetBloodParameterById(paramValue.Parameter.Id);
            if (param != null)
            {
                switch (param.FieldType)
                {
                    case BloodParameterFieldType.HEM_FIELD_TYPE_STRING:
                    case BloodParameterFieldType.HEM_FIELD_TYPE_NUMERIC:
                    case BloodParameterFieldType.HEM_FIELD_TYPE_DATETIME:
                    case BloodParameterFieldType.HEM_FIELD_TYPE_BOOLEAN:
                        return paramValue.Value;

                    case BloodParameterFieldType.HEM_FIELD_TYPE_ENUMERATION:
                    case BloodParameterFieldType.HEM_FIELD_TYPE_SEX:
                        return paramValue.Reference.Id.ToString();
                    case BloodParameterFieldType.HEM_FIELD_TYPE_SET:
                        String result = "";
                        foreach (ObjectRef value in paramValue.Values)
                        {
                            result += value.Id + ",";
                        }
                        return result;
                    case BloodParameterFieldType.HEM_FIELD_TYPE_UNKNOWN:
                        return "";
                }
            }
            return "";
        }

        private String GetPropertyByFormat(String format, Object obj)
        {
            List<String> frmts = new List<string>();
            frmts = format.Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries).ToList();
            Object subobject = obj;
            foreach (String frmt in frmts)
            {
                if (subobject.GetType().GetCustomProperty(frmt) != null)
                    subobject = obj.GetType().GetCustomProperty(frmt).GetValue(obj, null);
                else
                    return subobject.ToString();
            }
            return subobject.ToString();
        }


        private void InnerVisibility(ControlTag tag)
        {
            foreach (Control control in tag.visible)
            {
                ControlTag subtag = (ControlTag)control.Tag;
                if (subtag != null)
                {
                    InnerVisibility(subtag);
                    Boolean visibility = parser.GetFieldVisibility(control.Name);
                    if (visibility)
                        control.Visible = true;
                    else
                        control.Visible = false;
                }

            }
        }

        private void InnerFilter(ControlTag tag)
        {
            foreach (Control control in tag.filter)
            {
                ControlTag subtag = (ControlTag)control.Tag;
                if (subtag != null)
                {
                    InnerFilter(subtag);
                    ((ComboBox)control).SelectedIndex = -1;
                    ((ComboBox)control).Items.Clear();
                    ((ComboBox)control).Items.AddRange(parser.GetDictionaryField(control.Name, subtag.dictionaryName).ToArray());
                }
            }
        }

        public void WriteToObject(Object obj)
        {
            parser.GetResult(obj);
            UpdateReferences(obj);
        }

        public Dictionary<String, String> GetValues()
        {
            return parser.values;
        }


        private void UpdateObjectRefs(Object parentObject, PropertyInfo propInfo, object propValue, object[] index = null)
        {
            if (propValue == null) return;
            // Если текущий объект является совместимым с классом справочного объекта
            if (propInfo.PropertyType.IsSubclassOf(typeof(DictionaryItem)))
            {
                DictionaryItem value = (DictionaryItem)propValue;
                if ((value != null) && (value.Id > 0))
                {
                    Object dictionaryItem = ProgramContext.Dictionaries.GetItemByReference(propInfo.PropertyType, value.Id);
                    if (dictionaryItem != null)
                        propInfo.SetValue(parentObject, dictionaryItem, index);
                }
            }
            // Если текущий объект является экземпляром класса, отличного от справочного
            else
            {
                // Если текущий объект является коллекцией объектов, но не массивом
                if ((propInfo.PropertyType.GetInterface(typeof(IList<Type>).Name) != null) && (!propInfo.PropertyType.IsArray))
                {
                    IList list = (IList)propValue;
                    for (int i = 0; i < list.Count; i++)
                    {
                        UpdateObjectRefs(list, list.GetType().GetProperty("Item", new Type[] { typeof(int) }), list[i], new Object[] { i });
                    }
                }
                // Если текущий объект является экземпляром единичного класса
                else
                {
                    if (!(propInfo.PropertyType.GetInterface(typeof(IDictionary<Type, Type>).Name) != null))
                    {
                        foreach (PropertyInfo propertyInfo in propValue.GetType().GetProperties())
                            if ((propertyInfo.PropertyType.IsClass) && (!propertyInfo.PropertyType.Equals(typeof(String))))
                            {
                                UpdateObjectRefs(propValue, propertyInfo, propertyInfo.GetValue(propValue, null));
                            }
                    }
                }
            }
        }

        private void UpdateReferences(Object obj)
        {
            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
            {
                if ((propInfo.PropertyType.IsClass) && (!propInfo.PropertyType.Equals(typeof(String))) && (propInfo.PropertyType.IsSubclassOf(typeof(DictionaryItem)) || propInfo.PropertyType.Equals(typeof(DictionaryItem))))
                {
                    Object propValue = propInfo.GetValue(obj, null);
                    UpdateObjectRefs(obj, propInfo, propValue);
                }
            }
        }

        #endregion

    }

    public class ControlTag
    {
        public List<Control> filter = new List<Control>();
        public List<Control> visible = new List<Control>();
        public String dictionaryName = "";
        public String format = "";
    }

    
}
