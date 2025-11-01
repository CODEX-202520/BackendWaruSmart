namespace WaruSmart.API.IAM.Domain.Model.Commands;

public record UpdateSubscriptionCommand(int Id, string Name, string Description, decimal Price, int DurationInDays);