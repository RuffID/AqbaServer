using AqbaServer.Dto;
using AqbaServer.Helper;
using AqbaServer.Models.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AqbaServer.Controllers.Server
{
    [Authorize(Roles = UserRole.Admin)]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class configController : Controller
    {
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateConfig([FromBody] ConfigDto config)
        {
            if (config == null)
                return BadRequest("Передан пустой конфиг");
            Config.CreateConfig(config, true);

            return Ok("Запрос на обновление конфига отправлен.");
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult GetConfig()
        {
            var c = Config.ReadConfig();

            if (c == null)
                return StatusCode(500, "Ошибка при чтении конфига");

            return Ok(c);
        }
    }

}
