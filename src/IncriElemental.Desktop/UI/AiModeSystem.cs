using System.IO;
using IncriElemental.Core.Engine;

namespace IncriElemental.Desktop.UI;

public class AiModeSystem(GameEngine engine)
{
    private readonly GameEngine _engine = engine;

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
                    if (double.TryParse(parts[1], out var dt)) _engine.Update(dt);
                }
            }
        }
    }
}
