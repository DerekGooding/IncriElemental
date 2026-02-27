using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;

namespace IncriElemental.Tests;

public class AutomationSystemTests
{
    [Fact]
    public void Update_ProcessManaFlow_IncreasesAether()
    {
        var engine = TestHelper.CreateEngine();
        var state = engine.State;

        state.Discoveries["water_unlocked"] = true;
        state.GetResource(ResourceType.Water).Amount = 100;
        state.GetResource(ResourceType.Aether).Amount = 0;

        // Mana flow: water.Amount * 0.05 * deltaTime = 100 * 0.05 * 1.0 = 5.0
        engine.Update(1.0);

        Assert.Equal(5.0, state.GetResource(ResourceType.Aether).Amount);
    }
}
