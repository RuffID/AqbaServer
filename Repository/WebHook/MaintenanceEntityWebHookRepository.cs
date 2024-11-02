using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Interfaces.WebHook;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.WebHook;

namespace AqbaServer.Repository.WebHook
{
    public class MaintenanceEntityWebHookRepository : IMaintenanceEntityWebHookRepository
    {
        private readonly IMaintenanceEntityRepository _maintenanceEntityRepository;

        public MaintenanceEntityWebHookRepository(IMaintenanceEntityRepository maintenanceEntityRepository) 
        {
            _maintenanceEntityRepository = maintenanceEntityRepository;
        }        
        public async Task NewMaintenanceEntity(MaintenanceEntityWebHook? service_aim)
        {
            if (service_aim == null) return;

            MaintenanceEntity maintenance = new();
            maintenance.Id = service_aim.Id;
            maintenance.Name = service_aim.Name;
            await _maintenanceEntityRepository.CreateMaintenanceEntity(maintenance);
            WriteLog.Debug($"[Method: {nameof(NewMaintenanceEntity)}] Объект обслуживания {service_aim.Id} был создан");
        }

        public async Task ChangeMaintenanceEntity(MaintenanceEntityWebHook? service_aim)
        {
            if (service_aim?.Id == null || service_aim.Id == 0) return;

            MaintenanceEntity maintenance = new();
            maintenance.Id = service_aim.Id;
            maintenance.Name = service_aim.Name;

            await _maintenanceEntityRepository.UpdateMaintenanceEntity(maintenance.Id, maintenance);
            WriteLog.Debug($"[Method: {nameof(ChangeMaintenanceEntity)}] Объект обслуживания {service_aim.Id} был обновлён");
        }
    }
}
