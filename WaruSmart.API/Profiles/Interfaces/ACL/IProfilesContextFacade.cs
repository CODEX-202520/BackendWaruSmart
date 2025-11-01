namespace WaruSmart.API.Profiles.Interfaces.ACL;

public interface IProfilesContextFacade
{
    Task<int> CreateProfile(string firstName, string lastName, string email, int cityId,
        int countryId, int userId, string role);

    Task<int> FetchProfileIdByEmail(string email);
}