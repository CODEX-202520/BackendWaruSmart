using WaruSmart.API.IAM.Domain.Model.Commands;
using WaruSmart.API.IAM.Domain.Model.Queries;
using WaruSmart.API.IAM.Domain.Services;

namespace WaruSmart.API.IAM.Interfaces.ACL.Services;

public class IamContextFacade(
    IUserQueryService userQueryService, 
    IUserCommandService userCommandService) : IIamContextFacade
{
    public async Task<int> CreateUser(string username, string password, int subscriptionId)
    {
        var signUpCommand = new SignUpCommand(username, password, subscriptionId);
        await userCommandService.Handle(signUpCommand);
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? 0;
    }

    public async Task<int> FetchUserIdByUsername(string username)
    {
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? 0;
    }

    public async Task<string> FetchUsernameByUserId(int userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var result = await userQueryService.Handle(getUserByIdQuery);
        return result?.Username ?? string.Empty;
    }

    public async Task<int> FetchUserIdByUserId(int userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var result = await userQueryService.Handle(getUserByIdQuery);
        return result?.Id ?? 0;
    }
}