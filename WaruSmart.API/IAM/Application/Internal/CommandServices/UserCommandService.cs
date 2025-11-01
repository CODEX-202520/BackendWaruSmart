using WaruSmart.API.IAM.Application.Internal.OutboundServices;
using WaruSmart.API.IAM.Domain.Model.Aggregates;
using WaruSmart.API.IAM.Domain.Model.Commands;
using WaruSmart.API.IAM.Domain.Repositories;
using WaruSmart.API.IAM.Domain.Services;
using WaruSmart.API.Shared.Domain.Repositories;

namespace WaruSmart.API.IAM.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    ISubscriptionRepository subscriptionRepository,
    IHashingService hashingService,
    ITokenService tokenService,
    IUnitOfWork unitOfWork
    ) : IUserCommandService
{
    public async Task Handle(SignUpCommand command)
    {
        if (userRepository.ExistsByUsername(command.Username))
            throw new Exception($"Username {command.Username} is already taken");
        
        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new User(command.Username, hashedPassword);

        try
        {
            await userRepository.AddAsync(user);

            // Validar y configurar la suscripción del usuario
            var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId);
            if (subscription == null)
                throw new Exception($"Subscription with id {command.SubscriptionId} not found");

            // Configurar fechas de suscripción: desde ahora hasta 2 días después
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(2); // Configuración por defecto de 2 días

            user.UpdateSubscription(command.SubscriptionId, startDate, endDate);

            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            // Re-throw with full exception details to help debugging in development
            throw new Exception($"An error occurred while creating user: {e}");
        }
    }

    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByUsernameAsync(command.Username);
        if (user is null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");
        var token = tokenService.GenerateToken(user);
        return (user, token);
    }

    public async Task Handle(UpdateUserSubscriptionCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId) 
            ?? throw new Exception($"User with id {command.UserId} not found");

        var subscription = await subscriptionRepository.FindByIdAsync(command.SubscriptionId)
            ?? throw new Exception($"Subscription with id {command.SubscriptionId} not found");

        user.UpdateSubscription(command.SubscriptionId, subscription.DurationInDays);
        await unitOfWork.CompleteAsync();
    }

    public async Task Handle(CancelUserSubscriptionCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId)
            ?? throw new Exception($"User with id {command.UserId} not found");

        user.CancelSubscription();
        await unitOfWork.CompleteAsync();
    }
}