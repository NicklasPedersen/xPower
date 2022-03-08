using System.Text.Json.Serialization;

namespace xPowerHub.Communicators.WizMessages;

public class WizResult
{
    [JsonPropertyName("state")]
    public bool? State { get; set; }
}
