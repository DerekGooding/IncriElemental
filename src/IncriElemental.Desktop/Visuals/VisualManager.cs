using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Models;
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
}
