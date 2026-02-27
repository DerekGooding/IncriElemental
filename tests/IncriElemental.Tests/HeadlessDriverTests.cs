using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;

namespace IncriElemental.Tests;

public class HeadlessDriverTests
{
    [Fact]
    public void HeadlessDriver_ExecutesCommandsCorrectly()
    {
        var engine = new GameEngine();
        var driver = new HeadlessDriver(engine);

        // Execute Focus
        driver.ExecuteCommand("focus");
        Assert.Equal(1, engine.State.GetResource(ResourceType.Aether).Amount);

        // Execute Batch
        driver.ExecuteBatch(new List<string> { "focus", "focus", "focus" });
        Assert.Equal(4, engine.State.GetResource(ResourceType.Aether).Amount);

        // Execute Manifest (Not enough aether yet)
        var result = driver.ExecuteCommand("manifest:speck");
        Assert.Equal("Failed to manifest speck.", result);

        // Enough aether
        driver.ExecuteBatch(Enumerable.Repeat("focus", 10));
        driver.ExecuteCommand("manifest:speck");
        Assert.True(engine.State.Discoveries["first_manifestation"]);

        // State JSON
        var json = driver.ExecuteCommand("state");
        Assert.Contains("Aether", json);
        Assert.Contains("Earth", json);
    }
}
