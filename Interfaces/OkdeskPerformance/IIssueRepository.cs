using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IIssueRepository
    {
        Task<bool> UpdateIssue(Issue issue);
        Task<bool> CreateIssue(Issue issue);
        Task<Issue?> GetIssue(int issueId);
        Task<List<int>?> GetIssues(int statusIdNot);
        Task<Issue[]?> GetIssues(bool unknownIssues = false);
        Task<bool> UpdateIssueDictionary();
    }
}