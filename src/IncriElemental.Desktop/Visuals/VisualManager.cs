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

    private Effect? _bloomEffect;
    private RenderTarget2D? _renderTarget;

    public VisualManager(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
        _renderTarget = new RenderTarget2D(graphicsDevice, UiLayout.Width, UiLayout.Height);
    }

    public void LoadEffects(Microsoft.Xna.Framework.Content.ContentManager content)
    {
        try { _bloomEffect = content.Load<Effect>("Bloom"); }
        catch { /* Fallback if shader not compiled */ }
    }

    public void BeginRenderToTarget(GraphicsDevice graphicsDevice)
    {
        graphicsDevice.SetRenderTarget(_renderTarget);
    }

    public void EndRenderToTarget(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        graphicsDevice.SetRenderTarget(null);
        graphicsDevice.Clear(Color.Black);

        spriteBatch.Begin(effect: _bloomEffect);
        spriteBatch.Draw(_renderTarget, Vector2.Zero, Color.White);
        spriteBatch.End();
    }

    public void Clear(GraphicsDevice graphicsDevice, Color color) => graphicsDevice.Clear(color);

    public void DrawScene(SpriteBatch spriteBatch, LogSystem log, ParticleSystem particles, List<Button> buttons, GameTab currentTab, int curOffset)
    {
        log.Draw(spriteBatch, null, _pixel); // We handle font inside LogSystem
        particles.Draw(spriteBatch);

        foreach (var btn in buttons.Where(b => b.Tab == GameTab.None))
        {
            if (btn.IsVisible()) btn.Draw(spriteBatch, null, _pixel, 0);
        }
    }

    public void DrawOverlay(SpriteBatch spriteBatch, float alpha)
    {
        if (alpha > 0)
        {
            spriteBatch.Draw(_pixel, new Rectangle(0, 0, UiLayout.Width, UiLayout.Height), Color.White * alpha);
        }
    }

    public Color GetColor(ResourceType type) => _elementColors.GetValueOrDefault(type, Color.White);
    public Color GetCellColor(CellType type) => _cellColors.GetValueOrDefault(type, Color.Black);

    public static Color GetColorForId(string id)
    {
        if (id.Contains("aether") || id.Contains("attraction")) return Color.MediumPurple;
        if (id.Contains("speck") || id.Contains("foundation") || id.Contains("pickaxe")) return Color.SaddleBrown;
        if (id.Contains("spark") || id.Contains("forge") || id.Contains("brazier")) return Color.OrangeRed;
        if (id.Contains("droplet") || id.Contains("well")) return Color.DodgerBlue;
        if (id.Contains("breeze") || id.Contains("shaft") || id.Contains("clouds")) return Color.LightCyan;
        if (id.Contains("garden")) return Color.LimeGreen;
        if (id.Contains("constellation")) return Color.Gold;
        return Color.Gray;
    }

    public static GameTab GetTabForDef(ManifestationDefinition def)
    {
        if (def.Id.Contains("spire") || def.Id.Contains("well") || def.Id.Contains("brazier") || def.Id.Contains("forge") || def.Id.Contains("clouds"))
            return GameTab.Spire;
        if (def.Id.Contains("garden") || def.Id.Contains("familiar"))
            return GameTab.World;
        if (def.Id.Contains("constellation"))
            return GameTab.Constellation;
        return GameTab.Void;
    }

    public void DrawMap(SpriteBatch spriteBatch, WorldMap map, Point mousePos, Texture2D pixel, int startX, int startY)
    {
        var size = 20;
        var padding = 2;

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
