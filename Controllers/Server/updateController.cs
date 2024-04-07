using AqbaServer.Helper;
using AqbaServer.Interfaces.Service;
using AqbaServer.Models.Authorization;
using AqbaServer.Models.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace AqbaServer.Controllers.Server
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class updateController : Controller
    {
        private readonly IManageImage _manageImage;
        public updateController(IManageImage manageImage)
        {
            _manageImage = manageImage;
        }

        [HttpPost("uploadFile"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null)
                return BadRequest("Не передан файл");

            if (!Directory.Exists("Update"))
                Directory.CreateDirectory("Update");

            var fileName = await _manageImage.UploadFile("Update", file);

            if (string.IsNullOrEmpty(fileName))
                return StatusCode(500, "Ошибка при загрузке файла");

            return Ok($"Файл: {fileName} успешно загружен на сервер.");
        }

        [HttpGet, AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult DownloadFile()
        {
            string fileName = "AqbaApp.exe";
            string directory = "Update";

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var result = _manageImage.DownloadFile(directory, fileName);
            string message = $"[Download update method] Content type: {result.Item2}, file name: {result.Item2}, filestream: {result.Item1?.ToString()}";            

            if (result.Item1 == null || string.IsNullOrEmpty(result.Item2) || string.IsNullOrEmpty(result.Item3))
            {
                WriteLog.Info(message);
                return StatusCode(500, "Ошибка при скачивании файла");
            }

            return File(result.Item1, result.Item2, result.Item3);
        }

        [HttpGet("version"), AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetVersion()
        {
            AppVersionInfo? version = Helper.Version.GetVersion();
            if (version == null)
                return StatusCode(500, "При получении версия возникла ошибка");

            return Ok(version);
        }

        [HttpPut, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult UpdateVersion([FromBody] AppVersionInfo version)
        {
            if (version == null)
            {
                ModelState.AddModelError("", "Не передано значение version");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            if (!Helper.Version.UpdateVersionFile(version))
            {
                ModelState.AddModelError("", "Something went wrong updating version file");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
