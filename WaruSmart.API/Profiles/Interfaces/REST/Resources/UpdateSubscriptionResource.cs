namespace WaruSmart.API.Profiles.Interfaces.REST.Resources;

public record UpdateSubscriptionResource(string Name, string Description, decimal Price, int DurationInDays);
