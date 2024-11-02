using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AqbaServer.Models.WebHook;
using AqbaServer.Interfaces.WebHook;

namespace AqbaServer.Controllers.WebHook
{
    [Authorize]
    [Route("api/aqbaserver/[controller]")]
    [ApiController]
    public class issueWebhookController : Controller
    {
        private readonly IIssueWebHookRepository _issueWebHookRepository;

        public issueWebhookController(IIssueWebHookRepository issueWebHookRepository)
        {
            _issueWebHookRepository = issueWebHookRepository;
        }

        [HttpPost("issues"), AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> IssueWebHook([FromBody] RootEvent @event)
        {
            if (@event == null || @event.Event == null || string.IsNullOrEmpty(@event.Event.Event_type) || @event.Issue == null)
                return BadRequest("Empty event or issue.");

            switch (@event.Event.Event_type)
            {
                case "new_ticket_status":
                    await _issueWebHookRepository.UpdateIssueStatus(@event);
                    break;
                case "new_ticket":
                    await _issueWebHookRepository.NewTicket(@event.Issue);
                    break;
                case "new_assignee":
                    await _issueWebHookRepository.SaveTicket(@event.Issue);
                    break;
                case "update_issue_work_type":
                    await _issueWebHookRepository.SaveTicket(@event.Issue);
                    break;
                case "ticket_deleted":
                    await _issueWebHookRepository.MarkTicketAsDeleted(@event.Issue);
                    break;
                case "new_comment":
                    await _issueWebHookRepository.NewComment(@event);
                    break;                
                default:
                    break;
            }

            return Ok("Webhook accepted.");
        }
    }
}
