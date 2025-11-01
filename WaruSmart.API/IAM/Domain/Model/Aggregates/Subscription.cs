namespace WaruSmart.API.IAM.Domain.Model.Aggregates;

/// <summary>
/// Subscription aggregate root
/// </summary>
public class Subscription
{
    public Subscription()
    {
        Name = string.Empty;
        Description = string.Empty;
        Price = 0;
        DurationInDays = 30;
        IsActive = true;
    }

    public Subscription(string name, string description, decimal price, int durationInDays)
    {
        Name = name;
        Description = description;
        Price = price;
        DurationInDays = durationInDays;
        IsActive = true;
    }

    public int Id { get; set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int DurationInDays { get; private set; }
    public bool IsActive { get; private set; }

    public Subscription UpdateName(string name)
    {
        Name = name;
        return this;
    }

    public Subscription UpdateDescription(string description)
    {
        Description = description;
        return this;
    }

    public Subscription UpdatePrice(decimal price)
    {
        Price = price;
        return this;
    }

    public Subscription UpdateDuration(int durationInDays)
    {
        DurationInDays = durationInDays;
        return this;
    }

    public Subscription Activate()
    {
        IsActive = true;
        return this;
    }

    public Subscription Deactivate()
    {
        IsActive = false;
        return this;
    }
}