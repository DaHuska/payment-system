using System;
using MySqlConnector;
using is_payment_system.Service;

namespace is_payment_system.Logging
{
    public static class DbLogger
    {
        public static void Write(string eventId, string message, string loggerType)
        {
            try
            {
                using var con = DbConnection.CreateOpen();

                using var cmd = new MySqlCommand(
                    @"INSERT INTO Logs (EventId, Message, Timestamp, LoggerType)
                      VALUES (@eventId, @message, @timestamp, @loggerType);", con);

                cmd.Parameters.AddWithValue("@eventId", eventId ?? string.Empty);
                cmd.Parameters.AddWithValue("@message", message ?? string.Empty);
                cmd.Parameters.AddWithValue("@timestamp", DateTime.Now);
                cmd.Parameters.AddWithValue("@loggerType", loggerType ?? string.Empty);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbLogger] Failed to write log to database: {ex.Message}");
            }
        }
    }
}
