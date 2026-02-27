using IncriElemental.Core.Engine;
using System.IO;

namespace IncriElemental.Tests;

public static class TestHelper
{
    public static GameEngine CreateEngine()
    {
        var engine = new GameEngine();
        
        var jsonPath = "manifestations.json";
        if (File.Exists(jsonPath))
        {
            engine.LoadDefinitions(File.ReadAllText(jsonPath));
        }

        var lorePath = "lore.json";
        if (File.Exists(lorePath))
        {
            engine.LoadLore(File.ReadAllText(lorePath));
        }

        return engine;
    }
}
