using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Interfaces.WebHook;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.WebHook
{
    public class EquipmentWebHookRepository : IEquipmentWebHookRepository
    {
        private readonly IEquipmentRepository _equipmentRepository;

        public EquipmentWebHookRepository(IEquipmentRepository equipmentRepository) 
        {
            _equipmentRepository = equipmentRepository;
        }        

        public async Task NewEquipment(Equipment? equipment)
        {
            if (equipment == null) return;

            await _equipmentRepository.CreateEquipment(equipment);
            WriteLog.Debug($"[Method: {nameof(NewEquipment)}] Оборудование {equipment.Id} было создано");
        }

        public async Task ChangeEquipment(Equipment? equipment)
        {
            if (equipment == null) return;

            await _equipmentRepository.UpdateEquipment(equipment.Id, equipment);
            WriteLog.Debug($"[Method: {nameof(NewEquipment)}] Оборудование {equipment.Id} было обновлено");
        }
    }
}
