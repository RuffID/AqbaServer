namespace AqbaServer.Models.OkdeskPerformance
{
    public class GroupEmployee
    {
        public GroupEmployee()
        {
            Groups = [];
        }

        public GroupEmployee(int employeeId, int[] groupsId)
        {
            Id = employeeId;
            Groups = groupsId;
        }

        public int Id { get; set; }
        public int[] Groups { get; set; }
    }
}
