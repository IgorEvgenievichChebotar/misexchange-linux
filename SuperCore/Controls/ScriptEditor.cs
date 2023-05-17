using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using ru.novolabs.SuperCore.LimsDictionary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
//using System.Windows.Forms.Design;

namespace ru.novolabs.SuperCore.Controls
{
    /*class ScriptEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (value == null)
                value = "";
            Type t = GetBuisnessObjectType(context.Instance);
            Boolean filter = false;
            if (context.PropertyDescriptor.Name == "FilterCondition")
                filter = true;
            if (t != null)
            {
                IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;

                if (filter)
                {  
                    Type LeftObject;
                    PropertyInfo propInfo = context.Instance.GetType().GetProperty("PropertyName");
                    String PropertyName = propInfo.GetValue(context.Instance, null).ToString();
                    
                    if (PropertyName == null || PropertyName == "" || t.GetProperty(PropertyName) == null)
                        LeftObject = typeof(DictionaryItem);
                    else
                        LeftObject = t.GetProperty(PropertyName).PropertyType;
                    
                    using (ScriptForm scriptForm = new ScriptForm(LeftObject, t, value.ToString()))
                    {
                        if (svc.ShowDialog(scriptForm) == System.Windows.Forms.DialogResult.OK)
                        {
                            value = scriptForm.Value;
                        }
                    }
                }
                else
                {
                    string text = "";
                    var setting = ProgramContext.Settings[context.PropertyDescriptor.Name + "HelpText", false];
                    if (setting != null)
                    {
                        text = setting.ToString().Replace("\\r\\n", "\r\n");
                    }
                    using (ScriptForm scriptForm = new ScriptForm(t, value.ToString(), text))
                    {
                        if (svc.ShowDialog(scriptForm) == System.Windows.Forms.DialogResult.OK)
                        {
                            value = scriptForm.Value;
                        }
                    }
                }
                return value;
            }
            else
                return "";
            //return base.EditValue(context, provider, value);
        }


        //private Type GetDictionaryItemType(String DictionaryName)
        //{
        //    switch (DictionaryName)
        //    {
        //        case LisDictionaryNames.AccessRight:
        //            return typeof(AccessRightDictionaryItem);
        //        case LisDictionaryNames.ArchiveRackType:
        //            return typeof(ArchiveRackTypeDictionaryItem);
        //        case LisDictionaryNames.ArchiveStorage:
        //            return typeof(ArchiveStorageDictionaryItem);
        //        case LisDictionaryNames.AxisNumerationType:
        //            return typeof(AxisNumerationTypeDictionaryItem);
        //        case LisDictionaryNames.Billed:
        //            return typeof(BilledDictionaryItem);
        //        case LisDictionaryNames.Biomaterial:
        //            return typeof(BiomaterialDictionaryItem);
        //        case LisDictionaryNames.BiomaterialStateEx:
        //            return typeof(BiomaterialStateDictionaryItem);
        //        case LisDictionaryNames.CommentSource:
        //            return typeof(CommentSourceDictionaryItem);
        //        case LisDictionaryNames.CustDepartment:
        //            return typeof(CustDepartmentDictionaryItem);
        //        case LisDictionaryNames.CyclePeriod:
        //            return typeof(CyclePeriodDictionaryItem);
        //        case LisDictionaryNames.DefectType:
        //            return typeof(DefectTypeDictionaryItem);
        //    }
        //    return typeof(DictionaryItem);
        //}

        private Type GetBuisnessObjectType(Object BuisnessObject)
        {
            //switch ((ObjectType)BuisnessObject.GetType().GetProperty("BuisnessObjectType").GetValue(BuisnessObject, null))
            //{
            //    case ObjectType.Patient:
            //        return typeof(Patient);
            //    case ObjectType.Request:
            //        return typeof(CreateRequest3Request);
            //    case ObjectType.Sample:
            //        return typeof(BaseSample);
            //    case ObjectType.Hem_Product:
            //        return typeof(ru.novolabs.SuperCore.HemBusinessObjects.Product);
            //    case ObjectType.Hem_Donor:
            //        return typeof(ru.novolabs.SuperCore.HemBusinessObjects.Donor);
            //    case ObjectType.Hem_Transfusion:
            //        return typeof(ru.novolabs.SuperCore.HemBusinessObjects.Transfusion);
            //    case ObjectType.Hem_Transfusion_request:
            //        return typeof(ru.novolabs.SuperCore.HemBusinessObjects.TransfusionRequest);
            //    case ObjectType.Hem_Donor_Comparation:
            //        return typeof(ru.novolabs.SuperCore.HemBusinessObjects.DonorComparation);
            //}
            //return null;
            return WorkingFormsLayout.GetBuisnessObjectType(BuisnessObject);
        }
    }*/
}
