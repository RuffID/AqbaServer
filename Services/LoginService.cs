using AqbaServer.API;
using AqbaServer.Helper;

namespace AqbaServer.Services
{
    public class LoginService : BackgroundService
    {
        private readonly IHostApplicationLifetime _appLifetime;
        public LoginService(IHostApplicationLifetime applicationLifetime) 
        {
            _appLifetime = applicationLifetime;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!string.IsNullOrEmpty(Config.OkdeskDomainLink))
            {
                #if DEBUG
                    WriteLog.Info($"[Login method] Okdesk domain link: {Config.OkdeskDomainLink}");
                #endif
                await Request.Login(Immutable.OkdeskLoginLink, Immutable.OkdeskContentForLoginOnSite);
            }
            else
                WriteLog.Error("Please fill out the config to get started. The domain address is missing.");
            /*{
                _appLifetime.StopApplication();
            }*/
            //await Request.Login(Immutable.PartnersLink, Immutable.PartnersContentForLoginOnSite);
        }
    }
}