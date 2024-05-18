namespace AqbaServer.Dto
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public int SolvedIssues { get; set; }
        public double SpentedTime { get; set; }
        public IssueDto[]? Issues { get; set; }

        public string? Last_name { get; set; }
        public string? First_name { get; set; }
        public string? Patronymic { get; set; }
    }
}
