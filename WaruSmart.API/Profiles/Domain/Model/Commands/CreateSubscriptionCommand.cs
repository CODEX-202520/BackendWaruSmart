namespace WaruSmart.API.Profiles.Domain.Model.Commands;

public record CreateSubscriptionCommand(string Name, string Description, decimal Price, int DurationInDays);
