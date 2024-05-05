namespace AqbaServer.Models.OkdeskPerformance
{
    public class TimeEntries
    {
        public double Spent_time_total { get; set; }
        public ICollection<TimeEntry>? Time_entries { get; set; }
    }
}
