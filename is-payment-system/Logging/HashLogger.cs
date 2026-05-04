using System;
using System.Collections.Generic;
 
namespace is_payment_system.Logging
{
    public class HashLogger
    {
        private readonly Dictionary<string, string> _logs;
 
        public HashLogger()
        {
            _logs = new Dictionary<string, string>();
        }

        public void Log(string eventId, string message)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                throw new ArgumentException("eventId cannot be null or empty.", nameof(eventId));
 
            _logs[eventId] = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            DbLogger.Write(eventId, message, nameof(HashLogger));
        }

        public void PrintByEventId(string eventId)
        {
            if (_logs.TryGetValue(eventId, out string message))
            {
                Console.WriteLine($"[EventId: {eventId}] {message}");
            }
            else
            {
                Console.WriteLine($"[HashLogger] No log entry found for eventId: '{eventId}'");
            }
        }

        public void PrintAll()
        {
            if (_logs.Count == 0)
            {
                Console.WriteLine("[HashLogger] No log entries found.");
                return;
            }
 
            Console.WriteLine("[HashLogger] All logged messages:");
            foreach (var entry in _logs)
            {
                Console.WriteLine($"  [EventId: {entry.Key}] {entry.Value}");
            }
        }

        public bool DeleteByEventId(string eventId)
        {
            if (_logs.Remove(eventId))
            {
                Console.WriteLine($"[HashLogger] Event '{eventId}' removed.");
                return true;
            }
 
            Console.WriteLine($"[HashLogger] Event '{eventId}' not found, nothing removed.");
            return false;
        }
 
        public int Count => _logs.Count;
    }
}