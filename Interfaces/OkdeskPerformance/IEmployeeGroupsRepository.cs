namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IEmployeeGroupsRepository
    {
        Task<bool> GetEmployeeGroup(int id);
        Task<bool> CreateEmployeeGroup(int id, int employeeId, int groupId);
        Task<bool> UpdateEmployeeGroupsFromDBOkdesk();
    }
}