using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.WebHook;
using AqbaServer.Interfaces.WebHook;

namespace AqbaServer.Controllers.WebHook
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class companyWebhookController : Controller
    {
        private readonly ICompanyWebHookRepository _companyWebHookRepository;

        public companyWebhookController(ICompanyWebHookRepository companyWebHookRepository)
        {
            _companyWebHookRepository = companyWebHookRepository;
        }

        [HttpPost("companies"), AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> IssueWebHook([FromBody] RootEvent @event)
        {
            if (@event == null || @event.Event == null || string.IsNullOrEmpty(@event.Event.Event_type) || @event.Company == null)
                return BadRequest("Empty event or company.");

            switch (@event.Event.Event_type)
            {                
                case "new_company":
                    await _companyWebHookRepository.NewCompany(@event.Company);
                    break;
                case "change_company":
                    await _companyWebHookRepository.ChangeCompany(@event.Company);
                    break;                
                default:
                    break;
            }

            return Ok("Company webhook accepted.");
        }
    }
}
