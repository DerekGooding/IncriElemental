using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;

namespace IncriElemental.Tests;

public class GameEngineTests
{
    [Fact]
    public void Focus_IncreasesAether()
    {
        var engine = TestHelper.CreateEngine();

        engine.Focus();

        Assert.Equal(1, engine.State.GetResource(ResourceType.Aether).Amount);
    }

    [Fact]
    public void ManifestSpeck_ConsumesAether_And_IncreasesEarthPerSecond()
    {
        var engine = TestHelper.CreateEngine();

        // Setup state to allow manifestation
        for (var i = 0; i < 15; i++) engine.Focus();

        var success = engine.Manifest("speck");

        Assert.True(success);
        Assert.Equal(5, engine.State.GetResource(ResourceType.Aether).Amount);
        Assert.Equal(1.0, engine.State.GetResource(ResourceType.Earth).PerSecond);
        Assert.True(engine.State.Discoveries["first_manifestation"]);
    }

    [Fact]
    public void ManifestRuneOfAttraction_AutomatesAether()
    {
        var engine = TestHelper.CreateEngine();

        // Setup state to allow manifestation (30 Aether)
        for (var i = 0; i < 30; i++) engine.Focus();

        var success = engine.Manifest("rune_of_attraction");

        Assert.True(success);
        Assert.Equal(0, engine.State.GetResource(ResourceType.Aether).Amount);
        Assert.Equal(2.0, engine.State.GetResource(ResourceType.Aether).PerSecond);

        // Simulating 10 seconds of passive aether (10s * 2.0/s = 20 Aether)
        engine.Update(10.0);
        Assert.Equal(20.0, engine.State.GetResource(ResourceType.Aether).Amount);
    }
}
