using Newtonsoft.Json;

namespace AqbaServer.Models.OkdeskEntities
{
    public class Kind
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("visible")]
        public bool? Visible { get; set; }

        [JsonProperty("parameters")]
        public ICollection<KindParameter>? Parameters { get; set; }
    }
}
