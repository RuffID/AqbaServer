using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Models.OkdeskPerformance
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Last_name { get; set; }
        public string? First_name { get; set; }
        public string? Patronymic { get; set; }
        public string? Position { get; set; }
        public bool Active { get; set; }
        public string? Email { get; set; }
        public string? Login { get; set; }
        public string? Phone { get; set; }
        public string? Comment { get; set; }
        public Group[]? Groups { get; set; }
        public Role[]? Roles { get; set; }
        public int SolvedIssues { get; set; }
        public double SpentedTime { get; set; }
        public Issue[]? Issues { get; set; }

        public Employee() { Issues = []; }        
    }
}
