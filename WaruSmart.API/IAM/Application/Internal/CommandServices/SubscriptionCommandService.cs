using WaruSmart.API.IAM.Domain.Model.Aggregates;
using WaruSmart.API.IAM.Domain.Model.Commands;
using WaruSmart.API.IAM.Domain.Repositories;
using WaruSmart.API.IAM.Domain.Services;
using WaruSmart.API.Shared.Domain.Repositories;

namespace WaruSmart.API.IAM.Application.Internal.CommandServices;

public class SubscriptionCommandService : ISubscriptionCommandService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubscriptionCommandService(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
    {
        _subscriptionRepository = subscriptionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Subscription> Handle(CreateSubscriptionCommand command)
    {
        var subscription = new Subscription(command.Name, command.Description, command.Price, command.DurationInDays);
        await _subscriptionRepository.AddAsync(subscription);
        await _unitOfWork.CompleteAsync();
        return subscription;
    }

    public async Task<Subscription> Handle(UpdateSubscriptionCommand command)
    {
        var subscription = await _subscriptionRepository.FindByIdAsync(command.Id);
        if (subscription == null)
            throw new Exception($"Subscription with id {command.Id} not found");

        subscription
            .UpdateName(command.Name)
            .UpdateDescription(command.Description)
            .UpdatePrice(command.Price)
            .UpdateDuration(command.DurationInDays);

        _subscriptionRepository.Update(subscription);
        await _unitOfWork.CompleteAsync();
        return subscription;
    }

    public async Task Handle(int subscriptionId)
    {
        var subscription = await _subscriptionRepository.FindByIdAsync(subscriptionId);
        if (subscription == null)
            throw new Exception($"Subscription with id {subscriptionId} not found");

        subscription.Deactivate();
        _subscriptionRepository.Update(subscription);
        await _unitOfWork.CompleteAsync();
    }
}