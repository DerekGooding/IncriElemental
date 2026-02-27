using Xunit;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;

namespace IncriElemental.Tests;

public class GameEngineTests
{
    [Fact]
    public void Focus_IncreasesAether()
    {
        var engine = new GameEngine();
        
        engine.Focus();
        
        Assert.Equal(1, engine.State.GetResource(ResourceType.Aether).Amount);
    }

    [Fact]
    public void ManifestSpeck_ConsumesAether_And_IncreasesEarthPerSecond()
    {
        var engine = new GameEngine();
        
        // Setup state to allow manifestation
        for (int i = 0; i < 15; i++) engine.Focus();
        
        var success = engine.Manifest("speck");
        
        Assert.True(success);
        Assert.Equal(5, engine.State.GetResource(ResourceType.Aether).Amount);
        Assert.Equal(0.1, engine.State.GetResource(ResourceType.Earth).PerSecond);
        Assert.True(engine.State.Discoveries["first_manifestation"]);
    }

    [Fact]
    public void Update_GeneratesResourcesOverTime()
    {
        var engine = new GameEngine();
        
        // Setup state to allow manifestation
        for (int i = 0; i < 15; i++) engine.Focus();
        engine.Manifest("speck");
        
        // Update for 10 seconds (10s * 0.1 Earth/s = 1.0 Earth)
        engine.Update(10.0);
        
        Assert.Equal(1.0, engine.State.GetResource(ResourceType.Earth).Amount);
    }
}
