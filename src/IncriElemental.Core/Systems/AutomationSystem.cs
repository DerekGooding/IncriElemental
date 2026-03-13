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
        var definitions = _state.Manifestations.Keys
            .Select(id => _state.Resources.Values.Any() ? null : id) // Dummy to get defs
            .ToList(); // This is inefficient, better to pass defs or have a lookup

        // We'll use a simpler approach for now: AutomationSystem will focus on Resource.PerSecond
        // which should be maintained by ManifestationManager when components are added.
        // Wait, Resource.PerSecond is a flat value. It's better to calculate dynamic components here.

        foreach (var resource in _state.Resources.Values)
        {
            double totalPerSecond = resource.PerSecond;
            
            // Add component-based production (requires access to definitions)
            // For now, let's stick to the multiplier logic and ensure ManifestationManager
            // updates resource.PerSecond when a ProducerComponent is manifested.

            if (totalPerSecond > 0 || _state.Map.Cells.Values.Any(c => c.Influences.Any(a => a.Type == resource.Type)))
            {
                var alchemyMult = alchemy.GetMultiplier(resource.Type);
                var auraIntensity = _state.Map.Cells.Values.Sum(c => c.Influences.Where(a => a.Type == resource.Type).Sum(a => a.Intensity));
                var auraMult = 1.0 + auraIntensity * 0.1;
                
                resource.Add(totalPerSecond * alchemyMult * auraMult * deltaTime);
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
