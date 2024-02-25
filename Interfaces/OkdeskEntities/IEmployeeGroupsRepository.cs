using AqbaServer.Models.OkdeskEntities;

namespace AqbaServer.Interfaces.OkdeskEntities
{
    public interface IEmployeeGroupsRepository
    {
        Task<bool> GetEmployeeGroup(int employeeId, int groupId);
        Task<bool> CreateEmployeeGroup(int employeeId, int groupId);
    }
}