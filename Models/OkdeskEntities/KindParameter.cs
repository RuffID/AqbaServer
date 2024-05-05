using Newtonsoft.Json;

namespace AqbaServer.Models.OkdeskPerformance
{
    public class KindParameter
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("field_type")]
        public string? FieldType { get; set; }

        [JsonProperty("equipment_kind_codes")]
        public string[]? Equipment_kind_codes { get; set; }
    }
}
