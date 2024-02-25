using Microsoft.AspNetCore.Mvc;
using AqbaServer.Interfaces.OkdeskEntities;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.OkdeskReport;
using AqbaServer.Models.Authorization;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class groupController : Controller
    {
        private readonly IGroupRepository _groupRepository;

        public groupController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Group>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetGroups()
        {
            var groups = await _groupRepository.GetGroups();

            if (groups == null || groups.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(groups);
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetGroupsFromOkdesk()
        {
            if (!await _groupRepository.GetGroupsFromOkdesk())
            {
                ModelState.AddModelError("", "Something went wrong when retrieving groups from okdesk");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}
