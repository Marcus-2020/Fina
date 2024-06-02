using System.Text.Json.Serialization;

namespace Fina.Core.Common.Responses;

public class Response<TData>
{
    private int _code;

    [JsonConstructor]
    public Response() => _code = Configuration.DefaultStatusCode;
    
    public Response(TData? data, 
        int code = Configuration.DefaultStatusCode, 
        string? message = null)
    {
        Data = data;
        _code = code;
        Message = message;
    }

    public int StausCode => _code;
    
    public TData? Data { get; private set; }
    public string? Message { get; private set; }
    
    [JsonIgnore]
    public bool IsSuccess => _code is >= 200 and <= 299;
}