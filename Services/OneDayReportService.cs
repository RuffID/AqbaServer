using AqbaServer.Interfaces.OkdeskEntities;

namespace AqbaServer.Services
{
    // Данная служба предназначена для получения данных за предыдущий в день на случай если вчерашние данные были изменены
    public class OneDayReportService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1200000, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = serviceScopeFactory.CreateScope())
                {
                    IEmployeePerformanceRepository employeePerformanceRepository =
                        scope.ServiceProvider.GetRequiredService<IEmployeePerformanceRepository>();

                    await employeePerformanceRepository.GetEmployeePerformanceFromOkdesk(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(-1));
                }

                DateTime nextDay = DateTime.Now.AddDays(1);
                DateTime nextDayTime = new(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 1);
                TimeSpan remaining = nextDayTime - DateTime.Now;
                await Task.Delay(remaining, stoppingToken);
            }
        }
    }
}