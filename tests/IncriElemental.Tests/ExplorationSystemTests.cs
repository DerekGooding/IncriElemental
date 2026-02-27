using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;

namespace IncriElemental.Tests;

public class ExplorationSystemTests
{
    [Fact]
    public void Explore_WithoutFamiliar_ReturnsFalse()
    {
        var engine = TestHelper.CreateEngine();
        engine.State.GetResource(ResourceType.Aether).Amount = 200;

        var success = engine.Explore(0, 0);

        Assert.False(success);
    }

    [Fact]
    public void Explore_RevealsCell_And_ConsumesAether()
    {
        var engine = TestHelper.CreateEngine();
        var state = engine.State;

        // Directly enable exploration capability
        state.Discoveries["familiar_manifested"] = true;
        state.Manifestations["familiar"] = 1;
        state.GetResource(ResourceType.Aether).Amount = 1000;

        var success = engine.Explore(5, 5);

        Assert.True(success);
        Assert.True(state.Map.GetCell(5, 5).IsExplored);
        
        // 1000 - 500(explore) = 500. 
        // If it was Ruins, +2000 = 2500.
        var aether = state.GetResource(ResourceType.Aether).Amount;
        Assert.True(aether == 500 || aether == 2500, $"Aether should be 500 or 2500 (if Ruins), but was {aether}");
    }

    [Fact]
    public void Explore_TriggersLore()
    {
        var engine = TestHelper.CreateEngine();
        var state = engine.State;

        state.Discoveries["familiar_manifested"] = true;
        state.Manifestations["familiar"] = 1;
        state.GetResource(ResourceType.Aether).Amount = 1000;

        // Find a cell that triggers lore (Ruins)
        int rx = -1, ry = -1;
        for (var x = 0; x < state.Map.Width; x++)
        {
            for (var y = 0; y < state.Map.Height; y++)
            {
                if (state.Map.GetCell(x, y).Type == CellType.Ruins)
                {
                    rx = x; ry = y; break;
                }
            }
            if (rx != -1) break;
        }

        if (rx != -1)
        {
            engine.Explore(rx, ry);
            Assert.Contains("We are not the first to manifest here.", state.History);
        }
    }
}
