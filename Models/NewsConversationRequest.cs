namespace PindahWebsite3.Models;

public class NewsConversationRequest
{
    public string Target { get; set; } = string.Empty;
    public string Instruction { get; set; } = string.Empty;
    public string? Heading { get; set; }
    public string? Content { get; set; }
}
