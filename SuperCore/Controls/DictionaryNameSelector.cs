using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
//using System.Windows.Forms.Design;
using ru.novolabs.SuperCore.CommonBusinesObjects;
using ru.novolabs.SuperCore.DictionaryCore;

namespace ru.novolabs.SuperCore.Controls
{
    /*class DictionaryNameSelector : UITypeEditor
    {
        private IWindowsFormsEditorService edSvc;

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if ((context != null) && (provider != null))
            {
                try
                {
                    var map = File.ReadAllText(Path.Combine(Application.StartupPath, SettingsConst.Dictionary_Mapping_FileName)).Deserialize<DictionaryNamesMapping>(Encoding.UTF8);
                    var dictionaryNames = map.DictionaryList.Select(d => d.DictionaryName).ToArray();

                    var lbDictionaries = new ListBox() { ScrollAlwaysVisible = true };
                    lbDictionaries.BorderStyle = BorderStyle.None;
                    lbDictionaries.Click += new EventHandler(lb_Click);

                    lbDictionaries.Items.AddRange(dictionaryNames);
                    lbDictionaries.Height = (lbDictionaries.Items.Count + 1) * lbDictionaries.ItemHeight;
                    lbDictionaries.Height = lbDictionaries.Height < 450 ? lbDictionaries.Height : 450;

                    edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                    if (edSvc != null)
                    {
                        edSvc.DropDownControl(lbDictionaries);
                        value = lbDictionaries.SelectedItem;
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
