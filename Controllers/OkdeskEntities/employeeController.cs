using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.Authorization;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Dto;
using AutoMapper;
using AqbaServer.Models.OkdeskPerformance;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class employeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public employeeController(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EmployeeDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEmployees([FromQuery] int id)
        {
            var employees = _mapper.Map<ICollection<EmployeeDto>>(await _employeeRepository.GetEmployees(id));

            if (employees == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(employees);
        }

        [HttpGet("group_employee")]
        [ProducesResponseType(200, Type = typeof(ICollection<GroupEmployee>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetGroupEmployeeConnections()
        {
            var employees = await _employeeRepository.GetGroupEmployeeConnections();

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
            if (!await _employeeRepository.UpdateEmployeesFromAPIOkdesk())
            {
                ModelState.AddModelError("", "Something went wrong when retrieving employees from okdesk");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpGet("okdeskDB"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetEmployeesDBOkdesk()
        {
            if (await _employeeRepository.GetEmployeesFromDBOkdesk() == false)
                return StatusCode(500, "Внутренняя ошибка при получении пользователей из БД окдеска");

            return Ok("Пользователи успешно обновлены");
        }
    }
}
