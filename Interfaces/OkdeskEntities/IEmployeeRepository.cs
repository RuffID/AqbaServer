using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IEmployeeRepository
    {
        Task<bool> GetEmployeesFromOkdesk();
        Task<Employee?> GetEmployee(Employee employee);
        Task<int[]?> SelectEmployeesIdByGroup(int groupId);
        Task<bool> UpdateEmployee(int employeeId, Employee employee);
        Task<bool> CreateEmployee(Employee employee);
        Task<List<Employee>?> GetEmployees(int employeeId);
        Task<Employee?> GetEmployee(string email);
        Task<List<Employee>?> GetEmployees();
    }
}