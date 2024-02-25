using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AqbaServer.Dto;
using AqbaServer.Models.OkdeskEntities;
using AqbaServer.Interfaces.OkdeskEntities;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.Authorization;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class kindController : Controller
    {
        private readonly IKindRepository _kindRepository;
        private readonly IKindParameterRepository _kindParameterRepository;
        private readonly IMapper _mapper;

        public kindController(IKindRepository kindRepository, IKindParameterRepository kindParameterRepository, IMapper mapper)
        {
            _kindRepository = kindRepository;
            _kindParameterRepository = kindParameterRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Kind>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetKinds()
        {
            var kinds = _mapper.Map<ICollection<KindDto>>(await _kindRepository.GetKinds());

            if (kinds == null || kinds.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(kinds);
        }

        [HttpGet("{kindCode}")]
        [ProducesResponseType(200, Type = typeof(Kind))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetKind(string kindCode)
        {
            var kind = _mapper.Map<KindDto>(await _kindRepository.GetKind(kindCode));

            if (kind == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(kind);
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetKindsFromOkdesk()
        {
            if (!await _kindRepository.GetKindsFromOkdesk())
            {
                ModelState.AddModelError("", "Something went wrong when retrieving kinds from okdesk");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPost, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateKind([FromBody] KindDto kindCreate)
        {
            if (kindCreate == null || kindCreate?.Code == null)
                return BadRequest(ModelState);

            var kind = await _kindRepository.GetKind(kindCreate.Code);

            if (kind != null)
            {
                ModelState.AddModelError("", "Kind already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var kindMap = _mapper.Map<Kind>(kindCreate);


            if (!await _kindRepository.CreateKind(kindMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving kind");
                return StatusCode(500, ModelState);
            }

            return Ok("Kind successfully created");
        }

        /*[HttpPost("KindParam")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateKindParam([FromQuery] string kindCode, [FromQuery] string kindParamCode)
        {
            if (kindCode == null || kindParamCode == null)
                return BadRequest(ModelState);

            var kind = await _kindRepository.GetKind(kindCode);
            var kindParam = await _kindParameterRepository.GetKindParameter(kindParamCode);

            if (kind == null || kindParam == null)
                return NotFound();
            

            if (await _kindRepository.GetKindParam(kind.Id, kindParam.Id))
            {
                ModelState.AddModelError("", "Relations kind-param already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _kindRepository.CreateKindParam(kind.Code, kindParam.Code))
            {
                ModelState.AddModelError("", "Something went wrong while saving relationship");
                return StatusCode(500, ModelState);
            }

            return Ok("Relationship kind-param successfully created");
        }*/

        [HttpPut("{kindCode}"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateKind(string kindCode, [FromBody] KindDto updatedKind)
        {
            if (updatedKind == null)
                return BadRequest(ModelState);

            if (await _kindRepository.GetKind(kindCode) == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var kindMap = _mapper.Map<Kind>(updatedKind);

            if (!await _kindRepository.UpdateKind(kindCode, kindMap))
            {
                ModelState.AddModelError("", "Something went wrong updating kind");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteKind([FromQuery] string kindCode)
        {
            var kind = await _kindRepository.GetKind(kindCode);

            if (kind == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _kindRepository.DeleteKind(kind.Id))
                ModelState.AddModelError("", "Something went wrong deleting kind");

            return NoContent();
        }
    }
}
