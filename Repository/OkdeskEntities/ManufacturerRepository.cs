using AqbaServer.API;
using AqbaServer.Data;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        public async Task<bool> CreateManufacturer(Manufacturer manufacturer)
        {
            return await DBInsert.InsertManufacturer(manufacturer);
        }

        public async Task<bool> DeleteManufacturer(string manufacturerCode)
        {
            return await DBDelete.DeleteManufacturer(manufacturerCode);
        }

        public async Task<Manufacturer> GetManufacturer(string manufacturerCode)
        {
            return await DBSelect.SelectManufacturer(manufacturerCode);
        }

        public async Task<ICollection<Manufacturer>> GetManufacturers()
        {
            return await DBSelect.SelectManufacturers();
        }

        public async Task<bool> GetManufacturersFromOkdesk()
        {
            var manufacturers = await OkdeskEntitiesRequest.GetManufacturers();
            if (manufacturers == null) return false;

            foreach (var manufacturer in manufacturers)
            {
                var tempManuf = await GetManufacturer(manufacturer.Code);
                if (tempManuf == null)
                    if (!await CreateManufacturer(manufacturer))
                        return false;
                if (tempManuf != null)
                    if (!await UpdateManufacturer(tempManuf.Code, manufacturer))
                        return false;
            }
            return true;
        }

        public async Task<bool> UpdateManufacturer(string manufacturerCode, Manufacturer manufacturer)
        {
            return await DBUpdate.UpdateManufacturer(manufacturerCode, manufacturer);
        }
    }
}
