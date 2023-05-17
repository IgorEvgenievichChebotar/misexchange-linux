using System;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
//using System.Windows.Forms.Design;
using ru.novolabs.SuperCore.CommonBusinesObjects;

namespace ru.novolabs.SuperCore.Controls
{
    /*class BusinessObjectSelector : UITypeEditor
    {
        private IWindowsFormsEditorService edSvc;

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context != null) && (provider != null))
            {
                try
                {
                    var objectsDescription = File.ReadAllText(Path.Combine(Application.StartupPath, "BusinessObjectsDescription.xml")).Deserialize<BusinessObjectsDescription>(Encoding.UTF8);
                    var objectNames = objectsDescription.Objects.Select(o => o.Name).ToArray();                    
                    
                    var lbObjects = new ListBox() { ScrollAlwaysVisible = true };
                    lbObjects.BorderStyle = BorderStyle.None;
                    lbObjects.Click += new EventHandler(lb_Click);

                    lbObjects.Items.AddRange(objectNames);
                    lbObjects.Height = (lbObjects.Items.Count + 1) * lbObjects.ItemHeight;
                    lbObjects.Height = lbObjects.Height < 450 ? lbObjects.Height : 450;

                    edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                    if (edSvc != null)
                    {
                        edSvc.DropDownControl(lbObjects);
                        value = lbObjects.SelectedItem;
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
    }*/
}
