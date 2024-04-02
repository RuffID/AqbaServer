using AqbaServer.Interfaces.OkdeskPerformance;

namespace AqbaServer.Services
{
    // Данная служба предназначена для получения данных семь дней назад на случай если данные были изменены в старых незакрытых задачах
    public class WeekReportService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = serviceScopeFactory.CreateScope())
                {
                    IEmployeePerformanceRepository employeePerformanceRepository =
                        scope.ServiceProvider.GetRequiredService<IEmployeePerformanceRepository>();

                    await employeePerformanceRepository.GetEmployeePerformanceFromOkdesk(DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-7));
                    ThirtyMinutesReportService.TimeOfLastReportReceived = DateTime.UtcNow;
                }

                DateTime nextDay = DateTime.Now.AddDays(1);
                DateTime nextDayTime = new(nextDay.Year, nextDay.Month, nextDay.Day, 0, 1, 0);
                TimeSpan remaining = nextDayTime - DateTime.Now;
                await Task.Delay(remaining, stoppingToken);
            }
        }
    }
}