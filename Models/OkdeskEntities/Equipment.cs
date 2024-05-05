using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace AqbaServer.Models.OkdeskPerformance
{
    public class Equipment
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("serial_number")]
        public string? Serial_number { get; set; }

        [JsonProperty("inventory_number")]
        public string? Inventory_number { get; set; }

        [JsonProperty("parameters")]
        public List<EquipmentParameter>? Parameters { get; set; }

        [JsonProperty("equipment_kind")]
        public Kind? Equipment_kind { get; set; }

        [JsonProperty("equipment_manufacturer")]
        public Manufacturer? Equipment_manufacturer { get; set; }

        [JsonProperty("equipment_model")]
        public Model? Equipment_model { get; set; }

        [JsonProperty("company")]
        public Company? Company { get; set; }

        [JsonProperty("maintenance_entity")]
        public MaintenanceEntity? Maintenance_entity { get; set; }

        public string FullName
        {
            get
            {
                return
                    $"{Equipment_kind?.Name} " +
                    $"{Parameters?.Find(p => p.Code == "0001")?.Value} " + // Роль
                    $"{Parameters?.Find(p => p.Code == "0019")?.Value} " + // Название терминала
                    $"{Inventory_number} " +
                    $"{Equipment_model?.Name}";
            }
        }
    }
}
