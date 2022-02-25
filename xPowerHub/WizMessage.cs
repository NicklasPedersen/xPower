using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Runtime.Serialization;

namespace ConsoleApp4;

public enum EMethod
{
    setState,
    getPilot,
    firstBeat,
    getModelConfig,
}
public class Params
{
    [JsonPropertyName("state")]
    public bool? State { get; set; }
    [JsonPropertyName("mac")]
    public string? MacAddress { get; set; }
    [JsonPropertyName("homeId")]
    public int? HomeID { get; set; }
    [JsonPropertyName("fwVersion")]
    public string? FirmwareVersion { get; set; }
}
public class WizError
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("message")]
    public string? Message { get; set; }
}

public class WizResult
{
    [JsonPropertyName("state")]
    public bool? State { get; set; }
}
public class WizMessage
{
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
                throw new Exception("No such method " + value);
            }
        }
    }
    
    [JsonPropertyName("params")]
    public Params? Params { get; set; }
    [JsonPropertyName("error"), DataMember(EmitDefaultValue = false)]
    public WizError? Error { get; set; }
    [JsonPropertyName("result"), DataMember(EmitDefaultValue = false)]
    public WizResult? Result { get; set; }
    [JsonPropertyName("env")]
    public string? Environment { get; set; }
    public static WizMessage SetState(bool b)
    {
        return new WizMessage
        {
            method = EMethod.setState,
            Params = new Params
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
        var opts = new JsonSerializerOptions();
        opts.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        return JsonSerializer.Serialize(this, opts);
    }

    public static WizMessage FromJSON(string json)
    {
        return JsonSerializer.Deserialize<WizMessage>(json)!;
    }
}
