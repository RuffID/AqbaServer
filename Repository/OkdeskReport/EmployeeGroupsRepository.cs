using AqbaServer.Data;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Repository.OkdeskReport
{
    public class EmployeeGroupsRepository : IEmployeeGroupsRepository
    {
        public async Task<bool> CreateEmployeeGroup(int employeeId, int groupId)
        {
            return await DBInsert.InsertEmployeeGroup(employeeId, groupId);
        }

        public async Task<bool> GetEmployeeGroup(int employeeId, int groupId)
        {
            return await DBSelect.SelectEmployeeGroup(employeeId, groupId);
        }        
    }
}
