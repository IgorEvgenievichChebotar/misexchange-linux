using System;

namespace ru.novolabs.MisExchangeService
{
    internal static class ExternalPriorities
    {
        internal const Int32 Priority_Low = 0;
        internal const Int32 Priority_High = 1;
    }

    class LisRequestPriorities
    {
        internal const Int32 LIS_PRIORITY_LOW = 10;
        internal const Int32 LIS_PRIORITY_HIGH = 20;
    }

    enum RequestState : int
    {
        NotDefined = 0,
        Registration = 1, // Регистрация
        Opened = 2,       // Открыта
        Closed = 3,       // Закрыта
        Deleted = 4,      // Удалена. Фактически (по состоянию на 17.10.2016) сервер нигде не присваивает такой статус заявке
        Delayed = 5       // Просрочена. Фактически (по состоянию на 17.10.2016) сервер нигде не присваивает такой статус заявке
    }

    static class RequestStateDictionary
    {
        internal static string GetStateName(RequestState state)
        {
            switch (state)
            {
                case RequestState.Registration:
                    return "Регистрация";
                case RequestState.Opened:
                    return "Открыта";
                case RequestState.Closed:
                    return "Закрыта";
                case RequestState.Deleted:
                    return "Удалена";
                case RequestState.Delayed:
                    return "Просрочена";
                default:
                    return String.Empty;
            }
        }
    }

    static class EventTypes
    {
        internal const String ImportDictionaries = "import-dictionaries";
    }
}
