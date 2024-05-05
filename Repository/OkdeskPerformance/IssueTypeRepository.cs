using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class IssueTypeRepository : IIssueTypeRepository
    {
        public async Task<IssueType?> GetType(IssueType? type)
        {
            if (type == null) return null;
            return await DBSelect.SelectType(type);
        }

        public async Task<bool> CreateType(IssueType type)
        {
            return await DBInsert.InsertIssueType(type);
        }

        public async Task<bool> UpdateType(IssueType type)
        {
            return await DBUpdate.UpdateIssueType(type);
        }

        public async Task<bool> GetTypesFromOkdesk()
        {
            var types = await Request.GetTypes();
            
            return await SaveOrUpdateInDB(types);
        }

        public async Task<bool> GetTypesFromDBOkdesk()
        {
            var types = await PGSelect.SelectIssueTypes();
            return await SaveOrUpdateInDB(types);
        }

        public async Task<ICollection<IssueType>?> GetIssueTypes()
        {
            return await DBSelect.SelectTypes();
        }

        async Task<bool> SaveOrUpdateInDB(ICollection<IssueType>? types)
        {
            if (types == null || types.Count <= 0)
            {
                WriteLog.Warn("null при получении issue types с окдеска");
                return false;
            }

            foreach (var type in types)
            {
                var tempType = await GetType(type);

                if (tempType == null)
                {
                    if (!await CreateType(type))
                        return false;
                }
                else if (tempType != null)
                {
                    if (!await UpdateType(type))
                        return false;
                }
            }
            return true;
        }
    }
}

