using AqbaServer.API;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Interfaces.WebHook;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.WebHook;

namespace AqbaServer.Repository.WebHook
{
    public class IssueWebHookRepository : IIssueWebHookRepository
    {
        private readonly IIssueRepository _issueRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly static string authorContactType = "contact";

        public IssueWebHookRepository(IIssueRepository issueRepository, ITimeEntryRepository timeEntryRepository) 
        {
            _issueRepository = issueRepository;
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task UpdateIssueStatus(RootEvent @event)
        {
            if (@event == null || @event.Event == null || @event.Issue == null)
                return;

            ICollection<Issue> issues = [];
            issues.Add(@event.Issue.ConvertToIssue());
            issues.First().Employees_updated_at = DateTime.Now;
            await _issueRepository.SaveOrUpdateInDB(issues);
            await _timeEntryRepository.SaveOrUpdateInDB(TimeEntryWebHook.ConvertToListTimeEntries(@event.Event.Time_entries), @event.Issue.Id);  
            WriteLog.Debug($"[Method: {nameof(UpdateIssueStatus)}] Обновлён статус для заявки {@event.Issue.Id} на {@event.Issue?.Status?.Name}");
        }

        public async Task SaveTicket(IssueJSON ticket)
        {
            if (ticket == null)
                return;

            ICollection<Issue> issues = [];
            issues.Add(ticket.ConvertToIssue());
            issues.First().Employees_updated_at = DateTime.Now;
            await _issueRepository.SaveOrUpdateInDB(issues);
            WriteLog.Debug($"[Method: {nameof(SaveTicket)}] Заявка {ticket.Id} обновлена");
        }

        public async Task MarkTicketAsDeleted(IssueJSON ticket)
        {
            if (ticket == null)
                return;

            ICollection<Issue> issues = [];
            Issue issue = ticket.ConvertToIssue();
            issue.Deleted_at = DateTime.Now;
            issues.Add(issue);
            await _issueRepository.SaveOrUpdateInDB(issues);
            WriteLog.Debug($"[Method: {nameof(MarkTicketAsDeleted)}] Заявка {issue.Id} была удалена");
        }

        public async Task NewTicket(IssueJSON ticket)
        {
            if (ticket == null)
                return;
            
            ICollection <Issue> issues = [];
            issues.Add(ticket.ConvertToIssue());
            issues.First().Employees_updated_at = DateTime.Now;
            await _issueRepository.SaveOrUpdateInDB(issues);
            
            string content = string.Empty;
            string? priority = ticket.Priority?.Code?.ToLower();

            content += GetPriority(priority);
            content += $" {ticket.Title}\n";
            content += $"{ticket.Client?.Company?.Name}\n\n";
            content += $"{Config.OkdeskDomainLink}/issues/{ticket.Id}";

            if (ticket.Author?.Type == authorContactType)
                await TelegramNotification.SendMessage(Config.TGSupportGroupChatId, content);

            WriteLog.Debug($"[Method: {nameof(SaveTicket)}] Заявка {ticket.Id} создана");
        }

        public async Task NewComment(RootEvent @event)
        {
            if (@event?.Event?.Author?.Type != "contact" || string.IsNullOrEmpty(@event?.Event?.Comment?.Content))
                return;

            WriteLog.Debug($"[Method: {nameof(NewComment)}] Добавлен комментарий в заявке {@event?.Issue?.Id}");

            DateTime evening = DateTime.Now;
            evening = new DateTime(evening.Year, evening.Month, evening.Day, hour: 18, minute: 0, second: 0);
            DateTime morning = new(evening.Year, evening.Month, evening.Day, hour: 9, minute: 0, second: 0);

            // Если не выходной день и
            // Если новый комментарий от клиента был добавлен после 09:00 утра и раньше 18:00 вечера, то уведомлять не нужно

            if (evening.DayOfWeek != DayOfWeek.Sunday || evening.DayOfWeek != DayOfWeek.Saturday)
                if (DateTime.Now > morning && DateTime.Now < evening)
                    return;

            // Не уведомлять при объединении заявок
            if (@event?.Event?.Comment?.Content.Contains("Комментарий добавлен при объединении заявок") == true)
                return;

            string contact = string.Empty;
            string content = string.Empty;

            // Записывает ФИО
            if (!string.IsNullOrEmpty(@event?.Event?.Author.Last_name))
                contact += @event?.Event?.Author.Last_name;
            if (!string.IsNullOrEmpty(@event?.Event?.Author.First_name))
                contact += " " + @event?.Event?.Author.First_name;
            if (!string.IsNullOrEmpty(@event?.Event?.Author.First_name))
                contact += " " + @event?.Event?.Author.Patronymic;

            
            string? priority = @event?.Issue?.Priority?.Code?.ToLower();

            content += GetPriority(priority);
            content += " Добавлен комментарий: " + @event?.Event?.Comment?.Content + Environment.NewLine;
            content += contact + Environment.NewLine;

            content += $"{@event?.Issue?.Client?.Company?.Name}\n\n";
            content += $"{Config.OkdeskDomainLink}/issues/{@event?.Issue?.Id}";

            await TelegramNotification.SendMessage(Config.TGSupportGroupChatId, content);
        }

        static string GetPriority(string? priority)
        {
            if (!string.IsNullOrWhiteSpace(priority))
            {
                if (priority == "low")
                    return "⚪️"; // white circle
                else if (priority == "normal")
                    return "🟢"; // green circle
                else if (priority == "high")
                    return "🔴"; // red circle
                else if (priority == "block")
                    return "🆘"; // sos sqare
                else if (priority == "project")
                    return "🟡"; // yellow circle                    
            }
            return string.Empty;
        }
    }
}
