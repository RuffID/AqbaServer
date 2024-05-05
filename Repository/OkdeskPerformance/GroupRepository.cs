using AqbaServer.API;
using AqbaServer.Data.MySql;
using AqbaServer.Data.Postgresql;
using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Repository.OkdeskPerformance
{
    public class GroupRepository : IGroupRepository
    {
        /*private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeGroupsRepository _employeeGroupsRepository;
        public GroupRepository(IEmployeeRepository employeeRepository, IEmployeeGroupsRepository employeeGroupsRepository)
        {
            _employeeGroupsRepository = employeeGroupsRepository;
            _employeeRepository = employeeRepository;
        }*/
        public async Task<Group?> GetGroup(Group group)
        {
            return await DBSelect.SelectGroup(group.Id);
        }

        public async Task<bool> CreateGroup(Group group)
        {
            return await DBInsert.InsertGroup(group);
        }

        public async Task<ICollection<Group>?> GetGroups()
        {
            return await DBSelect.SelectGroups();
        }

        public async Task<bool> UpdateGroup(int groupId, Group group)
        {
            return await DBUpdate.UpdateGroup(groupId, group);
        }

        public async Task<bool> GetGroupsFromOkdesk()
        {
            var groups = await Request.GetGroups();

            return await SaveOrUpdateInDB(groups);
        }

        public async Task<bool> GetGroupsFromDBOkdesk()
        {
            var groups = await PGSelect.SelectEmployeeGroups();

            return await SaveOrUpdateInDB(groups?.ToArray());
        }

        async Task<bool> SaveOrUpdateInDB(Group[]? groups)
        {
            if (groups == null || groups.Length <= 0)
            {
                WriteLog.Warn("null при получении groups с окдеска");
                return false;
            }

            foreach (var group in groups)
            {
                var tempGroup = await GetGroup(group);

                if (tempGroup == null)
                {
                    if (!await CreateGroup(group))
                        return false;
                }
                else if (tempGroup != null)
                {
                    if (!await UpdateGroup(tempGroup.Id, group))
                        return false;
                }

                /*if (group?.Employees?.Count > 0)
                {
                    foreach (var employee in group.Employees)
                    {
                        var tempEmp = await _employeeRepository.GetEmployee(employee);
                        if (tempEmp == null) break;

                        if (!await _employeeGroupsRepository.GetEmployeeGroup(tempEmp.Id, group.Id))
                            await _employeeGroupsRepository.CreateEmployeeGroup(tempEmp.Id, group.Id);
                    }
                }*/
            }
            return true;
        }
    }
}

