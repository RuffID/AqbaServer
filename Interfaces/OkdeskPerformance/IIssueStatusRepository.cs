using AqbaServer.Dto;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IIssueStatusRepository
    {
        Task<Status?> GetStatus(Status? status);
        Task<bool> CreateStatus(Status status);
        Task<bool> UpdateStatus(Status status);
        Task<bool> GetStatusFromOkdesk();
        Task<bool> GetStatusesFromDBOkdesk();
        Task<ICollection<Status>?> GetIssueStatuses();
    }
}