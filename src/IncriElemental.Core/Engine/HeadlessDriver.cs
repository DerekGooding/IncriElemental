using System;
using System.Collections.Generic;
using System.Linq;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Core.Persistence;

namespace IncriElemental.Core.Engine;

public class HeadlessDriver
{
    private readonly GameEngine _engine;

    public HeadlessDriver(GameEngine engine)
    {
        _engine = engine;
    }

    public string ExecuteCommand(string command)
    {
        var parts = command.Split(':');
        var action = parts[0].ToLower();
        var parameter = parts.Length > 1 ? parts[1] : string.Empty;

        switch (action)
        {
            case "focus":
                _engine.Focus();
                return "Focused.";
            case "manifest":
                bool success = _engine.Manifest(parameter);
                return success ? $"Manifested {parameter}." : $"Failed to manifest {parameter}.";
            case "update":
                if (double.TryParse(parameter, out double deltaTime))
                {
                    _engine.Update(deltaTime);
                    return $"Updated by {deltaTime}s.";
                }
                return "Invalid delta time.";
            case "state":
                return SaveManager.Serialize(_engine.State);
            case "status":
                var res = _engine.State.Resources;
                return $"A:{res[ResourceType.Aether].Amount:F0} E:{res[ResourceType.Earth].Amount:F0} F:{res[ResourceType.Fire].Amount:F0} W:{res[ResourceType.Water].Amount:F0} L:{res.GetValueOrDefault(ResourceType.Life)?.Amount ?? 0:F0}";
            default:
                return "Unknown command.";
        }
    }

    public void ExecuteBatch(IEnumerable<string> commands)
    {
        foreach (var cmd in commands)
        {
            ExecuteCommand(cmd);
        }
    }
}
