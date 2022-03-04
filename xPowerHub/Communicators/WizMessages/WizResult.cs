using System.Text.Json.Serialization;

namespace xPowerHub.Communicators.WizMessages;

internal class WizResult
{
    [JsonPropertyName("state")]
    public bool? State { get; set; }
}
