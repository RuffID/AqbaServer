using AqbaServer.Models.OkdeskReport;
using Newtonsoft.Json;

namespace AqbaServer.Models.OkdeskPerformance
{
    public class IssueJSON
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? Updated_at { get; set; }
        public DateTime? Created_at { get; set; }
        public DateTime? Completed_at { get; set; }
        public DateTime? Deadline_at { get; set; }
        public DateTime? Delayed_to { get; set; }
        public Status? Status { get; set; }
        public Priority? Priority { get; set; }
        public IssueType? Type { get; set; }

        [JsonProperty("assignee")]
        public Assignee? Assignee { get; set; }

        [JsonProperty("author")]
        public Assignee? Author { get; set; }        

        public Issue? ConvertToIssue()
        {
            Issue issue = new();
            issue.Id = this.Id;
            issue.Title = this.Title;
            issue.Employees_updated_at = this.Updated_at;
            issue.Created_at = this.Created_at;
            issue.Completed_at = this.Completed_at;
            issue.Deadline_at = this.Deadline_at;
            issue.Delay_to = this.Delayed_to;
            issue.Assignee_id = this?.Assignee?.Id;
            issue.Author_id = this?.Author?.Id;
            issue.Status = this?.Status;
            issue.Priority = this?.Priority;
            issue.Type = this?.Type;

            return issue;
        }
    }
}
