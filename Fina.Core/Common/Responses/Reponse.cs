using System.Text.Json.Serialization;

namespace Fina.Core.Common.Responses;

public class Response<TData>
{
    [JsonConstructor]
    public Response(TData? data, 
        int statusCode = Configuration.DefaultStatusCode, 
        string? message = null)
    {
        Data = data;
        StatusCode = statusCode;
        Message = message;
    }
    
    private int _statusCode;
    [JsonPropertyName("statusCode")]
    public int StatusCode
    {
        get => _statusCode;
        private set => _statusCode = value;
    }
    
    [JsonPropertyName("data")]
    public TData? Data { get; private set; }
    
    [JsonPropertyName("message")]
    public string? Message { get; private set; }
    
    [JsonIgnore]
    public bool IsSuccess => _statusCode is >= 200 and <= 299;
}