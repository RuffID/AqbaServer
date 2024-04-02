namespace AqbaServer.Dto
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public int SolvedTasks { get; set; }
        public double SpentedTime { get; set; }
        public IssueDto[]? Issues { get; set; }
    }
}
