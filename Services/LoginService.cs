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
            await Request.Login(Immutable.OkdeskLoginLink, Immutable.OkdeskContentForLoginOnSite);
            /*{
                _appLifetime.StopApplication();
            }*/
            //await Request.Login(Immutable.PartnersLink, Immutable.PartnersContentForLoginOnSite);
        }
    }
}