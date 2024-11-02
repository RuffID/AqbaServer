using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.WebHook;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IMaintenanceEntityRepository
    {
        Task<bool> CreateMaintenanceEntity(MaintenanceEntity? maintenanceEntity);
        Task<bool> DeleteMaintenanceEntity(int maintenanceEntityId);
        Task<ICollection<MaintenanceEntity>?> GetMaintenanceEntities(int maintenanceEntityId);
        Task<MaintenanceEntity?> GetMaintenanceEntity(int maintenanceEntityId);
        Task<int?> GetLastMaintenanceEntitiyId();
        Task<bool> UpdateMaintenanceEntitiesFromOkdesk(int maintenanceEntityId = 0, int pageSize = 100, int companyId = 0);
        Task<bool> UpdateMaintenanceEntitiesFromDBOkdesk();
        Task<bool> UpdateMaintenanceEntity(int maintenanceEntityId, MaintenanceEntity? maintenanceEntity);
        Task<(ICollection<MaintenanceEntity>? maintenanceEntity, ICollection<Equipment>? equipments)> GetUpdatingMaintenanceEntities(int maintenanceEntityId = 0);
        Task<ICollection<MaintenanceEntity>?> GetMaintenanceEntitiesByCompany(int companyId);
    }
}
