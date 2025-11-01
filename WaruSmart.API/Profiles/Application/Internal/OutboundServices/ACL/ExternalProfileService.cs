using WaruSmart.API.Profiles.Domain.Model.ValueObjects;
using WaruSmart.API.Profiles.Interfaces.ACL;

namespace WaruSmart.API.Profiles.Application.Internal.OutboundServices.ACL;

public class ExternalProfileService(IProfilesContextFacade profilesContextFacade)
{

    public async Task<ProfileId?> FetchProfileIdByEmail(string email)
    {
        var profileId = await profilesContextFacade.FetchProfileIdByEmail(email);
        if (profileId == 0) return await Task.FromResult<ProfileId?>(null);
        return new ProfileId(profileId);
    }

    public async Task<ProfileId?> CreateProfile(string firstName, string lastName, string email, int cityId,
        int countryId, int userId, string role)
    {
        var profileId =
            await profilesContextFacade.CreateProfile(firstName, lastName, email, cityId, countryId, userId, role);
        if (profileId == 0) return await Task.FromResult<ProfileId?>(null);
        return new ProfileId(profileId);
    }
}