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
