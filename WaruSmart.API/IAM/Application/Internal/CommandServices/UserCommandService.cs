using WaruSmart.API.IAM.Application.Internal.OutboundServices;
using WaruSmart.API.IAM.Domain.Model.Aggregates;
using WaruSmart.API.IAM.Domain.Model.Commands;
using WaruSmart.API.IAM.Domain.Repositories;
using WaruSmart.API.IAM.Domain.Services;
using WaruSmart.API.Shared.Domain.Repositories;

namespace WaruSmart.API.IAM.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
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
}