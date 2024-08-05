using ApplicationLayer.CQRS.Interfaces;
using ApplicationLayer.CQRS.Shipment.Commands;
using ApplicationLayer.CQRS.Shipment.Queries;
using ApplicationLayer.DTOs;
using ApplicationLayer.DTOs.Shipment;
using ApplicationLayer.DTOs.User;
using DomainLayer.Enums;
using DomainLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.WebRequestMethods;

namespace WebAPI.Controllers;

/// <summary>
/// This is the controller responsible for managing Shipment Orders.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ShipmentController(HttpClient httpClient) : ControllerBase
{
    /// <summary>
    /// Create a new Shipment Order.
    /// </summary>
    /// <returns>A new Shipment Order.</returns>
    [HttpPost("create-shipment")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client))]
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
    [HttpPut("update-shipment")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client))]
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
    [HttpDelete("delete-shipment")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client))]
    public async Task<IActionResult> DeleteShipmentAsync(
        [FromServices] ICommandHandler<DeleteShipmentCommand, ServiceResponse> handler,
        [FromBody] DeleteShipmentCommand command,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets all Shipment Orders or a specific one if {guid} OrderId is provided.
    /// </summary>
    /// <returns>All or one Shipment Order.</returns>
    [HttpGet("readall")]
    [Authorize(Roles = nameof(AppUserTypeEnum.Client))]
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
    [HttpPost("shipment-price")]
    public async Task<IActionResult> ShipmentPrice(
        [FromServices] IQueryHandler<ShipmentPricingQuery, ShipmentPricingDTO> handler,
        [FromBody] ShipmentPricingQuery command,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleListAsync(command, cancellationToken);
        return Ok(result);
    }
}
