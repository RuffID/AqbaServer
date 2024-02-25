using AqbaServer.Interfaces.OkdeskEntities;

namespace AqbaServer.Services
{
    public class ThirtyMinutesReportService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        public static DateTime TimeOfLastReportReceived { get; set; } = DateTime.UtcNow.AddMinutes(-10); // - 10 чтобы при запуске сразу таймаута не было
        readonly int timeout = 30; // задержка в минутах для автоматического запроса

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(600000, stoppingToken); // Задержка при запуске сервиса
            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = serviceScopeFactory.CreateScope())
                {
                    IEmployeePerformanceRepository employeePerformanceRepository = 
                        scope.ServiceProvider.GetRequiredService<IEmployeePerformanceRepository>();

                    if (TimeOfLastReportReceived.AddMinutes(timeout + 1) > DateTime.UtcNow)
                    {                        
                        await employeePerformanceRepository.GetEmployeePerformanceFromOkdesk(DateTime.Now, DateTime.Now);
                        TimeOfLastReportReceived = DateTime.UtcNow;
                    }
                }

                TimeSpan remaining = TimeOfLastReportReceived.AddMinutes(timeout) - DateTime.UtcNow;
                await Task.Delay(remaining, stoppingToken);
            }
        }
    }
}