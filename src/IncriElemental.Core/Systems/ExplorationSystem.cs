using IncriElemental.Core.Models;

namespace IncriElemental.Core.Systems;

public class ExplorationSystem(GameState state)
{
    private readonly GameState _state = state;
    private readonly Random _random = new();

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

        if (!string.IsNullOrEmpty(cell.LandmarkName))
        {
            _state.History.Add($"Exploration: Discovered {cell.LandmarkName} at ({x}, {y})!");
            _state.History.Add(cell.LandmarkDescription);
            GrantLandmarkReward(cell.LandmarkName);
        }
        else
        {
            cell.Type = (CellType)_random.Next(1, 5); // Discover a type other than Void
            _state.History.Add($"Exploration: Found {cell.Type} at ({x}, {y}).");
            GrantReward(cell.Type);
        }

        return true;
    }

    private void GrantLandmarkReward(string name)
    {
        switch (name)
        {
            case "Ancient Archive":
                _state.GetResource(ResourceType.Aether).Add(5000);
                _state.Discoveries["archive_unlocked"] = true;
                break;
            case "Peak of Origins":
                _state.GetResource(ResourceType.Fire).Add(2000);
                break;
            case "Eternal Spring":
                _state.GetResource(ResourceType.Water).Add(2000);
                break;
            case "Deepest Trench":
                _state.GetResource(ResourceType.Water).Add(5000);
                break;
            case "Whispering Plain":
                _state.GetResource(ResourceType.Air).Add(2000);
                break;
        }
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
