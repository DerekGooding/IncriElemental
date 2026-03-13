using IncriElemental.Core.Models;

namespace IncriElemental.Core.Systems;

public class AutomationSystem(GameState state)
{
    private readonly GameState _state = state;

    public void Update(double deltaTime, AlchemySystem alchemy)
    {
        ProcessPassiveGeneration(deltaTime, alchemy);
        ProcessManaFlow(deltaTime);
    }

    private void ProcessPassiveGeneration(double deltaTime, AlchemySystem alchemy)
    {
        foreach (var resource in _state.Resources.Values)
        {
            if (resource.PerSecond > 0)
            {
                var alchemyMult = alchemy.GetMultiplier(resource.Type);
                var auraIntensity = _state.Map.Cells.Values.Sum(c => c.Influences.Where(a => a.Type == resource.Type).Sum(a => a.Intensity));
                var auraMult = 1.0 + auraIntensity * 0.1;
                
                resource.Add(resource.PerSecond * alchemyMult * auraMult * deltaTime);
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
