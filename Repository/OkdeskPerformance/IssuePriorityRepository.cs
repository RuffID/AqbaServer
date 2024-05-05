using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class IssuePriorityRepository : IIssuePriorityRepository
    {
        public async Task<Priority?> GetPriority(Priority? priority)
        {
            if (priority == null) return null;
            return await DBSelect.SelectIssuePriority(priority);
        }

        public async Task<bool> CreatePriority(Priority priority)
        {
            return await DBInsert.InsertIssuePriority(priority);
        }

        public async Task<bool> UpdatePriority(Priority priority)
        {
            return await DBUpdate.UpdateIssuePriority(priority);
        }

        public async Task<bool> GetPriorityFromOkdesk()
        {
            var priorities = await Request.GetPriorities();
            
            return await SaveOrUpdateInDB(priorities);
        }

        public async Task<bool> GetPrioritiesFromDBOkdesk()
        {
            var priorities = await PGSelect.SelectIssuePriorities();

            return await SaveOrUpdateInDB(priorities?.ToArray());
        }

        public async Task<ICollection<Priority>?> GetIssuePriorities()
        {
            return await DBSelect.SelectIssuePriorities();
        }

        async Task<bool> SaveOrUpdateInDB(Priority[]? priorities)
        {
            if (priorities == null || priorities.Length <= 0)
            {
                WriteLog.Warn("null при получении issue priorities с окдеска");
                return false;
            }

            foreach (var priority in priorities)
            {
                var tempPriority = await GetPriority(priority);

                if (tempPriority == null)
                {
                    if (!await CreatePriority(priority))
                        return false;
                }
                else if (tempPriority != null)
                {
                    if (!await UpdatePriority(priority))
                        return false;
                }

            }
            return true;
        }
    }
}

