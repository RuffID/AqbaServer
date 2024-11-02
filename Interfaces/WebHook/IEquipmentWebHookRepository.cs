using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.WebHook
{
    public interface IEquipmentWebHookRepository
    {
        Task NewEquipment(Equipment? equipment);
        Task ChangeEquipment(Equipment? equipment);
    }
}