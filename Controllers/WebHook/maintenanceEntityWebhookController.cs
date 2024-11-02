using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.WebHook;
using AqbaServer.Interfaces.WebHook;

namespace AqbaServer.Controllers.WebHook
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class maintenanceEntityWebhookController : Controller
    {
        private readonly IMaintenanceEntityWebHookRepository _maintenanceEntityWebHookRepository;

        public maintenanceEntityWebhookController(IMaintenanceEntityWebHookRepository maintenanceEntityWebHookRepository)
        {
            _maintenanceEntityWebHookRepository = maintenanceEntityWebHookRepository;
        }

        [HttpPost("maintenanceEntities"), AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> IssueWebHook([FromBody] RootEvent @event)
        {
            if (@event == null || @event.Event == null || string.IsNullOrEmpty(@event.Event.Event_type) || @event.Service_aim == null)
                return BadRequest("Empty event or maintenance entity.");

            switch (@event.Event.Event_type)
            {                
                case "new_service_aim":
                    await _maintenanceEntityWebHookRepository.NewMaintenanceEntity(@event.Service_aim);
                    break;
                case "change_service_aim":
                    await _maintenanceEntityWebHookRepository.ChangeMaintenanceEntity(@event.Service_aim);
                    break;
                default:
                    break;
            }

            return Ok("Maintenance entity webhook accepted.");
        }
    }
}
