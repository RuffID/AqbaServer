﻿using AqbaServer.Data;
using AqbaServer.API;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Dto;
using AqbaServer.Interfaces.OkdeskPerformance;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class EmployeePerformanceRepository : IEmployeePerformanceRepository
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IIssueRepository _issueRepository;
        public EmployeePerformanceRepository(IEmployeeRepository employeeRepository, IIssueRepository issueRepository) 
        {
            _employeeRepository = employeeRepository;
            _issueRepository = issueRepository;
        }

        public async Task<EmployeeDto?> GetEmployeePerformance(int employeeId, DateTime date)
        {
            return await DBSelect.SelectEmployeePerformance(employeeId, date);
        }

        public async Task<List<EmployeeDto>?> GetEmployeesPerformance(DateTime dateFrom, DateTime dateTo)
        {
            var employeesCurrentTasks = await GetOpenTasksFromDB();
            var employeesTaskAndTime = await DBSelect.SelectEmployeesPerformance(dateFrom, dateTo);

            // Объединение двух списков
            if (employeesTaskAndTime != null)
            {
                foreach (var employee in employeesTaskAndTime)
                {
                    // Поиск сотрудника в списке текущих заявок
                    var emp = employeesCurrentTasks?.FirstOrDefault(e => e.Id == employee.Id);
                    // Если был найден, то
                    if (emp != null)
                    {
                        // Добавление значений списанного времени и решённых заявок к сотруднику со списком текущих заявок
                        emp.SolvedTasks = employee.SolvedTasks;
                        emp.SpentedTime = employee.SpentedTime;
                    }
                    else
                    {
                        // Если не был найден, то добавить в коллекцию текущих заявок без списка текущих
                        employeesCurrentTasks?.Add(employee);
                    }
                }
            }

            return employeesCurrentTasks;
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
            if (employees == null) return false;
            var task0 = GetOpenTasks(employees.Where(e => e.Active).ToList());

            foreach (DateTime day in EachDay(dateFrom, dateTo))
            {              
                var task1 = GetSolvedTasks(employees, day, day);
                var task2 = GetSpentedTime(employees, day, day);

                await Task.WhenAll(task1, task2);                

                foreach (var employee in employees)
                {
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
            
            await task0;
            return true;
        }

        async Task GetOpenTasks(List<Employee> employees)
        {
            await Request.GetReportOpenTasks(employees);

            foreach (var employee in employees)
            {
                if (employee?.Issues != null)
                {
                    foreach (var issue in employee.Issues)
                    {
                        // Присвоение id ответственного в полученную с окдеска заявку т.к. при получении id является null
                        issue.Assignee_id = employee.Id;

                        Issue? issueFromDB = await _issueRepository.GetIssue(issue.Id);

                        // Если в БД уже есть такая заявка, то обновить
                        if (issueFromDB != null)
                            await _issueRepository.UpdateIssue(issue);

                        // Если нет, то создать
                        else
                            await _issueRepository.CreateIssue(issue);
                    }   
                }
            }

            // Получить все заявки из БД, кроме тех что закрыты (id статуса "closed" = 10)            

            List<int>? dbList = await _issueRepository.GetIssues(statusIdNot: 10);
            // Здесь нужно найти задачи которые есть в БД, но нет в окдеске, чтобы найти потеряшки
            if (dbList != null)
            {
                foreach (int issueId in dbList)
                {
                    // Поиск заявки в списке открытых (полученных с сайта)                    
                    if (!employees.Any(e => e?.Issues?.FirstOrDefault(i => i.Id == issueId)?.Id == issueId))
                    {
                        // Проверка заявки в БД, если точно есть, то обновить
                        Issue? issue = await _issueRepository.GetIssue(issueId);
                        if (issue != null)
                        {
                            // Если такой заявки нет на сайте в списке открытых, то присвоить статус "неизвестно"
                            issue.Internal_status = "unknown";
                            await _issueRepository.UpdateIssue(issue);
                        }
                    }
                }
            }
        }

        async Task<List<EmployeeDto>> GetOpenTasksFromDB()
        {
            List<Employee> employees = [];
            Issue[]? issues = await _issueRepository.GetIssues(unknownIssues: false);
            List<EmployeeDto> employeesDto = [];
            
            if (issues == null || issues.Length == 0) return employeesDto;
            

            // Проходит по всем найденным открытым заявкам в БД
            foreach (var issue in issues)
            {
                if (issue.Assignee_id != null)
                {
                    // Поиск сотрудника в самозаполняющемся списке
                    var employee = employees.FirstOrDefault(e => e.Id == issue.Assignee_id);
                    // Если не был найден, то
                    if (employee == null)
                    {
                        // Создание нового сотрудника и добавление в коллекцию вместе с заявкой
                        var tempEmp = new Employee() { Issues = [] };
                        tempEmp.Id = (int)issue.Assignee_id;
                        tempEmp.Issues.Add(issue);
                        employees.Add(tempEmp);
                    }
                    else
                    {
                        // Если найден, то добавление заявки к найденному сотруднику
                        employee.Issues ??= [];
                        employee.Issues.Add(issue);
                    }
                }
            }

            foreach (var employee in employees)
            {
                var tempEmp = new EmployeeDto() { Issues = [] };
                var tempIssues = new List<IssueDto>();
                if (employee.Issues != null)
                {
                    foreach (var issue in employee.Issues)
                    {
                        tempIssues.Add(new IssueDto() { PriorityId = issue?.Priority?.Id, StatusId = issue?.Status?.Id, TypeId = issue?.Type?.Id });
                    }
                    tempEmp.Id = employee.Id;
                    tempEmp.Issues = tempIssues.ToArray();
                }
                employeesDto.Add(tempEmp);
            }
            return employeesDto;
        }

        static async Task GetSolvedTasks(List<Employee> employees, DateTime dateFrom, DateTime dateTo)
        {
            await Request.GetTasks(employees, dateFrom, dateTo);
        }

        static async Task GetSpentedTime(List<Employee> employees, DateTime dateFrom, DateTime dateTo)
        {
            await Request.GetTime(employees, dateFrom, dateTo);
        }

        static IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}