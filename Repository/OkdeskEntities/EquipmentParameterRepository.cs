using AqbaServer.Data.MySql;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class EquipmentParameterRepository : IEquipmentParameterRepository
    {
        public async Task<bool> CreateEquipmentParameter(EquipmentParameter? equipmentParameter)
        {
            if (equipmentParameter == null) return false;

            equipmentParameter.Value = Convert.ToString(equipmentParameter.Value) ?? null;
            return await DBInsert.InsertEquipmentParameter(equipmentParameter);
        }

        public async Task<bool> DeleteEquipmentParameter(int equipmentParameterId)
        {
            return await DBDelete.DeleteEquipmentParameter(equipmentParameterId);
        }

        public async Task<ICollection<EquipmentParameter>?> GeEquipmentParameters()
        {
            return await DBSelect.SelectEquipmentParameters();
        }

        public async Task<EquipmentParameter?> GetEquipmentParameter(int equipmentId, int kindParamid)
        {
            return await DBSelect.SelectEquipmentParameter(equipmentId, kindParamid);
        }

        public async Task<bool> UpdateEquipmentParameter(int equipmentParameterId, EquipmentParameter? equipmentParameter)
        {
            if (equipmentParameter == null) return false;
            equipmentParameter.Value = Convert.ToString(equipmentParameter.Value) ?? null;
            return await DBUpdate.UpdateEquipmentParameter(equipmentParameterId, equipmentParameter);
        }
    }
}
