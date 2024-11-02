using AqbaServer.Models.WebHook;

namespace AqbaServer.Interfaces.WebHook
{
    public interface IIssueWebHookRepository
    {
        Task UpdateIssueStatus(RootEvent @event);
        Task SaveTicket(IssueJSON ticket);
        Task NewTicket(IssueJSON ticket);
        Task MarkTicketAsDeleted(IssueJSON ticket);
        Task NewComment(RootEvent @event);
    }
}