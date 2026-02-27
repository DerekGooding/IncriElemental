using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;

namespace IncriElemental.Tests;

public class AlchemySystemTests
{
    [Fact]
    public void Mix_Combustion_AppliesAetherMultiplier()
    {
        var engine = TestHelper.CreateEngine();
        var state = engine.State;

        // Setup resources
        state.GetResource(ResourceType.Fire).Amount = 100;
        state.GetResource(ResourceType.Air).Amount = 100;

        var success = engine.Mix(ResourceType.Fire, ResourceType.Air);

        Assert.True(success);
        Assert.Equal(0, state.GetResource(ResourceType.Fire).Amount);
        Assert.Equal(0, state.GetResource(ResourceType.Air).Amount);
        
        // Check multiplier
        var mult = engine.State.GetResource(ResourceType.Aether).PerSecond; // Should be 0 initially
        engine.State.GetResource(ResourceType.Aether).PerSecond = 2.0;
        
        // AlchemySystem is updated via GameEngine.Update
        engine.Update(1.0);
        
        // Default Aether gain is (PerSecond * Multiplier) = 2.0 * 2.0 = 4.0
        Assert.Equal(4.0, state.GetResource(ResourceType.Aether).Amount);
    }

    [Fact]
    public void Mix_Fertility_AppliesLifeMultiplier()
    {
        var engine = TestHelper.CreateEngine();
        var state = engine.State;

        state.GetResource(ResourceType.Water).Amount = 100;
        state.GetResource(ResourceType.Earth).Amount = 100;

        var success = engine.Mix(ResourceType.Water, ResourceType.Earth);

        Assert.True(success);
        
        state.GetResource(ResourceType.Life).PerSecond = 10.0;
        engine.Update(1.0);

        // Life gain: 10.0 * 3.0 = 30.0
        Assert.Equal(30.0, state.GetResource(ResourceType.Life).Amount);
    }

    [Fact]
    public void Mix_InsufficientResources_ReturnsFalse()
    {
        var engine = TestHelper.CreateEngine();
        var state = engine.State;
        state.GetResource(ResourceType.Fire).Amount = 50;

        var success = engine.Mix(ResourceType.Fire, ResourceType.Air);

        Assert.False(success);
    }
}
