using Microsoft.AspNetCore.Mvc;
using AqbaServer.Models.OkdeskPerformance;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.Authorization;
using AqbaServer.Interfaces.OkdeskPerformance;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class employeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public employeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Employee>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEmployees([FromQuery] int employeeId)
        {
            var employees = await _employeeRepository.GetEmployees(employeeId);

            if (employees == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(employees);
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetEmployeesFromOkdesk()
        {
            if (!await _employeeRepository.GetEmployeesFromOkdesk())
            {
                ModelState.AddModelError("", "Something went wrong when retrieving employees from okdesk");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}
