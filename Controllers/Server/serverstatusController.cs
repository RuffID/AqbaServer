using AqbaServer.Helper;
using AqbaServer.Models.Authorization;
using AqbaServer.Models.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AqbaServer.Controllers.Server
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class serverstatusController : Controller
    {
        private IHostApplicationLifetime _lifeTime;
        public serverstatusController(IHostApplicationLifetime appLifetime)
        {
            _lifeTime = appLifetime;
        }

        [HttpGet, AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(ServerData))]
        public IActionResult ServerStatus()
        {
            ServerData data = new() 
            {
                ServerName = "AqbaServer",
                ServerStartingTime = Immutable.ServerStartingTime.ToString("dd-MM-yyyy HH:mm"),
                Errors = Immutable.Errors
            };

            TimeSpan upTime = DateTime.Now - Immutable.ServerStartingTime;
            string upTimeString = $"Days: {upTime.Days}, Hours: {upTime.Hours}, Minutes: {upTime.Minutes}";
            data.ServerUpTime = upTimeString;
            return Ok(data);
        }        

        [HttpGet("restart"), Authorize(Roles = UserRole.Admin)]
        public async Task<int> StopServiceAsync()
        {
            try
            {
                _lifeTime.StopApplication();
                var process = Process.GetCurrentProcess().MainModule;
                string? _currentProcess;
                if (process != null)
                    _currentProcess = Path.GetFullPath(process.FileName);
                else return 1;

                if (!string.IsNullOrEmpty(_currentProcess))
                    Process.Start(_currentProcess);
            }
            catch (Exception ex) { WriteLog.Error(ex.ToString()); }            

            return await Task.FromResult(0);
        }

    }

}
