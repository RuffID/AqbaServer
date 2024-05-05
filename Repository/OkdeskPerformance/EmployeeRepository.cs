using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskPerformance
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

        public async Task<ICollection<Employee>?> GetEmployees(int employeeId)
        {
            return await DBSelect.SelectEmployees(employeeId);
        }

        public async Task<ICollection<Employee>?> GetEmployees()
        {
            return await DBSelect.SelectEmployees();
        }

        public async Task<bool> UpdateEmployeesFromAPIOkdesk()
        {
            var employees = await OkdeskEntitiesRequest.GetEmployees();

            return await SaveOrUpdateInDB(employees?.ToArray());
        }

        public async Task<bool> UpdateEmployee(int employeeId, Employee? employee)
        {
            if (employee == null) return false;
            return await DBUpdate.UpdateEmployee(employeeId, employee);
        }

        public async Task<bool> CreateEmployee(Employee? employee)
        {
            if (employee == null) return false;
            return await DBInsert.InsertEmployee(employee);
        }

        public async Task<Employee?> GetEmployee(Employee? employee)
        {
            if (employee == null) return null;
            return await DBSelect.SelectEmployee(employee.Id);
        }

        public async Task<Employee?> GetEmployee(string? email)
        {
            if (string.IsNullOrEmpty(email)) return null;

            return await DBSelect.SelectEmployee(email);
        }

        public async Task<bool> GetEmployeesFromDBOkdesk()
        {
            var employees = await PGSelect.SelectEmployees();

            return await SaveOrUpdateInDB(employees?.ToArray());
        }

        async Task<bool> SaveOrUpdateInDB(Employee[]? employees)
        {
            if (employees == null || employees.Length <= 0)
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
                            // Чтобы узнать id роли по новой обращается к БД и после создаёт связь сотрудника и роли
                            var newTempRole = await _roleRepository.GetRole(role);
                            if (newTempRole != null)
                                await _employeeRolesRepository.CreateEmployeeRole(employee.Id, newTempRole.Id);
                        }
                    }
                }
            }
            return true;
        }
    }
}

