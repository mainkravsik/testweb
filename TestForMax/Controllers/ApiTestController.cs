using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using TestForMax.Models;
using TestForMax.Services;

namespace TestForMax.Controllers;

[Route("api/test")]
[ApiController]
public class ApiTestController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IApiEndpointService _apiEndpointService;
    private readonly IHttpClientFactory _clientFactory;
    
    public ApiTestController(
        IProjectService projectService,
        IApiEndpointService apiEndpointService,
        IHttpClientFactory clientFactory)
    {
        _projectService = projectService;
        _apiEndpointService = apiEndpointService;
        _clientFactory = clientFactory;
    }
    
    // POST: api/test/projects/5/endpoints/1
    [HttpPost("projects/{projectId}/endpoints/{endpointId}")]
    public async Task<IActionResult> TestApiEndpoint(int projectId, int endpointId)
    {
        var project = await _projectService.GetProjectByIdAsync(projectId);
        if (project == null)
        {
            return NotFound("Проект не найден");
        }
        
        var apiEndpoint = await _apiEndpointService.GetApiEndpointByIdAsync(endpointId);
        if (apiEndpoint == null || apiEndpoint.ProjectId != projectId)
        {
            return NotFound("API-эндпоинт не найден");
        }
        
        // Создаем HttpClient
        var client = _clientFactory.CreateClient();
        
        // Добавляем токен авторизации, если он указан
        if (!string.IsNullOrEmpty(project.AuthToken))
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", project.AuthToken);
        }
        
        // Добавляем куки, если они указаны
        if (!string.IsNullOrEmpty(project.Cookies))
        {
            client.DefaultRequestHeaders.Add("Cookie", project.Cookies);
        }
        
        // Формируем URI запроса
        var requestUri = new Uri(new Uri(project.BaseUrl), apiEndpoint.Endpoint);
        
        // Создаем и выполняем запрос
        var request = new HttpRequestMessage(new HttpMethod(apiEndpoint.HttpMethod), requestUri);
        
        // Добавляем тело запроса, если оно указано и метод не GET
        if (!string.IsNullOrEmpty(apiEndpoint.RequestConfig) && 
            apiEndpoint.HttpMethod != "GET" && 
            apiEndpoint.HttpMethod != "HEAD")
        {
            request.Content = new StringContent(
                apiEndpoint.RequestConfig,
                Encoding.UTF8,
                "application/json");
        }
        
        try
        {
            var response = await client.SendAsync(request);
            
            // Читаем ответ
            var responseBody = await response.Content.ReadAsStringAsync();
            
            // Формируем результат
            var headers = response.Headers
                .Concat(response.Content.Headers)
                .ToDictionary(h => h.Key, h => h.Value);

            var result = new
            {
                StatusCode = (int)response.StatusCode,
                StatusMessage = response.StatusCode.ToString(),
                Headers = headers,
                Body = responseBody
            };
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
} 