using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace PindahWebsite3.Services;

public class OllamaChatService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OllamaChatService> _logger;

    public OllamaChatService(HttpClient httpClient, IConfiguration configuration, ILogger<OllamaChatService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["Ollama:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogError("Ollama API key not configured");
            return string.Empty;
        }

        var endpoint = _configuration["Ollama:Endpoint"] ?? "https://ollama.com/api/chat";
        var model = _configuration["Ollama:Model"] ?? "gpt-oss:120b";

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = JsonContent.Create(new OllamaChatRequest
        {
            Model = model,
            Messages = [new OllamaChatMessage { Role = "user", Content = prompt }],
            Stream = false
        });

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Ollama API returned {StatusCode}: {Body}", (int)response.StatusCode, body);
            return string.Empty;
        }

        var result = await response.Content.ReadFromJsonAsync<OllamaChatResponse>(cancellationToken);
        return result?.Message?.Content?.Trim() ?? string.Empty;
    }

    private sealed class OllamaChatRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("messages")]
        public OllamaChatMessage[] Messages { get; set; } = [];

        [JsonPropertyName("stream")]
        public bool Stream { get; set; }
    }

    private sealed class OllamaChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }

    private sealed class OllamaChatResponse
    {
        [JsonPropertyName("message")]
        public OllamaChatMessage? Message { get; set; }
    }
}
