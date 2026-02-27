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
}

public class WorldMap
{
    public int Width { get; set; } = 10;
    public int Height { get; set; } = 10;
    public Dictionary<string, WorldCell> Cells { get; set; } = new();

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
