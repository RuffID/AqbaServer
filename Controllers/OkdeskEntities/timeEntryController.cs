using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.Authorization;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class timeEntryController : Controller
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        public timeEntryController(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }        

        [HttpGet("okdeskDB"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetTimeEntryFromDBOkdesk([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            DateTime now = DateTime.Now;
            if (dateFrom == null)
                dateFrom = new(now.Year, now.Month, now.Day, hour: 0, minute: 0, second: 0);
            if (dateTo == null)
                dateTo = new(now.Year, now.Month, now.Day, hour: 23, minute: 59, second: 59);

            if (dateTo.Value.Hour == 0 && dateTo.Value.Minute == 0 && dateTo.Value.Second == 0)
                dateTo = new(dateTo.Value.Year, dateTo.Value.Month, dateTo.Value.Day, hour: 23, minute: 59, second: 59);

            if (await _timeEntryRepository.UpdateTimeEntryFromDBOkdesk((DateTime)dateFrom, (DateTime)dateTo) == false)
                return StatusCode(500, "Внутренняя ошибка при обновлении записей списанного времени из БД окдеска");

            return Ok("Записи о списаниях времени успешно обновлены");
        }
    }
}