namespace IncriElemental.Core.Models;

public enum ResourceType
{
    Aether,
    Earth,
    Fire,
    Water,
    Air
}

public class Resource
{
    public ResourceType Type { get; set; }
    public double Amount { get; set; }
    public double MaxAmount { get; set; } = double.MaxValue;
    public double PerSecond { get; set; } = 0;

    public Resource() { }
    public Resource(ResourceType type, double amount = 0)
    {
        Type = type;
        Amount = amount;
    }

    public void Add(double amount)
    {
        Amount = Math.Clamp(Amount + amount, 0, MaxAmount);
    }
}
