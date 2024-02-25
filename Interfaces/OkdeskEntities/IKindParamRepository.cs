using AqbaServer.Models;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IKindParamRepository
    {
        Task<bool> GetKindParam(int kindId, int kindParamId);
        Task<bool> CreateKindParam(int kindId, int kindParameterId);
    }
}