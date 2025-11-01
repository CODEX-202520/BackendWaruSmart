namespace WaruSmart.API.Profiles.Domain.Model.Commands;

public record CreateProfileCommand(
    string FirstName,
    string LastName, 
    string Email,
    int CityId, 
    int CountryId, 
    int UserId,
    string Role
);