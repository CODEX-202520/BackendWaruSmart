namespace WaruSmart.API.Profiles.Interfaces.REST.Resources;

public record CreateProfileResource(
    string FirstName, 
    string LastName,
    string Email, 
    int CityId,
    int CountryId, 
    int UserId, 
    string Role);