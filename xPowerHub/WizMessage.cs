using System.Text.Json.Serialization;
using System.Text.Json;

namespace xPowerHub;

public class WizMessage
{
    enum EMethod
    {
        setState,
        getPilot,
        firstBeat,
        getModelConfig,
    }

    public class WizParameters
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
    public class WizError
    {
        [JsonPropertyName("code")]
        public int Code { get; init; }
        [JsonPropertyName("message")]
        public string? Message { get; init; }
    }

    public class WizResult
    {
        [JsonPropertyName("state")]
        public bool? State { get; set; }
    }

    private EMethod method;
    [JsonPropertyName("method")]

    public string Method
    {
        get => method.ToString();
        set {
            if (Enum.TryParse(value, out EMethod m))
            {
                method = m;
            } 
            else
            {
                throw new InvalidDataException("No such Wiz method " + value);
            }
        }
    }
    
    [JsonPropertyName("params")]
    public WizParameters? Parameters { get; set; }
    [JsonPropertyName("error")]
    public WizError? Error { get; set; }
    [JsonPropertyName("result")]
    public WizResult? Result { get; set; }
    [JsonPropertyName("env")]
    public string? Environment { get; set; }
    public static WizMessage SetState(bool b)
    {
        return new WizMessage
        {
            method = EMethod.setState,
            Parameters = new WizParameters
            {
                State = b
            }
        };
    }
    public static WizMessage GetPilot()
    {
        return new WizMessage
        {
            method = EMethod.getPilot,
        };
    }

    public string ToJSON()
    {
        var opts = new JsonSerializerOptions
        {
            // WiZ devices ignore you if you send any null values
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        return JsonSerializer.Serialize(this, opts);
    }

    public static WizMessage FromJSON(string json)
    {
        return JsonSerializer.Deserialize<WizMessage>(json)!;
    }
}
