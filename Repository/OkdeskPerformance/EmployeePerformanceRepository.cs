using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Dto;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Services;
using AqbaServer.Helper;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class EmployeePerformanceRepository : IEmployeePerformanceRepository
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;
        
        public EmployeePerformanceRepository(IEmployeeRepository employeeRepository, IIssueRepository issueRepository, ITimeEntryRepository timeEntryRepository) 
        {
            _employeeRepository = employeeRepository;
            _issueRepository = issueRepository;
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task<bool> UpdatePerformanceFromOkdeskAPI(DateTime dateFrom, DateTime dateTo)
        {
            // В этом методы обновляются все заявки и списанное время с okdesk API

            ThirtyMinutesReportService.TimeOfLastUpdateRequest = DateTime.Now;

            ICollection<Issue>? issues = [];
            // Получение списка сотрудников из локальной БД
            ICollection<Employee>? employees = await _employeeRepository.GetEmployees();

            if (employees == null || employees.Count <= 0)
            {
                WriteLog.Error($"[Method: {nameof(UpdatePerformanceFromOkdeskAPI)}] Не удалось получить список сотрудников из локальной БД");
                return false;
            }

            // Парсинг всех обновлённых заявок за выбранный период, но только для неудалённых учётных записей сотрудников с окдеска
            foreach(var employee in employees.Where(e => e.Active))
                await UpdateIssuesFromOkdeskAPI(issues, dateFrom, dateTo, employee.Id);

            // Поиск удалённых заявок
            //await SearchForDeletedIssues(issues, dateFrom, dateTo);

            // Получение списанного времени
            await UpdateSpentTimeFromOkdeskAPI(issues);

            Immutable.UpdateTime = DateTime.Now;
            return true;
        }        

        public async Task<List<EmployeeDto>?> GetEmployeePerformanceFromLocalDB(DateTime dateFrom, DateTime dateTo)
        {
            ICollection<Employee>? employees = await _employeeRepository.GetEmployees();

            if (employees == null) return null;

            // Цикл проходит по всем сотрудникам и получает количество решённых заявок, список текущих заявок за период и количество списанного времени из локальной БД
            foreach (var employee in employees)
            {
                employee.SolvedIssues = await _issueRepository.GetCompletedOrClosedIssues(dateFrom, dateTo, employee.Id);
                employee.Issues = await _issueRepository.GetOpenAndCompletedOrClosedIssues(dateFrom, dateTo, employee.Id);
                employee.SpentedTime = await _timeEntryRepository.GetTimeEntryByEmployeeId(dateFrom, dateTo, employee.Id);                
            }

            return ConvertEmployeeToEmployeeDto(employees);
        }

        async Task UpdateIssuesFromOkdeskAPI(ICollection<Issue> issues, DateTime dateFrom, DateTime dateTo, int assigneeId)
        {
            // Получает все заявки, которые были изменены после dateFrom
            var tempList = await _issueRepository.GetIssuesFromAPIOkdesk(dateFrom, dateTo, assigneeId);

            // Если список пустой, то завершает метод
            if (tempList == null || tempList.Count <= 0) return;

            // Иначе добавляет новые заявки в общий список заявок
            foreach (var issue in tempList)
                issues.Add(issue);

            return;
        }

        async Task UpdateSpentTimeFromOkdeskAPI(ICollection<Issue> issues)
        {
            if (issues == null || issues.Count < 0) return;

            foreach (var issue in issues)
            {
                // Таймаут дабы не было спама запросов т.к. заявок может быть до нескольких сотен
                await Task.Delay(200);
                // Получение всех записей списания времени с окдеска по id заявки и их запись/обновление
                var timeEntriesFromOkdesk = await _timeEntryRepository.GetTimeEntriesFromOkdesk(issue.Id);

                // Получение всех записей сохранённых в локальной БД сервера
                var timeEntriesFromLocalDB = await _timeEntryRepository.GetTimeEntriesByIssueIdFromLocalDB(issue.Id);

                if (timeEntriesFromLocalDB == null || timeEntriesFromLocalDB.Count <= 0 || timeEntriesFromOkdesk == null || timeEntriesFromOkdesk?.Time_entries == null) continue;

                // Прохоидит циклом по каждой записи из БД сервера для поиска удалённых в окдеске записей списанного времени
                foreach (var entryFromLocalDB in timeEntriesFromLocalDB)
                {
                    // Если запись из локальной БД есть в записях полученных из API, то проверяется следующая запись
                    if (timeEntriesFromOkdesk.Time_entries.Any(te => te.Id == entryFromLocalDB.Id)) continue;

                    // Если запись отсутствует среди записей полученных с API окдеска, то значит она была удалена в окдеске и её нужно удалить и в локальной БД
                    await _timeEntryRepository.DeleteTimeEntryFromLocalDB(entryFromLocalDB.Id);
                }
            }
        }

        /*async Task SearchForDeletedIssues(ICollection<Issue> issuesFromOkdeskAPI, DateTime updatedSinceFrom, DateTime updatedUntilTo)
        {
            ICollection<Issue>? issuesFromLocalDB = await _issueRepository.GetIssuesByUpdatedDate(updatedSinceFrom, updatedUntilTo);

            // Здесь нужно найти задачи которые есть в БД, но нет в окдеске, чтобы найти потеряшки
            // дабы они в будущем не выводились при парсинге открытых заявок из БД
            if (issuesFromLocalDB != null && issuesFromOkdeskAPI != null)
            {
                foreach (var issue in issuesFromLocalDB)
                {
                    // Поиск заявки в списке открытых (полученных с сайта), если нет, то проверить следующую заявку
                    if (issuesFromOkdeskAPI.Any(e => e?.Id == issue.Id)) continue;

                    // Проверка, вдруг заявку передали другому инженеру и сменился assigneeId, поэтому в локальной базе она есть на данном сотруднике, а в окдеске - нет
                    var temp = await _issueRepository.GetIssueFromOkdesk(issue.Id);  // Получение конкретной заявки из API окдеска
                    if (temp != null && temp.Assignee != null)
                    {
                        // Сохранение данных полученных при обновлении заявки в окдеске
                        issue.UpdateIssue(temp.ConvertToIssue());
                    }
                    else
                    {
                        // Если такой заявки нет на сайте в списке открытых, то установить дату удаления т.к. её удалили/объединили
                        // Более точная дата установится во время парсинга задач с SQL API
                        issue.Deleted_at = DateTime.Now;
                    }

                    // После в любом случае обновить заявку в локальной БД
                    await _issueRepository.UpdateIssue(issue);
                }
            }
        }*/

        static List<EmployeeDto> ConvertEmployeeToEmployeeDto(ICollection<Employee> employees)
        {
            // Метод необходим для конвертации в сокращённый класс для передачи его в клиентское приложение
            List<EmployeeDto> employeesDto = [];
            foreach (var employee in employees)
            {
                var tempEmp = new EmployeeDto() { Issues = [] };
                var tempIssues = new List<IssueDto>();

                if (employee.Issues != null)
                {
                    // Цикл проходит по всем заявкам сотрудника и создаёт новый экземпляр класса IssueDto и добавляет в коллекцию
                    foreach (var issue in employee.Issues)
                        tempIssues.Add(new IssueDto() { PriorityId = issue?.Priority?.Id, StatusId = issue?.Status?.Id, TypeId = issue?.Type?.Id });
                                        
                    // Присвоение данных для конвертированного в employeeDto экземпляра класса
                    tempEmp.Id = employee.Id;
                    tempEmp.Issues = tempIssues.ToArray();
                    tempEmp.SpentedTime = employee.SpentedTime;
                    tempEmp.SolvedIssues = employee.SolvedIssues;
                }
                // Добавление конвертированного сотрудника (employeeDto) в общий список Dto сотрудников
                employeesDto.Add(tempEmp);
            }
            return employeesDto;
        }
    }
}
