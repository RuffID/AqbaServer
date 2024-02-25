using AqbaServer.Models.Server;
using Newtonsoft.Json;
using System.Text;

namespace AqbaServer.Helper
{
    public static class Version
    {
        static AppVersionInfo? Info { get; set; }

        static Version()
        {
            Info = new();
            LoadVersion();
        }

        public static AppVersionInfo? GetVersion()
        {
            LoadVersion();
            return Info;
        }

        public static void LoadVersion()
        {
            try
            {
                if (!File.Exists(Immutable.PATH_TO_VERSION_JSON))
                    if (!UpdateVersionFile(Info))
                        return;

                string? configString = File.ReadAllText(Immutable.PATH_TO_VERSION_JSON);
                if (!string.IsNullOrEmpty(configString))
                    Info = JsonConvert.DeserializeObject<AppVersionInfo>(configString);

                if (Info == null)
                    WriteLog.Error("Failed to load version file");
            }
            catch (Exception ex) { WriteLog.Error(ex.ToString()); }
        }

        public static bool UpdateVersionFile(AppVersionInfo? version)
        {
            if (version == null)
                return false;

            string json = JsonConvert.SerializeObject(version, Formatting.Indented);

            if (!Directory.Exists(Immutable.PATH_TO_VERSION_DIR))
                Directory.CreateDirectory(Immutable.PATH_TO_VERSION_DIR);

            File.WriteAllText(Immutable.PATH_TO_VERSION_JSON, json, Encoding.UTF8);
            return true;
        }
    }
}
