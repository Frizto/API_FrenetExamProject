using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.CQRS.User.Queries;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

/// <summary>
/// This is the controller responsible for managing users.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    /// <summary>
    /// Logs in a user and generate a valid a JWT Token from service.
    /// </summary>
    /// <param name="command">The Login command of the user.</param>
    /// <returns>A logged user and a JWT Token for Authorization.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginUserAsync(
        [FromServices] ICommandHandler<LoginUserCommand, ServiceResponse> handler,
        [FromBody] LoginUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create a new User.
    /// </summary>
    /// <param name="command">The Create command of the user.</param>
    /// <returns>A new User based on the specified role.</returns>
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = nameof(AppPoliciesEnum.SystemLevelPolicy))]
    [HttpPost("create")]
    public async Task<IActionResult> CreateUserAsync(
        [FromServices] ICommandHandler<CreateUserCommand, ServiceResponse> handler,
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing User.
    /// </summary>
    /// <param name="command">The Update command of the user.</param>
    /// <returns>Update the User's basic information.</returns>
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUserAsync(
        [FromServices] ICommandHandler<UpdateUserCommand, ServiceResponse> handler,
        [FromBody] UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Delete a User with an valid Id.
    /// </summary>
    /// <param name="command">The Delete command of the user.</param>
    /// <returns>Delete the User based on a given Id.</returns>
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteUserAsync(
        [FromServices] ICommandHandler<DeleteUserCommand, ServiceResponse> handler,
        [FromBody] DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all clients or a specific one if Id is provided.
    /// </summary>
    /// <param name="query">The Read query for the users.</param>
    /// <returns>All or one user.</returns>
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("readall")]
    public async Task<IActionResult> ReadAllUsersAsync(
        [FromServices] IQueryHandler<ReadUserQuery, ReadUserDTO> handler,
        [FromQuery] ReadUserQuery query,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleListAsync(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Refreshes the current User Token.
    /// </summary>
    /// <param name="query">The Refresh Toke Query of the user.</param>
    /// <returns>A valid new JWT Token.</returns>
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshUserTokenAsync(
        [FromServices] IQueryHandler<RefreshUserTokenQuery, TokenResultDTO> handler,
        [FromBody] RefreshUserTokenQuery query,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }
}
