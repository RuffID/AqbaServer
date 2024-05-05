using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.OkdeskReport;
using AqbaServer.Models.Authorization;
using AqbaServer.Interfaces.OkdeskPerformance;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class groupController : Controller
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IEmployeeGroupsRepository _employeeGroupsRepository;

        public groupController(IGroupRepository groupRepository, IEmployeeGroupsRepository employeeGroupsRepository)
        {
            _groupRepository = groupRepository;
            _employeeGroupsRepository = employeeGroupsRepository;
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

        [HttpGet("okdeskDB")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetGroupsFromDBOkdesk()
        {
            if (await _groupRepository.GetGroupsFromDBOkdesk() == false)
                return StatusCode(500, "Something went wrong when retrieving groups from SQL API okdesk");

            return Ok("Группы успешно обновлены");
        }

        [HttpGet("employee_groups_update")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateEmployeeGroupsFromDBOkdesk()
        {
            if (await _employeeGroupsRepository.UpdateEmployeeGroupsFromDBOkdesk() == false)
                return StatusCode(500, "Что то пошло не так при обновлении связей групп и сотрудников через SQL API окдеска");

            return Ok("Связи групп и сотрудников успешно обновлены");
        }
    }
}
