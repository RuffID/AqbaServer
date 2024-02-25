using AqbaServer.Data;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class KindParameterRepository : IKindParameterRepository
    {
        private readonly IKindParamRepository _kindParamRepository;

        public KindParameterRepository(IKindParamRepository kindParamRepository)
        {
            _kindParamRepository = kindParamRepository;
        }

        public async Task<bool> CreateKindParameter(int kindId, EquipmentParameter param)
        {
            KindParameter kindParam = new() { Code = param.Code, Name = param.Name, FieldType = param.FieldType };

            if (await DBInsert.InsertKindParameter(kindParam))
            {
                var tempKindParam = await GetKindParameter(kindParam?.Code);
                if (await _kindParamRepository.CreateKindParam(kindId, tempKindParam.Id))
                    return false;
            }

            return true;
        }

        public async Task<bool> UpdateKindParameter(int kindId, string kindParameterCode, EquipmentParameter param)
        {
            KindParameter kindParam = new() { Code = param.Code, Name = param.Name, FieldType = param.FieldType };

            if (await DBUpdate.UpdateKindParameter(kindParameterCode, kindParam))
            {
                var tempKindParam = await GetKindParameter(kindParam?.Code);
                await _kindParamRepository.CreateKindParam(kindId, tempKindParam.Id);
            }

            return true;
        }

        public async Task<bool> DeleteKindParameter(int kindParameterId)
        {
            // Сначала удаляется связь из таблицы много ко многим, а после, если связь успешно удалена - удаляется сам параметр типа
            if (await DBDelete.DeleteKindParamByKindParam(kindParameterId))
                return await DBDelete.DeleteKindParameter(kindParameterId);
            else return false;
        }

        public async Task<KindParameter> GetKindParameter(string kindParameterCode)
        {
            return await DBSelect.SelectKindParameter(kindParameterCode);
        }

        public async Task<ICollection<KindParameter>> GetKindParameters()
        {
            return await DBSelect.SelectKindParameters();
        }

        public async Task<ICollection<KindParameter>> GetKindParameters(int kindId)
        {
            return await DBSelect.SelectKindParameters(kindId);
        }
    }
}
