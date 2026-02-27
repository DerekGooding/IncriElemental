using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Models;

namespace IncriElemental.Desktop.Visuals;

public class VisualManager
{
    private readonly Texture2D _pixel;
    private readonly Dictionary<ResourceType, Color> _elementColors = new()
    {
        { ResourceType.Aether, Color.MediumPurple },
        { ResourceType.Earth, Color.SaddleBrown },
        { ResourceType.Fire, Color.OrangeRed },
        { ResourceType.Water, Color.DodgerBlue },
        { ResourceType.Air, Color.LightCyan },
        { ResourceType.Life, Color.LimeGreen }
    };

    private readonly Dictionary<CellType, Color> _cellColors = new()
    {
        { CellType.Void, Color.Black },
        { CellType.Plain, Color.DarkGreen },
        { CellType.Mountain, Color.SlateGray },
        { CellType.Ocean, Color.MidnightBlue },
        { CellType.Ruins, Color.DarkGoldenrod }
    };

    public VisualManager(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    public Color GetColor(ResourceType type) => _elementColors.GetValueOrDefault(type, Color.White);
    public Color GetCellColor(CellType type) => _cellColors.GetValueOrDefault(type, Color.Black);

    public void DrawMap(SpriteBatch spriteBatch, WorldMap map, Point mousePos, Texture2D pixel)
    {
        var size = 20;
        var padding = 2;
        var startX = 350;
        var startY = 400;

        for (var x = 0; x < map.Width; x++)
        {
            for (var y = 0; y < map.Height; y++)
            {
                var cell = map.GetCell(x, y);
                var rect = new Rectangle(startX + x * (size + padding), startY + y * (size + padding), size, size);

                var color = cell.IsExplored ? GetCellColor(cell.Type) : Color.DarkSlateGray * 0.3f;

                // Highlight if mouse is over
                if (rect.Contains(mousePos)) color *= 1.5f;

                spriteBatch.Draw(pixel, rect, color);

                // Border for unexplored but visible
                if (!cell.IsExplored)
                {
                    var t = 1;
                    spriteBatch.Draw(pixel, new Rectangle(rect.Left, rect.Top, rect.Width, t), Color.Gray * 0.1f);
                    spriteBatch.Draw(pixel, new Rectangle(rect.Left, rect.Bottom - t, rect.Width, t), Color.Gray * 0.1f);
                    spriteBatch.Draw(pixel, new Rectangle(rect.Left, rect.Top, t, rect.Height), Color.Gray * 0.1f);
                    spriteBatch.Draw(pixel, new Rectangle(rect.Right - t, rect.Top, t, rect.Height), Color.Gray * 0.1f);
                }
            }
        }
    }

    public void DrawElement(SpriteBatch spriteBatch, ResourceType type, Vector2 position, float scale = 10f)
    {
        var color = GetColor(type);
        spriteBatch.Draw(_pixel, position, null, color, 0f, new Vector2(0.5f, 0.5f), scale, SpriteEffects.None, 0f);
    }

    public void DrawSpire(SpriteBatch spriteBatch, Dictionary<string, bool> discoveries, double totalTime)
    {
        if (discoveries.ContainsKey("spire_foundation_ready"))
            spriteBatch.Draw(_pixel, new Rectangle(502, 600, 20, 100), Color.Gray * 0.5f);
        if (discoveries.ContainsKey("spire_shaft_ready"))
            spriteBatch.Draw(_pixel, new Rectangle(505, 500, 14, 100), Color.LightGray * 0.5f);
        if (discoveries.ContainsKey("spire_complete"))
        {
            var pulse = (float)Math.Sin(totalTime * 2) * 0.2f + 0.8f;
            spriteBatch.Draw(_pixel, new Rectangle(502, 480, 20, 20), Color.Gold * pulse);
        }
    }

    public string FormatValue(double value)
    {
        if (value >= 1_000_000_000) return $"{value / 1_000_000_000:F2}G";
        if (value >= 1_000_000) return $"{value / 1_000_000:F2}M";
        if (value >= 1_000) return $"{value / 1_000:F2}K";
        return value.ToString("F1");
    }
}
