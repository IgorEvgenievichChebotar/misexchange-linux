//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows.Forms;
//using System.Reflection;
//using System.Collections;
//using ru.novolabs.SuperCore.HemBusinessObjects;


//namespace ru.novolabs.SuperCore.Core
//{
//    public class ControlReader
//    {

//        private Control readControl;

//        public ControlReader(Control control)
//        {
//            readControl = control;
//        }

//        public void Read(Object obj)
//        {
//            ReadControls(readControl, obj);
//        }

//        private void ReadControls(Control controlHolder, Object obj)
//        {
//            foreach (Control control in controlHolder.Controls)
//            {
//                ReadControls(control, obj);
//                if (control.GetType().Equals(typeof(TextBox)))
//                {
//                    ReadTextBox((TextBox)control, obj);
//                }
//                /*else if (control is IDictionaryControl)
//                {
//                    ReadDictionaryControl((IDictionaryControl)control, obj);
//                }
//                else if (control is IParamValueControl)
//                {
//                    ReadParamValueControl((IParamValueControl)control, obj);
//                }*/
//                else if (control.GetType().Equals(typeof(CheckBox)))
//                {
//                    ReadCheckBox((CheckBox)control, obj);
//                }
//                else if (control.GetType().Equals(typeof(DateTimePicker)))
//                {
//                    ReadDateTimePicker((DateTimePicker)control, obj);
//                }
//                /*else if (control is IDataControl)
//                {
//                    ReadDataControl((IDataControl)control, obj);
//                } */
//            }
//        }

//        /*private void ReadParamValueControl(IParamValueControl control, Object obj)
//        {
//            string fieldName = control.GetPropName();
//            PropertyInfo propInfo = FindField(obj, fieldName);
//            int value = control.ValueId;
//            string str = control.Value;
//            if ((propInfo != null) && (value >= 0))
//            {
//                Object propValue = propInfo.GetValue(obj, null);
//                if (propValue.GetType().GetInterface("IList") != null)
//                {
//                    IList paramList = (IList)propValue;
//                    Type paramType = paramList.GetType().GetGenericArguments()[0];
////                    if (paramType.Equals(typeof(BaseParameterValue)))
//                    {
//                        ReadParameterValue(paramList, control.ParamId, control.ValueId, str);
//                    }
//                }
//            }
//        } */

///*        private void ReadParameterValue(IList paramList, int paramId, int valueId, string value)
//        {
//            foreach (BaseParameterValue paramValue in paramList)
//            { 
//                if (paramValue.Parameter.GetRef() == paramId)
//                {
//                    paramList.Remove(paramValue);
//                    break;
//                }
//            }
//           Type paramType = paramList.GetType().GetGenericArguments()[0];
//           Object param = Activator.CreateInstance(paramType);
//           ((BaseParameterValue)param).Parameter.SetRef(paramId);
//           ((BaseParameterValue)param).Reference.SetRef(valueId);
//           ((BaseParameterValue)param).Value = value;
//           paramList.Add(param); 
//        }


//        private void ReadDictionaryControl(IDictionaryControl control, Object obj)
//        {
//            string fieldName = control.GetPropName();
//            PropertyInfo propInfo = FindField(obj, fieldName);
//            int value = control.SelectedId;
//            if ((propInfo != null) && (value >= 0))
//            {
//                if (!TrySetInt(obj, propInfo, value.ToString()))
//                    if (!TrySetRef(obj, propInfo, value))
//                {
//                    MessageBox.Show(string.Format(Properties.Resources.Errors_ErrorReadingControl,
//                        obj.GetType().ToString(), propInfo.PropertyType.ToString(),
//                        control.GetControl().Name, "DictionaryControl"),
//                        Properties.Resources.Errors_ErrorReadingControlCaption,
//                        MessageBoxButtons.OK, 
//                        MessageBoxIcon.Exclamation);
//                }
//            }
//        } */

        

//        private bool ControlIsHolder(Control control)
//        {
//            return control.Name[0].ToString().ToUpper() == control.Name[0].ToString();
//        }



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


//        private void ReadTextBox(TextBox textBox, Object obj)
//        {
//            string fieldName = ExtractName(textBox.Name);
//            PropertyInfo propInfo = FindField(obj, fieldName);
//            string value = textBox.Text;
//            if (propInfo != null)
//            {
//                if (!TrySetString(obj, propInfo, value))
//                    if (!TrySetInt(obj, propInfo, value))
//                        if (!TrySetFloat(obj, propInfo, value))
//                            if (!TrySetDateTime(obj, propInfo, value))
//                            { 
///*                              MessageBox.Show(string.Format(Properties.Resources.Errors_ErrorReadingControl, 
//                                  obj.GetType().ToString(), propInfo.PropertyType.ToString(),
//                                  textBox.Name, "TextBox"),
//                                  Properties.Resources.Errors_ErrorReadingControlCaption,
//                                  MessageBoxButtons.OK, 
//                                  MessageBoxIcon.Exclamation); */
//                            }
//            }
//        }

//        delegate bool DataTimePickerGetCheckedCallback(DateTimePicker picker);

//        private bool DataTimePickerGetChecked(DateTimePicker picker)
//        {
//            return picker.Checked;
//        }

//        private bool DataTimePickerGetCheckBoxVisible(DateTimePicker picker)
//        {
//            return picker.ShowCheckBox;
//        }


//        private void ReadDateTimePicker(DateTimePicker dateTimePicker, Object obj)
//        {           
//           bool isChecked = false;
//           bool isCheckBoxVisible = false;

