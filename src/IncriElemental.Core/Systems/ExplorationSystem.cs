using System;
using System.Linq;
using IncriElemental.Core.Models;

namespace IncriElemental.Core.Systems;

public class ExplorationSystem
{
    private readonly GameState _state;
    private readonly Random _random = new();

    public ExplorationSystem(GameState state)
    {
        _state = state;
    }

    public bool Explore(int x, int y)
    {
        // Require at least one familiar to explore
        if (_state.Manifestations.GetValueOrDefault("familiar") < 1) return false;

        var cell = _state.Map.GetCell(x, y);
        if (cell.IsExplored) return false;

        // Exploration cost: 500 Aether
        var aether = _state.GetResource(ResourceType.Aether);
        if (aether.Amount < 500) return false;

        aether.Add(-500);
        cell.IsExplored = true;
        cell.Type = (CellType)_random.Next(1, 5); // Discover a type other than Void

        _state.History.Add($"Exploration: Found {cell.Type} at ({x}, {y}).");
        
        // Grant a random reward
        GrantReward(cell.Type);
        
        return true;
    }

    private void GrantReward(CellType type)
    {
        switch (type)
        {
            case CellType.Plain:
                _state.GetResource(ResourceType.Earth).Add(1000);
                break;
            case CellType.Mountain:
                _state.GetResource(ResourceType.Fire).Add(500);
                break;
            case CellType.Ocean:
                _state.GetResource(ResourceType.Water).Add(500);
                break;
            case CellType.Ruins:
                _state.GetResource(ResourceType.Aether).Add(2000);
                _state.History.Add("Exploration: Found an ancient magical artifact!");
                break;
        }
    }
}
