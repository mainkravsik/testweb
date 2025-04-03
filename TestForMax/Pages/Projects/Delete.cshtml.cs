using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestForMax.Models;
using TestForMax.Services;

namespace TestForMax.Pages.Projects;

public class DeleteModel : PageModel
{
    private readonly IProjectService _projectService;

    public DeleteModel(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [BindProperty]
    public Project Project { get; set; } = default!;

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
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!await _projectService.ProjectExistsAsync(id))
        {
            return NotFound();
        }

        await _projectService.DeleteProjectAsync(id);

        return RedirectToPage("./Index");
    }
} 