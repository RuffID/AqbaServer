using AqbaServer.Dto;

namespace AqbaServer.Interfaces.OkdeskPerformance
{
    public interface IEmployeePerformanceRepository
    {
        public Task<bool> UpdatePerformanceFromOkdeskAPI(DateTime dateFrom, DateTime dateTo);
        Task<List<EmployeeDto>?> GetEmployeePerformanceFromLocalDB(DateTime dateFrom, DateTime dateTo);
    }
}