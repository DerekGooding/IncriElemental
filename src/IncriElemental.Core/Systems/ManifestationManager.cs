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
            case "rune_of_attraction":
                if (_state.Discoveries.GetValueOrDefault("automation_unlocked")) return false;
                return TryManifest(thing, aether, 30, () => _state.GetResource(ResourceType.Aether).PerSecond += 2.0, "automation_unlocked");
            
            case "speck":
                if (_state.Manifestations.GetValueOrDefault("speck") >= 20) return false;
                return TryManifest(thing, aether, 10, () => {
                    var earthRes = _state.GetResource(ResourceType.Earth);
                    earthRes.PerSecond += 1.0;
                }, "first_manifestation");

            case "spark":
                if (_state.Manifestations.GetValueOrDefault("spark") >= 10) return false;
                return TryManifest(thing, aether, 50, earth, 50, () => _state.GetResource(ResourceType.Fire).PerSecond += 0.5, "fire_unlocked");

            case "droplet":
                if (_state.Manifestations.GetValueOrDefault("droplet") >= 10) return false;
                return TryManifest(thing, aether, 100, fire, 50, () => _state.GetResource(ResourceType.Water).PerSecond += 0.5, "water_unlocked");
            
            case "breeze":
                if (_state.Manifestations.GetValueOrDefault("breeze") >= 10) return false;
                return TryManifest(thing, aether, 200, water, 50, () => _state.GetResource(ResourceType.Air).PerSecond += 1.0, "air_unlocked");

            case "altar":
                if (_state.Discoveries.GetValueOrDefault("altar_constructed")) return false;
                return TryManifest(thing, aether, 100, earth, 20, () => ExpandStorage(1000), "altar_constructed");

            case "forge":
                if (_state.Discoveries.GetValueOrDefault("forge_constructed")) return false;
                return TryManifest(thing, fire, 50, earth, 100, null, "forge_constructed");
            
            // --- Tools ---
            case "pickaxe":
                if (!_state.Discoveries.GetValueOrDefault("forge_constructed") || _state.Discoveries.GetValueOrDefault("pickaxe_manifested")) return false;
                return TryManifest(thing, earth, 200, () => _state.GetResource(ResourceType.Earth).Amount += 50, "pickaxe_manifested");

            case "focus_crystal":
                if (!_state.Discoveries.GetValueOrDefault("forge_constructed") || _state.Discoveries.GetValueOrDefault("crystal_manifested")) return false;
                return TryManifest(thing, fire, 100, aether, 200, () => _state.GetResource(ResourceType.Aether).PerSecond += 5.0, "crystal_manifested");

            // --- Advanced Materials ---
            case "clay":
                if (_state.Discoveries.GetValueOrDefault("clay_discovered")) return false;
                return TryManifest(thing, earth, 50, water, 50, () => _state.GetResource(ResourceType.Earth).MaxAmount += 500, "clay_discovered");

            case "glass":
                if (_state.Discoveries.GetValueOrDefault("glass_discovered")) return false;
                return TryManifest(thing, fire, 100, earth, 100, () => _state.GetResource(ResourceType.Aether).MaxAmount += 500, "glass_discovered");
            
            // --- Goal 9: Mastery Structures ---
            case "well":
                if (_state.Manifestations.GetValueOrDefault("well") >= 5) return false;
                return TryManifest(thing, earth, 300, water, 100, () => _state.GetResource(ResourceType.Water).PerSecond += 5.0, "well_manifested");

            case "brazier":
                if (_state.Manifestations.GetValueOrDefault("brazier") >= 5) return false;
                return TryManifest(thing, earth, 300, fire, 100, () => _state.GetResource(ResourceType.Fire).PerSecond += 5.0, "brazier_manifested");

            case "wind_chime":
                if (!_state.Discoveries.GetValueOrDefault("glass_discovered") || _state.Manifestations.GetValueOrDefault("wind_chime") >= 5) return false;
                return TryManifest(thing, aether, 500, _state.GetResource(ResourceType.Air), 100, () => _state.GetResource(ResourceType.Air).PerSecond += 5.0, "chime_manifested");

            // --- Goal 10: Garden of Life ---
            case "garden":
                if (_state.Manifestations.GetValueOrDefault("garden") >= 5) return false;
                return TryManifest(thing, earth, 500, water, 500, () => _state.GetResource(ResourceType.Life).PerSecond += 2.0, "garden_manifested");

            case "familiar":
                if (_state.Manifestations.GetValueOrDefault("familiar") >= 5) return false;
                return TryManifest(thing, aether, 1000, _state.GetResource(ResourceType.Life), 100, () => {
                    _state.GetResource(ResourceType.Aether).PerSecond += 20.0;
                    _state.GetResource(ResourceType.Earth).PerSecond += 10.0;
                }, "familiar_manifested");

            // --- Goal 11: The Spire ---
            case "spire_foundation":
                if (_state.Discoveries.GetValueOrDefault("spire_foundation_ready")) return false;
                return TryManifest(thing, earth, 1000, _state.GetResource(ResourceType.Life), 200, null, "spire_foundation_ready");

            case "spire_shaft":
                if (!_state.Discoveries.GetValueOrDefault("spire_foundation_ready") || _state.Discoveries.GetValueOrDefault("spire_shaft_ready")) return false;
                return TryManifest(thing, fire, 2000, _state.GetResource(ResourceType.Air), 1000, null, "spire_shaft_ready");

            case "spire_core":
                if (!_state.Discoveries.GetValueOrDefault("spire_shaft_ready") || _state.Discoveries.GetValueOrDefault("spire_complete")) return false;
                return TryManifest(thing, aether, 5000, water, 3000, null, "spire_complete");

            case "ascend":
                if (!_state.Discoveries.GetValueOrDefault("spire_complete") || _state.Discoveries.GetValueOrDefault("ascended")) return false;
                _state.Discoveries["ascended"] = true;
                return true;
            case "reset":
                if (_state.Discoveries.GetValueOrDefault("ascended"))
                {
                    ResetForNewGamePlus();
                    return true;
                }
                return false;

            default:
                return false;
        }
    }

    private void ResetForNewGamePlus()
    {
        double multiplier = _state.CosmicInsight + 0.5;
        _state.Resources.Clear();
        _state.Discoveries.Clear();
        _state.Manifestations.Clear();
        _state.History.Clear();
        _state.TotalGameTime = 0;
        
        // Initialize new state with multiplier
        _state.CosmicInsight = multiplier;
        _state.Resources[ResourceType.Aether] = new Resource(ResourceType.Aether);
        _state.History.Add($"You awaken with Cosmic Insight x{multiplier:F1}.");
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
