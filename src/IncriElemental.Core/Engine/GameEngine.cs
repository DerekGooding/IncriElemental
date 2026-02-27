using System;
using System.Linq;
using IncriElemental.Core.Models;
using IncriElemental.Core.Systems;

namespace IncriElemental.Core.Engine;

public class GameEngine
{
    public GameState State { get; private set; }
    private readonly ManifestationManager _manifestManager;
    private readonly AutomationSystem _automation;

    public GameEngine(GameState? initialState = null)
    {
        State = initialState ?? new GameState();
        _manifestManager = new ManifestationManager(State);
        _automation = new AutomationSystem(State);
    }

    public void Update(double deltaTime)
    {
        State.TotalGameTime += deltaTime;
        _automation.Update(deltaTime);
    }

    public void Focus()
    {
        double aetherGain = 1.0;
        if (State.Discoveries.GetValueOrDefault("pickaxe_manifested")) aetherGain += 2.0;

        State.GetResource(ResourceType.Aether).Add(aetherGain);
        
        if (State.GetResource(ResourceType.Aether).Amount >= 10 && !State.Discoveries.GetValueOrDefault("void_observed"))
        {
            State.Discoveries["void_observed"] = true;
            State.History.Add("The void feels thick with potential.");
        }
    }

    public bool Manifest(string thing) => _manifestManager.Manifest(thing);
}
