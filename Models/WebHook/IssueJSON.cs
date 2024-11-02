using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Models.WebHook
{
    public class IssueJSON
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public IssueType? Type { get; set; }
        public Priority? Priority { get; set; }
        public Status? Status { get; set; }
        public Client? Client { get; set; }
        public MaintenanceEntityWebHook? Maintenance_entity { get; set; }
        public EmployeeWebHook? Author { get; set; }
        public AssigneeWebHook? Assignee { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Deadline_at { get; set; }
        public DateTime? Completed_at { get; set; }

        public Issue ConvertToIssue()
        {
            Issue convertIssue = new();

            convertIssue.Id = Id;
            convertIssue.Title = Title;
            convertIssue.Type = Type;
            convertIssue.Priority = Priority;
            convertIssue.Status = Status;
            convertIssue.Author_id = Author?.Id;
            convertIssue.Assignee_id = Assignee?.Employee?.Id;
            convertIssue.Created_at = Created_at;
            convertIssue.Deadline_at = Deadline_at;
            convertIssue.Completed_at = Completed_at;
            convertIssue.Employees_updated_at = DateTime.Now;
            convertIssue.Company = Client?.Company;
            if (Maintenance_entity != null)
                convertIssue.Service_object = new MaintenanceEntity() { Id = Maintenance_entity.Id, Name = Maintenance_entity.Name };

            return convertIssue;
        }
    }    
}
