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
        public int SolvedTasks { get; set; }
        public double SpentedTimeDouble { get; set; }
        public ICollection<Issue>? Issues { get; set; }

        public Employee() { Issues = []; }
        public Employee(Employee employee)
        {
            Id = employee.Id;
            Last_name = employee.Last_name;
            First_name = employee.First_name;
            Patronymic = employee.Patronymic;
            Position = employee.Position;
            Email = employee.Email;
            Login = employee.Login;
            Phone = employee.Phone;
            Comment = employee.Comment;
            Groups = employee.Groups;
            Roles = employee.Roles;
            SolvedTasks = employee.SolvedTasks;
            Groups = [];
            Roles = [];
            Issues = [];
        }
    }
}
