using AqbaServer.API;
using AqbaServer.Data;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class MaintenanceEntityRepository : IMaintenanceEntityRepository
    {
        public async Task<bool> CreateMaintenanceEntity(MaintenanceEntity maintenanceEntity)
        {
            return await DBInsert.InsertMaintenanceEntity(maintenanceEntity);
        }

        public async Task<bool> DeleteMaintenanceEntity(int maintenanceEntityId)
        {
            return await DBDelete.DeleteMaintenanceEntity(maintenanceEntityId);
        }

        public async Task<ICollection<MaintenanceEntity>> GetMaintenanceEntities(int maintenanceEntityId)
        {
            return await DBSelect.SelectMaintenanceEntities(maintenanceEntityId);
        }

        public async Task<int?> GetLastMaintenanceEntitiyId()
        {
            return await DBSelect.SelectLastMaintenanceEntity();
        }

        public async Task<bool> GetMaintenanceEntitiesFromOkdesk(int maintenanceEntityId = 0)
        {
            var maintenanceEntities = await OkdeskEntitiesRequest.GetMaintenanceEntities(maintenanceEntityId);
            if (maintenanceEntities == null) return false;

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
            return true;
        }

        public async Task<MaintenanceEntity> GetMaintenanceEntity(int maintenanceEntityId)
        {
            return await DBSelect.SelectMaintenanceEntity(maintenanceEntityId);
        }

        public async Task<bool> UpdateMaintenanceEntity(int maintenanceEntityId, MaintenanceEntity maintenanceEntity)
        {
            return await DBUpdate.UpdateMaintenanceEntity(maintenanceEntityId, maintenanceEntity);
        }
    }
}
