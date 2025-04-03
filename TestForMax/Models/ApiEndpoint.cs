using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestForMax.Models;

public class ApiEndpoint
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "HTTP метод обязателен")]
    public string HttpMethod { get; set; } = "GET";
    
    [Required(ErrorMessage = "Эндпоинт обязателен")]
    public string Endpoint { get; set; } = string.Empty;
    
    [Column(TypeName = "jsonb")]
    public string? RequestConfig { get; set; }
    
    public int ProjectId { get; set; }
    
    [ForeignKey("ProjectId")]
    public virtual Project Project { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
} 