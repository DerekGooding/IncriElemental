namespace IncriElemental.Core.Models;

public class ManifestationCost
{
    public ResourceType Type { get; set; }
    public double Amount { get; set; }
}

public class ManifestationEffect
{
    public ResourceType Type { get; set; }
    public double PerSecondBonus { get; set; }
    public double MaxAmountBonus { get; set; }
    public double InstantAmount { get; set; }
}

public class ManifestationDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string OutcomeName { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string DiscoveryKey { get; set; } = string.Empty;
    public string RequiredDiscovery { get; set; } = string.Empty;
    public int MaxCount { get; set; } = int.MaxValue;
    public List<ManifestationCost> Costs { get; set; } = [];
    public List<ManifestationEffect> Effects { get; set; } = [];
}
