namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IEmployeeGroupsRepository
    {
        Task<bool> GetEmployeeGroup(int employeeId, int groupId);
        Task<bool> CreateEmployeeGroup(int employeeId, int groupId);
    }
}