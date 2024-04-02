using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskPerformance;

namespace AqbaServer.Services
{
    public class ThirtyMinutesReportService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        public static DateTime TimeOfLastReportReceived { get; set; } = DateTime.UtcNow.AddMinutes(-10); // - 10 чтобы при запуске сразу таймаута не было
        readonly int timeout = 30; // задержка в минутах для автоматического запроса        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken); // Задержка при запуске сервиса
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
            catch (Exception ex) 
            { 
                WriteLog.Error(ex.ToString()); 
            }
        }
    }
}