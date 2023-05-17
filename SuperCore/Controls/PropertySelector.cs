using System;
using System.Collections.Generic;
//using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
//using System.Windows.Forms.Design;
using ru.novolabs.SuperCore.CommonBusinesObjects;
using ru.novolabs.SuperCore.DictionaryCore;
using ru.novolabs.SuperCore.LimsBusinessObjects;
using System.Reflection;
using ru.novolabs.SuperCore.HemBusinessObjects;

namespace ru.novolabs.SuperCore.Controls
{
    /*class PropertySelector : UITypeEditor
    {
        private IWindowsFormsEditorService edSvc;

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context != null) && (provider != null))
            {
                try
                {

                    Type t = GetBuisnessObjectType(context.Instance);
                    if (t != null)
                    {
                        List<PropertyInfo> Properties = t.GetProperties().ToList();
                        var lbProperties = new ListBox() { ScrollAlwaysVisible = true };
                        lbProperties.BorderStyle = BorderStyle.None;
                        lbProperties.Click += new EventHandler(lb_Click);
                        Properties.Sort((x, y) => string.Compare(x.Name, y.Name));
                        foreach (PropertyInfo property in Properties)
                            lbProperties.Items.Add(property.Name);
                        edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                        if (edSvc != null)
                        {
                            edSvc.DropDownControl(lbProperties);
                            value = lbProperties.SelectedItem;
                        }
                    }
                }
                catch (Exception)
                { }
            }
            return base.EditValue(context, provider, value);
        }

        void lb_Click(object sender, EventArgs e)
        {
            ((ListBox)sender).Click -= new EventHandler(lb_Click);
            edSvc.CloseDropDown();
            
        }

        

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
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
            //    case ObjectType.Hem_Donor:
            //        return typeof(Donor);
            //    case ObjectType.Hem_Product:
            //        return typeof(Product);
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
