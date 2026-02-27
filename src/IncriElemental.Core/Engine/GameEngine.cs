using System;
using System.Linq;
using IncriElemental.Core.Models;

namespace IncriElemental.Core.Engine;

public class GameEngine
{
    public GameState State { get; private set; }

    public GameEngine(GameState? initialState = null)
    {
        State = initialState ?? new GameState();
    }

    public void Update(double deltaTime)
    {
        State.TotalGameTime += deltaTime;
        
        // Update resources based on per-second generation
        foreach (var resource in State.Resources.Values)
        {
            if (resource.PerSecond > 0)
            {
                resource.Add(resource.PerSecond * deltaTime);
            }
        }

        // --- Mana Flow (Water Automation) ---
        var water = State.GetResource(ResourceType.Water);
        if (State.Discoveries.GetValueOrDefault("water_unlocked") && water.Amount > 0)
        {
            // Water facilitates flow: a small percentage of water amount helps gather Aether
            State.GetResource(ResourceType.Aether).Add(water.Amount * 0.05 * deltaTime);
        }
    }

    // --- Actions ---

    public void Focus()
    {
        // Basic Focus action: Gather Aether
        double aetherGain = 1.0;
        State.GetResource(ResourceType.Aether).Add(aetherGain);
        
        if (State.GetResource(ResourceType.Aether).Amount >= 10 && !State.Discoveries["void_observed"])
        {
            State.Discoveries["void_observed"] = true;
        }
    }

    public bool Manifest(string thing)
    {
        var aether = State.GetResource(ResourceType.Aether);
        var earth = State.GetResource(ResourceType.Earth);
        var fire = State.GetResource(ResourceType.Fire);
        var water = State.GetResource(ResourceType.Water);

        if (thing == "speck" && aether.Amount >= 10)
        {
            aether.Add(-10);
            State.Manifestations["speck"] = State.Manifestations.GetValueOrDefault("speck") + 1;
            State.Discoveries["first_manifestation"] = true;
            State.GetResource(ResourceType.Earth).PerSecond += 0.1;
            return true;
        }

        if (thing == "spark" && aether.Amount >= 50 && earth.Amount >= 50)
        {
            aether.Add(-50);
            earth.Add(-50);
            State.Manifestations["spark"] = State.Manifestations.GetValueOrDefault("spark") + 1;
            State.Discoveries["fire_unlocked"] = true;
            State.GetResource(ResourceType.Fire).PerSecond += 0.05;
            return true;
        }

        if (thing == "droplet" && aether.Amount >= 100 && fire.Amount >= 50)
        {
            aether.Add(-100);
            fire.Add(-50);
            State.Manifestations["droplet"] = State.Manifestations.GetValueOrDefault("droplet") + 1;
            State.Discoveries["water_unlocked"] = true;
            State.GetResource(ResourceType.Water).PerSecond += 0.02;
            return true;
        }

        if (thing == "breeze" && aether.Amount >= 200 && water.Amount >= 50)
        {
            aether.Add(-200);
            water.Add(-50);
            State.Manifestations["breeze"] = State.Manifestations.GetValueOrDefault("breeze") + 1;
            State.Discoveries["air_unlocked"] = true;
            State.GetResource(ResourceType.Air).PerSecond += 0.01;
            return true;
        }

        if (thing == "rune_of_attraction" && aether.Amount >= 30)
        {
            aether.Add(-30);
            State.Manifestations["rune_of_attraction"] = State.Manifestations.GetValueOrDefault("rune_of_attraction") + 1;
            State.Discoveries["automation_unlocked"] = true;
            State.GetResource(ResourceType.Aether).PerSecond += 0.5;
            return true;
        }

        if (thing == "altar" && aether.Amount >= 100 && earth.Amount >= 20)
        {
            aether.Add(-100);
            earth.Add(-20);
            State.Manifestations["altar"] = State.Manifestations.GetValueOrDefault("altar") + 1;
            State.Discoveries["altar_constructed"] = true;
            
            // Expand storage for all resources
            foreach (var resource in State.Resources.Values)
            {
                resource.MaxAmount += 500;
            }
            return true;
        }

        if (thing == "forge" && fire.Amount >= 50 && earth.Amount >= 100)
        {
            fire.Add(-50);
            earth.Add(-100);
            State.Manifestations["forge"] = State.Manifestations.GetValueOrDefault("forge") + 1;
            State.Discoveries["forge_constructed"] = true;
            return true;
        }

        return false;
    }
}
