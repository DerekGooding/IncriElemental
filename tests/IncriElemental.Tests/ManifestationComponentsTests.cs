using Xunit;
using IncriElemental.Core.Models;

namespace IncriElemental.Tests;

public class ManifestationComponentsTests
{
    [Fact]
    public void ProducerComponent_ShouldReturnCorrectDescription()
    {
        var component = new ProducerComponent { Type = ResourceType.Aether, AmountPerSecond = 1.5 };
        var description = component.GetDescription();
        Assert.Contains("1.5", description);
        Assert.Contains("Aether", description);
    }

    [Fact]
    public void StorageComponent_ShouldReturnCorrectDescription()
    {
        var component = new StorageComponent { Type = ResourceType.Aether, Bonus = 100 };
        var description = component.GetDescription();
        Assert.Contains("100", description);
        Assert.Contains("Aether", description);
    }

    [Fact]
    public void AuraComponent_ShouldReturnCorrectDescription()
    {
        var component = new AuraComponent { Type = ResourceType.Aether, Intensity = 0.5 };
        var description = component.GetDescription();
        Assert.Contains("0.5", description);
        Assert.Contains("Aether", description);
    }

    [Fact]
    public void UnlockComponent_ShouldReturnCorrectDescription()
    {
        var component = new UnlockComponent { DiscoveryKey = "test_discovery" };
        var description = component.GetDescription();
        Assert.Contains("test_discovery", description);
    }

    [Fact]
    public void Aura_ShouldStoreProperties()
    {
        var aura = new Aura(ResourceType.Aether, 0.75);
        Assert.Equal(ResourceType.Aether, aura.Type);
        Assert.Equal(0.75, aura.Intensity);
    }
}
