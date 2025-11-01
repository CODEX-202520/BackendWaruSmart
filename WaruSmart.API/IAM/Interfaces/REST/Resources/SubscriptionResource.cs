namespace WaruSmart.API.IAM.Interfaces.REST.Resources;

public record SubscriptionResource(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int DurationInDays,
    bool IsActive
);