namespace is_payment_system.Logging
{
    public interface ILogger
    {
        void Log(string eventId, string message);
        void PrintAll();
    }
}