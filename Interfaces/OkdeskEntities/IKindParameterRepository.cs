using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IKindParameterRepository
    {
        Task<bool> CreateKindParameter(KindParameter? param);
        Task<bool> DeleteKindParameter(int id);
        Task<KindParameter?> GetKindParameter(string? kindParameterCode);
        Task<ICollection<KindParameter>?> GetKindParameters();
        Task<ICollection<KindParameter>?> GetKindParameters(int kindId);
        Task<bool> UpdateKindParameter(string? kindParameterCode, KindParameter? param);
        Task<bool> UpdateKindParametersFromDBOkdesk();
        Task<bool> UpdateKindParametersFromAPIOkdesk();
    }
}