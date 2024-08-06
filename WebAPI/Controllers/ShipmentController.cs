using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.Shipment.Commands;
using ApplicationLayer.CQRS.Shipment.Queries;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Shipment;
using DomainLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

/// <summary>
/// This is the controller responsible for managing Shipment Orders.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ShipmentController : ControllerBase
{
    /// <summary>
    /// Create a new Shipment Order.
    /// </summary>
    /// <returns>A new Shipment Order.</returns>
    /// <example></example>
    /// <response code = "200" > OK: Operation Success!</response>
    /// <response code = "400" > Error: Bad Request!</response>
    /// <response code = "401" > Error: User is not authorized!</response>
    [HttpPost("create-shipment")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client) + "," + nameof(AppUserTypeEnum.Admin))]
    [ProducesResponseType(typeof(ServiceResponse), 200)]
    public async Task<IActionResult> CreateShipmentAsync(
        [FromServices] ICommandHandler<CreateShipmentCommand, ServiceResponse> handler,
        [FromBody] CreateShipmentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Update an existing Shipment with a valid {guid} Id.
    /// </summary>
    /// <returns>Update the Shipment Order.</returns>
    /// <example></example>
    /// <response code = "200" > OK: Operation Success!</response>
    /// <response code = "400" > Error: Bad Request!</response>
    /// <response code = "401" > Error: User is not authorized!</response>
    [HttpPut("update-shipment")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client) + "," + nameof(AppUserTypeEnum.Admin))]
    [ProducesResponseType(typeof(ServiceResponse), 200)]
    public async Task<IActionResult> UpdateShipmentAsync(
        [FromServices] ICommandHandler<UpdateShipmentCommand, ServiceResponse> handler,
        [FromBody] UpdateShipmentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Delete a Shipment Order with an valid {guid} Id.
    /// </summary>
    /// <returns>Delete the Shipment Order based on a given Id.</returns>
    /// <example></example>
    /// <response code = "200" > OK: Operation Success!</response>
    /// <response code = "400" > Error: Bad Request!</response>
    /// <response code = "401" > Error: User is not authorized!</response>
    [HttpDelete("delete-shipment")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client) + "," + nameof(AppUserTypeEnum.Admin))]
    [ProducesResponseType(typeof(ServiceResponse), 200)]
    public async Task<IActionResult> DeleteShipmentAsync(
        [FromServices] ICommandHandler<DeleteShipmentCommand, ServiceResponse> handler,
        [FromBody] DeleteShipmentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all Shipment Orders or a specific one if {guid} ShipmentId is provided.
    /// </summary>
    /// <returns>All or one Shipment Order.</returns>
    /// <example></example>
    /// <remarks>  
    /// If you provide a valid Id it will look in Database for the Shipment Order 
    /// otherwise it will return a list with them all.
    /// </remarks> 
    /// <response code = "200" > OK: Operation Success!</response>
    /// <response code = "400" > Error: Bad Request!</response>
    /// <response code = "401" > Error: User is not authorized!</response>
    [HttpGet("readall")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client) + "," + nameof(AppUserTypeEnum.Admin))]
    [ProducesResponseType(typeof(ReadShipmentDTO), 200)]
    public async Task<IActionResult> ReadAllShipmentsAsync(
        [FromServices] IQueryHandler<ReadShipmentQuery, ReadShipmentDTO> handler,
        [FromQuery] ReadShipmentQuery query,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleListAsync(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets the prices for a Shipment Order.
    /// </summary>
    /// <returns>The prices and delivery companies.</returns>
    /// <remarks>  
    /// If you provide a valid ShipmentId it will look in Database for the Shipment Order and get the prices for it and delivery companies.
    /// </remarks> 
    /// <response code = "200" > OK: Operation Success!</response>
    /// <response code = "400" > Error: Bad Request!</response>
    /// <response code = "401" > Error: User is not authorized!</response>
    [HttpPost("shipment-price")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client) + "," + nameof(AppUserTypeEnum.Admin))]
    [ProducesResponseType(typeof(ShipmentPricingDTO), 200)]
    public async Task<IActionResult> ShipmentPrice(
            [FromServices] IQueryHandler<ShipmentPricingQuery, ShipmentPricingDTO> handler,
            [FromQuery] ShipmentPricingQuery command,
            CancellationToken cancellationToken)
    {
        var result = await handler.HandleListAsync(command, cancellationToken);
        return Ok(result);
    }
}
