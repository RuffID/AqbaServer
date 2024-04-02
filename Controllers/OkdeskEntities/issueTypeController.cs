using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AqbaServer.Dto;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Interfaces.OkdeskPerformance;
using AqbaServer.Models.OkdeskReport;

namespace AqbaServer.Controllers.OkdeskEntities
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class issueTypeController : Controller
    {
        private readonly IIssueTypeRepository _issueTypeRepository;
        private readonly IMapper _mapper;

        public issueTypeController(IIssueTypeRepository issueTypeRepository, IMapper mapper)
        {
            _issueTypeRepository = issueTypeRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TaskType>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetIssueTypes()
        {
            var types = _mapper.Map<ICollection<TaskTypeDto>>(await _issueTypeRepository.GetIssueTypes());

            if (types == null || types.Count <= 0)
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(types);
        }        
    }
}