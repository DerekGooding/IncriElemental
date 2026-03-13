using Microsoft.Xna.Framework;
using IncriElemental.Core.Models;

namespace IncriElemental.Desktop.Visuals;

public static class ColorPalette
{
    public static readonly Dictionary<ResourceType, Color> ElementColors = new()
    {
        { ResourceType.Aether, Color.MediumPurple },
        { ResourceType.Earth, Color.SaddleBrown },
        { ResourceType.Fire, Color.OrangeRed },
        { ResourceType.Water, Color.DodgerBlue },
        { ResourceType.Air, Color.LightCyan },
        { ResourceType.Life, Color.LimeGreen }
    };

    public static readonly Dictionary<CellType, Color> CellColors = new()
    {
        { CellType.Void, Color.Black },
        { CellType.Plain, Color.DarkGreen },
        { CellType.Mountain, Color.SlateGray },
        { CellType.Ocean, Color.MidnightBlue },
        { CellType.Ruins, Color.DarkGoldenrod }
    };
}
