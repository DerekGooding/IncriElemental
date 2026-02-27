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
            
            // Auto-manifest speck if enough Aether (Every 10 Aether)
            if (engine.State.GetResource(ResourceType.Aether).Amount >= 10)
            {
                engine.Manifest("speck");
            }

            // Auto-manifest Altar if enough resources
            if (engine.State.GetResource(ResourceType.Aether).Amount >= 100 && engine.State.GetResource(ResourceType.Earth).Amount >= 20)
            {
                engine.Manifest("altar");
            }
        }

        // Final State Check
        double totalAetherGathered = engine.State.TotalGameTime * 0.5; // Rune of Attraction gathers 0.5/s
        Assert.True(engine.State.GetResource(ResourceType.Aether).Amount > 0);
        Assert.True(engine.State.GetResource(ResourceType.Earth).Amount > 0);
        Assert.True(engine.State.Manifestations["speck"] > 0);
        Assert.True(engine.State.Discoveries["altar_constructed"]);
        
        Console.WriteLine($"[Balance Simulator] After 1 hour: Aether: {engine.State.GetResource(ResourceType.Aether).Amount:F1}, Earth: {engine.State.GetResource(ResourceType.Earth).Amount:F1}, Specks: {engine.State.Manifestations["speck"]}");
    }
}
