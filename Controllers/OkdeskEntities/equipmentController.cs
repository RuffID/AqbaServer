using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AqbaServer.Dto;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Interfaces.OkdeskEntities;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.Authorization;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class equipmentController : Controller
    {
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IKindRepository _kindRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IModelRepository _modelRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMaintenanceEntityRepository _maintenanceEntityRepository;
        private readonly IMapper _mapper;

        public equipmentController(IEquipmentRepository equipmentRepository, IKindRepository kindRepository, IManufacturerRepository manufacturerRepository, IModelRepository modelRepository, ICompanyRepository companyRepository, IMaintenanceEntityRepository maintenanceEntityRepository, IMapper mapper)
        {
            _equipmentRepository = equipmentRepository;
            _kindRepository = kindRepository;
            _manufacturerRepository = manufacturerRepository;
            _modelRepository = modelRepository;
            _companyRepository = companyRepository;
            _maintenanceEntityRepository = maintenanceEntityRepository;
            _mapper = mapper;
        }

        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Equipment>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEquipments([FromQuery] int equipmentId)
        {
            var equipments = _mapper.Map<ICollection<EquipmentDto>>(await _equipmentRepository.GetEquipments(equipmentId));

            if (equipments == null || equipments.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(equipments);
        }

        [HttpGet("maintenanceEntity")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Equipment>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEquipmentsByMaintenanceEntity([FromQuery] int maintenanceEntityId)
        {
            var equipments = _mapper.Map<ICollection<EquipmentDto>>(await _equipmentRepository.GetEquipmentsByMaintenanceEntity(maintenanceEntityId));

            if (equipments == null || equipments.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(equipments);
        }

        [HttpGet("company")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Equipment>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEquipmentsByCompany([FromQuery] int companyId)
        {
            var equipments = _mapper.Map<ICollection<EquipmentDto>>(await _equipmentRepository.GetEquipmentsByCompany(companyId));

            if (equipments == null || equipments.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(equipments);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Equipment))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetEquipment([FromQuery] int equipmentId)
        {
            if (!await _equipmentRepository.GetEquipmentsFromOkdesk(equipmentId, pageSize: 1))
            {
                ModelState.AddModelError("", "Something went wrong when retrieving equipments from okdesk");
                return StatusCode(500, ModelState);
            }

            var equipment = _mapper.Map<EquipmentDto>(await _equipmentRepository.GetEquipment(equipmentId));

            if (equipment == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(equipment);
        }

        [HttpPost, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateEquipment([FromQuery] string kindCode, [FromQuery] string manufacturerCode, [FromQuery] string modelCode, [FromQuery] int companyId, [FromQuery] int maintenanceEntityId, [FromBody] EquipmentDto equipmentCreate)
        {
            if (equipmentCreate == null)
                return BadRequest(ModelState);

            var equipment = await _equipmentRepository.GetEquipment(equipmentCreate.Id);

            if (equipment != null)
            {
                ModelState.AddModelError("", "Equipment already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var equipmentMap = _mapper.Map<Equipment>(equipmentCreate);

            var company = await _companyRepository.GetCompany(companyId);
            equipmentMap.Company = company;
            var maintenanceEntity = await _maintenanceEntityRepository.GetMaintenanceEntity(maintenanceEntityId);
            equipmentMap.Maintenance_entity = maintenanceEntity;

            if (!await _equipmentRepository.CreateEquipment(equipmentMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving equipment");
                return StatusCode(500, ModelState);
            }

            return Ok("Equipment successfully created");
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetEquipmentsFromOkdesk([FromQuery] int lastEquipmentId = 0)
        {
            if (!await _equipmentRepository.GetEquipmentsFromOkdesk(lastEquipmentId))
            {
                ModelState.AddModelError("", "Something went wrong when retrieving equipments from okdesk");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPut, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateEquipment([FromQuery] int equipmentId, [FromQuery] string kindCode, [FromQuery] string manufacturerCode, [FromQuery] string modelCode, [FromQuery] int companyId, [FromQuery] int maintenanceEntityId, [FromBody] EquipmentDto updatedEquipment)
        {
            if (updatedEquipment == null)
                return BadRequest(ModelState);

            if (await _equipmentRepository.GetEquipment(equipmentId) == null)
                return NotFound();
            
            var company = await _companyRepository.GetCompany(companyId);
            var maintenanceEntity = await _maintenanceEntityRepository.GetMaintenanceEntity(maintenanceEntityId);

            if (!ModelState.IsValid)
                return BadRequest();

            var equipmentMap = _mapper.Map<Equipment>(updatedEquipment);
            
            equipmentMap.Company = company;
            equipmentMap.Maintenance_entity = maintenanceEntity;

            if (!await _equipmentRepository.UpdateEquipment(equipmentId, equipmentMap))
            {
                ModelState.AddModelError("", "Something went wrong updating equipment");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteEquipment([FromQuery] int equipmentId)
        {
            var equipment = await _equipmentRepository.GetEquipment(equipmentId);

            if (equipment == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _equipmentRepository.DeleteEquipment(equipment))
                ModelState.AddModelError("", "Something went wrong deleting equipment");

            return NoContent();
        }
    }
}
