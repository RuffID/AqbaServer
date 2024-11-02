using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface ITimeEntryRepository
    {
        Task<bool> UpdateTimeEntryFromDBOkdesk(DateTime dateFrom, DateTime dateTo);
        Task<TimeEntry?> GetTimeEntry(int timeEntryId);
        Task<ICollection<TimeEntry>?> GetTimeEntries(DateTime dateFrom, DateTime dateTo);
        Task<ICollection<TimeEntry>?> GetTimeEntriesByIssueIdFromLocalDB(int issueId);
        Task<TimeEntries?> GetTimeEntriesFromOkdesk(int issueId);
        Task<bool> CreateTimeEntry(TimeEntry? timeEntry);
        Task<bool> UpdateTimeEntry(TimeEntry? timeEntry);
        Task<bool> DeleteTimeEntryFromLocalDB(int timeEntryId);
        Task<double> GetTimeEntryByEmployeeId(DateTime dateFrom, DateTime dateTo, int employeeId);
        Task<bool> SaveOrUpdateInDB(ICollection<TimeEntry>? timeEntries, int? issueId = null);
    }
}