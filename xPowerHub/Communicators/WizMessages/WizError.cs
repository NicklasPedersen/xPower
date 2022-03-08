using System.Text.Json.Serialization;

namespace xPowerHub.Communicators.WizMessages;

public class WizError
{
    [JsonPropertyName("code")]
    public int Code { get; init; }
    [JsonPropertyName("message")]
    public string? Message { get; init; }
}
