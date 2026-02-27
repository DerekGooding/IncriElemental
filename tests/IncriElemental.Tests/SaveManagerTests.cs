using IncriElemental.Core.Models;
using IncriElemental.Core.Persistence;
using IncriElemental.Core.Engine;

namespace IncriElemental.Tests;

public class SaveManagerTests
{
    [Fact]
    public void Serialize_Deserialize_RestoresStateCorrecty()
    {
        // Setup state with some progress
        var engine = TestHelper.CreateEngine();
        for (var i = 0; i < 20; i++) engine.Focus();
        engine.Manifest("speck");
        engine.Update(10.0); // 1.0 Earth

        var originalState = engine.State;

        // Serialize
        var json = SaveManager.Serialize(originalState);

        // Deserialize
        var restoredState = SaveManager.Deserialize(json);

        Assert.NotNull(restoredState);
        Assert.Equal(originalState.TotalGameTime, restoredState.TotalGameTime);
        Assert.Equal(originalState.GetResource(ResourceType.Aether).Amount, restoredState.GetResource(ResourceType.Aether).Amount);
        Assert.Equal(originalState.GetResource(ResourceType.Earth).Amount, restoredState.GetResource(ResourceType.Earth).Amount);
        Assert.True(restoredState.Discoveries["first_manifestation"]);
        Assert.Equal(1, restoredState.Manifestations["speck"]);
    }

    [Fact]
    public void SaveToFile_LoadFromFile_PersistsState()
    {
        var testFile = "test_save.json";
        var state = new GameState();
        state.GetResource(ResourceType.Aether).Amount = 42;

        SaveManager.SaveToFile(state, testFile);

        Assert.True(File.Exists(testFile));

        var loadedState = SaveManager.LoadFromFile(testFile);

        Assert.NotNull(loadedState);
        Assert.Equal(42, loadedState.GetResource(ResourceType.Aether).Amount);

        // Cleanup
        File.Delete(testFile);
    }
}
