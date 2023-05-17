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
    /*class PowerScriptEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            Type t = WorkingFormsLayout.GetBuisnessObjectType(context.Instance);
            if (t != null)
            {
                IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                string text = "";
                var setting = ProgramContext.Settings[context.PropertyDescriptor.Name + "HelpText", false];
                if (setting != null)
                {
                    text = setting.ToString().Replace("\\r\\n", "\r\n");
                }
                using (PowerScriptForm scriptForm = new PowerScriptForm(t, value.ToString(), text))
                {
                    if (svc.ShowDialog(scriptForm) == System.Windows.Forms.DialogResult.OK)
                    {
                        value = scriptForm.Value;
                    }
                }
                return value;
            }
            else
                return "";
        }
    }*/
}
