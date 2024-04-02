using AqbaServer.Data;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class EquipmentParameterRepository : IEquipmentParameterRepository
    {
        private readonly IKindParameterRepository _kindParameterRepository;
        public EquipmentParameterRepository(IKindParameterRepository kindParameterRepository)
        {
            _kindParameterRepository = kindParameterRepository;
        }

        public async Task<bool> CreateEquipmentParameter(EquipmentParameter equipmentParameter, Equipment equipment)
        {
            if (equipmentParameter.Value == null || equipmentParameter.Value == "")
                return true;

            var kindParameter = await _kindParameterRepository.GetKindParameter(equipmentParameter.Code);
            equipmentParameter.Equipment = equipment;
            equipmentParameter.KindParam = kindParameter;

            return await DBInsert.InsertEquipmentParameter(equipmentParameter);
        }

        public async Task<bool> DeleteEquipmentParameter(int equipmentParameterId)
        {
            return await DBDelete.DeleteEquipmentParameter(equipmentParameterId);
        }

        public async Task<ICollection<EquipmentParameter>> GeEquipmentParameters()
        {
            return await DBSelect.SelectEquipmentParameters();
        }

        public async Task<EquipmentParameter> GetEquipmentParameter(int equipmentId, int kindParamid)
        {
            return await DBSelect.SelectEquipmentParameter(equipmentId, kindParamid);
        }

        public async Task<bool> UpdateEquipmentParameter(int equipmentParameterId, EquipmentParameter equipmentParameter)
        {
            if (equipmentParameter.Value == null || equipmentParameter.Value == "")
                return true;

            return await DBUpdate.UpdateEquipmentParameter(equipmentParameterId, equipmentParameter);
        }
    }
}
