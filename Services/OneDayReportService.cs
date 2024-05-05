using AqbaServer.Interfaces.OkdeskPerformance;

namespace AqbaServer.Services
{
    // Данная служба предназначена для получения данных за предыдущий в день на случай если вчерашние данные были изменены
    public class OneDayReportService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromHours(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (IServiceScope scope = serviceScopeFactory.CreateScope())
                {
                    IIssueRepository issueRepository =
                        scope.ServiceProvider.GetRequiredService<IIssueRepository>();
                    ITimeEntryRepository timeEntryRepository =
                        scope.ServiceProvider.GetRequiredService<ITimeEntryRepository>();

                    DateTime dateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour: 0, minute: 0, second: 0).AddDays(-7);
                    DateTime dateTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour: 23, minute: 59, second: 59).AddDays(-7);

                    await issueRepository.UpdateIssuesFromDBOkdesk(dateFrom, dateTo);
                    await timeEntryRepository.UpdateTimeEntryFromDBOkdesk(dateFrom, dateTo);
                }

                DateTime nextDay = DateTime.Now.AddDays(1);
                DateTime nextDayTime = new(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 1);
                TimeSpan remaining = nextDayTime - DateTime.Now;
                await Task.Delay(remaining, stoppingToken);
            }
        }
    }
}