//           DataTimePickerGetCheckedCallback delegateCallback = new DataTimePickerGetCheckedCallback(DataTimePickerGetChecked);
//           isChecked = (bool)dateTimePicker.Invoke(delegateCallback, new object[] { dateTimePicker });

//           delegateCallback = new DataTimePickerGetCheckedCallback(DataTimePickerGetCheckBoxVisible);
//           isCheckBoxVisible = (bool)dateTimePicker.Invoke(delegateCallback, new object[] { dateTimePicker });

//           if ((!isChecked) && isCheckBoxVisible)
//            {
//                return;
//            }
            
//            string fieldName = ExtractName(dateTimePicker.Name);
//            PropertyInfo propInfo = FindField(obj, fieldName);
//            DateTime value = dateTimePicker.Value;
//            if (propInfo != null)
//            {
//                if (!TrySetDateTime(obj, propInfo, value))
//                {
//                 /*   MessageBox.Show(string.Format(Properties.Resources.Errors_ErrorReadingControl,
//                        obj.GetType().ToString(), propInfo.PropertyType.ToString(),
//                        dateTimePicker.Name, "DateTimePicker"),
//                        Properties.Resources.Errors_ErrorReadingControlCaption,
//                        MessageBoxButtons.OK,
//                        MessageBoxIcon.Exclamation); */
//                }
//            }
//        }

//        private void ReadCheckBox(CheckBox checkBox, Object obj)
//        {
//            string fieldName = ExtractName(checkBox.Name);
//            PropertyInfo propInfo = FindField(obj, fieldName);
//            bool value = checkBox.Checked;
//            if (propInfo != null)
//            {
//                if (!TrySetBool(obj, propInfo, value))                    
//                            {
//                          /*      MessageBox.Show(string.Format(Properties.Resources.Errors_ErrorReadingControl,
//                                    obj.GetType().ToString(), propInfo.PropertyType.ToString(),
//                                    checkBox.Name, "CheckBox"),
//                                    Properties.Resources.Errors_ErrorReadingControlCaption,
//                                    MessageBoxButtons.OK,
//                                    MessageBoxIcon.Exclamation); */
//                            }
//            }
//        }


//        private bool TrySetString(object obj, PropertyInfo propInfo, string value)
//        {
//            if (propInfo.PropertyType.Equals(typeof(string)))
//            {
//                try
//                {
//                    propInfo.SetValue(obj, value, null);
//                    return true;
//                }
//                catch { }
//            }
//            return false;
//        }

///*        private void ReadDataControl(IDataControl control, Object obj)
//        {
//            string fieldName = control.GetPropName();
//            PropertyInfo propInfo = FindField(obj, fieldName);
//            object value = control.Value;
//            if (propInfo != null)
//            {
//                if (!TrySetObject(obj, propInfo, value))
//                {
         
//                        obj.GetType().ToString(), propInfo.PropertyType.ToString(),
//                        control.GetControl().Name, "IDataControl"),
//                        Properties.Resources.Errors_ErrorReadingControlCaption,
//                        MessageBoxButtons.OK,
//                        MessageBoxIcon.Exclamation); 
//                }
//            }
//        } */

//        private bool TrySetObject(object obj, PropertyInfo propInfo, Object value)
//        {
//            if (propInfo.PropertyType.Equals(typeof(string)))
//            {
//                try
//                {
//                    propInfo.SetValue(obj, value, null);
//                    return true;
//                }
//                catch { }
//            }
//            return false;
//        }

//        private bool TrySetBool(object obj, PropertyInfo propInfo, bool value)
//        {
//            if (propInfo.PropertyType.Equals(typeof(bool)))
//            {
//                try
//                {
//                    propInfo.SetValue(obj, value, null);
//                    return true;
//                }
//                catch { }
//            }
//            return false;
//        }

//        private bool TrySetDateTime(object obj, PropertyInfo propInfo, DateTime value)
//        {
//            if (propInfo.PropertyType.Equals(typeof(DateTime)))
//            {
//                try
//                {
//                    propInfo.SetValue(obj, value, null);
//                    return true;
//                }
//                catch { }
//            }
//            return false;
//        }



//        private bool TrySetInt(object obj, PropertyInfo propInfo, string value)
//        {
//            if (propInfo.PropertyType.Equals(typeof(int)))
//            {
//                int i = 0;
//                int.TryParse(value, out i);
//                propInfo.SetValue(obj, i, null);

//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private bool TrySetRef(object obj, PropertyInfo propInfo, int value)
//        {
//            if (propInfo.PropertyType.Equals(typeof(ObjectRef)))
//            {
//                propInfo.SetValue(obj, new ObjectRef(value), null);

//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private bool TrySetFloat(object obj, PropertyInfo propInfo, string value)
//        {
//            if (propInfo.PropertyType.Equals(typeof(float)))
//            {
//                propInfo.SetValue(obj, Convert.ToDouble(value), null);

//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }


//        private bool TrySetDateTime(object obj, PropertyInfo propInfo, string value)
//        {
//            if (propInfo.PropertyType.Equals(typeof(DateTime)))
//            {
//                propInfo.SetValue(obj, Convert.ToDateTime(value), null);

//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private PropertyInfo FindField(Object obj, string fieldName)
//        {
//            foreach (PropertyInfo propInfo in obj.GetType().GetProperties())
//            {
//                if (propInfo.Name.Equals(fieldName))
//                {
//                    return propInfo;
//                }
//            }
//            return null;
//        }
//    }
//}
