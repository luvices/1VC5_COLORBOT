using System;
using System.IO;
using System.Text;

namespace Utils
{
    public static class Logger
    {
        private static readonly string LogPath = Path.Combine("Logs", "bot.log");
        private static readonly object LockObject = new();

        public static void Info(string message)
        {
            WriteLog("INFO", message);
        }

        public static void Warning(string message)
        {
            WriteLog("WARNING", message);
        }

        public static void Error(string message)
        {
            WriteLog("ERROR", message);
        }

        private static void WriteLog(string level, string message)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var logMessage = $"[{timestamp}] [{level}] {message}";

            lock (LockObject)
            {
                if (!Directory.Exists("Logs"))
                    Directory.CreateDirectory("Logs");

                File.AppendAllText(LogPath, logMessage + Environment.NewLine);
            }
        }
    }
}
