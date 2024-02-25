using AqbaServer.API;
using AqbaServer.Interfaces.OkdeskEntities;

namespace AqbaServer.Repository.OkdeskReport
{
    public class PerformanceRepository : IPerformanceRepository
    {
        private IEmployeeRepository _employeeRepository;
        public PerformanceRepository(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task GetSolvedTaskFromOkdesk()
        {
            var employees = await OkdeskEntitiesRequest.GetEmployees();

            /*foreach (DateTime day in EachDay(DateTime.Now, DateTime.Now))
            {

            }*/
        }
    }
}
