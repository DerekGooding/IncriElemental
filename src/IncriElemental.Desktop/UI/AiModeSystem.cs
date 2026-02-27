using System;
using System.IO;
using IncriElemental.Core.Engine;

namespace IncriElemental.Desktop.UI;

public class AiModeSystem
{
    private readonly GameEngine _engine;

    public AiModeSystem(GameEngine engine)
    {
        _engine = engine;
    }

    public void Process(string commandPath)
    {
        if (File.Exists(commandPath))
        {
            var commands = File.ReadAllLines(commandPath);
            foreach (var cmd in commands)
            {
                var parts = cmd.Split(':');
                if (parts[0].ToLower() == "focus") _engine.Focus();
                if (parts[0].ToLower() == "manifest") _engine.Manifest(parts[1]);
                if (parts[0].ToLower() == "update") {
                    if (double.TryParse(parts[1], out double dt)) _engine.Update(dt);
                }
            }
        }
    }
}
