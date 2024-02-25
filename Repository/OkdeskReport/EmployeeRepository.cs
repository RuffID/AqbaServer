using AqbaServer.API;
using AqbaServer.Data;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Repository.OkdeskReport
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IEmployeeRolesRepository _employeeRolesRepository;

        public EmployeeRepository(IRoleRepository roleRepository, IEmployeeRolesRepository employeeRolesRepository)
        {
            _roleRepository = roleRepository;
            _employeeRolesRepository = employeeRolesRepository;
        }

        public async Task<int[]?> SelectEmployeesIdByGroup(int groupId)
        {
            return await DBSelect.SelectEmployeesByGroup(groupId);
        }

        public async Task<List<Employee>?> GetEmployees(int employeeId)
        {
            return await DBSelect.SelectEmployees(employeeId);
        }

        public async Task<List<Employee>?> GetEmployees()
        {
            return await DBSelect.SelectEmployees();
        }

        public async Task<bool> GetEmployeesFromOkdesk()
        {
            var employees = await OkdeskEntitiesRequest.GetEmployees();
            if (employees == null || employees.Count <= 0)
            {
                WriteLog.Warn("null при получении employees с окдеска");
                return false;
            }

            foreach (var employee in employees)
            {
                var tempEmp = await GetEmployee(employee);

                if (tempEmp == null)
                {
                    if (!await CreateEmployee(employee))
                        return false;
                }
                else if (tempEmp != null)
                {
                    if (!await UpdateEmployee(tempEmp.Id, employee))
                        return false;
                }

                if (employee?.Roles?.Length > 0)
                {
                    foreach (var role in employee.Roles)
                    {
                        var tempRole = await _roleRepository.GetRole(role);

                        if (tempRole != null)
                        {
                            if (await _roleRepository.UpdateRole(tempRole.Name, role))
                            {
                                if (!await _employeeRolesRepository.GetEmployeeRole(employee.Id, tempRole.Id))
                                    await _employeeRolesRepository.CreateEmployeeRole(employee.Id, tempRole.Id);
                            }
                            else return false;
                        }
                        else if (tempRole == null)
                        {
                            await _roleRepository.CreateRole(role);
                            await _employeeRolesRepository.CreateEmployeeRole(employee.Id, tempRole.Id);
                        }
                    }
                }
            }
            return true;

        }

        public async Task<bool> UpdateEmployee(int employeeId, Employee employee)
        {
            return await DBUpdate.UpdateEmployee(employeeId, employee);
        }

        public async Task<bool> CreateEmployee(Employee employee)
        {
            return await DBInsert.InsertEmployee(employee);
        }

        public async Task<Employee?> GetEmployee(Employee employee)
        {
            return await DBSelect.SelectEmployee(employee.Id);
        }

        public async Task<Employee?> GetEmployee(string email)
        {
            return await DBSelect.SelectEmployee(email);
        }

    }
}

