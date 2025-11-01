using WaruSmart.API.IAM.Domain.Model.Aggregates;
using WaruSmart.API.Profiles.Domain.Model.Commands;
using WaruSmart.API.Profiles.Domain.Model.ValueObjects;

namespace WaruSmart.API.Profiles.Domain.Model.Aggregates;

public partial class Profile
{
    public Profile()
    {
        Name = new PersonName();
        Email = new EmailAddress();
    }

    public Profile(string firstName, string lastName, string email, int cityId, int countryId)
    {
        Name = new PersonName(firstName, lastName);
        Email = new EmailAddress(email);
        CityId = cityId;
        CountryId = countryId;
    }

    public Profile(CreateProfileCommand command, int userId)
    {
        Name = new PersonName(command.FirstName, command.LastName);
        Email = new EmailAddress(command.Email);
        CityId = command.CityId;
        CountryId = command.CountryId;
        this.UserId = new UserId(userId);
        this.ERole = Enum.TryParse<ERole>(command.Role, true, out var role) ? role : ERole.AGRICULTOR;
    }

    public int Id { get; }
    public int CityId { get; private set; }
    public int CountryId { get; private set; }
    public PersonName Name { get; private set; }
    public EmailAddress Email { get; private set; }

    public string FullName => Name.FullName;

    public string EmailAddress => Email.Address;
    
    public UserId UserId { get;  set; }
    
    public ERole ERole { get; set; } 
    
    //public int UserIdValue { get; set; }
    

    public void UpdateProfile(string fullName, string emailAddress, int countryId, int cityId)
    {
        Name = new PersonName(fullName);
        Email = new EmailAddress(emailAddress);
        CountryId = countryId;
        CityId = cityId;
    }
}