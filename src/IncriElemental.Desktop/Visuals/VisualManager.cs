using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Models;
using System;
using System.Collections.Generic;

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

    public VisualManager(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }

    public Color GetColor(ResourceType type) => _elementColors.GetValueOrDefault(type, Color.White);

    public void DrawElement(SpriteBatch spriteBatch, ResourceType type, Vector2 position, float scale = 10f)
    {
        Color color = GetColor(type);
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
        if (value >= 1_000_000_000) return $"{(value / 1_000_000_000):F2}G";
        if (value >= 1_000_000) return $"{(value / 1_000_000):F2}M";
        if (value >= 1_000) return $"{(value / 1_000):F2}K";
        return value.ToString("F1");
    }
}
