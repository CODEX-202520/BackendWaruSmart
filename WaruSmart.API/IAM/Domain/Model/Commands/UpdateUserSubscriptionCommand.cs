namespace WaruSmart.API.IAM.Domain.Model.Commands;

public record UpdateUserSubscriptionCommand(int UserId, int SubscriptionId);