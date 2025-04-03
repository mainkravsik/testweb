using Microsoft.EntityFrameworkCore;
using TestForMax.Models;
using TestForMax.Models.DTOs;

namespace TestForMax.Services;

public class ApiEndpointService : IApiEndpointService
{
    private readonly ApplicationDbContext _context;
    
    public ApiEndpointService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ApiEndpointListDto>> GetApiEndpointsByProjectIdAsync(int projectId)
    {
        return await _context.ApiEndpoints
            .Where(a => a.ProjectId == projectId)
            .Select(a => new ApiEndpointListDto
            {
                Id = a.Id,
                HttpMethod = a.HttpMethod,
                Endpoint = a.Endpoint,
                CreatedAt = a.CreatedAt
            })
            .ToListAsync();
    }
    
    public async Task<ApiEndpoint?> GetApiEndpointByIdAsync(int id)
    {
        return await _context.ApiEndpoints.FindAsync(id);
    }
    
    public async Task<ApiEndpoint> CreateApiEndpointAsync(int projectId, CreateApiEndpointDto apiEndpointDto)
    {
        var apiEndpoint = new ApiEndpoint
        {
            HttpMethod = apiEndpointDto.HttpMethod,
            Endpoint = apiEndpointDto.Endpoint,
            RequestConfig = apiEndpointDto.RequestConfig,
            ProjectId = projectId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        
        _context.ApiEndpoints.Add(apiEndpoint);
        await _context.SaveChangesAsync();
        
        return apiEndpoint;
    }
    
    public async Task<ApiEndpoint?> UpdateApiEndpointAsync(int id, UpdateApiEndpointDto apiEndpointDto)
    {
        var apiEndpoint = await _context.ApiEndpoints.FindAsync(id);
        
        if (apiEndpoint == null)
        {
            return null;
        }
        
        apiEndpoint.HttpMethod = apiEndpointDto.HttpMethod;
        apiEndpoint.Endpoint = apiEndpointDto.Endpoint;
        apiEndpoint.RequestConfig = apiEndpointDto.RequestConfig;
        apiEndpoint.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        
        return apiEndpoint;
    }
    
    public async Task<bool> DeleteApiEndpointAsync(int id)
    {
        var apiEndpoint = await _context.ApiEndpoints.FindAsync(id);
        
        if (apiEndpoint == null)
        {
            return false;
        }
        
        _context.ApiEndpoints.Remove(apiEndpoint);
        await _context.SaveChangesAsync();
        
        return true;
    }
    
    public async Task<bool> ApiEndpointExistsAsync(int id)
    {
        return await _context.ApiEndpoints.AnyAsync(a => a.Id == id);
    }
    
    public async Task<bool> BelongsToProjectAsync(int id, int projectId)
    {
        return await _context.ApiEndpoints.AnyAsync(a => a.Id == id && a.ProjectId == projectId);
    }
} 