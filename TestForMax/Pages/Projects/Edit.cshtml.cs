using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestForMax.Models.DTOs;
using TestForMax.Services;

namespace TestForMax.Pages.Projects;

public class EditModel : PageModel
{
    private readonly IProjectService _projectService;

    public EditModel(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [BindProperty]
    public UpdateProjectDto Project { get; set; } = default!;

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

        Project = new UpdateProjectDto
        {
            Name = project.Name,
            BaseUrl = project.BaseUrl,
            AuthToken = project.AuthToken,
            Cookies = project.Cookies
        };
        
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _projectService.UpdateProjectAsync(id, Project);
        if (result == null)
        {
            return NotFound();
        }

        return RedirectToPage("./Index");
    }
} 