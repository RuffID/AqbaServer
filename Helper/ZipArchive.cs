using System.IO.Compression;

namespace AqbaServer.Helper
{
    public class Zip
    {
        static readonly string logsDir = Path.Combine(Environment.CurrentDirectory, "Logs");

        public static void AddLogToZip(string fileName)
        {
            string zipFile = Path.Combine(logsDir, "logs.zip");

            if (!File.Exists(zipFile))
            {
                using (ZipFile.Open(zipFile, ZipArchiveMode.Create))
                {
                    Console.WriteLine($"[{DateTime.Now}] - Create zip file");
                    WriteLog.Info("Create zip file");
                }
            }

            try
            {
                string[] files = Directory.GetFiles(logsDir);

                foreach (string file in files)
                {
                    if (!file.Contains(fileName) && !file.Contains(".zip"))
                    {
                        using (ZipArchive zipArchive = ZipFile.Open(zipFile, ZipArchiveMode.Update))
                        {
                            zipArchive.CreateEntryFromFile(file, file.Substring(logsDir.Length + 1));
                            File.Delete(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog.Error($"AddLogToZip method error, message: {ex}");
            }
        }
    }
}

