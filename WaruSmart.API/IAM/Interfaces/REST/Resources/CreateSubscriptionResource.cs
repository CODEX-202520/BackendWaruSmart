namespace WaruSmart.API.IAM.Interfaces.REST.Resources;

public record CreateSubscriptionResource(
    string Name,
    string Description,
    decimal Price,
    int DurationInDays
);