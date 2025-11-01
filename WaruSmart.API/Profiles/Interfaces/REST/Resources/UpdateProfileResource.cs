namespace WaruSmart.API.Profiles.Interfaces.REST.Resources;

public record UpdateProfileResource(string FullName, string EmailAddress, int CountryId, int CityId);