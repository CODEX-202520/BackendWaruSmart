using WaruSmart.API.Profiles.Domain.Model.Commands;
using WaruSmart.API.Profiles.Interfaces.REST.Resources;

namespace WaruSmart.API.Profiles.Interfaces.REST.Transform;

public static class UpdateProfileCommandFromResourceAssembler
{
    public static UpdateProfileCommand ToCommandFromResource(int profileId, UpdateProfileResource resource)
    {
        return new UpdateProfileCommand(
            profileId,
            resource.FullName,
            resource.EmailAddress,
            resource.CountryId,
            resource.CityId
        );
    }
}