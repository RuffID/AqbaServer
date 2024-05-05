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
    public class maintenanceEntityController : Controller
    {
        private readonly IMaintenanceEntityRepository _maintenanceEntityRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public maintenanceEntityController(IMaintenanceEntityRepository maintenanceEntityRepository, ICompanyRepository companyRepository, IMapper mapper)
        {
            _maintenanceEntityRepository = maintenanceEntityRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MaintenanceEntityDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetMaintenanceEntities([FromQuery] int maintenanceEntityId)
        {
            var maintenanceEntities = _mapper.Map<ICollection<MaintenanceEntityDto>>(await _maintenanceEntityRepository.GetMaintenanceEntities(maintenanceEntityId));

            if (maintenanceEntities == null || maintenanceEntities.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(maintenanceEntities);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(MaintenanceEntityDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMaintenanceEntitiy([FromQuery] int maintenanceEntityId)
        {
            if (!await _maintenanceEntityRepository.UpdateMaintenanceEntitiesFromOkdesk(maintenanceEntityId, pageSize: 1))
            {
                ModelState.AddModelError("", "Something went wrong when retrieving maintenance entity from okdesk");
                return StatusCode(500, ModelState);
            }

            var maintenanceEntity = _mapper.Map<MaintenanceEntityDto>(await _maintenanceEntityRepository.GetMaintenanceEntity(maintenanceEntityId));

            if (maintenanceEntity == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(maintenanceEntity);
        }

        [HttpGet("update_maintenance_entities")]
        [ProducesResponseType(200, Type = typeof(Models.OkdeskEntities.UpdateInformation))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateMaintenanceEntitiesDirectories([FromQuery] int maintenanceEntityId)
        {
            if(!await _maintenanceEntityRepository.UpdateMaintenanceEntitiesFromOkdesk(maintenanceEntityId, pageSize: 1))
                return BadRequest("Не удалось обновить информацию по maintenance entiry id");

            var (objects, equipments) = await _maintenanceEntityRepository.GetUpdatingMaintenanceEntities(maintenanceEntityId: maintenanceEntityId);

            if (objects == null || equipments == null)
                return NotFound("Не удалось обновить информацию по объектам обслуживания");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Models.OkdeskEntities.UpdateInformation? directory = new();
            directory.Company = null;
            directory.MaintenanceEntity = objects;

            var equipmentDto = _mapper.Map<ICollection<EquipmentDto>>(equipments);

            directory.Equipments = equipmentDto;

            return Ok(directory);
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetMaintenanceEntitiesFromOkdesk()
        {
            if (!await _maintenanceEntityRepository.UpdateMaintenanceEntitiesFromOkdesk())
            {
                ModelState.AddModelError("", "Something went wrong when retrieving maintenance entities from okdesk");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpGet("okdeskDB"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetMaintenanceEntitiesFromDBOkdesk()
        {
            if (await _maintenanceEntityRepository.UpdateMaintenanceEntitiesFromDBOkdesk() == false)
                return StatusCode(500, "Something went wrong when retrieving maintenance entities from DB okdesk");

            return Ok("Объекты успешно обновлены");
        }

        [HttpPost, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateMaintenanceEntity([FromQuery] int companyId, [FromBody] MaintenanceEntityDto maintenanceEntityCreate)
        {
            if (maintenanceEntityCreate == null)
                return BadRequest(ModelState);

            var maintenanceEntity = await _maintenanceEntityRepository.GetMaintenanceEntity(maintenanceEntityCreate.Id);

            if (maintenanceEntity != null)
            {
                ModelState.AddModelError("", "Maintenance entity already exists");
                return StatusCode(422, ModelState);
            }

            var company = await _companyRepository.GetCompany(companyId);

            if (company == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var maintenanceEntityMap = _mapper.Map<MaintenanceEntity>(maintenanceEntityCreate);

            maintenanceEntityMap.Company_Id = company.Id;

            if (!await _maintenanceEntityRepository.CreateMaintenanceEntity(maintenanceEntityMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving maintenance entity");
                return StatusCode(500, ModelState);
            }

            return Ok("Maintenance entity successfully created");
        }

        [HttpPut, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateMaintenanceEntity([FromQuery] int maintenanceEntityId, [FromQuery] int companyId, [FromBody] MaintenanceEntityDto updatedMaintenanceEntity)
        {
            if (updatedMaintenanceEntity == null)
                return BadRequest(ModelState);

            if (await _maintenanceEntityRepository.GetMaintenanceEntity(maintenanceEntityId) == null)
                return NotFound();

            var company = await _companyRepository.GetCompany(companyId);

            if (company == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var maintenanceEntityMap = _mapper.Map<MaintenanceEntity>(updatedMaintenanceEntity);

            maintenanceEntityMap.Company_Id = company.Id;

            if (!await _maintenanceEntityRepository.UpdateMaintenanceEntity(maintenanceEntityId, maintenanceEntityMap))
            {
                ModelState.AddModelError("", "Something went wrong updating maintenance entity");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteMaintenanceEntity([FromQuery] int maintenanceEntityId)
        {
            var maintenanceEntity = await _maintenanceEntityRepository.GetMaintenanceEntity(maintenanceEntityId);

            if (maintenanceEntity == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _maintenanceEntityRepository.DeleteMaintenanceEntity(maintenanceEntityId))
                ModelState.AddModelError("", "Something went wrong deleting maintenance entity");

            return NoContent();
        }
    }
}
