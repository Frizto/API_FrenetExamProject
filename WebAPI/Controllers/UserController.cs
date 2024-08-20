using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.CQRS.User.Queries;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.User;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Authorization;
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
    /// Logs in an user and generate a valid a JWT Token from service.
    /// </summary>
    /// <returns>A logged user and a JWT Token for Authorization.</returns>
    /// <example>description</example>
    /// <remarks>  
    /// It will log an User in and generate a valid JWT Token for Authorization.
    /// </remarks> 
    /// <response code = "200" > OK: Operation Success!</response>
    /// <response code = "400" > Error: Bad Request!</response>
    [HttpPost("login-user")]
    [ProducesResponseType(typeof(ServiceResponse), 200)]
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
    /// <returns>A new User based on the specified role.</returns>
    /// <example>description</example>
    /// <response code = "200" > OK: Operation Success!</response>
    /// <response code = "400" > Error: Bad Request!</response>
    /// <remarks>The Create operation can create an User based on role, 0: Admin - 1: Client</remarks>
    [HttpPost("create-user")]
    [ProducesResponseType(typeof(ServiceResponse), 200)]
    public async Task<IActionResult> CreateUserAsync(
        [FromServices] ICommandHandler<CreateUserCommand, ServiceResponse> handler,
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing User with a valid {int} Id.
    /// </summary>
    /// <returns>Update the User's basic information.</returns>
    /// <example>description</example>
    /// <response code = "200" > OK: Operation Success!</response>
    /// <response code = "400" > Error: Bad Request!</response>
    /// <response code = "401" > Error: User is not authorized!</response>
    /// <remarks>The Update operation is only allowed for the current logged user.</remarks>
    [HttpPut("update-user")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client))]
    [ProducesResponseType(typeof(ServiceResponse), 200)]
    public async Task<IActionResult> UpdateUserAsync(
        [FromServices] ICommandHandler<UpdateUserCommand, ServiceResponse> handler,
        [FromBody] UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Delete a User with an valid {guid} Id.
    /// </summary>
    /// <returns>Delete the User based on a given Id.</returns>
    /// <example>description</example>
    /// <response code = "200" > OK: Operation Success!</response>
    /// <response code = "400" > Error: Bad Request!</response>
    /// <response code = "401" > Error: User is not authorized!</response>
    /// <remarks>The Delete operation also removes the AspUser that is the reason to use a Guid.</remarks>
    [HttpDelete("delete-user")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client) + "," + nameof(AppUserTypeEnum.Admin))]
    [ProducesResponseType(typeof(ServiceResponse), 200)]
    public async Task<IActionResult> DeleteUserAsync(
        [FromServices] ICommandHandler<DeleteUserCommand, ServiceResponse> handler,
        [FromBody] DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all clients or a specific one if {int} Id is provided.
    /// </summary>
    /// <returns>All or one user.</returns>
    /// <example>description</example>
    /// <remarks>  
    /// If you provide a valid Id it will look in Database for the User 
    /// otherwise it will return a list with them all.
    /// </remarks> 
    /// <response code="200">OK: Operation Success!</response>
    /// <response code="400">Error: Bad Request!</response>
    /// <response code="401">Error: User is not authorized!</response>
    [HttpGet("readall-users")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client) + "," + nameof(AppUserTypeEnum.Admin))]
    [ProducesResponseType(typeof(ReadUserDTO), 200)]
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
    /// <returns>A valid new JWT Token.</returns>
    /// <example>description</example>
    /// <remarks>  
    /// It will refresh the current User's token, the user must be authenticated.
    /// </remarks> 
    /// <response code="200">OK: Operation Success!</response>
    /// <response code="400">Error: Bad Request!</response>
    /// <response code="401">Error: User is not authorized!</response>
    [HttpPost("refresh-token")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client) + "," + nameof(AppUserTypeEnum.Admin))]
    [ProducesResponseType(typeof(TokenResultDTO), 200)]
    public async Task<IActionResult> RefreshUserTokenAsync(
        [FromServices] IQueryHandler<RefreshUserTokenQuery, TokenResultDTO> handler,
        [FromBody] RefreshUserTokenQuery query,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }
}
