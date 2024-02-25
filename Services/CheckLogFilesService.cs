using AqbaServer.Helper;

namespace AqbaServer.Services
{
    public class CheckLogFilesService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                WriteLog.CheckLogFiles();
                
                DateTime nextDay = DateTime.Now.AddDays(1);
                DateTime nextDayTime = new(nextDay.Year, nextDay.Month, nextDay.Day, 0, 0, 0);
                TimeSpan remaining = nextDayTime - DateTime.Now;
                await Task.Delay(remaining, stoppingToken);
            }
        }
    }
}