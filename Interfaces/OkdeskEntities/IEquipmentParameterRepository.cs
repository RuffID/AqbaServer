using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IEquipmentParameterRepository
    {
        Task<bool> CreateEquipmentParameter(EquipmentParameter? equipmentParameter);
        Task<bool> DeleteEquipmentParameter(int equipmentParameterId);
        Task<ICollection<EquipmentParameter>?> GeEquipmentParameters();
        Task<EquipmentParameter?> GetEquipmentParameter(int equipmentId, int kindParamid);
        Task<bool> UpdateEquipmentParameter(int equipmentParameterId, EquipmentParameter? equipmentParameter);
    }
}
