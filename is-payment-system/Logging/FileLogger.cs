using System;
using System.IO;
 
namespace is_payment_system.Logging
{
    public class FileLogger
    {
        private readonly string _filePath;
        private readonly object _lock = new object();

        public FileLogger(string filePath = "payment_system.log")
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
 
            _filePath = filePath;

            string directory = Path.GetDirectoryName(Path.GetFullPath(_filePath));
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public void Log(string eventId, string message)
        {
            if (string.IsNullOrWhiteSpace(eventId))
                throw new ArgumentException("eventId cannot be null or empty.", nameof(eventId));
 
            string entry = FormatEntry(eventId, message);
            WriteToFile(entry);
            DbLogger.Write(eventId, message, nameof(FileLogger));
        }

        public void LogInfo(string message)
        {
            string entry = FormatEntry("INFO", message);
            WriteToFile(entry);
            DbLogger.Write("INFO", message, nameof(FileLogger));
        }

        public void LogWarning(string message)
        {
            string entry = FormatEntry("WARNING", message);
            WriteToFile(entry);
            DbLogger.Write("WARNING", message, nameof(FileLogger));
        }

        public void LogError(string message, Exception ex = null)
        {
            string fullMessage = ex != null
                ? $"{message} | Exception: {ex.GetType().Name}: {ex.Message}"
                : message;
 
            string entry = FormatEntry("ERROR", fullMessage);
            WriteToFile(entry);
            DbLogger.Write("ERROR", fullMessage, nameof(FileLogger));
        }

        public void PrintAll()
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("[FileLogger] Log file does not exist yet.");
                return;
            }
 
            Console.WriteLine($"[FileLogger] Contents of '{_filePath}':");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine(File.ReadAllText(_filePath));
            Console.WriteLine(new string('-', 60));
        }

        public void ClearLog()
        {
            if (File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, string.Empty);
                Console.WriteLine("[FileLogger] Log file cleared.");
            }
        }

        public string FilePath => Path.GetFullPath(_filePath);
 
        private string FormatEntry(string eventId, string message)
        {
            return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{eventId}] {message}";
        }
 
        private void WriteToFile(string entry)
        {
            lock (_lock)
            {
                try
                {
                    File.AppendAllText(_filePath, entry + Environment.NewLine);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"[FileLogger] Failed to write to log file: {ex.Message}");
                }
            }
        }
    }
}