using Xunit;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IncriElemental.Tests;

public class BalanceSimulator
{
    [Fact]
    public void Simulate_20Minute_Endgame_Progression()
    {
        var engine = new GameEngine();
        var driver = new HeadlessDriver(engine);
        
        // --- PHASE 1: Manual Awakening ---
        for (int i = 0; i < 30; i++) engine.Focus();
        engine.Manifest("rune_of_attraction");
        
        // --- PHASE 2: Passive Unfolding ---
        for (int i = 0; i < 1200; i++)
        {
            engine.Update(1.0);
            
            if (i % 60 == 0) Console.WriteLine($"[Min {i/60}] {driver.ExecuteCommand("status")}");

            // The simulator tries everything every tick.
            engine.Manifest("rune_of_attraction");
            engine.Manifest("speck");
            engine.Manifest("spark");
            engine.Manifest("droplet");
            engine.Manifest("breeze");
            engine.Manifest("altar");
            engine.Manifest("forge");
            engine.Manifest("garden");
            engine.Manifest("well");
            engine.Manifest("brazier");
            engine.Manifest("wind_chime");
            engine.Manifest("pickaxe");
            engine.Manifest("focus_crystal");
            engine.Manifest("familiar");
            
            // Endgame
            engine.Manifest("spire_foundation");
            engine.Manifest("spire_shaft");
            engine.Manifest("spire_core");
            engine.Manifest("ascend");
        }

        // Final State Check
        var state = engine.State;
        Assert.True(state.Discoveries.ContainsKey("spire_foundation_ready"), "Should have reached Spire Foundation");
        Assert.True(state.Discoveries.ContainsKey("spire_shaft_ready"), "Should have reached Spire Shaft");
        Assert.True(state.Discoveries.ContainsKey("spire_complete"), "Should have reached Spire Core");
        Assert.True(state.Discoveries.ContainsKey("ascended"), "Should have Ascended");
    }
}
