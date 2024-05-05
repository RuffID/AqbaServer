using AqbaServer.Dto;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Models.OkdeskPerformance
{
    public class Issue
    {
        public int Id { get; set; }
        public int? Assignee_id { get; set; }
        public int? Author_id { get; set; }
        public string? Title { get; set; }
        public DateTime? Employees_updated_at { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Completed_at { get; set; }
        public DateTime? Deadline_at { get; set; }
        public DateTime? Delay_to { get; set; }
        public DateTime? Deleted_at { get; set; }
        public IssueType? Type { get; set; }
        public Status? Status { get; set; }
        public Priority? Priority { get; set; }
        public Company? Company { get; set; }
        public MaintenanceEntity? Service_object { get; set; }

        public void UpdateIssue(Issue? entity)
        {
            if (entity == null) return;
            Id = entity.Id;
            Assignee_id = entity.Assignee_id;
            Author_id = entity.Author_id;
            Title = entity.Title;
            Employees_updated_at = entity.Employees_updated_at;
            Created_at = entity.Created_at;
            Completed_at = entity.Completed_at;
            Deadline_at = entity.Deadline_at;
            Delay_to = entity.Delay_to;
            Type = entity.Type;
            Status = entity.Status;
            Priority = entity.Priority;            
        }

        public override bool Equals(object? obj)
        {
            if (obj != null && obj is Issue issue)
            {
                return Id == issue.Id;
            }
            return false;
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
