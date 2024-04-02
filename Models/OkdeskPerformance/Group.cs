using AqbaServer.Models.OkdeskPerformance;
using System.Text.Json.Serialization;

namespace AqbaServer.Models.OkdeskReport
{
    public class Group
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
        public string? Description { get; set; }
        [JsonIgnore]
        public ICollection<Employee>? Employees { get; set; }
        public int[]? EmployeesId { get; set; }

        public Group()
        {
            Employees = new List<Employee>();
            EmployeesId = [];
        }
    }
}
