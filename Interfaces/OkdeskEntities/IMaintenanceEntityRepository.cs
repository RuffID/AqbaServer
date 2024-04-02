using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IMaintenanceEntityRepository
    {
        Task<bool> CreateMaintenanceEntity(MaintenanceEntity maintenanceEntity);
        Task<bool> DeleteMaintenanceEntity(int maintenanceEntityId);
        Task<ICollection<MaintenanceEntity>> GetMaintenanceEntities(int maintenanceEntityId);
        Task<MaintenanceEntity> GetMaintenanceEntity(int maintenanceEntityId);
        Task<int?> GetLastMaintenanceEntitiyId();
        Task<bool> GetMaintenanceEntitiesFromOkdesk(int maintenanceEntityId = 0, int pageSize = 100);
        Task<bool> UpdateMaintenanceEntity(int maintenanceEntityId, MaintenanceEntity maintenanceEntity);
    }
}
