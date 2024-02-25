using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IEquipmentParameterRepository
    {
        Task<bool> CreateEquipmentParameter(EquipmentParameter equipmentParameter, Equipment equipment);
        Task<bool> DeleteEquipmentParameter(int equipmentParameterId);
        Task<ICollection<EquipmentParameter>> GeEquipmentParameters();
        Task<EquipmentParameter> GetEquipmentParameter(int equipmentId, int kindParamid);
        Task<bool> UpdateEquipmentParameter(int equipmentParameterId, EquipmentParameter equipmentParameter);
    }
}
