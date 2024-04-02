using AqbaServer.API;
using AqbaServer.Data;
using AqbaServer.Dto;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class IssueStatusRepository : IIssueStatusRepository
    {
        public async Task<Status?> GetStatus(Status status)
        {
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

        public async Task<ICollection<Status>?> GetIssueStatuses()
        {
            return await DBSelect.SelectIssueStatuses();
        }
    }
}

