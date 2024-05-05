using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Models.Authorization;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class issueController : Controller
    {
        private readonly IIssueRepository _issueRepository;

        public issueController(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Issue))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetIssue([FromQuery] int issueId)
        {
            var issue = await _issueRepository.GetIssue(issueId);

            if (issue == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(issue);
        }

        [HttpGet("db_update"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetIssuesFromDBOkdesk([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            if (dateFrom == null)
                dateFrom = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour: 0, minute: 0, second: 0);
            if (dateTo == null)
                dateTo = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour: 23, minute: 59, second: 59);

            if (dateTo.Value.Hour == 0 && dateTo.Value.Minute == 0 && dateTo.Value.Second == 0)
                dateTo = new(dateTo.Value.Year, dateTo.Value.Month, dateTo.Value.Day, hour: 23, minute: 59, second: 59);

            if (await _issueRepository.UpdateIssuesFromDBOkdesk((DateTime)dateFrom, (DateTime)dateTo) == false)
                return StatusCode(500, "Внутренняя ошибка при обновлении заявок из БД окдеска");

            return Ok("Заявки успешно обновлены");
        }        
    }
}