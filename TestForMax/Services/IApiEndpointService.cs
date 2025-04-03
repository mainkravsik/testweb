using TestForMax.Models;
using TestForMax.Models.DTOs;

namespace TestForMax.Services;

public interface IApiEndpointService
{
    Task<IEnumerable<ApiEndpointListDto>> GetApiEndpointsByProjectIdAsync(int projectId);
    Task<ApiEndpoint?> GetApiEndpointByIdAsync(int id);
    Task<ApiEndpoint> CreateApiEndpointAsync(int projectId, CreateApiEndpointDto apiEndpointDto);
    Task<ApiEndpoint?> UpdateApiEndpointAsync(int id, UpdateApiEndpointDto apiEndpointDto);
    Task<bool> DeleteApiEndpointAsync(int id);
    Task<bool> ApiEndpointExistsAsync(int id);
    Task<bool> BelongsToProjectAsync(int id, int projectId);
} 