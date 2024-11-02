using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskEntities;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class IssueRepository : IIssueRepository
    {
        private readonly IIssuePriorityRepository _priorityRepository;
        private readonly IIssueStatusRepository _statusRepository;
        private readonly IIssueTypeRepository _typeRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMaintenanceEntityRepository _maintenanceEntityRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public IssueRepository(IIssueTypeRepository issueTypeRepository, IIssueStatusRepository issueStatusRepository, IIssuePriorityRepository issuePriorityRepository, ICompanyRepository companyRepository, IMaintenanceEntityRepository maintenanceEntityRepository, IEmployeeRepository employeeRepository) 
        {
            _priorityRepository = issuePriorityRepository;
            _statusRepository = issueStatusRepository;
            _typeRepository = issueTypeRepository;
            _companyRepository = companyRepository;
            _maintenanceEntityRepository = maintenanceEntityRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<bool> UpdateIssue(Issue? issue)
        {
            if (issue == null) return false;

            if (await CheckIssue(issue))
                return await DBUpdate.UpdateIssue(issue);
            else return false;
        }

        public async Task<bool> CreateIssue(Issue issue)
        {
            if (await CheckIssue(issue))
                return await DBInsert.InsertIssue(issue);
            else return false;
        }

        public async Task<Issue?> GetIssue(int issueId)
        {
            return await DBSelect.SelectIssue(issueId);
        }

        /*public async Task<IssueJSON?> GetIssueFromOkdesk(int issueId)
        {
            return await Request.GetIssue(issueId);
        }*/

        public async Task<ICollection<Issue>?> GetIssuesByUpdatedDate(DateTime updatedFrom, DateTime updatedTo)
        {
            return await DBSelect.SelectIssues(updatedFrom, updatedTo);
        }

        public async Task<Issue[]?> GetOpenAndCompletedOrClosedIssues(DateTime dateFrom, DateTime dateTo, int employeeId)
        {
            return (await DBSelect.SelectOpenAndCompletedIssues(dateFrom, dateTo, employeeId))?.ToArray();
        }

        public async Task<int> GetCompletedOrClosedIssues(DateTime closedOrCompletedFrom, DateTime closedOrCompletedTo, int employeeId)
        {
            int? numberOfCompletedOrClosedIssues = await DBSelect.SelectCountCompletedOrClosedIssues(closedOrCompletedFrom, closedOrCompletedTo, employeeId);
            return numberOfCompletedOrClosedIssues == null ? 0 : (int)numberOfCompletedOrClosedIssues;
        }

        public async Task<ICollection<Issue>?> GetIssuesFromAPIOkdesk(DateTime updatedSinceFrom, DateTime updatedUntilTo, int assignee_id)
        {
            var issuesFromOkdeskAPI = await Request.GetUpdatedIssues(updatedSinceFrom, updatedUntilTo, assignee_id);

            if (!await SaveOrUpdateInDB(issuesFromOkdeskAPI))
                return null;            
            
            return issuesFromOkdeskAPI;
        }

        public async Task<bool> UpdateIssuesFromDBOkdesk(DateTime dateFrom, DateTime dateTo)
        {
            ICollection<Issue>? issues = [];
            long lastIssueId = 0;
            while (true)
            {
                if (issues.Count > 0)
                    lastIssueId = issues.Last().Id;
                #if DEBUG
                await Console.Out.WriteLineAsync($"[Method: {nameof(UpdateIssuesFromDBOkdesk)}] Last issue ID: " + lastIssueId);
                #endif

                issues = await PGSelect.SelectIssues(dateFrom, dateTo, lastIssueId);

                if (issues == null || issues.Count <= 0)
                    break;    
                else await SaveOrUpdateInDB(issues);

                if (issues.Count < PGSelect.limit)
                {
                    #if DEBUG
                    await Console.Out.WriteLineAsync($"[Method: {nameof(UpdateIssuesFromDBOkdesk)} has been completed]");
                    #endif
                    break;
                }
                // Небольшая задержка для более плавной нагрузки сервера
                await Task.Delay(200);
            }
            return true;
        }        

        public async Task<bool> UpdateIssueDictionaryFromDB()
        {
            if (!await _typeRepository.GetTypesFromDBOkdesk())
                return false;
            if (!await _priorityRepository.GetPrioritiesFromDBOkdesk())
                return false;
            if (!await _statusRepository.GetStatusesFromDBOkdesk())
                return false;

            return true;
        }

        async Task<bool> CheckIssue(Issue issue)
        {
            Status? status = null;
            Priority? priority = null;
            IssueType? type = null;
            Company? company = null;
            MaintenanceEntity? maintenanceEntity = null;

            if (issue.Status != null)
                status = await _statusRepository.GetStatus(issue.Status);
            if (issue.Priority != null)
                priority = await _priorityRepository.GetPriority(issue.Priority);
            if (issue.Type != null)
                type = await _typeRepository.GetType(issue.Type);
            if (issue.Company != null)
                company = await _companyRepository.GetCompany(issue.Company.Id);
            if (issue.Service_object != null)
                maintenanceEntity = await _maintenanceEntityRepository.GetMaintenanceEntity(issue.Service_object.Id);
            // Проверка ответственного в заявке т.к. могут завести нового сотрудника, а в базу он ещё не попал и из за этого возникают ошибки
            if (issue.Assignee_id != null)
            {
                if (!await _employeeRepository.GetEmployee((int)issue.Assignee_id))
                    await _employeeRepository.UpdateEmployeesFromAPIOkdesk();
            }

            issue.Status = status;
            issue.Priority = priority;
            issue.Type = type;
            issue.Company = company;
            issue.Service_object = maintenanceEntity;

            return true;
        }

        public async Task<bool> SaveOrUpdateInDB(ICollection<Issue>? issues)
        {
            if (issues != null && issues.Count > 0)
            {
                foreach (var issue in issues)
                {
                    var tempIssue = await GetIssue(issue.Id);

                    if (tempIssue == null)
                    {
                        if (!await CreateIssue(issue))
                        {
                            WriteLog.Debug($"Не удалось создать заявку {issue.Id}");
                            return false;
                        }
                    }
                    else if (tempIssue != null)
                    {
                        if (!await UpdateIssue(issue))
                        {
                            WriteLog.Debug($"Не удалось обновить заявку {issue.Id}");
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
