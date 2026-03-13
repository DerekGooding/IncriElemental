namespace IncriElemental.Core.Models;

public enum CellType
{
    Void,
    Plain,
    Mountain,
    Ocean,
    Ruins
}

public class WorldCell
{
    public int X { get; set; }
    public int Y { get; set; }
    public CellType Type { get; set; } = CellType.Void;
    public bool IsExplored { get; set; } = false;
    public string RewardId { get; set; } = string.Empty;
    public string LandmarkName { get; set; } = string.Empty;
    public string LandmarkDescription { get; set; } = string.Empty;
    public List<Aura> Influences { get; set; } = [];
}

public class WorldMap
{
    public int Width { get; set; } = 10;
    public int Height { get; set; } = 10;
    public Dictionary<string, WorldCell> Cells { get; set; } = [];

    public WorldMap()
    {
        InitializeLandmarks();
    }

    public void RecalculateAuras(Dictionary<string, int> manifestations, List<ManifestationDefinition> defs)
    {
        foreach (var cell in Cells.Values) cell.Influences.Clear();

        // For now, each landmark projects an aura if explored
        foreach (var cell in Cells.Values.Where(c => c.IsExplored && !string.IsNullOrEmpty(c.LandmarkName)))
        {
            var auraType = GetAuraForCell(cell.Type);
            if (auraType != ResourceType.Aether) // Void doesn't project auras yet
            {
                ApplyAura(cell.X, cell.Y, auraType, 0.5); // Base cell
                // Project to neighbors
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        ApplyAura(cell.X + dx, cell.Y + dy, auraType, 0.2);
                    }
                }
            }
        }
    }

    private void ApplyAura(int x, int y, ResourceType type, double intensity)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return;
        var cell = GetCell(x, y);
        var existing = cell.Influences.FirstOrDefault(a => a.Type == type);
        if (existing != null) existing.Intensity += intensity;
        else cell.Influences.Add(new Aura(type, intensity));
    }

    private ResourceType GetAuraForCell(CellType type) => type switch
    {
        CellType.Mountain => ResourceType.Earth,
        CellType.Ocean => ResourceType.Water,
        CellType.Plain => ResourceType.Air,
        CellType.Ruins => ResourceType.Fire,
        _ => ResourceType.Aether
    };

    private void InitializeLandmarks()
    {
        SetLandmark(2, 2, "Ancient Archive", "A dusty collection of the void's past.", CellType.Ruins);
        SetLandmark(5, 5, "Peak of Origins", "The highest mountain where light first struck.", CellType.Mountain);
        SetLandmark(8, 2, "Eternal Spring", "Pure water flowing since the first manifestation.", CellType.Ocean);
        SetLandmark(2, 8, "Deepest Trench", "A place of immense pressure and ancient power.", CellType.Ocean);
        SetLandmark(7, 7, "Whispering Plain", "The wind carries voices from another time.", CellType.Plain);
    }

    private void SetLandmark(int x, int y, string name, string desc, CellType type)
    {
        var cell = GetCell(x, y);
        cell.LandmarkName = name;
        cell.LandmarkDescription = desc;
        cell.Type = type;
    }

    public string GetKey(int x, int y) => $"{x},{y}";

    public WorldCell GetCell(int x, int y)
    {
        var key = GetKey(x, y);
        if (!Cells.ContainsKey(key))
        {
            Cells[key] = new WorldCell { X = x, Y = y };
        }
        return Cells[key];
    }
}
