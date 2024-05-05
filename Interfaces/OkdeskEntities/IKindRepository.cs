using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IKindRepository
    {
        Task<bool> CreateKind(Kind? kind);
        Task<bool> DeleteKind(int kindId);
        Task<Kind?> GetKind(string? kindCode);
        Task<ICollection<Kind>?> GetKinds();
        Task<bool> UpdateKindsFromDBOkdesk();
        Task<bool> UpdateKindsFromAPIOkdesk();
        Task<bool> UpdateKind(string? kindCode, Kind? kind);
    }
}
