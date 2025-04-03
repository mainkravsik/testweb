using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestForMax.Models;

public class Project
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Название проекта обязательно")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 100 символов")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "URL проекта обязателен")]
    [Url(ErrorMessage = "Укажите корректный URL")]
    public string BaseUrl { get; set; } = string.Empty;
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AuthToken { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Cookies { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<ApiEndpoint> ApiEndpoints { get; set; } = new List<ApiEndpoint>();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
} 