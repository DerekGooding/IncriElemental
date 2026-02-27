using System;
using System.Collections.Generic;
using System.Linq;
using IncriElemental.Core.Models;

namespace IncriElemental.Core.Systems;

public class ManifestationManager
{
    private readonly GameState _state;

    public ManifestationManager(GameState state)
    {
        _state = state;
    }

    public bool Manifest(string thing)
    {
        var aether = _state.GetResource(ResourceType.Aether);
        var earth = _state.GetResource(ResourceType.Earth);
        var fire = _state.GetResource(ResourceType.Fire);
        var water = _state.GetResource(ResourceType.Water);

        switch (thing.ToLower())
        {
            case "speck":
                return TryManifest(thing, aether, 10, () => _state.GetResource(ResourceType.Earth).PerSecond += 0.1, "first_manifestation");
            case "spark":
                return TryManifest(thing, aether, 50, earth, 50, () => _state.GetResource(ResourceType.Fire).PerSecond += 0.05, "fire_unlocked");
            case "droplet":
                return TryManifest(thing, aether, 100, fire, 50, () => _state.GetResource(ResourceType.Water).PerSecond += 0.02, "water_unlocked");
            case "breeze":
                return TryManifest(thing, aether, 200, water, 50, () => _state.GetResource(ResourceType.Air).PerSecond += 0.01, "air_unlocked");
            case "rune_of_attraction":
                return TryManifest(thing, aether, 30, () => _state.GetResource(ResourceType.Aether).PerSecond += 0.5, "automation_unlocked");
            case "altar":
                return TryManifest(thing, aether, 100, earth, 20, () => ExpandStorage(500), "altar_constructed");
            case "forge":
                return TryManifest(thing, fire, 50, earth, 100, null, "forge_constructed");
            
            // --- Tools ---
            case "pickaxe":
                if (_state.Discoveries.GetValueOrDefault("forge_constructed"))
                {
                    return TryManifest(thing, earth, 200, () => _state.GetResource(ResourceType.Earth).Amount += 5, "pickaxe_manifested");
                }
                return false;
            case "focus_crystal":
                if (_state.Discoveries.GetValueOrDefault("forge_constructed"))
                {
                    return TryManifest(thing, fire, 100, aether, 200, () => _state.GetResource(ResourceType.Aether).PerSecond += 1.0, "crystal_manifested");
                }
                return false;

            // --- Advanced Materials ---
            case "clay":
                return TryManifest(thing, earth, 50, water, 50, () => _state.GetResource(ResourceType.Earth).MaxAmount += 100, "clay_discovered");
            case "glass":
                return TryManifest(thing, fire, 100, earth, 100, () => _state.GetResource(ResourceType.Aether).MaxAmount += 100, "glass_discovered");
            
            // --- Goal 9: Mastery Structures ---
            case "well":
                return TryManifest(thing, earth, 300, water, 100, () => _state.GetResource(ResourceType.Water).PerSecond += 0.5, "well_manifested");
            case "brazier":
                return TryManifest(thing, earth, 300, fire, 100, () => _state.GetResource(ResourceType.Fire).PerSecond += 0.5, "brazier_manifested");
            case "wind_chime":
                if (_state.Discoveries.GetValueOrDefault("glass_discovered"))
                {
                    return TryManifest(thing, aether, 500, _state.GetResource(ResourceType.Air), 100, () => _state.GetResource(ResourceType.Air).PerSecond += 0.2, "chime_manifested");
                }
                return false;

            // --- Goal 10: Garden of Life ---
            case "garden":
                return TryManifest(thing, earth, 500, water, 500, () => _state.GetResource(ResourceType.Life).PerSecond += 0.1, "garden_manifested");
            case "familiar":
                return TryManifest(thing, aether, 1000, _state.GetResource(ResourceType.Life), 100, () => {
                    _state.GetResource(ResourceType.Aether).PerSecond += 2.0;
                    _state.GetResource(ResourceType.Earth).PerSecond += 1.0;
                }, "familiar_manifested");

            // --- Goal 11: The Spire ---
            case "spire_foundation":
                return TryManifest(thing, earth, 2000, _state.GetResource(ResourceType.Life), 500, null, "spire_foundation_ready");
            case "spire_shaft":
                if (_state.Discoveries.GetValueOrDefault("spire_foundation_ready"))
                {
                    return TryManifest(thing, fire, 2000, _state.GetResource(ResourceType.Air), 1000, null, "spire_shaft_ready");
                }
                return false;
            case "spire_core":
                if (_state.Discoveries.GetValueOrDefault("spire_shaft_ready"))
                {
                    return TryManifest(thing, aether, 5000, water, 3000, null, "spire_complete");
                }
                return false;
            case "ascend":
                if (_state.Discoveries.GetValueOrDefault("spire_complete"))
                {
                    _state.Discoveries["ascended"] = true;
                    return true;
                }
                return false;

            default:
                return false;
        }
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
