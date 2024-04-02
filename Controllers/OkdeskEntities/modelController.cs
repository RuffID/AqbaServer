using AutoMapper;
using AqbaServer.Models;
using Microsoft.AspNetCore.Mvc;
using AqbaServer.Models.OkdeskPerformance;
using AqbaServer.Interfaces.OkdeskEntities;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.Authorization;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class modelController : Controller
    {
        private readonly IModelRepository _modelRepository;
        private readonly IKindRepository _kindRepository;
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IMapper _mapper;

        public modelController(IModelRepository modelRepository, IKindRepository kindRepository, IManufacturerRepository manufacturerRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _kindRepository = kindRepository;
            _manufacturerRepository = manufacturerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Model>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetModels()
        {
            var models = _mapper.Map<ICollection<ModelDto>>(await _modelRepository.GetModels());

            if (models == null || models.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(models);
        }

        [HttpGet("{modelCode}")]
        [ProducesResponseType(200, Type = typeof(Model))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetModel(string modelCode)
        {
            var model = _mapper.Map<ModelDto>(await _modelRepository.GetModel(modelCode));

            if (model == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(model);
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetModelsFromOkdesk()
        {
            if (!await _modelRepository.GetModelsFromOkdesk())
            {
                ModelState.AddModelError("", "Something went wrong when retrieving models from okdesk");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPost, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateModel([FromQuery] string kindCode, [FromQuery] string manufacturerCode, [FromBody] ModelDto modelCreate)
        {
            if (modelCreate == null)
                return BadRequest(ModelState);

            var model = await _modelRepository.GetModel(modelCreate.Code);

            if (model != null)
            {
                ModelState.AddModelError("", "Model already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var modelMap = _mapper.Map<Model>(modelCreate);

            if (!await _modelRepository.CreateModel(kindCode, manufacturerCode, modelMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving model");
                return StatusCode(500, ModelState);
            }

            return Ok("Model successfully created");
        }

        [HttpPut, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateModel([FromQuery] string modelCode, [FromQuery] string kindCode, [FromQuery] string manufacturerCode, [FromBody] ModelDto updatedModel)
        {
            if (updatedModel == null)
                return BadRequest(ModelState);

            if (await _modelRepository.GetModel(modelCode) == null)
                return NotFound();

            var kind = await _kindRepository.GetKind(kindCode);

            if (kind == null)
                return NotFound();

            var manufacturer = await _manufacturerRepository.GetManufacturer(manufacturerCode);

            if (manufacturer == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var modelMap = _mapper.Map<Model>(updatedModel);

            modelMap.EquipmentKind = kind;
            modelMap.EquipmentManufacturer = manufacturer;

            if (!await _modelRepository.UpdateModel(modelCode, modelMap))
            {
                ModelState.AddModelError("", "Something went wrong updating model");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteModel([FromQuery] string modelCode)
        {
            var model = await _modelRepository.GetModel(modelCode);

            if (model == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _modelRepository.DeleteModel(model.Id))
                ModelState.AddModelError("", "Something went wrong deleting model");

            return NoContent();
        }
    }
}
