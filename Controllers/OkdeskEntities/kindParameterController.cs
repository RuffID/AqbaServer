using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AqbaServer.Dto;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Interfaces.OkdeskEntities;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.Authorization;
using AqbaServer.Repository.OkdeskEntities;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class kindParameterController : Controller
    {
        private readonly IKindParameterRepository _kindParameterRepository;
        private readonly IMapper _mapper;

        public kindParameterController(IKindParameterRepository kindParameterRepository, IMapper mapper)
        {
            _kindParameterRepository = kindParameterRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<KindParameter>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetKindParameters()
        {
            var kindParameters = _mapper.Map<IEnumerable<KindParameterDto>>(await _kindParameterRepository.GetKindParameters());

            if (kindParameters == null || kindParameters.ToList().Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(kindParameters);
        }

        [HttpGet("{kindParameterName}")]
        [ProducesResponseType(200, Type = typeof(KindParameter))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetKindParameterByKind(string kindParameterName)
        {
            var kindParameter = _mapper.Map<IEnumerable<KindParameterDto>>(await _kindParameterRepository.GetKindParameter(kindParameterName));

            if (kindParameter == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(kindParameter);
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetKindsFromAPIOkdesk()
        {
            if (await _kindParameterRepository.UpdateKindParametersFromAPIOkdesk() == false)
                return StatusCode(500, "Внутренняя ошибка при получении kinds parameters из API окдеска");

            return Ok("Kinds parameters успешно обновлены");
        }

        [HttpGet("okdeskDB"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetKindsFromDBOkdesk()
        {
            if (await _kindParameterRepository.UpdateKindParametersFromDBOkdesk() == false)
                return StatusCode(500, "Внутренняя ошибка при получении kinds parameters из БД окдеска");

            return Ok("Kinds parameters успешно обновлены");
        }

        [HttpDelete, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteKindParameter([FromQuery] string kindParameterCode)
        {
            var kindParameter = await _kindParameterRepository.GetKindParameter(kindParameterCode);

            if (kindParameter == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _kindParameterRepository.DeleteKindParameter(kindParameter.Id))
                ModelState.AddModelError("", "Something went wrong deleting kind parameter");

            return NoContent();
        }
    }
}
