using WaruSmart.API.Shared.Domain.Repositories;
using WaruSmart.API.Profiles.Domain.Model.Aggregates;
using WaruSmart.API.Profiles.Domain.Model.Commands;
using WaruSmart.API.Profiles.Domain.Repositories;
using WaruSmart.API.Profiles.Domain.Services;

namespace WaruSmart.API.Profiles.Application.Internal.CommandServices;

public class SubscriptionCommandService(ISubscriptionRepository subscriptionRepository, IUnitOfWork unitOfWork)
    : ISubscriptionCommandService
{
    public async Task<Subscription?> Handle(CreateSubscriptionCommand command)
    {
        var subscription = new Subscription(command.Name, command.Description, command.Price, command.DurationInDays);
        await subscriptionRepository.AddAsync(subscription);
        await unitOfWork.CompleteAsync();
        return subscription;
    }

    public async Task<Subscription?> Handle(UpdateSubscriptionCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.Id);
        if (subscription == null) return null;

        subscription.UpdateName(command.Name)
            .UpdateDescription(command.Description)
            .UpdatePrice(command.Price)
            .UpdateDuration(command.DurationInDays);

        subscriptionRepository.Update(subscription);
        await unitOfWork.CompleteAsync();
        return subscription;
    }

    public async Task<bool> Handle(DeleteSubscriptionCommand command)
    {
        var subscription = await subscriptionRepository.FindByIdAsync(command.Id);
        if (subscription == null) return false;

        subscriptionRepository.Remove(subscription);
        await unitOfWork.CompleteAsync();
        return true;
    }
}
