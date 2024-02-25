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
    public class equipmentParameterController : Controller
    {
        private readonly IEquipmentParameterRepository _equipmentParameterRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IKindParameterRepository _kindParameterRepository;
        private readonly IMapper _mapper;

        public equipmentParameterController(IEquipmentParameterRepository equipmentParameterRepository, IEquipmentRepository equipmentRepository, IKindParameterRepository kindParameterRepository, IMapper mapper)
        {
            _equipmentParameterRepository = equipmentParameterRepository;
            _equipmentRepository = equipmentRepository;
            _kindParameterRepository = kindParameterRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<EquipmentParameter>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEquipmentParameters()
        {
            var equipmentParameters = _mapper.Map<ICollection<EquipmentParameterDto>>(await _equipmentParameterRepository.GeEquipmentParameters());

            if (equipmentParameters == null || equipmentParameters.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(equipmentParameters);
        }

        /*[HttpGet]
        [ProducesResponseType(200, Type = typeof(EquipmentParameter))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEquipmentParameter([FromQuery] int equipmentId, [FromQuery] int kindParamId)
        {
            var equipmentParameter = _mapper.Map<EquipmentParameterDto>(await _equipmentParameterRepository.GetEquipmentParameter(equipmentId, kindParamId));

            if (equipmentParameter == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(equipmentParameter);
        }*/

        [HttpPost, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateEquipmentParameter([FromQuery] int equipmentId, [FromQuery] string kindParameterCode, [FromBody] EquipmentParameterDto equipmentParameterCreate)
        {
            if (equipmentParameterCreate == null)
                return BadRequest(ModelState);

            var kindParameter = await _kindParameterRepository.GetKindParameter(kindParameterCode);
            var equipmentParameter = await _equipmentParameterRepository.GetEquipmentParameter(equipmentId, kindParameter.Id);

            if (equipmentParameter != null)
            {
                ModelState.AddModelError("", "Equipment parameter already exists");
                return StatusCode(422, ModelState);
            }

            var equipment = await _equipmentRepository.GetEquipment(equipmentId);


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentParameterMap = _mapper.Map<EquipmentParameter>(equipmentParameterCreate);

            equipmentParameterMap.Equipment = equipment;
            equipmentParameterMap.KindParam = kindParameter;

            if (!await _equipmentParameterRepository.CreateEquipmentParameter(equipmentParameterMap, equipment))
            {
                ModelState.AddModelError("", "Something went wrong while saving equipment parameter");
                return StatusCode(500, ModelState);
            }

            return Ok("Equipment parameter successfully created");
        }

        [HttpPut, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateEquipmentParameter([FromQuery] int equipmentParameterId, [FromQuery] int equipmentId, [FromQuery] string kindParameterCode, [FromBody] EquipmentParameterDto updatedEquipmentParameter)
        {
            if (updatedEquipmentParameter == null)
                return BadRequest(ModelState);

            var kindParameter = await _kindParameterRepository.GetKindParameter(kindParameterCode);

            if (await _equipmentParameterRepository.GetEquipmentParameter(equipmentId, kindParameter.Id) == null)
                return NotFound();

            var equipment = await _equipmentRepository.GetEquipment(equipmentId);

            if (equipment == null)
                return NotFound();


            if (kindParameter == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var equipmentParameterMap = _mapper.Map<EquipmentParameter>(updatedEquipmentParameter);

            equipmentParameterMap.Equipment = equipment;
            equipmentParameterMap.KindParam = kindParameter;

            if (!await _equipmentParameterRepository.UpdateEquipmentParameter(equipmentParameterId, equipmentParameterMap))
            {
                ModelState.AddModelError("", "Something went wrong updating equipment parameter");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteEquipmentParameter([FromQuery] int equipmentId, [FromQuery] int kindParamId)
        {
            var equipmentParameter = await _equipmentParameterRepository.GetEquipmentParameter(equipmentId, kindParamId);

            if (equipmentParameter == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //TODO ИСПРАВИТь
            if (!await _equipmentParameterRepository.DeleteEquipmentParameter(equipmentId))
                ModelState.AddModelError("", "Something went wrong deleting equipment parameter");

            return NoContent();
        }
    }
}
