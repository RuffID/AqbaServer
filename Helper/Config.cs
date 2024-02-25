using AqbaServer.Dto;
using Newtonsoft.Json;
using System.Text;

namespace AqbaServer.Helper
{
    public class Config
    {
        [JsonProperty]
        public static string OkdeskApiToken { get; set; } = string.Empty;
        [JsonProperty]
        public static string OkdeskDomainLink { get; set; } = string.Empty;
        [JsonProperty]
        public static string OkdeskLogin { get; set; } = string.Empty;
        [JsonProperty]
        public static string OkdeskPassword { get; set; } = string.Empty;
        [JsonProperty]
        public static string PartnersLogin { get; set; } = string.Empty;
        [JsonProperty]
        public static string PartnersPassword { get; set; } = string.Empty;
        [JsonProperty]
        public static string ConnectionString { get; set; } = "Server=localhost;Database=test;port=3306;UserId=root;Password=12345";
        [JsonProperty]
        public static string JwtKey { get; set; } = string.Empty;
        [JsonProperty]
        public static int TokenLifeTimeFromMinutes { get; set; } = 60;
        [JsonProperty]
        public static int RefreshTokenLifeTimeFromDays { get; set; } = 14;
        [JsonProperty]
        public static string SMTPEmail { get; set; } = string.Empty;
        [JsonProperty]
        public static string SMTPPAssword { get; set; } = string.Empty;
        [JsonIgnore]
        public static string Path { get; set; } = System.IO.Path.Combine("Config", "config.json");

        static Config() 
        {
            CreateConfig(new Config(), false);
            WriteLog.Info("Start service");
        }

        public static void LoadConfig(IConfiguration configuration)
        {
            OkdeskApiToken = configuration[nameof(OkdeskApiToken)] ?? string.Empty;
            OkdeskDomainLink = configuration[nameof(OkdeskDomainLink)] ?? string.Empty;
            OkdeskLogin = configuration[nameof(OkdeskLogin)] ?? string.Empty;
            OkdeskPassword = configuration[nameof(OkdeskPassword)] ?? string.Empty;
            PartnersLogin = configuration[nameof(PartnersLogin)] ?? string.Empty;
            PartnersPassword = configuration[nameof(PartnersPassword)] ?? string.Empty;
            ConnectionString = configuration[nameof(ConnectionString)] ?? string.Empty;
            JwtKey = configuration[nameof(JwtKey)] ?? string.Empty;
            TokenLifeTimeFromMinutes = Convert.ToInt32(configuration[nameof(TokenLifeTimeFromMinutes)]);
            RefreshTokenLifeTimeFromDays = Convert.ToInt32(configuration[nameof(RefreshTokenLifeTimeFromDays)]);
            SMTPEmail = configuration[nameof(SMTPEmail)] ?? string.Empty;
            SMTPPAssword = configuration[nameof(SMTPPAssword)] ?? string.Empty;
        }
        
        public static async void CreateConfig(Config config, bool update)
        {
            try
            {
                if (!File.Exists(Path) || update == true)
                {
                    if (!Directory.Exists("Config"))
                        Directory.CreateDirectory("Config");

                    string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                    await File.WriteAllTextAsync(Path, json, Encoding.UTF8);
                }
            }
            catch (Exception ex) { WriteLog.Error(ex.Message); }
        }

        public static async void CreateConfig(ConfigDto config, bool update)
        {
            try
            {
                if (!File.Exists(Path) || update == true)
                {
                    if (!Directory.Exists("Config"))
                        Directory.CreateDirectory("Config");

                    string json = JsonConvert.SerializeObject(config, Formatting.Indented);
                    await File.WriteAllTextAsync(Path, json, Encoding.UTF8);
                }
            }
            catch (Exception ex) { WriteLog.Error(ex.Message); }
        }

        public static ConfigDto? ReadConfig()
        {
            try
            {
                ConfigDto? readConf = null;

                if (!File.Exists(Path))
                    return null;

                string configString = File.ReadAllText(Path);
                if (!string.IsNullOrEmpty(configString))
                    readConf = JsonConvert.DeserializeObject<ConfigDto>(configString);

                if (readConf == null || (string.IsNullOrEmpty(configString) || configString == "[]"))
                    WriteLog.Error("Failed to read config");
                return readConf;

            }
            catch (Exception ex) { WriteLog.Error(ex.ToString()); return null; }
        }
    }
}
