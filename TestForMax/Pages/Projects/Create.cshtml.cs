using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestForMax.Models.DTOs;
using TestForMax.Services;

namespace TestForMax.Pages.Projects;

public class CreateModel : PageModel
{
    private readonly IProjectService _projectService;

    public CreateModel(IProjectService projectService)
    {
        _projectService = projectService;
    }

    [BindProperty]
    public CreateProjectDto Project { get; set; } = default!;

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        await _projectService.CreateProjectAsync(Project);

        return RedirectToPage("./Index");
    }
} 