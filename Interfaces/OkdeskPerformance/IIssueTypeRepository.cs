using AqbaServer.Dto;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IIssueTypeRepository
    {
        Task<IssueType?> GetType(IssueType? type);
        Task<bool> CreateType(IssueType type);
        Task<bool> UpdateType(IssueType type);
        Task<bool> GetTypesFromOkdesk();
        Task<bool> GetTypesFromDBOkdesk();
        Task<ICollection<IssueType>?> GetIssueTypes();
    }
}