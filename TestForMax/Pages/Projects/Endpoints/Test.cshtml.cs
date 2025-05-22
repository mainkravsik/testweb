using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestForMax.Models;
using TestForMax.Services;

namespace TestForMax.Pages.Projects.Endpoints;

public class TestModel : PageModel
{
    private readonly IApiEndpointService _apiEndpointService;
    private readonly IProjectService _projectService;
    private readonly IHttpClientFactory _clientFactory;

    public TestModel(
        IApiEndpointService apiEndpointService,
        IProjectService projectService,
        IHttpClientFactory clientFactory)
    {
        _apiEndpointService = apiEndpointService;
        _projectService = projectService;
        _clientFactory = clientFactory;
    }

    [BindProperty(SupportsGet = true)]
    public int ProjectId { get; set; }

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public Project Project { get; set; } = default!;
    public ApiEndpoint ApiEndpoint { get; set; } = default!;

    [BindProperty]
    public string RequestConfig { get; set; } = string.Empty;
    
    public string ResponseBody { get; set; } = string.Empty;
    public Dictionary<string, IEnumerable<string>> ResponseHeaders { get; set; } = new();
    public int StatusCode { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
    public bool HasResponse { get; set; } = false;
    public string RequestError { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync()
    {
        if (!await _projectService.ProjectExistsAsync(ProjectId))
        {
            return NotFound();
        }

        var apiEndpoint = await _apiEndpointService.GetApiEndpointByIdAsync(Id);
        if (apiEndpoint == null || apiEndpoint.ProjectId != ProjectId)
        {
            return NotFound();
        }

        Project = await _projectService.GetProjectByIdAsync(ProjectId) ?? new Project();
        ApiEndpoint = apiEndpoint;
        RequestConfig = apiEndpoint.RequestConfig ?? string.Empty;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!await _projectService.ProjectExistsAsync(ProjectId))
        {
            return NotFound();
        }

        var apiEndpoint = await _apiEndpointService.GetApiEndpointByIdAsync(Id);
        if (apiEndpoint == null || apiEndpoint.ProjectId != ProjectId)
        {
            return NotFound();
        }

        var project = await _projectService.GetProjectByIdAsync(ProjectId);
        if (project == null)
        {
            return NotFound();
        }
        
        Project = project;
        ApiEndpoint = apiEndpoint;

        // Создаем HttpClient
        var client = _clientFactory.CreateClient();
        
        // Добавляем токен авторизации, если он указан
        if (!string.IsNullOrEmpty(Project.AuthToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Project.AuthToken);
        }
        
        // Добавляем куки, если они указаны
        if (!string.IsNullOrEmpty(Project.Cookies))
        {
            client.DefaultRequestHeaders.Add("Cookie", Project.Cookies);
        }
        
        // Формируем URI запроса
        var requestUri = new Uri(new Uri(Project.BaseUrl), ApiEndpoint.Endpoint);
        
        // Создаем запрос
        var request = new HttpRequestMessage(new HttpMethod(ApiEndpoint.HttpMethod), requestUri);
        
        // Добавляем тело запроса, если оно указано и метод не GET
        if (!string.IsNullOrEmpty(RequestConfig) && 
            ApiEndpoint.HttpMethod != "GET" && 
            ApiEndpoint.HttpMethod != "HEAD")
        {
            request.Content = new StringContent(
                RequestConfig,
                Encoding.UTF8,
                "application/json");
        }
        
        try
        {
            var response = await client.SendAsync(request);
            
            // Читаем ответ
            ResponseBody = await response.Content.ReadAsStringAsync();
            ResponseHeaders = response.Headers
                .Concat(response.Content.Headers)
                .ToDictionary(h => h.Key, h => h.Value);
            StatusCode = (int)response.StatusCode;
            StatusMessage = response.StatusCode.ToString();
            HasResponse = true;
        }
        catch (Exception ex)
        {
            RequestError = ex.Message;
            HasResponse = true;
        }

        return Page();
    }
} 