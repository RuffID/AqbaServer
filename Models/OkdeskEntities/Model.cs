using Newtonsoft.Json;

namespace AqbaServer.Models.OkdeskPerformance
{
    public class Model
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

        [JsonProperty("equipment_kind")]
        public Kind? EquipmentKind { get; set; }

        [JsonProperty("equipment_manufacturer")]
        public Manufacturer? EquipmentManufacturer { get; set; }
    }
}
