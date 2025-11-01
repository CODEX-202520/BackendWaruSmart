namespace WaruSmart.API.IAM.Domain.Model.Commands;

public record CreateSubscriptionCommand(string Name, string Description, decimal Price, int DurationInDays);