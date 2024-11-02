using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.WebHook;
using AqbaServer.Interfaces.WebHook;

namespace AqbaServer.Controllers.WebHook
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class equipmentWebhookController : Controller
    {
        private readonly IEquipmentWebHookRepository _equipmentWebHookRepository;

        public equipmentWebhookController(IEquipmentWebHookRepository equipmentWebHookRepository)
        {
            _equipmentWebHookRepository = equipmentWebHookRepository;
        }

        [HttpPost("equipments"), AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> IssueWebHook([FromBody] RootEvent @event)
        {
            if (@event == null || @event.Event == null || string.IsNullOrEmpty(@event.Event.Event_type) || @event.Equipment == null)
                return BadRequest("Empty event or equipment.");

            switch (@event.Event.Event_type)
            {                
                case "new_equipment":
                    await _equipmentWebHookRepository.NewEquipment(@event.Equipment);
                    break;
                case "change_equipment":
                    await _equipmentWebHookRepository.ChangeEquipment(@event.Equipment);
                    break;
                default:
                    break;
            }

            return Ok("Equipment webhook accepted.");
        }
    }
}
