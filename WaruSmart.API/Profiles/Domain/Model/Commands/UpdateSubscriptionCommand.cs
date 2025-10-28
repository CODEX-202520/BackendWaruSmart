namespace WaruSmart.API.Profiles.Domain.Model.Commands;

public record UpdateSubscriptionCommand(int Id, string Name, string Description, decimal Price, int DurationInDays);
