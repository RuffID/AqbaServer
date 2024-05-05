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
    public class manufacturerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IManufacturerRepository _manufacturerRepository;

        public manufacturerController(IManufacturerRepository manufacturerRepository, IMapper mapper)
        {
            _manufacturerRepository = manufacturerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Manufacturer>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetManufacturers()
        {
            var manufacturers = _mapper.Map<List<ManufacturerDto>>(await _manufacturerRepository.GetManufacturers());

            if (manufacturers == null || manufacturers.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(manufacturers);
        }

        [HttpGet("{manufacturerCode}")]
        [ProducesResponseType(200, Type = typeof(Manufacturer))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetManufacturer(string manufacturerCode)
        {
            var manufacturer = _mapper.Map<ManufacturerDto>(await _manufacturerRepository.GetManufacturer(manufacturerCode));

            if (manufacturer == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(manufacturer);
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetManufacturersFromOkdesk()
        {
            if (!await _manufacturerRepository.GetManufacturersFromOkdesk())
            {
                ModelState.AddModelError("", "Something went wrong when retrieving manufacturers from okdesk");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpGet("okdeskDB")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetManufacturersFromDBOkdesk()
        {
            if (await _manufacturerRepository.UpdateManufacturersFromDBOkdesk() == false)
                return StatusCode(500, "Внутренняя ошибка при получении производителей из БД окдеска");

            return Ok("Производители успешно обновлены");
        }

        [HttpPost, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateManufacturer([FromBody] ManufacturerDto manufacturerCreate)
        {
            if (manufacturerCreate == null)
                return BadRequest(ModelState);

            var manufacturer = await _manufacturerRepository.GetManufacturer(manufacturerCreate.Code);

            if (manufacturer != null)
            {
                ModelState.AddModelError("", "Manufacturer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var manufacturerMap = _mapper.Map<Manufacturer>(manufacturerCreate);

            if (!await _manufacturerRepository.CreateManufacturer(manufacturerMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving manufacturer");
                return StatusCode(500, ModelState);
            }

            return Ok("Manufacturer successfully created");
        }

        [HttpPut("{manufacturerCode}"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateManufacturer(string manufacturerCode, [FromBody] ManufacturerDto updatedManufacturer)
        {
            if (updatedManufacturer == null)
                return BadRequest(ModelState);

            if (await _manufacturerRepository.GetManufacturer(manufacturerCode) == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var manufacturerMap = _mapper.Map<Manufacturer>(updatedManufacturer);

            if (!await _manufacturerRepository.UpdateManufacturer(manufacturerCode, manufacturerMap))
            {
                ModelState.AddModelError("", "Something went wrong updating manufacturer");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteManufacturer([FromQuery] string manufacturerCode)
        {
            var manufacturer = await _manufacturerRepository.GetManufacturer(manufacturerCode);

            if (manufacturer == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _manufacturerRepository.DeleteManufacturer(manufacturerCode))
                ModelState.AddModelError("", "Something went wrong deleting manufacturer");

            return NoContent();
        }
    }
}
