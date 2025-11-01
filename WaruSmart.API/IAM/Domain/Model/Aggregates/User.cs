using System.Text.Json.Serialization;
using Mysqlx.Datatypes;
using WaruSmart.API.Crops.Domain.Model.Aggregates;
using WaruSmart.API.IAM.Domain.Model.ValueObjects;

namespace WaruSmart.API.IAM.Domain.Model.Aggregates;

public class User(string username, string passwordHash, ERole role = ERole.AGRICULTOR)
{
    public User() : this(string.Empty, string.Empty, ERole.AGRICULTOR)
    {
    }
    
    public int Id { get; set; }
    
    public string Username { get; private set; } = username;

    [JsonIgnore] public string PasswordHash { get; private set; } = passwordHash;
    
    public ERole Role { get; private set; } = role;
    
    public ICollection<Sowing> Sowings { get; set; } = new List<Sowing>();
    
    public int? SubscriptionId { get; private set; }
    public DateTime? SubscriptionStartDate { get; private set; }
    public DateTime? SubscriptionEndDate { get; private set; }
    
    //TODO: Add feature about user with profile in bounded context IAM
    //public ProfileId Profile { get; set; }

    public User UpdatePasswordHash(string passwordHash)
    {
        PasswordHash = passwordHash;
        return this;
    }

    public User UpdateUsername(string username)
    {
        Username = username;
        return this;
    }
    
    public User UpdateRole(ERole role)
    {
        Role = role;
        return this;
    }

    public User UpdateSubscription(int subscriptionId, int durationInDays)
    {
        SubscriptionId = subscriptionId;
        SubscriptionStartDate = DateTime.Now;
        SubscriptionEndDate = DateTime.Now.AddDays(durationInDays);
        return this;
    }

    public User UpdateSubscription(int subscriptionId, DateTime startDate, DateTime endDate)
    {
        SubscriptionId = subscriptionId;
        SubscriptionStartDate = startDate;
        SubscriptionEndDate = endDate;
        return this;
    }

    public User CancelSubscription()
    {
        SubscriptionId = null;
        SubscriptionStartDate = null;
        SubscriptionEndDate = null;
        return this;
    }

    public bool HasActiveSubscription()
    {
        return SubscriptionId.HasValue && 
               SubscriptionEndDate.HasValue && 
               SubscriptionEndDate.Value > DateTime.Now;
    }
}