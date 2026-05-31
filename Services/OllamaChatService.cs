using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PindahWebsite3.Services;

public class OllamaChatService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<OllamaChatService> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public OllamaChatService(HttpClient httpClient, IConfiguration configuration, ILogger<OllamaChatService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken cancellationToken = default)
    {
        var builder = new System.Text.StringBuilder();
        await foreach (var chunk in StreamGenerateAsync(prompt, cancellationToken))
        {
            if (!string.IsNullOrEmpty(chunk.Content))
            {
                builder.Append(chunk.Content);
            }
        }

        return builder.ToString().Trim();
    }

    public async IAsyncEnumerable<OllamaStreamChunk> StreamGenerateAsync(
        string prompt,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var chunk in StreamChatAsync(
            [new OllamaChatMessage { Role = "user", Content = prompt }],
            cancellationToken))
        {
            yield return chunk;
        }
    }

    public async IAsyncEnumerable<OllamaStreamChunk> StreamChatAsync(
        IReadOnlyList<OllamaChatMessage> messages,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var apiKey = _configuration["Ollama:ApiKey"];
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogError("Ollama API key not configured");
            yield break;
        }

        if (messages.Count == 0)
        {
            yield break;
        }

        var endpoint = _configuration["Ollama:Endpoint"] ?? "https://ollama.com/api/chat";
        var model = _configuration["Ollama:Model"] ?? "gpt-oss:120b";

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = JsonContent.Create(new OllamaChatRequest
        {
            Model = model,
            Messages = messages.Select(m => new OllamaApiMessage
            {
                Role = m.Role,
                Content = m.Content
            }).ToArray(),
            Stream = true
        });

        using var response = await _httpClient.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Ollama API returned {StatusCode}: {Body}", (int)response.StatusCode, body);
            yield break;
        }

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        using var reader = new StreamReader(stream);

        while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
        {
            var line = await reader.ReadLineAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            OllamaChatResponse? parsed;
            try
            {
                parsed = JsonSerializer.Deserialize<OllamaChatResponse>(line, JsonOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Failed to parse Ollama stream line");
                continue;
            }

            if (parsed?.Message == null)
            {
                continue;
            }

            yield return new OllamaStreamChunk
            {
                Content = parsed.Message.Content ?? string.Empty,
                Thinking = parsed.Message.Thinking ?? string.Empty,
                Done = parsed.Done
            };

            if (parsed.Done)
            {
                yield break;
            }
        }
    }

    private sealed class OllamaChatRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = string.Empty;

        [JsonPropertyName("messages")]
        public OllamaApiMessage[] Messages { get; set; } = [];

        [JsonPropertyName("stream")]
        public bool Stream { get; set; }
    }

    private sealed class OllamaApiMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;

        [JsonPropertyName("thinking")]
        public string? Thinking { get; set; }
    }

    private sealed class OllamaChatResponse
    {
        [JsonPropertyName("message")]
        public OllamaApiMessage? Message { get; set; }

        [JsonPropertyName("done")]
        public bool Done { get; set; }
    }
}

public class OllamaChatMessage
{
    public string Role { get; set; } = "user";
    public string Content { get; set; } = string.Empty;
}

public class OllamaStreamChunk
{
    public string Content { get; set; } = string.Empty;
    public string Thinking { get; set; } = string.Empty;
    public bool Done { get; set; }
}
