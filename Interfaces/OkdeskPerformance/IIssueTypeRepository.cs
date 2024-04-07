using AqbaServer.Dto;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IIssueTypeRepository
    {
        Task<TaskType?> GetType(TaskType? type);
        Task<bool> CreateType(TaskType type);
        Task<bool> UpdateType(TaskType type);
        Task<bool> GetTypesFromOkdesk();
        Task<ICollection<TaskType>?> GetIssueTypes();
    }
}