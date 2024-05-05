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
        public async Task<IActionResult> UpdateEmployeePerformanceFromOkdesk([FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            if (dateFrom > dateTo)
                return BadRequest("Дата начала периода не может быть больше даты окончания");

            if (dateFrom == null)
                dateFrom = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour: 0, minute: 0, second: 0);
            if (dateTo == null)
                dateTo = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour: 23, minute: 59, second: 59);

            if (dateTo.Value.Hour == 0 && dateTo.Value.Minute == 0 && dateTo.Value.Second == 0)
                dateTo = new(dateTo.Value.Year, dateTo.Value.Month, dateTo.Value.Day, hour: 23, minute: 59, second: 59);

            if (!await _employeePerformanceRepository.UpdatePerformanceFromOkdeskAPI((DateTime)dateFrom, (DateTime)dateTo))
            {
                ModelState.AddModelError("", "Something went wrong when retrieving employee performance from okdesk");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ICollection<EmployeeDto>?>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetEmployeesPerformance([FromQuery] string? requestType, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            if (dateFrom > dateTo)
                return BadRequest("Дата начала периода не может быть больше даты окончания");

            if (dateFrom == null)
                dateFrom = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour: 0, minute: 0, second: 0);
            if (dateTo == null)
                dateTo = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour: 23, minute: 59, second: 59);

            if (dateTo.Value.Hour == 0 && dateTo.Value.Minute == 0 && dateTo.Value.Second == 0)
                dateTo = new(dateTo.Value.Year, dateTo.Value.Month, dateTo.Value.Day, hour: 23, minute: 59, second: 59);

            if (requestType == "manual")
            {
                if (ThirtyMinutesReportService.TimeOfLastUpdateRequest.AddMinutes(5) < DateTime.Now)
                {
                    DateTime today = new (DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                    if (dateTo >= today)
                        _ = _employeePerformanceRepository.UpdatePerformanceFromOkdeskAPI(ThirtyMinutesReportService.TimeOfLastUpdateRequest.AddMinutes(-10), DateTime.Now);                    
                }
            }

            var employees = await _employeePerformanceRepository.GetEmployeePerformanceFromLocalDB((DateTime)dateFrom, (DateTime)dateTo);

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
            // Возвращает время последнего обновления отчёта по закрытым заявкам и списанному времени
            return Ok(Helper.Immutable.UpdateTime);
        }
    }
}
