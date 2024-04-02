using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Dto
{
    public class EquipmentDto
    {
        public int Id { get; set; }

        public string? Serial_number { get; set; }

        public string? Inventory_number { get; set; }

        public List<EquipmentParameter>? Parameters { get; set; }

        public Kind? Equipment_kind { get; set; }

        public Manufacturer? Equipment_manufacturer { get; set; }

        public Model? Equipment_model { get; set; }
    }
}
