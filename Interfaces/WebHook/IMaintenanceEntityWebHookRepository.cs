using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.WebHook;

namespace AqbaServer.Interfaces.WebHook
{
    public interface IMaintenanceEntityWebHookRepository
    {
        Task NewMaintenanceEntity(MaintenanceEntityWebHook? service_aim);
        Task ChangeMaintenanceEntity(MaintenanceEntityWebHook? service_aim);
    }
}