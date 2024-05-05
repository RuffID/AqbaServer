using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskPerformance;
using Mysqlx.Crud;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class KindParameterRepository : IKindParameterRepository
    {
        private readonly IKindRepository _kindRepository;
        private readonly IKindParamRepository _kindParamRepository;

        public KindParameterRepository(IKindRepository kindRepository, IKindParamRepository kindParamRepository)
        {
            _kindRepository = kindRepository;
            _kindParamRepository = kindParamRepository;
        }

        public async Task<bool> CreateKindParameter(KindParameter? param)
        {
            if (param == null) return false;

            return await DBInsert.InsertKindParameter(param);
        }

        public async Task<bool> UpdateKindParameter(string? kindParameterCode, KindParameter? param)
        {
            if (param == null || string.IsNullOrEmpty(kindParameterCode)) return false;
            
            return await DBUpdate.UpdateKindParameter(kindParameterCode, param);
        }

        public async Task<bool> UpdateKindParametersFromDBOkdesk()
        {
            var kindParameters = await PGSelect.SelectEquipmentParameters();

            return await SaveOrUpdateInDB(kindParameters);
        }

        public async Task<bool> UpdateKindParametersFromAPIOkdesk()
        {
            var kindParameters = await Request.GetKindParameters();

            if (!await SaveOrUpdateInDB(kindParameters)) return false;

            if (kindParameters == null) return false;

            foreach (var param in kindParameters)
            {
                // В данном цикле обновляется таблица kind param для связей между kind и kind_parameters
                if (param.Equipment_kind_codes == null || param.Equipment_kind_codes.Length <= 0) continue;

                foreach (var equipKindCode in param.Equipment_kind_codes)
                {
                    var kindParameter = await GetKindParameter(param.Code);
                    if (kindParameter == null) continue;

                    var kind = await _kindRepository.GetKind(equipKindCode);
                    // Проверка на всякий случай
                    if (kind == null) continue;

                    // Уточнение есть ли уже связь между kind и kind parameter в таблице kind_param
                    // Если связи нет, то создаёт
                    if (await _kindParamRepository.GetKindParam(kind.Id, kindParameter.Id) == false)
                        await _kindParamRepository.CreateKindParam(kind.Id, kindParameter.Id);
                }
            }
            return true;
        }

        async Task<bool> SaveOrUpdateInDB(ICollection<KindParameter>? kindParameters)
        {
            if (kindParameters == null) return false;

            foreach (var param in kindParameters)
            {
                var tempParam = await GetKindParameter(param?.Code);

                if (tempParam == null)
                {
                    if (!await CreateKindParameter(param))
                        return false;
                }
                else if (tempParam != null)
                {
                    if (!await UpdateKindParameter(tempParam?.Code, param))
                        return false;
                }
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

        public async Task<KindParameter?> GetKindParameter(string? kindParameterCode)
        {
            if (string.IsNullOrEmpty(kindParameterCode)) return null;
            return await DBSelect.SelectKindParameter(kindParameterCode);
        }        

        public async Task<ICollection<KindParameter>?> GetKindParameters()
        {
            return await DBSelect.SelectKindParameters();
        }

        public async Task<ICollection<KindParameter>?> GetKindParameters(int kindId)
        {
            return await DBSelect.SelectKindParameters(kindId);
        }
    }
}
