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

        /*[HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateKindParameter([FromQuery] int kindId, [FromBody] KindParameterDto kindParameterCreate)
        {
            if (kindParameterCreate == null)
                return BadRequest(ModelState);

            var kindParameter = await _kindParameterRepository.GetKindParameter(kindParameterCreate.Name);

            if (kindParameter != null)
            {
                ModelState.AddModelError("", "Kind parameter already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var kindParameterMap = _mapper.Map<KindParameter>(kindParameterCreate);

            if (!await _kindParameterRepository.CreateKindParameter(kindId, kindParameterMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving kind parameter");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }*/

        /*[HttpPut("{kindParameterCode}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateKindParameter([FromQuery] int kindId, [FromQuery] string kindParameterCode, [FromBody] KindParameterDto updatedKindParameter)
        {
            if(updatedKindParameter == null)
                return BadRequest(ModelState);

            if(await _kindParameterRepository.GetKindParameter(kindParameterCode) == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var kindParameterMap = _mapper.Map<KindParameter>(updatedKindParameter);

            if (! await _kindParameterRepository.UpdateKindParameter(kindId, kindParameterCode, kindParameterMap))
            {
                ModelState.AddModelError("", "Something went wrong updating kind parameter");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }*/

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
