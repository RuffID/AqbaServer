namespace AqbaServer.Models.OkdeskReport
{
    public class Group
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
        public string? Description { get; set; }       
        //public int[]? EmployeesId { get; set; }

        /*public Group()
        {
            EmployeesId = [];
        }*/
    }
}
