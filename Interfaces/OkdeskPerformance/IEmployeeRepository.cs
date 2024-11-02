using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IEmployeeRepository
    {
        Task<bool> UpdateEmployeesFromAPIOkdesk();
        Task<Employee?> GetEmployee(Employee? employee);
        Task<ICollection<GroupEmployee>> GetGroupEmployeeConnections();
        Task<bool> UpdateEmployee(int employeeId, Employee? employee);
        Task<bool> CreateEmployee(Employee? employee);
        Task<ICollection<Employee>?> GetEmployees(int employeeId);
        Task<Employee?> GetEmployee(string? email);
        Task<bool> GetEmployee(int employeeId);
        Task<ICollection<Employee>?> GetEmployees();
        Task<bool> GetEmployeesFromDBOkdesk();
    }
}