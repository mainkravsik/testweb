using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestForMax.Models;
using TestForMax.Models.DTOs;
using TestForMax.Services;

namespace TestForMax.Pages.Projects;

public class DetailsModel : PageModel
{
    private readonly IProjectService _projectService;
    private readonly IApiEndpointService _apiEndpointService;

    public DetailsModel(
        IProjectService projectService, 
        IApiEndpointService apiEndpointService)
    {
        _projectService = projectService;
        _apiEndpointService = apiEndpointService;
    }

    public Project Project { get; set; } = default!;
    public IList<ApiEndpointListDto> ApiEndpoints { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var project = await _projectService.GetProjectByIdAsync(id.Value);
        if (project == null)
        {
            return NotFound();
        }
        
        Project = project;
        ApiEndpoints = (await _apiEndpointService.GetApiEndpointsByProjectIdAsync(id.Value)).ToList();
        
        return Page();
    }
} 
 