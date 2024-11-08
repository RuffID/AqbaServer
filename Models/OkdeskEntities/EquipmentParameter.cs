﻿using Newtonsoft.Json;

namespace AqbaServer.Models.OkdeskPerformance
{
    public class EquipmentParameter
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        /*[JsonProperty("field_type")]
        public string? FieldType { get; set; }*/

        [JsonProperty("value")]
        public object? Value { get; set; }

        public KindParameter? KindParam { get; set; }
        public Equipment? Equipment { get; set; }
    }
}
