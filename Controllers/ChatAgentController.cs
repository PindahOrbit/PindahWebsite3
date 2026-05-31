using Microsoft.AspNetCore.Mvc;
using PindahWebsite3.Models;
using PindahWebsite3.Services;
using System.Text.Json;

namespace PindahWebsite3.Controllers;

[Route("[controller]")]
public class ChatAgentController : Controller
{
    private readonly SalesAgentService _salesAgentService;
    private readonly ILogger<ChatAgentController> _logger;

    public ChatAgentController(SalesAgentService salesAgentService, ILogger<ChatAgentController> logger)
    {
        _salesAgentService = salesAgentService;
        _logger = logger;
    }

    [HttpPost("stream")]
    public async Task Stream([FromBody] ChatAgentRequest? request, CancellationToken cancellationToken)
    {
        if (request?.Messages == null || request.Messages.Count == 0)
        {
            Response.StatusCode = StatusCodes.Status400BadRequest;
            await Response.WriteAsync("At least one message is required.", cancellationToken);
            return;
        }

        Response.StatusCode = StatusCodes.Status200OK;
        Response.ContentType = "text/event-stream";
        Response.Headers.CacheControl = "no-cache";
        Response.Headers.Connection = "keep-alive";
        Response.Headers.Append("X-Accel-Buffering", "no");

        try
        {
            await foreach (var chunk in _salesAgentService.StreamReplyAsync(request.Messages, cancellationToken))
            {
                var payload = JsonSerializer.Serialize(new
                {
                    content = chunk.Content,
                    thinking = chunk.Thinking,
                    done = chunk.Done
                });

                await Response.WriteAsync($"data: {payload}\n\n", cancellationToken);
                await Response.Body.FlushAsync(cancellationToken);
            }

            await Response.WriteAsync("data: {\"done\":true}\n\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Sales chat stream cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sales chat stream failed.");
            var errorPayload = JsonSerializer.Serialize(new { error = "Chat unavailable. Please try again or contact us on WhatsApp." });
            await Response.WriteAsync($"data: {errorPayload}\n\n", cancellationToken);
            await Response.Body.FlushAsync(cancellationToken);
        }
    }

    [HttpPost("whatsapp")]
    public IActionResult BuildWhatsAppLink([FromBody] WhatsAppHandoffPayload? handoff)
    {
        if (handoff == null || string.IsNullOrWhiteSpace(handoff.Summary))
        {
            return BadRequest(new { error = "Handoff summary is required." });
        }

        var url = _salesAgentService.BuildWhatsAppUrl(handoff);
        return Ok(new { url });
    }
}
