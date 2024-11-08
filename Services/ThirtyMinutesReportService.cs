﻿using AqbaServer.Helper;
using AqbaServer.Interfaces.OkdeskPerformance;

namespace AqbaServer.Services
{
    public class ThirtyMinutesReportService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        public static DateTime TimeOfLastUpdateRequest { get; set; } = DateTime.Now.AddMinutes(-10);
        readonly int timeout = 30; // задержка в минутах для автоматического запроса        

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                #if DEBUG
                    await Task.Delay(2700000, stoppingToken); // Задержка при запуске сервиса 45 минут
                #endif

                while (!stoppingToken.IsCancellationRequested)
                {
                    using (IServiceScope scope = serviceScopeFactory.CreateScope())
                    {
                        IEmployeePerformanceRepository employeePerformanceRepository =
                            scope.ServiceProvider.GetRequiredService<IEmployeePerformanceRepository>();

                        DateTime now = DateTime.Now;
                        DateTime dateTo = new(now.Year, now.Month, now.Day, hour: 23, minute: 59, second: 59);

                        if (TimeOfLastUpdateRequest.AddMinutes(timeout + 1) > DateTime.Now)
                            await employeePerformanceRepository.UpdatePerformanceFromOkdeskAPI(now.AddMinutes(-35), dateTo);
                    }

                    TimeSpan remaining = TimeOfLastUpdateRequest.AddMinutes(timeout) - DateTime.Now;
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