using AqbaServer.Interfaces.Service;
using AqbaServer.Models.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AqbaServer.Controllers.Server
{
    [Authorize(Roles = UserRole.Admin)]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class downloadLogController : Controller
    {
        private readonly IManageImage _manageImage;
        public downloadLogController(IManageImage manageImage)
        {
            _manageImage = manageImage;
        }

        [HttpGet("today")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DownloadTodayLog()
        {
            string fileName = $"{DateTime.Now:dd.MM.yyyy}_log.txt";

            var result = _manageImage.DownloadFile("Logs", fileName);

            if (result.Item1 == null || string.IsNullOrEmpty(result.Item2) || string.IsNullOrEmpty(result.Item3))
                return NotFound("Today's log was not found.");

            return File(result.Item1, result.Item2, result.Item3);
        }

        [HttpGet("all_time")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult DownloadZip()
        {
            var result = _manageImage.DownloadFile("Logs", "logs.zip");

            if (result.Item1 == null || string.IsNullOrEmpty(result.Item2) || string.IsNullOrEmpty(result.Item3))
                return NotFound("Today's log was not found.");

            return File(result.Item1, result.Item2, result.Item3);
        }
    }
}
