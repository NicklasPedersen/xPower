using System.Text.Json.Serialization;
using System.Text.Json;

namespace xPowerHub.Communicators.WizMessages;

public class WizMessage
{
    public class WizResult
    {
    }

    private WizMethod method;
    [JsonPropertyName("method")]

    public string Method
    {
        get => method.ToString();
        set {
            if (Enum.TryParse(value, out WizMethod m))
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
            method = WizMethod.setState,
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
            method = WizMethod.getPilot,
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

    public static WizMessage? FromJSON(string json)
    {
        return JsonSerializer.Deserialize<WizMessage>(json);
    }
}
