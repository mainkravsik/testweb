using System.ComponentModel.DataAnnotations;

namespace TestForMax.Models.DTOs;

public class CreateProjectDto
{
    [Required(ErrorMessage = "Название проекта обязательно")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 100 символов")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "URL проекта обязателен")]
    [Url(ErrorMessage = "Укажите корректный URL")]
    public string BaseUrl { get; set; } = string.Empty;
    
    public string? AuthToken { get; set; }
    
    public string? Cookies { get; set; }
}

public class UpdateProjectDto : CreateProjectDto
{
    // Наследуем все свойства от CreateProjectDto
}

public class ProjectListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
} 