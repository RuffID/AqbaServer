using Microsoft.AspNetCore.Mvc;
using AqbaServer.Interfaces.OkdeskEntities;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.Authorization;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class roleController : Controller
    {
        private readonly IRoleRepository _roleRepository;

        public roleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetRolesFromOkdesk()
        {
            if (!await _roleRepository.GetRolesFromOkdesk())
            {
                ModelState.AddModelError("", "Something went wrong when retrieving roles from okdesk");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}
