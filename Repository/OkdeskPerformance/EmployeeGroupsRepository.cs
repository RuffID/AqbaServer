using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Interfaces.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class EmployeeGroupsRepository : IEmployeeGroupsRepository
    {
        public async Task<bool> CreateEmployeeGroup(int id, int employeeId, int groupId)
        {
            return await DBInsert.InsertEmployeeGroup(id, employeeId, groupId);
        }

        public async Task<bool> GetEmployeeGroup(int id)
        {
            return await DBSelect.SelectEmployeeGroup(id);
        }

        public async Task<bool> UpdateEmployeeGroupsFromDBOkdesk()
        {
            var employeeGroups = await PGSelect.SelectEmployeeGroupsConnections();

            if (employeeGroups == null || employeeGroups.Count <= 0) return false;

            foreach (var group in employeeGroups)
            {
                // Если связи нет, то создать
                if (await GetEmployeeGroup(group.id) == false)
                    await CreateEmployeeGroup(group.id, group.employeeId, group.groupId);
            }

            return true;
        }
    }
}
