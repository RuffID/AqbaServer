using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class TimeEntryRepository : ITimeEntryRepository
    {
        public async Task<TimeEntry?> GetTimeEntry(int timeEntryId)
        {
            return await DBSelect.SelectTimeEntry(timeEntryId);
        }

        public async Task<TimeEntries?> GetTimeEntriesFromOkdesk(int issueId)
        {
            var timeEntry = await Request.GetTimeEntries(issueId);
            await SaveOrUpdateInDB(timeEntry?.Time_entries, issueId);
            return timeEntry;
        }

        public async Task<ICollection<TimeEntry>?> GetTimeEntries(DateTime dateFrom, DateTime dateTo)
        {
            return await DBSelect.SelectTimeEntries(dateFrom, dateTo);
        }

        public async Task<double> GetTimeEntryByEmployeeId(DateTime dateFrom, DateTime dateTo, int employeeId)
        {
            double? timeEntry = await DBSelect.SelectTimeEntry(dateFrom, dateTo, employeeId);
            return timeEntry == null ? 0 : (double)timeEntry;
        }

        public async Task<ICollection<TimeEntry>?> GetTimeEntriesByIssueIdFromLocalDB(int issueId)
        {
            return await DBSelect.SelectTimeEntriesByIssueId(issueId);
        }

        public async Task<bool> CreateTimeEntry(TimeEntry? timeEntry)
        {
            if (timeEntry == null) return false;

            return await DBInsert.InsertTimeEntry(timeEntry);
        }
        
        public async Task<bool> UpdateTimeEntry(TimeEntry? timeEntry)
        {
            if (timeEntry == null) return false;

            return await DBUpdate.UpdateTimeEntry(timeEntry);
        }

        public async Task<bool> DeleteTimeEntryFromLocalDB(int issueId)
        {
            return await DBDelete.DeleteTimeEntry(issueId);
        }

        public async Task<bool> UpdateTimeEntryFromDBOkdesk(DateTime dateFrom, DateTime dateTo)
        {
            ICollection<TimeEntry>? timeEntries = [];
            long lastTimeEntryId = 0;
            while (true)
            {
                if (timeEntries.Count > 0)
                    lastTimeEntryId = timeEntries.Last().Id;
                #if DEBUG
                await Console.Out.WriteLineAsync($"[Method: {nameof(UpdateTimeEntryFromDBOkdesk)}] Last time entry ID: " + lastTimeEntryId);
                #endif

                timeEntries = await PGSelect.SelectTimeEntries(dateFrom, dateTo, lastTimeEntryId);

                if (timeEntries == null || timeEntries.Count <= 0)
                    break;
                else await SaveOrUpdateInDB(timeEntries);

                if (timeEntries.Count < PGSelect.limit)
                {
                    #if DEBUG
                    await Console.Out.WriteLineAsync($"[Method: {nameof(UpdateTimeEntryFromDBOkdesk)} has been completed]");
                    #endif
                    break;
                }
            }
            return true;
        }

        public async Task<bool> SaveOrUpdateInDB(ICollection<TimeEntry>? timeEntries, int? issueId = null)
        {
            if (timeEntries == null)
                return false;

            // Это нужно т.к. при парсинге трудозатрат с API отсутствует issueId в выгрузке
            if (issueId != null && issueId > 0)
                foreach (var item in timeEntries)
                    item.Issue_id = (int)issueId;

            foreach (var timeEntry in timeEntries)
            {
                var tempEntry = await GetTimeEntry(timeEntry.Id);

                if (tempEntry == null)
                {
                    if (!await CreateTimeEntry(timeEntry))
                        continue;
                }
                else if (tempEntry != null)
                {
                    if (!await UpdateTimeEntry(timeEntry))
                        continue;
                }
            }
            return true;
        }

        
    }
}
