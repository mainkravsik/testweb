using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestForMax.Models;
using TestForMax.Models.DTOs;
using TestForMax.Services;

namespace TestForMax.Pages.Projects.Endpoints;

public class CreateModel : PageModel
{
    private readonly IApiEndpointService _apiEndpointService;
    private readonly IProjectService _projectService;

    public CreateModel(
        IApiEndpointService apiEndpointService,
        IProjectService projectService)
    {
        _apiEndpointService = apiEndpointService;
        _projectService = projectService;
    }

    [BindProperty(SupportsGet = true)]
    public int ProjectId { get; set; }

    [BindProperty]
    public CreateApiEndpointDto ApiEndpoint { get; set; } = default!;
    
    public Project? CurrentProject { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        if (!await _projectService.ProjectExistsAsync(ProjectId))
        {
            return NotFound();
        }
        
        CurrentProject = await _projectService.GetProjectByIdAsync(ProjectId);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!await _projectService.ProjectExistsAsync(ProjectId))
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _apiEndpointService.CreateApiEndpointAsync(ProjectId, ApiEndpoint);

        return RedirectToPage("/Projects/Details", new { id = ProjectId });
    }
} 