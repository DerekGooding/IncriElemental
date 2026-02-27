using Xunit;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using System;
using System.Collections.Generic;

namespace IncriElemental.Tests;

public class BalanceSimulator
{
    [Fact]
    public void Simulate_OneHour_Progression()
    {
        var engine = new GameEngine();
        var driver = new HeadlessDriver(engine);
        
        // Initial manual push
        driver.ExecuteBatch(Enumerable.Repeat("focus", 30));
        
        // Manifest first automation (Rune of Attraction)
        bool runeSuccess = engine.Manifest("rune_of_attraction");
        Assert.True(runeSuccess);
        
        // Simulate 1 hour (3600 seconds) in 1-second ticks
        for (int i = 0; i < 3600; i++)
        {
            engine.Update(1.0);
            
            // Auto-manifest speck if enough Aether and we need more earth gen
            if (engine.State.GetResource(ResourceType.Aether).Amount >= 10 && engine.State.Manifestations.GetValueOrDefault("speck") < 10)
            {
                engine.Manifest("speck");
            }

            // Auto-manifest Altar if enough resources
            if (engine.State.GetResource(ResourceType.Aether).Amount >= 100 && engine.State.GetResource(ResourceType.Earth).Amount >= 20 && !engine.State.Discoveries.GetValueOrDefault("altar_constructed"))
            {
                engine.Manifest("altar");
            }
        }

        // Final State Check
        double aether = engine.State.GetResource(ResourceType.Aether).Amount;
        double earth = engine.State.GetResource(ResourceType.Earth).Amount;
        int specks = engine.State.Manifestations.GetValueOrDefault("speck");
        bool altar = engine.State.Discoveries.GetValueOrDefault("altar_constructed");

        // Use Assert with message to see values on failure
        Assert.True(aether > 0, $"Aether should be > 0, was {aether}");
        Assert.True(earth > 0, $"Earth should be > 0, was {earth}");
        Assert.True(specks > 0, $"Specks should be > 0, was {specks}");
        Assert.True(altar, "Altar should be constructed");
    }
}
