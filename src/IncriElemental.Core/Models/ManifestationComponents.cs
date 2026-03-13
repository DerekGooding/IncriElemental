using System.Text.Json.Serialization;

namespace IncriElemental.Core.Models;

[JsonDerivedType(typeof(ProducerComponent), "producer")]
[JsonDerivedType(typeof(StorageComponent), "storage")]
[JsonDerivedType(typeof(AuraComponent), "aura")]
[JsonDerivedType(typeof(UnlockComponent), "unlock")]
public interface IManifestationComponent
{
    string GetDescription();
}

public class ProducerComponent : IManifestationComponent
{
    public ResourceType Type { get; set; }
    public double AmountPerSecond { get; set; }

    public string GetDescription() => $"Produces {AmountPerSecond:F1} [i:{Type}] [c:{Type}]{Type}[/c]/s";
}

public class StorageComponent : IManifestationComponent
{
    public ResourceType Type { get; set; }
    public double Bonus { get; set; }

    public string GetDescription() => $"+{Bonus} [i:{Type}] [c:{Type}]{Type}[/c] Storage";
}

public class AuraComponent : IManifestationComponent
{
    public ResourceType Type { get; set; }
    public double Intensity { get; set; }

    public string GetDescription() => $"Projects {Intensity:F1} [i:{Type}] [c:{Type}]{Type}[/c] Aura";
}

public class UnlockComponent : IManifestationComponent
{
    public string DiscoveryKey { get; set; } = string.Empty;

    public string GetDescription() => $"Unlocks knowledge of [c:gold]{DiscoveryKey}[/c]";
}
