using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IIssueRepository
    {
        Task<bool> UpdateIssue(Issue? issue);
        Task<bool> CreateIssue(Issue issue);
        Task<Issue?> GetIssue(int issueId);
        //Task<IssueJSON?> GetIssueFromOkdesk(int issueId);
        Task<ICollection<Issue>?> GetIssuesFromAPIOkdesk(DateTime updatedSinceFrom, DateTime updatedUntilTo, int assignee_id);
        Task<bool> UpdateIssueDictionaryFromDB();
        Task<bool> UpdateIssuesFromDBOkdesk(DateTime dateFrom, DateTime dateTo);
        Task<int> GetCompletedOrClosedIssues(DateTime closedOrCompletedFrom, DateTime closedOrCompletedTo, int employeeId);
        Task<Issue[]?> GetOpenAndCompletedOrClosedIssues(DateTime dateFrom, DateTime dateTo, int employeeId);
        Task<ICollection<Issue>?> GetIssuesByUpdatedDate(DateTime updatedFrom, DateTime updatedTo);
        Task<bool> SaveOrUpdateInDB(ICollection<Issue>? issues);
    }
}