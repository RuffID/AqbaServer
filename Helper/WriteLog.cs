using System.Collections.Concurrent;
using System.Diagnostics;

namespace AqbaServer.Helper
{
    public class WriteLog
    {
        static readonly string logsDir = Path.Combine(Environment.CurrentDirectory, "Logs");
        private static readonly ConcurrentQueue<string> queue = new();
        private static readonly bool[] isProcessing = [false, false];
        private static StreamWriter sw;
        private static readonly object lockObj = new();
        private static bool updateLogFile = false;

        static WriteLog()
        {
            if (!Directory.Exists(logsDir))
                Directory.CreateDirectory(logsDir);

            sw = new(Path.Combine(logsDir, GetFileName(DateTime.Now)), true);
        }

        public static void Info(string data)
        {
            AddToQueue(data, " INFO");
        }

        public static void Warn(string data)
        {
            AddToQueue(data, " WARN");
        }

        public static void Error(string data)
        {
            Immutable.Errors++;
            AddToQueue(data, " ERROR");
        }

        public static void CheckLogFiles()
        {
            if (!Directory.Exists(logsDir))
                Directory.CreateDirectory(logsDir);

            string logsFileName = GetFileName(DateTime.Now);
            string logFilePath = Path.Combine(logsDir, logsFileName);

            if (!File.Exists(logFilePath))
            {
                updateLogFile = true;
                lock (lockObj)
                {
                    sw.Close();
                    sw.Dispose();
                    sw = new StreamWriter(logFilePath, true);
                }
                updateLogFile = false;

                string? emptyString = null;
                AddToQueue(emptyString, "EMPTY");
            }

            Zip.AddLogToZip(logsFileName);
        }

        private static void AddToQueue(string? data, string type)
        {
            if (data != null)
            {
                string logItemData = $"[{DateTime.Now:dd.MM.yyyy HH:mm:ss}] {type} - {data}";
                queue.Enqueue(logItemData);
            }

            if (!isProcessing[0])
                Task.Run(() => Write(ref isProcessing[0]));
            else if (!isProcessing[1])
                Task.Run(() => Write(ref isProcessing[1]));
        }

        private static string GetFileName(DateTime date)
        {
            return $"{date:dd.MM.yyyy}_log.txt";
        }

        private static void Write(ref bool processing)
        {
            processing = true;

            lock (lockObj)
            {
                while (!updateLogFile && queue.TryDequeue(out string? data) && data != null)
                {
                    try
                    {
                        sw.WriteLine(data);
                        sw.Flush();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }

            processing = false;
        }

        public static string GetMethodName()
        {
            return " (Method name: " + new StackTrace(1)?.GetFrame(0)?.GetMethod()?.Name + ")";
        }
    }
}
