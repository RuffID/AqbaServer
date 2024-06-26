﻿using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IGroupRepository
    {
        Task<Group?> GetGroup(Group group);
        Task<ICollection<Group>?> GetGroups();
        Task<bool> CreateGroup(Group group);
        Task<bool> UpdateGroup(int groupId, Group group);
        Task<bool> GetGroupsFromOkdesk();
        Task<bool> GetGroupsFromDBOkdesk();
    }
}