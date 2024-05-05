using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        public async Task<bool> CreateManufacturer(Manufacturer? manufacturer)
        {
            if (manufacturer == null) return false;
            return await DBInsert.InsertManufacturer(manufacturer);
        }

        public async Task<bool> DeleteManufacturer(string manufacturerCode)
        {
            return await DBDelete.DeleteManufacturer(manufacturerCode);
        }

        public async Task<Manufacturer?> GetManufacturer(string? manufacturerCode)
        {
            if (string.IsNullOrEmpty(manufacturerCode)) return null;

            return await DBSelect.SelectManufacturer(manufacturerCode);
        }

        public async Task<ICollection<Manufacturer>?> GetManufacturers()
        {
            return await DBSelect.SelectManufacturers();
        }

        public async Task<bool> GetManufacturersFromOkdesk()
        {
            var manufacturers = await OkdeskEntitiesRequest.GetManufacturers();

            return await SaveOrUpdateInDB(manufacturers);
        }

        public async Task<bool> UpdateManufacturersFromDBOkdesk()
        {
            var manufacturers = await PGSelect.SelectManufacturers();

            return await SaveOrUpdateInDB(manufacturers);
        }

        public async Task<bool> UpdateManufacturer(string? manufacturerCode, Manufacturer? manufacturer)
        {
            if (string.IsNullOrEmpty(manufacturerCode) || manufacturer == null) return false;

            return await DBUpdate.UpdateManufacturer(manufacturerCode, manufacturer);
        }

        async Task<bool> SaveOrUpdateInDB(ICollection<Manufacturer>? manufacturers)
        {
            if (manufacturers == null || manufacturers.Count <= 0) return false;

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
    }
}
