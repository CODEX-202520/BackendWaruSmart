using WaruSmart.API.Profiles.Domain.Model.Commands;
using WaruSmart.API.Profiles.Interfaces.REST.Resources;

namespace WaruSmart.API.Profiles.Interfaces.REST.Transform;

public static class CreateProfileCommandFromResourceAssembler
{
    public static CreateProfileCommand ToCommandFromResource(CreateProfileResource resource)
    {
        return new CreateProfileCommand(
            resource.FirstName, 
            resource.LastName, 
            resource.Email,
            resource.CityId, 
            resource.CountryId, 
            resource.UserId, 
            resource.Role);
    }
}