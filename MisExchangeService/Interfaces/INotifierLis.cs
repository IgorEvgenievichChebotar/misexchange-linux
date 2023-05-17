namespace ru.novolabs.MisExchange.Interfaces
{
    public interface INotifierLis
    {
        void NotifyLisServer(string requestId, string state);
    }
}
