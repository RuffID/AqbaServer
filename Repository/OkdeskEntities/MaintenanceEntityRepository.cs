using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class MaintenanceEntityRepository : IMaintenanceEntityRepository
    {
        private readonly IEquipmentRepository _equipmentRepository;

        public MaintenanceEntityRepository(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        public async Task<MaintenanceEntity?> GetMaintenanceEntity(int maintenanceEntityId)
        {
            return await DBSelect.SelectMaintenanceEntity(maintenanceEntityId);
        }

        public async Task<bool> CreateMaintenanceEntity(MaintenanceEntity? maintenanceEntity)
        {
            if (maintenanceEntity == null) return false;
            return await DBInsert.InsertMaintenanceEntity(maintenanceEntity);
        }

        public async Task<bool> DeleteMaintenanceEntity(int maintenanceEntityId)
        {
            return await DBDelete.DeleteMaintenanceEntity(maintenanceEntityId);
        }

        public async Task<ICollection<MaintenanceEntity>?> GetMaintenanceEntities(int maintenanceEntityId)
        {
            return await DBSelect.SelectMaintenanceEntities(maintenanceEntityId);
        }

        public async Task<int?> GetLastMaintenanceEntitiyId()
        {
            return await DBSelect.SelectLastMaintenanceEntity();
        }

        public async Task<bool> UpdateMaintenanceEntitiesFromOkdesk(int maintenanceEntityId = 0, int pageSize = 100, int companyId = 0)
        {
            var maintenanceEntities = await OkdeskEntitiesRequest.GetMaintenanceEntities(pageSize, maintenanceEntityId, companyId);
            return await SaveOrUpdateInDB(maintenanceEntities);
        }

        public async Task<(ICollection<MaintenanceEntity>? maintenanceEntity, ICollection<Equipment>? equipments)> GetUpdatingMaintenanceEntities(int maintenanceEntityId = 0)
        {
            ICollection<MaintenanceEntity>? maintenanceEntities = [];
            if (!await UpdateMaintenanceEntitiesFromOkdesk(maintenanceEntityId: maintenanceEntityId, pageSize: 1)) return (null, null);

            var maintenanceEntity = await GetMaintenanceEntity(maintenanceEntityId);

            if (maintenanceEntity == null) return (null, null);
            maintenanceEntities.Add(maintenanceEntity);

            if (!await _equipmentRepository.UpdateEquipmentsFromAPIOkdesk(maintenanceEntityId: maintenanceEntity.Id)) return (null, null);

            var equipments = await _equipmentRepository.GetEquipmentsByMaintenanceEntity(maintenanceEntity.Id);

            return (maintenanceEntities, equipments);
        }

        public async Task<ICollection<MaintenanceEntity>?> GetMaintenanceEntitiesByCompany(int companyId)
        {
            return await DBSelect.SelectMaintenanceEntitiesByCompany(companyId);
        }

        public async Task<bool> UpdateMaintenanceEntitiesFromDBOkdesk()
        {
            var maintenanceEntities = await PGSelect.SelectMaintenanceEntities();
            return await SaveOrUpdateInDB(maintenanceEntities);
        }        

        public async Task<bool> UpdateMaintenanceEntity(int maintenanceEntityId, MaintenanceEntity? maintenanceEntity)
        {
            if (maintenanceEntity == null) return false;
            return await DBUpdate.UpdateMaintenanceEntity(maintenanceEntityId, maintenanceEntity);
        }

        async Task<bool> SaveOrUpdateInDB(ICollection<MaintenanceEntity>? maintenanceEntities)
        {
            if (maintenanceEntities != null && maintenanceEntities.Count > 0)
            {
                foreach (var maintenanceEntity in maintenanceEntities)
                {
                    var tempMaintenance = await GetMaintenanceEntity(maintenanceEntity.Id);
                    if (tempMaintenance == null)
                        if (!await CreateMaintenanceEntity(maintenanceEntity))
                            return false;
                    if (tempMaintenance != null)
                        if (!await UpdateMaintenanceEntity(tempMaintenance.Id, maintenanceEntity))
                            return false;
                }
            }
            return true;
        }
    }
}
