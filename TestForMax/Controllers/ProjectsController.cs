using Microsoft.AspNetCore.Mvc;
using TestForMax.Models;
using TestForMax.Models.DTOs;
using TestForMax.Services;

namespace TestForMax.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    
    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }
    
    // GET: api/Projects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectListDto>>> GetProjects()
    {
        var projects = await _projectService.GetAllProjectsAsync();
        return Ok(projects);
    }
    
    // GET: api/Projects/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        
        if (project == null)
        {
            return NotFound();
        }
        
        return project;
    }
    
    // POST: api/Projects
    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject(CreateProjectDto projectDto)
    {
        var project = await _projectService.CreateProjectAsync(projectDto);
        
        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }
    
    // PUT: api/Projects/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, UpdateProjectDto projectDto)
    {
        var project = await _projectService.UpdateProjectAsync(id, projectDto);
        
        if (project == null)
        {
            return NotFound();
        }
        
        return NoContent();
    }
    
    // DELETE: api/Projects/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var result = await _projectService.DeleteProjectAsync(id);
        
        if (!result)
        {
            return NotFound();
        }
        
        return NoContent();
    }
} 