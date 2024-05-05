using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Dto;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class IssueStatusRepository : IIssueStatusRepository
    {
        public async Task<Status?> GetStatus(Status? status)
        {
            if (status == null) return null;
            return await DBSelect.SelectIssueStatus(status);
        }

        public async Task<bool> CreateStatus(Status status)
        {
            return await DBInsert.InsertIssueStatus(status);
        }

        public async Task<bool> UpdateStatus(Status status)
        {
            return await DBUpdate.UpdateIssueStatus(status);
        }

        public async Task<bool> GetStatusFromOkdesk()
        {
            var statuses = await Request.GetStatuses();
            
            return await SaveOrUpdateInDB(statuses);
        }

        public async Task<bool> GetStatusesFromDBOkdesk()
        {
            var statuses = await PGSelect.SelectIssueStatuses();
            return await SaveOrUpdateInDB(statuses?.ToArray());
        }

        public async Task<ICollection<Status>?> GetIssueStatuses()
        {
            return await DBSelect.SelectIssueStatuses();
        }

        async Task<bool> SaveOrUpdateInDB(Status[]? statuses)
        {
            if (statuses == null || statuses.Length <= 0)
            {
                WriteLog.Warn("null при получении issue statuses с окдеска");
                return false;
            }

            foreach (var status in statuses)
            {
                var tempStatus = await GetStatus(status);

                if (tempStatus == null)
                {
                    if (!await CreateStatus(status))
                        return false;
                }
                else if (tempStatus != null)
                {
                    if (!await UpdateStatus(status))
                        return false;
                }
            }
            return true;
        }
    }
}

