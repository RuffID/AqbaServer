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
    public class categoryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICompanyRepository _companyRepository;

        public categoryController(ICategoryRepository categoryRepository, ICompanyRepository companyRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        
        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategories()
        {
            var categories = _mapper.Map<ICollection<CategoryDto>>(await _categoryRepository.GetCategories());

            if (categories == null || categories.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(categories);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategory([FromQuery] string categoryCode)
        {
            var category = _mapper.Map<CategoryDto>(await _categoryRepository.GetCategory(categoryCode));

            if (category == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpGet("companies"), Authorize(Roles = $"{UserRole.Admin}, {UserRole.Engineer}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Company>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCompaniesByCategory([FromQuery] string categoryCode, [FromQuery] int companyId)
        {
            var category = _mapper.Map<CategoryDto>(await _categoryRepository.GetCategory(categoryCode));
            var companies = _mapper.Map<ICollection<CompanyDto>>(await _companyRepository.GetCompaniesByCategory(categoryCode, companyId));

            if (category == null || companies == null)
                return NotFound();


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(companies);
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCategoriesFromOkdesk()
        {
            if (!await _categoryRepository.UpdateCategoriesFromDBOkdesk())
            {
                ModelState.AddModelError("", "Something went wrong while retrieving categories");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPost, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);

            var category = await _categoryRepository.GetCategory(categoryCreate.Code);

            if (category != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!await _categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving category");
                return StatusCode(500, ModelState);
            }

            return Ok("Category successfully created");
        }

        [HttpPut, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCategory([FromQuery] string categoryCode, [FromBody] CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);

            if (await _categoryRepository.GetCategory(categoryCode) == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var categoryMap = _mapper.Map<Category>(updatedCategory);

            if (!await _categoryRepository.UpdateCategory(categoryCode, categoryMap))
            {
                ModelState.AddModelError("", "Something went wrong updating category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory([FromQuery] string categoryCode)
        {
            var category = await _categoryRepository.GetCategory(categoryCode);

            if (category == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _categoryRepository.DeleteCategory(category.Code))
                ModelState.AddModelError("", "Something went wrong deleting category");

            return NoContent();
        }
    }
}
