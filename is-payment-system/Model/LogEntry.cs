using System;

namespace is_payment_system.Model
{
    public class LogEntry
    {
        private int _id;
        private string _eventId;
        private string _message;
        private DateTime _timestamp;
        private string _loggerType;

        public LogEntry()
        {
        }

        public int Id
        {
            get => _id;
            set => _id = value;
        }

        public string EventId
        {
            get => _eventId;
            set => _eventId = value;
        }

        public string Message
        {
            get => _message;
            set => _message = value;
        }

        public DateTime Timestamp
        {
            get => _timestamp;
            set => _timestamp = value;
        }

        public string LoggerType
        {
            get => _loggerType;
            set => _loggerType = value;
        }
    }
}
