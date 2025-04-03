using System.ComponentModel.DataAnnotations;

namespace TestForMax.Models.DTOs;

public class CreateApiEndpointDto
{
    [Required(ErrorMessage = "HTTP метод обязателен")]
    public string HttpMethod { get; set; } = "GET";
    
    [Required(ErrorMessage = "Эндпоинт обязателен")]
    public string Endpoint { get; set; } = string.Empty;
    
    public string? RequestConfig { get; set; }
}

public class UpdateApiEndpointDto : CreateApiEndpointDto
{
    // Наследуем все свойства от CreateApiEndpointDto
}

public class ApiEndpointListDto
{
    public int Id { get; set; }
    public string HttpMethod { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
} 