namespace PindahWebsite3.Models;

public class ChatAgentRequest
{
    public List<ChatAgentMessage> Messages { get; set; } = [];
}

public class ChatAgentMessage
{
    public string Role { get; set; } = "user";
    public string Content { get; set; } = string.Empty;
}

public class WhatsAppHandoffPayload
{
    public string Summary { get; set; } = string.Empty;
    public List<string> Features { get; set; } = [];
    public string Pricing { get; set; } = string.Empty;
}
