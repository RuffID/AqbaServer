using AqbaServer.Data.MySql;
using AqbaServer.Interfaces.OkdeskEntities;

namespace AqbaServer.Repository.OkdeskEntities
{
    public class KindParamRepository : IKindParamRepository
    {
        public async Task<bool> CreateKindParam(int kindId, int kindParameterId)
        {
            return await DBInsert.InsertKindParam(kindId, kindParameterId);
        }

        public async Task<bool> GetKindParam(int kindId, int kindParameterId)
        {
            return await DBSelect.SelectKindParam(kindId, kindParameterId);
        }
    }
}
