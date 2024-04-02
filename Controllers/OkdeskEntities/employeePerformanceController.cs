using AqbaServer.Dto;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.Authorization;
using AqbaServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class employeePerformanceController : Controller
    {
        private readonly IEmployeePerformanceRepository _employeePerformanceRepository;
        public employeePerformanceController(IEmployeePerformanceRepository employeePerformanceRepository)
        {
            _employeePerformanceRepository = employeePerformanceRepository;
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmployeePerformanceFromOkdesk([FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            if (dateFrom > dateTo)
                return BadRequest("Дата начала периода не может быть больше даты окончания");

            if (!await _employeePerformanceRepository.GetEmployeePerformanceFromOkdesk(dateFrom, dateTo))
            {
                ModelState.AddModelError("", "Something went wrong when retrieving employee performance from okdesk");
                return StatusCode(500, ModelState);
            }

            ThirtyMinutesReportService.TimeOfLastReportReceived = DateTime.UtcNow;

            return Ok();
        }

        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<List<EmployeeDto>?>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeesPerformance([FromQuery] string? requestType, [FromQuery] DateTime dateFrom, [FromQuery] DateTime dateTo)
        {
            if (dateFrom > dateTo)
                return BadRequest("Дата начала периода не может быть больше даты окончания");

            if (requestType == "manual")
            {

                if (ThirtyMinutesReportService.TimeOfLastReportReceived.AddMinutes(5) < DateTime.UtcNow)
                {
                    DateTime today = new (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                    if (dateTo >= today)
                    {
                        ThirtyMinutesReportService.TimeOfLastReportReceived = DateTime.UtcNow;
                        _ = _employeePerformanceRepository.GetEmployeePerformanceFromOkdesk(DateTime.Now, DateTime.Now);
                    }
                }
            }

            var employees = await _employeePerformanceRepository.GetEmployeesPerformance(dateFrom, dateTo);

            if (employees == null || employees?.Count <= 0)
            {                
                return NotFound($"Данных с {dateFrom:dd.MM.yyyy} по {dateTo:dd.MM.yyyy} не найдено.");
            }

            return Ok(employees);
        }

        [HttpGet("time")]
        [ProducesResponseType(200, Type = typeof(DateTime))]
        [ProducesResponseType(400)]
        public IActionResult GetTimeOfLastReportReceived()
        {
            // Возвращает время последнего обновления 
            return Ok(ThirtyMinutesReportService.TimeOfLastReportReceived.ToLocalTime());
        }
    }
}
