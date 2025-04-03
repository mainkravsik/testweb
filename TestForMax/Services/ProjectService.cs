using Microsoft.EntityFrameworkCore;
using TestForMax.Models;
using TestForMax.Models.DTOs;

namespace TestForMax.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;
    
    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ProjectListDto>> GetAllProjectsAsync()
    {
        return await _context.Projects
            .Select(p => new ProjectListDto
            {
                Id = p.Id,
                Name = p.Name,
                BaseUrl = p.BaseUrl,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }
    
    public async Task<Project?> GetProjectByIdAsync(int id)
    {
        return await _context.Projects
            .Include(p => p.ApiEndpoints)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
    
    public async Task<Project> CreateProjectAsync(CreateProjectDto projectDto)
    {
        var project = new Project
        {
            Name = projectDto.Name,
            BaseUrl = projectDto.BaseUrl,
            AuthToken = projectDto.AuthToken,
            Cookies = projectDto.Cookies,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        
        return project;
    }
    
    public async Task<Project?> UpdateProjectAsync(int id, UpdateProjectDto projectDto)
    {
        var project = await _context.Projects.FindAsync(id);
        
        if (project == null)
        {
            return null;
        }
        
        project.Name = projectDto.Name;
        project.BaseUrl = projectDto.BaseUrl;
        project.AuthToken = projectDto.AuthToken;
        project.Cookies = projectDto.Cookies;
        project.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        
        return project;
    }
    
    public async Task<bool> DeleteProjectAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        
        if (project == null)
        {
            return false;
        }
        
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        
        return true;
    }
    
    public async Task<bool> ProjectExistsAsync(int id)
    {
        return await _context.Projects.AnyAsync(p => p.Id == id);
    }
} 