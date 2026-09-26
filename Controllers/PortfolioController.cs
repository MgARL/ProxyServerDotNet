using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProxyServerDotNet.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProxyServerDotNet.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PortfolioController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PortfolioController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpPost("contact-form/submit")]
    public async Task<IActionResult> SubmitContactFormTemplate([FromBody] ContactFormRequest model)
    {
        try
        {
            //Getting Envs
            string targetEmail = Environment.GetEnvironmentVariable("BREVO_TARGET_EMAIL") ?? string.Empty;
            string brevoApiKey = Environment.GetEnvironmentVariable("BREVO_API_KEY") ?? string.Empty;
            string brevoURL = Environment.GetEnvironmentVariable("BREVO_API_URL") ?? string.Empty;
            string templateId = Environment.GetEnvironmentVariable("BREVO_TEMPLATE_ID") ?? string.Empty;

            if (string.IsNullOrWhiteSpace(targetEmail)
                || string.IsNullOrWhiteSpace(brevoApiKey)
                || string.IsNullOrWhiteSpace(brevoURL)
                || string.IsNullOrWhiteSpace(templateId))
            {
                throw new ArgumentException("Invalid Brevo Connection Env Variables");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            // Build payload according to Brevo's template specification
            var emailPayload = new
            {
                to = new[] { new { email = targetEmail, name = "Inbox Admin" } },
                replyTo = new { email = model.Email, name = model.Name },
                templateId = Convert.ToInt16(templateId),
                @params = new
                {
                    name = model.Name,
                    email = model.Email,
                    subject = model.Subject,
                    message = model.Message
                }
            };

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("api-key", brevoApiKey);

            var jsonContent = new StringContent(JsonSerializer.Serialize(emailPayload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(brevoURL, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return Ok(new { message = "Contact email queued via template!" });
            }

            var errorResponse = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, new { message = "Failed to send template email.", details = errorResponse });
        }
        catch (Exception exp)
        {
            return StatusCode(500, exp.Message);
        }
    }
}
