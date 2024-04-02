using AqbaServer.Data;
using AqbaServer.Interfaces.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class EmployeeRolesRepository : IEmployeeRolesRepository
    {
        public async Task<bool> CreateEmployeeRole(int employeeId, int roleId)
        {
            return await DBInsert.InsertEmployeeRole(employeeId, roleId);
        }

        public async Task<bool> GetEmployeeRole(int employeeId, int roleId)
        {
            return await DBSelect.SelectEmployeeRole(employeeId, roleId);
        }
    }
}
