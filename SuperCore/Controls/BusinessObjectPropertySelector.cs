using System;
//using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
//using System.Windows.Forms.Design;
using ru.novolabs.SuperCore.CommonBusinesObjects;

namespace ru.novolabs.SuperCore.Controls
{
    /*class BusinessObjectPropertySelector : UITypeEditor
    {
        private IWindowsFormsEditorService edSvc;

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context != null) && (provider != null))
            {
                try
                {
                    var controlRow = (ControlRow)context.Instance;
                    if (!String.IsNullOrEmpty(controlRow.BusinessObjectName))
                    {
                        var objectsDescription = File.ReadAllText(Path.Combine(Application.StartupPath, "BusinessObjectsDescription.xml")).Deserialize<BusinessObjectsDescription>(Encoding.UTF8);
                        var propNames =
                            (from obj in objectsDescription.Objects
                             where obj.Name.Equals(controlRow.BusinessObjectName)
                             select obj.PropertyNames).First().ToArray();

                        var lbProps = new ListBox() { ScrollAlwaysVisible = true };
                        lbProps.BorderStyle = BorderStyle.None;
                        lbProps.Click += new EventHandler(lb_Click);


                        lbProps.Items.AddRange(propNames);
                        lbProps.Height = (lbProps.Items.Count + 1) * lbProps.ItemHeight;
                        lbProps.Height = lbProps.Height < 450 ? lbProps.Height : 450;

                        edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                        if (edSvc != null)
                        {
                            edSvc.DropDownControl(lbProps);
                            value = lbProps.SelectedItem;
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
    }*/
}
