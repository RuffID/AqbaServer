using AqbaServer.Data;
using AqbaServer.API;
using AqbaServer.Models.OkdeskEntities;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Dto;

namespace AqbaServer.Repository.OkdeskReport
{
    public class EmployeePerformanceRepository : IEmployeePerformanceRepository
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeePerformanceRepository(IEmployeeRepository employeeRepository) 
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<EmployeeDto?> GetEmployeePerformance(int employeeId, DateTime date)
        {
            return await DBSelect.SelectEmployeePerformance(employeeId, date);
        }

        public async Task<List<EmployeeDto>?> GetEmployeesPerformance(DateTime dateFrom, DateTime dateTo)
        {
            return await DBSelect.SelectEmployeesPerformance(dateFrom, dateTo);
        }

        public async Task<bool> CreateEmployeePerformance(Employee employee, DateTime day)
        {
            return await DBInsert.InsertEmployeePerformance(employee, day);
        }

        public async Task<bool> UpdateEmployeePerformance(Employee employee, DateTime day)
        {
            return await DBUpdate.UpdateEmployeePerformance(employee, day);
        }

        public async Task<bool> GetEmployeePerformanceFromOkdesk(DateTime dateFrom, DateTime dateTo)
        {
            List<Employee>? employees = await _employeeRepository.GetEmployees();

            foreach (DateTime day in EachDay(dateFrom, dateTo))
            {
                if (employees == null || employees?.Count < 0) return false;

                var task1 = GetSolvedTasks(employees, day, day);
                var task2 = GetSpentedTime(employees, day, day);

                await Task.WhenAll(task1, task2);

                if (employees == null) return false;

                foreach (var employee in employees)
                {
                    if (employee == null) continue;
                    if (employee.SolvedTasks <= 0 && employee.SpentedTimeDouble <= 0) continue;

                    var tempEmployee = await GetEmployeePerformance(employee.Id, day);

                    if (tempEmployee != null)
                    {
                        if (!await UpdateEmployeePerformance(employee, day))
                            return false;
                    }
                    else
                    {
                        if (!await CreateEmployeePerformance(employee, day))
                            return false;
                    }

                }
            }
            return true;
        }

        static async Task GetSolvedTasks(List<Employee>? employees, DateTime dateFrom, DateTime dateTo)
        {
            if (employees != null && employees?.Count > 0)
                await Request.GetTasks(employees, dateFrom, dateTo);
        }

        static async Task GetSpentedTime(List<Employee>? employees, DateTime dateFrom, DateTime dateTo)
        {
            if (employees != null && employees?.Count > 0)
                await Request.GetTime(employees, dateFrom, dateTo);
        }

        static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}
