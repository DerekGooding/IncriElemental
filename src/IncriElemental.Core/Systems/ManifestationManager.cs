using IncriElemental.Core.Models;

namespace IncriElemental.Core.Systems;

public class ManifestationManager
{
    private readonly GameState _state;
    private readonly Dictionary<string, ManifestationDefinition> _definitions = new();

    public ManifestationManager(GameState state, IEnumerable<ManifestationDefinition>? definitions = null)
    {
        _state = state;
        if (definitions != null)
        {
            foreach (var def in definitions) _definitions[def.Id] = def;
        }
    }

    public IEnumerable<ManifestationDefinition> GetDefinitions() => _definitions.Values;

    public bool Manifest(string id)
    {
        if (!_definitions.TryGetValue(id.ToLower(), out var def))
        {
            if (id.ToLower() == "reset" && _state.Discoveries.GetValueOrDefault("ascended"))
            {
                ResetForNewGamePlus();
                return true;
            }
            return false;
        }

        // Discovery and Count Checks
        if (!string.IsNullOrEmpty(def.RequiredDiscovery) && !_state.Discoveries.GetValueOrDefault(def.RequiredDiscovery)) return false;
        if (def.Id == "void_infusion" && !_state.VoidInfusionUnlocked) return false;
        if (_state.Manifestations.GetValueOrDefault(def.Id) >= def.MaxCount) return false;
        if (!string.IsNullOrEmpty(def.DiscoveryKey) && _state.Discoveries.GetValueOrDefault(def.DiscoveryKey) && def.MaxCount == 1) return false;

        // Cost Check
        foreach (var cost in def.Costs)
        {
            if (_state.GetResource(cost.Type).Amount < cost.Amount) return false;
        }

        // Execute Manifestation
        foreach (var cost in def.Costs)
        {
            _state.GetResource(cost.Type).Add(-cost.Amount);
        }

        _state.Manifestations[def.Id] = _state.Manifestations.GetValueOrDefault(def.Id) + 1;
        if (!string.IsNullOrEmpty(def.DiscoveryKey)) _state.Discoveries[def.DiscoveryKey] = true;

        foreach (var effect in def.Effects)
        {
            var res = _state.GetResource(effect.Type);
            res.PerSecond += effect.PerSecondBonus;
            res.MaxAmount += effect.MaxAmountBonus;
            res.Add(effect.InstantAmount);
        }

        if (def.Id == "speck" && _state.Manifestations["speck"] == 1)
            _state.History.Add("A speck of matter appears.");

        return true;
    }

    private void ResetForNewGamePlus()
    {
        var multiplier = _state.CosmicInsight + 0.5;
        _state.Resources.Clear();
        _state.Discoveries.Clear();
        _state.Manifestations.Clear();
        _state.History.Clear();
        _state.TotalGameTime = 0;

        // Initialize new state with multiplier
        _state.CosmicInsight = multiplier;
        _state.VoidInfusionUnlocked = true;
        
        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            _state.Resources[type] = new Resource(type);
        }
        
        _state.History.Add($"You awaken with Cosmic Insight x{multiplier:F1}.");
        _state.History.Add("The void feels thinner. Void Infusion is now possible.");
    }

    private bool TryManifest(string thing, Resource primary, double cost, Action? onComplete, string discovery)
    {
        if (primary.Amount >= cost)
        {
            primary.Add(-cost);
            UpdateState(thing, discovery, onComplete);
            return true;
        }
        return false;
    }

    private bool TryManifest(string thing, Resource primary, double pCost, Resource secondary, double sCost, Action? onComplete, string discovery)
    {
        if (primary.Amount >= pCost && secondary.Amount >= sCost)
        {
            primary.Add(-pCost);
            secondary.Add(-sCost);
            UpdateState(thing, discovery, onComplete);
            return true;
        }
        return false;
    }

    private void UpdateState(string thing, string discovery, Action? onComplete)
    {
        _state.Manifestations[thing] = _state.Manifestations.GetValueOrDefault(thing) + 1;
        _state.Discoveries[discovery] = true;
        onComplete?.Invoke();
    }

    private void ExpandStorage(double amount)
    {
        foreach (var resource in _state.Resources.Values)
        {
            resource.MaxAmount += amount;
        }
    }
}
