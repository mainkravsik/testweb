using Microsoft.AspNetCore.Mvc;
using TestForMax.Models;
using TestForMax.Models.DTOs;
using TestForMax.Services;

namespace TestForMax.Controllers;

[Route("api/projects/{projectId}/endpoints")]
[ApiController]
public class ApiEndpointsController : ControllerBase
{
    private readonly IApiEndpointService _apiEndpointService;
    private readonly IProjectService _projectService;
    
    public ApiEndpointsController(
        IApiEndpointService apiEndpointService,
        IProjectService projectService)
    {
        _apiEndpointService = apiEndpointService;
        _projectService = projectService;
    }
    
    // GET: api/projects/5/endpoints
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ApiEndpointListDto>>> GetApiEndpoints(int projectId)
    {
        if (!await _projectService.ProjectExistsAsync(projectId))
        {
            return NotFound();
        }
        
        var apiEndpoints = await _apiEndpointService.GetApiEndpointsByProjectIdAsync(projectId);
        return Ok(apiEndpoints);
    }
    
    // GET: api/projects/5/endpoints/1
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiEndpoint>> GetApiEndpoint(int projectId, int id)
    {
        if (!await _projectService.ProjectExistsAsync(projectId))
        {
            return NotFound();
        }
        
        var apiEndpoint = await _apiEndpointService.GetApiEndpointByIdAsync(id);
        
        if (apiEndpoint == null || apiEndpoint.ProjectId != projectId)
        {
            return NotFound();
        }
        
        return apiEndpoint;
    }
    
    // POST: api/projects/5/endpoints
    [HttpPost]
    public async Task<ActionResult<ApiEndpoint>> CreateApiEndpoint(int projectId, CreateApiEndpointDto apiEndpointDto)
    {
        if (!await _projectService.ProjectExistsAsync(projectId))
        {
            return NotFound();
        }
        
        var apiEndpoint = await _apiEndpointService.CreateApiEndpointAsync(projectId, apiEndpointDto);
        
        return CreatedAtAction(
            nameof(GetApiEndpoint),
            new { projectId = projectId, id = apiEndpoint.Id },
            apiEndpoint);
    }
    
    // PUT: api/projects/5/endpoints/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApiEndpoint(int projectId, int id, UpdateApiEndpointDto apiEndpointDto)
    {
        if (!await _projectService.ProjectExistsAsync(projectId))
        {
            return NotFound();
        }
        
        if (!await _apiEndpointService.BelongsToProjectAsync(id, projectId))
        {
            return NotFound();
        }
        
        var apiEndpoint = await _apiEndpointService.UpdateApiEndpointAsync(id, apiEndpointDto);
        
        if (apiEndpoint == null)
        {
            return NotFound();
        }
        
        return NoContent();
    }
    
    // DELETE: api/projects/5/endpoints/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApiEndpoint(int projectId, int id)
    {
        if (!await _projectService.ProjectExistsAsync(projectId))
        {
            return NotFound();
        }
        
        if (!await _apiEndpointService.BelongsToProjectAsync(id, projectId))
        {
            return NotFound();
        }
        
        var result = await _apiEndpointService.DeleteApiEndpointAsync(id);
        
        if (!result)
        {
            return NotFound();
        }
        
        return NoContent();
    }
} 