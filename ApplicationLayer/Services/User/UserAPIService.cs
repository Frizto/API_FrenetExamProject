using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.CQRS.User.Queries;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.User;
using System.Net.Http.Json;

namespace ApplicationLayer.Services.User;
public class UserAPIService(HttpClient httpClient) : IUserAPIService
{
    public async Task<ServiceResponse> API_CreateUserAsync(CreateUserCommand command)
    {
        var data = await httpClient.PostAsJsonAsync("api/user/create-user", command);
        var response = await data.Content.ReadFromJsonAsync<ServiceResponse>();
        return response!;
    }

    public async Task<ServiceResponse> API_DeleteUserAsync(DeleteUserCommand command)
    {
        var data = await httpClient.PostAsJsonAsync("api/user/delete-user", command);
        var response = await data.Content.ReadFromJsonAsync<ServiceResponse>();
        return response!;
    }

    public async Task<ServiceResponse> API_LogInUserAsync(LoginUserCommand command)
    {
        var data = await httpClient.PostAsJsonAsync("api/user/login-user", command);
        var response = await data.Content.ReadFromJsonAsync<ServiceResponse>();
        return response!;
    }

    public async Task<IEnumerable<ReadUserDTO>> API_ReadAllUsers(ReadUserQuery query)
    {
        var data = await httpClient.GetFromJsonAsync<IEnumerable<ReadUserDTO>>("api/user/readall-users");
        var result = data!.ToList();
        return result;
    }

    public async Task<ServiceResponse> API_UpdateUserAsync(UpdateUserCommand command)
    {
        var data = await httpClient.PostAsJsonAsync("api/user/update-user", command);
        var response = await data.Content.ReadFromJsonAsync<ServiceResponse>();
        return response!;
    }
}
