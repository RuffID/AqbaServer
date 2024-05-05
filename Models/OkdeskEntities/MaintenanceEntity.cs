using Newtonsoft.Json;

namespace AqbaServer.Models.OkdeskPerformance
{
    public class MaintenanceEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("address")]
        public string? Address { get; set; }

        [JsonProperty("company_id")]
        public int? Company_Id { get; set; }
        [JsonProperty("active")]
        public bool? Active { get; set; }
    }
}
