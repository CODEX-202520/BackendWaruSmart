namespace WaruSmart.API.Profiles.Interfaces.REST.Resources;

public record ProfileResource(
    int Id, 
    string FullName, 
    string Email,
    int CountryId, 
    int CityId, 
    int UserId);