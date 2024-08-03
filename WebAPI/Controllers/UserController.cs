using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.User.Commands;
using ApplicationLayer.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
    public async Task<IActionResult> CreateAdminUserAsync(
        [FromServices] ICommandHandler<CreateUserCommand, ServiceResponse> handler,
        [FromBody] CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = nameof(AppPoliciesEnum.SystemLevelPolicy))]
    //[HttpPut("update")]
    //public async Task<IActionResult> UpdateAdminUserAsync(
    //    [FromServices] ICommandHandler<UpdateAdminUserCommand, ServiceResponse> handler,
    //    [FromBody] UpdateAdminUserCommand command,
    //    CancellationToken cancellationToken)
    //{
    //    var result = await handler.Handle(command, cancellationToken);
    //    return Ok(result);
    //}

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = nameof(AppPoliciesEnum.SystemLevelPolicy))]
    //[HttpDelete("delete")]
    //public async Task<IActionResult> DeleteAdminUserAsync(
    //    [FromServices] ICommandHandler<DeleteAdminUserCommand, ServiceResponse> handler,
    //    [FromBody] DeleteAdminUserCommand command,
    //    CancellationToken cancellationToken)
    //{
    //    var result = await handler.Handle(command, cancellationToken);
    //    return Ok(result);
    //}

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = nameof(AppPoliciesEnum.SystemLevelPolicy))]
    //[HttpGet("readall")]
    //public async Task<IActionResult> ReadAllAdminUsersAsync(
    //    [FromServices] IQueryHandler<ReadAdminUserQuery, ReadAdminUserResultDTO> handler,
    //    [FromQuery] ReadAdminUserQuery query,
    //    CancellationToken cancellationToken)
    //{
    //    var result = await handler.HandleListAsync(query, cancellationToken);
    //    return Ok(result);
    //}

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
