using AqbaServer.Data;
using AqbaServer.Dto;
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

        public IssueRepository(IIssueTypeRepository issueTypeRepository, IIssueStatusRepository issueStatusRepository, IIssuePriorityRepository issuePriorityRepository, ICompanyRepository companyRepository, IMaintenanceEntityRepository maintenanceEntityRepository) 
        {
            _priorityRepository = issuePriorityRepository;
            _statusRepository = issueStatusRepository;
            _typeRepository = issueTypeRepository;
            _companyRepository = companyRepository;
            _maintenanceEntityRepository = maintenanceEntityRepository;
        }

        public async Task<bool> UpdateIssue(Issue issue)
        {
            await CheckIssue(issue);

            return await DBUpdate.UpdateIssue(issue);
        }

        public async Task<bool> CreateIssue(Issue issue)
        {
            await CheckIssue(issue);

            return await DBInsert.InsertIssue(issue);
        }

        public async Task<Issue?> GetIssue(int issueId)
        {
            Issue? issue = await DBSelect.SelectIssue(issueId);

            if (issue == null) return null;

            await CheckIssue(issue);

            return issue;
        }

        public async Task<Issue[]?> GetNotClosedIssues(bool unknownIssues = false)
        {
            Status? closedStatus = await _statusRepository.GetStatus(new Status() { Code = "closed" });
            if (closedStatus == null)
            {
                WriteLog.Error("[Method GetNotClosedIssues] Не удалось получить \"закрытый\" статус");
                return null;
            }
            return (await DBSelect.SelectIssues(closedStatus, unknownIssues))?.ToArray();
        }

        public async Task<bool> UpdateIssueDictionary()
        {
            if (!await _typeRepository.GetTypesFromOkdesk())
                return false;
            if (!await _priorityRepository.GetPriorityFromOkdesk())
                return false;
            if (!await _statusRepository.GetStatusFromOkdesk())
                return false;

            return true;
        }

        async Task CheckIssue(Issue issue)
        {
            Status? status = null;
            Priority? priority = null;
            TaskType? type = null;
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

            issue.Status = status;
            issue.Priority = priority;
            issue.Type = type;
            issue.Company = company;
            issue.Service_object = maintenanceEntity;
        }
    }
}
