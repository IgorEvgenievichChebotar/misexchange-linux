using System;
namespace ru.novolabs.MisExchange.Utils
{
    public interface IHelperSettingsProvider
    {
        T GetHelperSettings<T>();
    }
}
