using System.Text.Json.Serialization;

namespace xPowerHub.Communicators.WizMessages;

internal class WizParameters
{
    [JsonPropertyName("state")]
    public bool? State { get; init; }
    [JsonPropertyName("mac")]
    public string? MacAddress { get; init; }
    [JsonPropertyName("homeId")]
    public int? HomeID { get; init; }
    [JsonPropertyName("fwVersion")]
    public string? FirmwareVersion { get; init; }
}
