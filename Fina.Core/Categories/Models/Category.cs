using System.Text.Json.Serialization;

namespace Fina.Core.Categories.Models;

public class Category
{
    public Category()
    {
        
    }
    
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;
}