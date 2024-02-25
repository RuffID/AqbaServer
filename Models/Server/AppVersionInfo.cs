using Newtonsoft.Json;

namespace AqbaServer.Models.Server
{
    public class AppVersionInfo
    {
        [JsonProperty]
        public string? Version { get; set; }

        [JsonProperty]
        public string? Description { get; set; }
    }
}
