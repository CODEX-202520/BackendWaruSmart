namespace WaruSmart.API.IAM.Interfaces.ACL;

public interface IIamContextFacade
{
    Task<int> CreateUser(string username, string password, int subscriptionId);
    Task<int> FetchUserIdByUsername(string username);
    Task<string> FetchUsernameByUserId(int userId);
    
    Task<int> FetchUserIdByUserId(int userId);
}