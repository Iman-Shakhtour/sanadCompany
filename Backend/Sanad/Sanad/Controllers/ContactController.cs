using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sanad.Models;
using Sanad.Settings;

namespace Sanad.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly EmailSettings _emailSettings;

        public ContactController(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] Email form)
        {
            if (string.IsNullOrWhiteSpace(form.Subject) ||
                string.IsNullOrWhiteSpace(form.Body) ||
                string.IsNullOrWhiteSpace(form.Recivers))
            {
                return BadRequest(new { message = "Subject, Body, and Recivers are required." });
            }

            try
            {
                _emailSettings.SendEmail(form);
                return Ok(new { message = "Email sent successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to send email", error = ex.Message });
            }
        }
    }
}
