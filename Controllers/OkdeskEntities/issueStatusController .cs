using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AqbaServer.Dto;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class issueStatusController : Controller
    {
        private readonly IIssueStatusRepository _issueStatusRepository;
        private readonly IMapper _mapper;

        public issueStatusController(IIssueStatusRepository issueStatusRepository, IMapper mapper)
        {
            _issueStatusRepository = issueStatusRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Status>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetIssueStatuses()
        {
            var statuses = _mapper.Map<ICollection<StatusDto>>(await _issueStatusRepository.GetIssueStatuses());

            if (statuses == null || statuses.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(statuses);
        }

        [HttpGet("okdesk")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetIssueStatusesFromOkdesk()
        {
            if (await _issueStatusRepository.GetStatusesFromDBOkdesk() == false)
                return StatusCode(500, "Внутренняя ошибка при получении статусов из БД окдеска");

            return Ok("Статусы успешно обновлены");
        }
    }
}