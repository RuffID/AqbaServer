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
    public class companyController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICompanyRepository _companyRepository;

        public companyController(ICategoryRepository categoryRepository, ICompanyRepository companyRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        [HttpGet("list")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Company>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCompanies([FromQuery] int companyId)
        {
            var companies = _mapper.Map<ICollection<CompanyDto>>(await _companyRepository.GetCompanies(companyId));

            if (companies == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(companies);
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Company))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCompany([FromQuery] int companyId)
        {
            if (!await _companyRepository.GetCompanyFromOkdesk(companyId))
            {
                ModelState.AddModelError("", "Something went wrong when retrieving company from okdesk");
                return StatusCode(500, ModelState);
            }

            var company = _mapper.Map<CompanyDto>(await _companyRepository.GetCompany(companyId));

            if (company == null)
                return NotFound("Company not found");


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(company);
        }

        [HttpGet("okdesk"), Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(500)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetCompaniesFromOkdesk()
        {
            if (!await _companyRepository.GetCompaniesFromOkdesk())
            {
                ModelState.AddModelError("", "Something went wrong when retrieving companies from okdesk");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPost, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCompany([FromQuery] int categoryId, [FromBody] CompanyDto companyCreate)
        {
            if (companyCreate == null)
                return BadRequest(ModelState);

            var company = await _companyRepository.GetCompany(companyCreate.Id);

            if (company != null)
            {
                ModelState.AddModelError("", "Company already exists");
                return StatusCode(422, ModelState);
            }

            var category = _categoryRepository.GetCategory(categoryId);

            if (category == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var companyMap = _mapper.Map<Company>(companyCreate);

            if (!await _companyRepository.CreateCompany(categoryId, companyMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving company");
                return StatusCode(500, ModelState);
            }

            return Ok("Company successfully created");
        }

        [HttpPut, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCompany([FromQuery] int companyId, [FromQuery] int categoryId, [FromBody] CompanyDto updatedCompany)
        {
            if (updatedCompany == null)
                return BadRequest(ModelState);

            if (await _companyRepository.GetCompany(companyId) == null)
                return NotFound();

            var category = await _categoryRepository.GetCategory(categoryId);

            if (category == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var companyMap = _mapper.Map<Company>(updatedCompany);

            companyMap.Category = category;

            if (!await _companyRepository.UpdateCompany(companyId, companyMap))
            {
                ModelState.AddModelError("", "Something went wrong updating company");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete, Authorize(Roles = UserRole.Admin)]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCompany([FromQuery] int companyId)
        {
            var company = await _companyRepository.GetCompany(companyId);

            if (company == null)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _companyRepository.DeleteCompany(companyId))
                ModelState.AddModelError("", "Something went wrong deleting company");

            return NoContent();
        }
    }
}
