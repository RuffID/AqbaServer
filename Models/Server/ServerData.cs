using Newtonsoft.Json;

namespace AqbaServer.Models.Server
{
    public class ServerData
    {
        [JsonProperty]
        public string? ServerName { get; set; }
        [JsonProperty]
        public string? ServerStartingTime { get; set; }
        [JsonProperty]
        public string? ServerUpTime { get; set; }
        [JsonProperty]
        public int Errors { get; set; }        
    }
}
