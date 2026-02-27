using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;

namespace IncriElemental.Tests;

public class AscensionTests
{
    [Fact]
    public void ResetForNewGamePlus_IncreasesCosmicInsight_And_UnlocksVoidInfusion()
    {
        var engine = TestHelper.CreateEngine();
        var state = engine.State;

        // Setup for Ascension
        state.Discoveries["spire_complete"] = true;
        
        var success = engine.Manifest("ascend");
        Assert.True(success);

        // Reset
        var resetSuccess = engine.Manifest("reset");
        Assert.True(resetSuccess);

        Assert.Equal(1.5, state.CosmicInsight);
        Assert.True(state.VoidInfusionUnlocked);
        Assert.Empty(state.Manifestations);
        Assert.Equal(0, state.TotalGameTime);
    }

    [Fact]
    public void VoidInfusion_IncreasesMaxAmount()
    {
        var engine = TestHelper.CreateEngine();
        engine.State.VoidInfusionUnlocked = true;
        engine.State.GetResource(ResourceType.Aether).Amount = 5000;

        var initialMax = engine.State.GetResource(ResourceType.Earth).MaxAmount;
        
        var success = engine.Manifest("void_infusion");
        
        Assert.True(success);
        Assert.Equal(initialMax + 5000, engine.State.GetResource(ResourceType.Earth).MaxAmount);
    }
}
