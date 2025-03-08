using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.CQRS.User.Queries;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.User;
using System.Net.Http.Json;

namespace ApplicationLayer.Services.User;
public class UserAPIService : IUserAPIService
{
    private readonly HttpClient _httpClient;

    public UserAPIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ServiceResponse> API_CreateUserAsync(CreateUserCommand command)
    {
        try
        {
            var data = await _httpClient.PostAsJsonAsync($"api/User/create-user", command);
            var response = await data.Content.ReadFromJsonAsync<ServiceResponse>();
            return response!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request Error: {ex.Message}");
            throw;
        }
    }

    public async Task<ServiceResponse> API_DeleteUserAsync(DeleteUserCommand command)
    {
        try
        {
            var data = await _httpClient.PostAsJsonAsync($"api/User/delete-user", command);
            var response = await data.Content.ReadFromJsonAsync<ServiceResponse>();
            return response!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request Error: {ex.Message}");
            throw;
        }
    }

    public async Task<ServiceResponse> API_LogInUserAsync(LoginUserCommand command)
    {
        try
        {
            var data = await _httpClient.PostAsJsonAsync($"api/User/login-user", command);
            var response = await data.Content.ReadFromJsonAsync<ServiceResponse>();
            return response!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request Error: {ex.Message}");
            throw;
        }
    }

    public async Task<List<ReadUserDTO>> API_ReadAllUsers(ReadUserQuery query)
    {
        try
        {
            var data = await _httpClient.PostAsJsonAsync($"api/User/readall-users", query);
            var response = await data.Content.ReadFromJsonAsync<IEnumerable<ReadUserDTO>>();
            var result = response.ToList();
            return result!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request Error: {ex.Message}");
            throw;
        }
    }

    public async Task<ServiceResponse> API_UpdateUserAsync(UpdateUserCommand command)
    {
        try
        {
            var data = await _httpClient.PostAsJsonAsync($"api/User/update-user", command);
            var response = await data.Content.ReadFromJsonAsync<ServiceResponse>();
            return response!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request Error: {ex.Message}");
            throw;
        }
    }
}
