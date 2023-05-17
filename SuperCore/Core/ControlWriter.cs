//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows.Forms;
//using System.Reflection;
//using System.Collections;
//using ru.novolabs.SuperCore.DictionaryCore;


//namespace ru.novolabs.SuperCore
//{
//    public class ControlWriter
//    {

//        private Control writeControl = null;
//        private ToolTip writeToolTip = null;

//        public ControlWriter(Control control, ToolTip toolTip)
//        {
//            writeControl = control;
//            //   writeToolTip = toolTip;
//        }

//        public ControlWriter(Control control)
//        {
//            writeControl = control;
//        }


//        public void Write(Object obj)
//        {
//            WriteControls(writeControl, obj);
//        }

//        private void WriteControls(Control controlHolder, Object obj)
//        {
//            foreach (Control control in controlHolder.Controls)
//            {
//                if (control.Name == string.Empty)
//                {
//                    continue;
//                }

//                //if (ControlIsHolder(control))
//                {
//                    WriteControls(control, obj);
//                }
//                //else
//                {
//                    if (control.GetType().Equals(typeof(TextBox)))
//                    {
//                        WriteTextBox((TextBox)control, obj);
//                    }
//                    if (control.GetType().Equals(typeof(CheckBox)))
//                    {
//                        WriteCheckBox((CheckBox)control, obj);
//                    }
//                    if (control.GetType().Equals(typeof(Label)))
//                    {
//                        WriteLabel((Label)control, obj);
//                    }
///*                    else if (control is IDictionaryControl)
//                    {
//                        WriteDictionaryControl((IDictionaryControl)control, obj);
//                    }
//                    else if (control is IParamValueControl)
//                    {
//                        WrireParamValueControl((IParamValueControl)control, obj);
//                    }
//                    else if (control is IDataControl)
//                    {
//                        WriteDataControl((IDataControl)control, obj);
//                    } */
//                    else if (control.GetType().Equals(typeof(DateTimePicker)))
//                    {
//                        WriteDateTime((DateTimePicker)control, obj);
//                    }
//                }
//            }

//        }



//        /*private void WrireParamValueControl(IParamValueControl control, Object obj)
//        {
//            string propName = control.GetPropName();
//            PropertyInfo propInfo = GetProperty(obj, propName);
//            if (propInfo != null)
//            {
//                Object value = propInfo.GetValue(obj, null);
//                if (value.GetType().GetInterface("IList") != null)
//                {
//                    IList paramList = (IList)propInfo.GetValue(obj, null);
//                    {
//                        WriteParameterValue(paramList, control);
//                    }
//                }
//            }
//        }

//        private void WriteParameterValue(IList paramList, IParamValueControl control)
//        {
//            foreach (BaseParameterValue paramValue in paramList)
//            {
//                if (paramValue.Parameter.GetRef() == control.ParamId)
//                {
//                    control.ValueId = paramValue.Reference.GetRef();
//                    break;
//                }
//            }
//        } */



//        private string ExtractName(string controlName)
//        {
//            for (int i = 0; i < controlName.Length; i++)
//            {
//                if (controlName[i].ToString().ToUpper() == controlName[i].ToString())
//                {
//                    return controlName.Substring(i);
//                }
//            }
//            return string.Empty;
//        }

//        private PropertyInfo GetProperty(Object obj, string propName)
//        {
//            PropertyInfo propInfo = obj.GetType().GetProperty(propName);
//            if (propInfo == null)
//            {
//                propName = propName.Substring(0, 1).ToUpper() + propName.Substring(1);
//                propInfo = obj.GetType().GetProperty(propName);
//            }
//            return propInfo;
//        }


//        private void WriteTextBox(TextBox textBox, Object obj)
//        {
//            string propName = ExtractName(textBox.Name);
//            PropertyInfo propInfo = GetProperty(obj, propName);
//            if (propInfo != null)
//            {
//                string value = GetStringValue(propInfo, obj);
//                textBox.Text = value;
//                SetToolTip(textBox, textBox.Text);
//            }
//        }


//        private void WriteCheckBox(CheckBox checkBox, Object obj)
//        {
//            string propName = ExtractName(checkBox.Name);
//            PropertyInfo propInfo = GetProperty(obj, propName);
//            if (propInfo != null)
//            {
//                bool value = GetBoolValue(propInfo, obj);
//                checkBox.Checked = value;
//                SetToolTip(checkBox, checkBox.Text);
//            }
//        }


//        private void WriteLabel(Label label, Object obj)
//        {
//            string propName = ExtractName(label.Name);
//            PropertyInfo propInfo = GetProperty(obj, propName);
//            if (propInfo != null)
//            {
//                string value = GetStringValue(propInfo, obj);
//                label.Text = value;
//                SetToolTip(label, label.Text);
//            }
//        }


//        delegate void DataTimePickerSetCheckedCallback(DateTimePicker picker, bool state);
//        private void DataTimePickerSetChecked(DateTimePicker picker, bool state)
//        {
//            picker.Checked = state;
//        }

