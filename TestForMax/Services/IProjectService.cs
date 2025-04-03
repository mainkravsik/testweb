using TestForMax.Models;
using TestForMax.Models.DTOs;

namespace TestForMax.Services;

public interface IProjectService
{
    Task<IEnumerable<ProjectListDto>> GetAllProjectsAsync();
    Task<Project?> GetProjectByIdAsync(int id);
    Task<Project> CreateProjectAsync(CreateProjectDto projectDto);
    Task<Project?> UpdateProjectAsync(int id, UpdateProjectDto projectDto);
    Task<bool> DeleteProjectAsync(int id);
    Task<bool> ProjectExistsAsync(int id);
} 