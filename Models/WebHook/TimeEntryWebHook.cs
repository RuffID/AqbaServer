using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Models.WebHook
{
    public class TimeEntryWebHook
    {
        public int Id { get; set; }
        public double Spent_time { get; set; }
        public EmployeeWebHook? Employee { get; set; }

        public static ICollection<TimeEntry>? ConvertToListTimeEntries(ICollection<TimeEntryWebHook>? timeEntryWebHooks)
        {
            if (timeEntryWebHooks == null || timeEntryWebHooks.Count <= 0)
                return null;

            ICollection<TimeEntry> timeEntries = [];
            foreach (var timeEntry in timeEntryWebHooks)
            {
                if (timeEntry.Employee != null)
                    timeEntries.Add( 
                        new TimeEntry() 
                        { 
                            Id = timeEntry.Id, 
                            Employee = new Assignee() { Id = timeEntry.Employee.Id }, 
                            Spent_Time = timeEntry.Spent_time,
                            Logged_At = DateTime.Now
                        });
            }
            return timeEntries;
        }
    }
}
