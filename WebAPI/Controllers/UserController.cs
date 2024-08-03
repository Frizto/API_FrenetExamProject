using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.CQRS.User.Queries;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.User;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(ILogger<UserController> logger) : ControllerBase
{

    //[HttpPost("login")]
    //public async Task<IActionResult> LogInAdminUserAsync(
    //    [FromServices] ICommandHandler<LogInAdminUserCommand, ServiceResponse> handler,
    //    [FromBody] LogInAdminUserCommand command,
    //    CancellationToken cancellationToken)
    //{
    //    var result = await handler.Handle(command, cancellationToken);
    //    return Ok(result);
    //}

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

    //[HttpPost("refresh-token")]
    //public async Task<IActionResult> RefreshAdminUserTokenAsync(
    //    [FromServices] IQueryHandler<RefreshAdminUserTokenQuery, RefreshAdminUserTokenResultDTO> handler,
    //    [FromBody] RefreshAdminUserTokenQuery query,
    //    CancellationToken cancellationToken)
    //{
    //    var result = await handler.Handle(query, cancellationToken);
    //    return Ok(result);
    //}
}
