﻿using Newtonsoft.Json;

namespace AqbaServer.Models.OkdeskPerformance
{
    public class Manufacturer
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

    }
}
