namespace AqbaServer.Models.OkdeskPerformance
{
    public class TimeEntry
    {
        public int Id { get; set; }
        public Assignee? Employee { get; set; }
        public double Spent_Time { get; set; }
        public int Issue_id { get; set; }
        public DateTime Logged_At { get; set; }        
    }    
}
