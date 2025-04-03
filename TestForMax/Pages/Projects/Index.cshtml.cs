using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestForMax.Models.DTOs;
using TestForMax.Services;

namespace TestForMax.Pages.Projects;

public class IndexModel : PageModel
{
    private readonly IProjectService _projectService;

    public IndexModel(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public IList<ProjectListDto> Projects { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Projects = (await _projectService.GetAllProjectsAsync()).ToList();
    }
} 