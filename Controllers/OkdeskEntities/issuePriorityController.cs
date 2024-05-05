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
    public class issuePriorityController : Controller
    {
        private readonly IIssuePriorityRepository _issuePriorityRepository;
        private readonly IMapper _mapper;

        public issuePriorityController(IIssuePriorityRepository issuePriorityRepository, IMapper mapper)
        {
            _issuePriorityRepository = issuePriorityRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Priority>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetIssuePriorities()
        {
            var priorities = _mapper.Map<ICollection<PriorityDto>>(await _issuePriorityRepository.GetIssuePriorities());

            if (priorities == null || priorities.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(priorities);
        }

        [HttpGet("okdesk")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetIssuePrioritiesFromOkdesk()
        {
            if (await _issuePriorityRepository.GetPrioritiesFromDBOkdesk() == false)
                return StatusCode(500, "Внутренняя ошибка при получении приоритетов из БД окдеска");

            return Ok("Приоритеты успешно обновлены");
        }
    }
}