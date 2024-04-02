using AqbaServer.API;
using AqbaServer.Data;
using AqbaServer.Dto;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class IssueTypeRepository : IIssueTypeRepository
    {
        public async Task<TaskType?> GetType(TaskType type)
        {
            return await DBSelect.SelectType(type);
        }

        public async Task<bool> CreateType(TaskType type)
        {
            return await DBInsert.InsertIssueType(type);
        }

        public async Task<bool> UpdateType(TaskType type)
        {
            return await DBUpdate.UpdateIssueType(type);
        }

        public async Task<bool> GetTypesFromOkdesk()
        {
            var types = await Request.GetTypes();
            if (types == null || types.Length <= 0)
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

        public async Task<ICollection<TaskType>?> GetIssueTypes()
        {
            return await DBSelect.SelectTypes();
        }
    }
}

