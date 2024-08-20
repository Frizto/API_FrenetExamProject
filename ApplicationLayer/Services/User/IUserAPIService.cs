using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.CQRS.User.Queries;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.User;

namespace ApplicationLayer.Services.User;
public interface IUserAPIService
{
    Task<ServiceResponse> API_LogInUserAsync(LoginUserCommand command);
    Task<ServiceResponse> API_CreateUserAsync(CreateUserCommand command);
    Task<ServiceResponse> API_UpdateUserAsync(UpdateUserCommand command);
    Task<ServiceResponse> API_DeleteUserAsync(DeleteUserCommand command);

    Task<IEnumerable<ReadUserDTO>> API_ReadAllUsers(ReadUserQuery query);
}
