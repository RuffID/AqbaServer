using Newtonsoft.Json;

namespace AqbaServer.Models.OkdeskPerformance
{
    public class Company
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("additional_name")]
        public string? AdditionalName { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        [JsonProperty("category")]
        public Category? Category { get; set; }

        public Company()
        {
            Category = new Category();
        }
    }
}
