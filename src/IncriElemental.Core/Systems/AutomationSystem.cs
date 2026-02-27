using System;
using IncriElemental.Core.Models;

namespace IncriElemental.Core.Systems;

public class AutomationSystem
{
    private readonly GameState _state;

    public AutomationSystem(GameState state)
    {
        _state = state;
    }

    public void Update(double deltaTime)
    {
        ProcessPassiveGeneration(deltaTime);
        ProcessManaFlow(deltaTime);
    }

    private void ProcessPassiveGeneration(double deltaTime)
    {
        foreach (var resource in _state.Resources.Values)
        {
            if (resource.PerSecond > 0)
            {
                resource.Add(resource.PerSecond * deltaTime);
            }
        }
    }

    private void ProcessManaFlow(double deltaTime)
    {
        var water = _state.GetResource(ResourceType.Water);
        if (_state.Discoveries.GetValueOrDefault("water_unlocked") && water.Amount > 0)
        {
            _state.GetResource(ResourceType.Aether).Add(water.Amount * 0.05 * deltaTime);
        }
    }
}
