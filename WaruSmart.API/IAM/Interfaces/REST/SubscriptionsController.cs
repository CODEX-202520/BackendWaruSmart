using Microsoft.AspNetCore.Mvc;
using WaruSmart.API.IAM.Domain.Model.Commands;
using WaruSmart.API.IAM.Domain.Services;
using WaruSmart.API.IAM.Interfaces.REST.Resources;
using WaruSmart.API.IAM.Interfaces.REST.Transform;
using WaruSmart.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace WaruSmart.API.IAM.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionCommandService _subscriptionCommandService;
    private readonly ISubscriptionQueryService _subscriptionQueryService;
    private readonly SubscriptionResourceFromEntityAssembler _resourceFromEntityAssembler;
    private readonly CreateSubscriptionCommandFromResourceAssembler _createCommandFromResourceAssembler;
    private readonly UpdateSubscriptionCommandFromResourceAssembler _updateCommandFromResourceAssembler;

    public SubscriptionsController(
        ISubscriptionCommandService subscriptionCommandService,
        ISubscriptionQueryService subscriptionQueryService,
        SubscriptionResourceFromEntityAssembler resourceFromEntityAssembler,
        CreateSubscriptionCommandFromResourceAssembler createCommandFromResourceAssembler,
        UpdateSubscriptionCommandFromResourceAssembler updateCommandFromResourceAssembler)
    {
        _subscriptionCommandService = subscriptionCommandService;
        _subscriptionQueryService = subscriptionQueryService;
        _resourceFromEntityAssembler = resourceFromEntityAssembler;
        _createCommandFromResourceAssembler = createCommandFromResourceAssembler;
        _updateCommandFromResourceAssembler = updateCommandFromResourceAssembler;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IEnumerable<SubscriptionResource>> GetAll()
    {
        var subscriptions = await _subscriptionQueryService.ListAsync();
        return subscriptions.Select(subscription => _resourceFromEntityAssembler.ToResource(subscription));
    }

    [AllowAnonymous]
    [HttpGet("active")]
    public async Task<IEnumerable<SubscriptionResource>> GetActive()
    {
        var subscriptions = await _subscriptionQueryService.ListActiveAsync();
        return subscriptions.Select(subscription => _resourceFromEntityAssembler.ToResource(subscription));
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<SubscriptionResource>> GetById(int id)
    {
        var subscription = await _subscriptionQueryService.FindByIdAsync(id);
        if (subscription == null)
            return NotFound();

        return _resourceFromEntityAssembler.ToResource(subscription);
    }

    [HttpPost]
    public async Task<ActionResult<SubscriptionResource>> Create([FromBody] CreateSubscriptionResource resource)
    {
        var command = _createCommandFromResourceAssembler.ToCommand(resource);
        var subscription = await _subscriptionCommandService.Handle(command);
        var subscriptionResource = _resourceFromEntityAssembler.ToResource(subscription);
        return CreatedAtAction(nameof(GetById), new { id = subscription.Id }, subscriptionResource);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SubscriptionResource>> Update(int id, [FromBody] UpdateSubscriptionResource resource)
    {
        var command = _updateCommandFromResourceAssembler.ToCommand(id, resource);
        var subscription = await _subscriptionCommandService.Handle(command);
        return _resourceFromEntityAssembler.ToResource(subscription);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _subscriptionCommandService.Handle(id);
        return NoContent();
    }
}