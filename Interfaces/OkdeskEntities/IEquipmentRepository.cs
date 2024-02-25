using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IEquipmentRepository
    {
        Task<ICollection<Equipment>> GetEquipments(int equipmentId);
        Task<ICollection<Equipment>?> GetEquipmentsByMaintenanceEntity(int maintenanceEntityId);
        Task<ICollection<Equipment>?> GetEquipmentsByCompany(int companyId);
        Task<Equipment?> GetEquipment(int equipmentId);
        Task<int?> GetLastEquipmentId();
        Task<bool> GetEquipmentsFromOkdesk(int equipmentId = 0, int pageSize = 100);
        Task<bool> CreateEquipment(Equipment equipment);
        Task<bool> UpdateEquipment(int equipmentId, Equipment equipment);
        Task<bool> DeleteEquipment(Equipment equipment);
    }
}