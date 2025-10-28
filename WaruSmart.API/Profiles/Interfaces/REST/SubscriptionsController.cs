using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using WaruSmart.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using WaruSmart.API.Profiles.Domain.Model.Queries;
using WaruSmart.API.Profiles.Domain.Services;
using WaruSmart.API.Profiles.Interfaces.REST.Resources;
using WaruSmart.API.Profiles.Interfaces.REST.Transform;

namespace WaruSmart.API.Profiles.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class SubscriptionsController(
    ISubscriptionCommandService subscriptionCommandService,
    ISubscriptionQueryService subscriptionQueryService) : ControllerBase
{
    /// <summary>
    /// Get all subscriptions - Accessible to all authenticated users
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllSubscriptions()
    {
        var getAllSubscriptionsQuery = new GetAllSubscriptionsQuery();
        var subscriptions = await subscriptionQueryService.Handle(getAllSubscriptionsQuery);
        var resources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    /// <summary>
    /// Get subscription by ID - Accessible to all authenticated users
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSubscriptionById(int id)
    {
        var getSubscriptionByIdQuery = new GetSubscriptionByIdQuery(id);
        var subscription = await subscriptionQueryService.Handle(getSubscriptionByIdQuery);
        if (subscription == null) return NotFound();
        var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
        return Ok(resource);
    }

    /// <summary>
    /// Create a new subscription - Only accessible to ADMINISTRADOR_WARU_SMART
    /// </summary>
    [HttpPost]
    [Authorize("ADMINISTRADOR_WARU_SMART")]
    public async Task<IActionResult> CreateSubscription([FromBody] CreateSubscriptionResource resource)
    {
        var createSubscriptionCommand = CreateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(resource);
        var subscription = await subscriptionCommandService.Handle(createSubscriptionCommand);
        if (subscription is null) return BadRequest();
        var subscriptionResource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
        return CreatedAtAction(nameof(GetSubscriptionById), new { id = subscriptionResource.Id }, subscriptionResource);
    }

    /// <summary>
    /// Update an existing subscription - Only accessible to ADMINISTRADOR_WARU_SMART
    /// </summary>
    [HttpPut("{id}")]
    [Authorize("ADMINISTRADOR_WARU_SMART")]
    public async Task<IActionResult> UpdateSubscription(int id, [FromBody] UpdateSubscriptionResource resource)
    {
        var updateSubscriptionCommand = UpdateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var subscription = await subscriptionCommandService.Handle(updateSubscriptionCommand);
        if (subscription is null) return NotFound();
        var subscriptionResource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
        return Ok(subscriptionResource);
    }

    /// <summary>
    /// Delete a subscription - Only accessible to ADMINISTRADOR_WARU_SMART
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize("ADMINISTRADOR_WARU_SMART")]
    public async Task<IActionResult> DeleteSubscription(int id)
    {
        var deleteSubscriptionCommand = new Domain.Model.Commands.DeleteSubscriptionCommand(id);
        var result = await subscriptionCommandService.Handle(deleteSubscriptionCommand);
        if (!result) return NotFound();
        return NoContent();
    }
}
