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
        public string? Internal_status { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Completed_at { get; set; }
        public DateTime? Deadline_at { get; set; }
        public DateTime? Delay_to { get; set; }
        public DateTime? Deleted_at { get; set; }
        public TaskType? Type { get; set; }
        public Status? Status { get; set; }
        public Priority? Priority { get; set; }
        public Company? Company { get; set; }
        public MaintenanceEntity? Service_object { get; set; }

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
