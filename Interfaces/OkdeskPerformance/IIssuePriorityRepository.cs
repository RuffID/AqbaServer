using AqbaServer.Dto;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IIssuePriorityRepository
    {
        Task<Priority?> GetPriority(Priority? priority);
        Task<bool> CreatePriority(Priority priority);
        Task<bool> UpdatePriority(Priority priority);
        Task<bool> GetPriorityFromOkdesk();
        Task<ICollection<Priority>?> GetIssuePriorities();
    }
}