//        private void WriteDateTime(DateTimePicker dateTimePicker, Object obj)
//        {
//            string propName = ExtractName(dateTimePicker.Name);
//            PropertyInfo propInfo = GetProperty(obj, propName);
//            DataTimePickerSetCheckedCallback delegateCallback = new DataTimePickerSetCheckedCallback(DataTimePickerSetChecked);
//            if (propInfo != null)
//            {
//                DateTime dateTime = GetDateTimeValue(propInfo, obj);
//                if (dateTime != DateTime.MinValue)
//                {
//                    dateTimePicker.Invoke(delegateCallback, new Object[] { dateTimePicker, true });
//                    dateTimePicker.Value = dateTime;
//                    SetToolTip(dateTimePicker, dateTimePicker.Text);
//                }
//                else
//                {
//                    dateTimePicker.Invoke(delegateCallback, new Object[] { dateTimePicker, false });
//                }
//            }
//        }

//        private void SetToolTip(Control control, string hint)
//        {
//            if (writeToolTip != null)
//            {
//                writeToolTip.SetToolTip(control, hint);
//            }
//        }


///*        private void WriteDataControl(IDataControl control, Object obj)
//        {
//            string propName = control.GetPropName();
//            PropertyInfo propInfo = GetProperty(obj, propName);
//            control.Value = null;
//            if (propInfo != null)
//            {
//                Object value = propInfo.GetValue(obj, null);
//                control.Value = value;
//                SetToolTip(control.GetControl(), control.GetText());
//            }
//        }


//        private void WriteDictionaryControl(IDictionaryControl control, Object obj)
//        {
//            string propName = control.GetPropName();
//            PropertyInfo propInfo = GetProperty(obj, propName);
//            if (propInfo != null)
//            {
//                ObjectRef reference = GetRefValue(propInfo, obj);
//                SetDictionary(control, propInfo);
//                if (reference != null)
//                {
//                    control.SelectedId = reference.ID;
//                    SetToolTip(control.GetControl(), control.GetText());
//                }
//            }
//        } */

///*        private void SetDictionary(IDictionaryControl control, PropertyInfo propInfo)
//        {
//            LinkedDictionary displayValue = GetDisplayValueAttribute(propInfo);

//            if (displayValue != null)
//            {
//                if (control.DictionaryName.Equals(string.Empty) && !displayValue.DictionaryName.Equals(string.Empty))
//                {
//                    control.DictionaryName = displayValue.DictionaryName;
//                }
//                if (control.DictionaryPropertyName.Equals(string.Empty) && !displayValue.PropertyName.Equals(string.Empty))
//                {
//                    control.DictionaryPropertyName = displayValue.PropertyName;
//                }
//            }
//        } */

//        private LinkedDictionary GetDisplayValueAttribute(PropertyInfo propInfo)
//        {
//            object[] attributes = propInfo.GetCustomAttributes(typeof(LinkedDictionary), true);
//            foreach (object obj in attributes)
//            {
//                return (LinkedDictionary)obj;
//            }
//            return null;
//        } 


//        private bool GetBoolValue(PropertyInfo propInfo, Object obj)
//        {
//            if (obj == null)
//            {
//                return false;
//            }

//            return (bool)propInfo.GetValue(obj, null);
//        }

//        private ObjectRef GetRefValue(PropertyInfo propInfo, Object obj)
//        {

//            if (obj == null)
//            {
//                return null;
//            }

//            Object reference = propInfo.GetValue(obj, null);

//            if (reference is ObjectRef)
//            {
//                if (((ObjectRef)reference).Id > 0)
//                {
//                    return (ObjectRef)reference;
//                }
//            }
//            else if (reference.GetType().Equals(typeof(int)))
//            {
//                int value = (int)reference;
//                if (value > 0)
//                {
//                    return new ObjectRef(value);
//                }
//            }
//            return null;
//        }

//        private string GetStringValue(PropertyInfo propInfo, object obj)
//        {
//            if (propInfo == null)
//            {
//                return string.Empty;
//            }

//            LinkedDictionary displayValue = GetDisplayValueAttribute(propInfo);

//            Object value = propInfo.GetValue(obj, null);

//            if (value.GetType().Equals(typeof(DateTime)))
//            {
//                if ((DateTime)value != DateTime.MinValue)
//                {
//                    return ((DateTime)value).ToShortDateString();
//                }
//                else
//                {
//                    return string.Empty;
//                }
//            }
//            else if (value.GetType().Equals(typeof(float)))
//            {
//                return ((float)value).ToString("0.0");
//            }
//            else if ((value.GetType().Equals(typeof(int)) || (value.GetType().Equals(typeof(ObjectRef))))
//                     && (displayValue != null))
//            {
//                //return ClientCore.Client.DictionaryCash.GetDictionaryValue(value, displayValue);
//                return "";
//            }
//            else return value.ToString();
//        }


//        private DateTime GetDateTimeValue(PropertyInfo propInfo, object obj)
//        {
//            if (propInfo == null)
//            {
//                return DateTime.Now;
//            }

//            Object value = propInfo.GetValue(obj, null);
//            if (value.GetType().Equals(typeof(DateTime)))
//            {
//                return (DateTime)value;
//            }
//            return DateTime.Now;
//        }


//    }
//}

