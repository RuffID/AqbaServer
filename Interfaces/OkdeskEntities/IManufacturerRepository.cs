using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IManufacturerRepository
    {
        Task<ICollection<Manufacturer>> GetManufacturers();
        Task<bool> GetManufacturersFromOkdesk();
        Task<Manufacturer> GetManufacturer(string manufacturerCode);
        Task<bool> CreateManufacturer(Manufacturer manufacturerMap);
        Task<bool> UpdateManufacturer(string manufacturerCode, Manufacturer manufacturerMap);
        Task<bool> DeleteManufacturer(string manufacturerCode);
    }
}
