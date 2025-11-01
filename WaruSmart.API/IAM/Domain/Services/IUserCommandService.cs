using WaruSmart.API.IAM.Domain.Model.Aggregates;
using WaruSmart.API.IAM.Domain.Model.Commands;

namespace WaruSmart.API.IAM.Domain.Services;

public interface IUserCommandService
{
    Task Handle(SignUpCommand command);
    Task<(User user, string token)> Handle(SignInCommand command);
    Task Handle(UpdateUserSubscriptionCommand command);
    Task Handle(CancelUserSubscriptionCommand command);
}