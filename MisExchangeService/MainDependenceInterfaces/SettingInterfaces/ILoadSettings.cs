using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ru.novolabs.MisExchange.MainDependenceInterfaces.SettingInterfaces
{
    public interface ILoadSettings
    {
        void LoadSettings(string settingsFileName);
        void SaveSettings(string settingsFileName);
    }
}
