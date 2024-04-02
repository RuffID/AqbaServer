namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IEmployeeRolesRepository
    {
        Task<bool> GetEmployeeRole(int employeeId, int roleId);
        Task<bool> CreateEmployeeRole(int employeeId, int roleId);
    }
}