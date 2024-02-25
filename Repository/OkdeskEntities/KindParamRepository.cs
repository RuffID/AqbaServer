using AqbaServer.Data;
using AqbaServer.Interfaces.OkdeskEntities;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class KindParamRepository : IKindParamRepository
    {
        public async Task<bool> CreateKindParam(int kindId, int kindParameterId)
        {
            // Если связи в БД нет, то создать
            if (!await GetKindParam(kindId, kindParameterId))
                return await DBInsert.InsertKindParam(kindId, kindParameterId);
            else return true;
        }

        public async Task<bool> GetKindParam(int kindId, int kindParamId)
        {
            return await DBSelect.SelectKindParam(kindId, kindParamId);
        }
    }
}
