using ru.novolabs.SuperCore.LimsBusinessObjects;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
//using System.Windows.Forms.Design;

namespace ru.novolabs.SuperCore.Controls
{
    /*class PropertiesSelector : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            if ((context.Instance as ControlField).FieldType == "UserValue")
                return UITypeEditorEditStyle.None;
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Type t = GetBuisnessObjectType(context.Instance);
            if (t != null)
            {
                IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                PropertyInfo propInfo = context.Instance.GetType().GetProperty("PropertyName");
                Object val = propInfo.GetValue(context.Instance, null);

                String PropertyName = val == null ? "" : val.ToString();

                using (PropertyEditor form = new PropertyEditor(t))
                {
                    if (svc.ShowDialog(form) == System.Windows.Forms.DialogResult.OK)
                    {
                        value = form.Value;
                    }
                }
                return value;

            }

            return "";
        }

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
