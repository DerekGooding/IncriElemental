using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Desktop.Visuals;

namespace IncriElemental.Desktop.UI;

public static class FlowSystem
{
    private static Dictionary<ResourceType, Vector2> _nodePositions = new();
    private static bool _initialized = false;

    private static void InitializeLayout()
    {
        var centerX = UiLayout.Width / 2; var centerY = UiLayout.Height / 2; var layerWidth = 200;
        _nodePositions[ResourceType.Aether] = new Vector2(centerX - layerWidth * 1.5f, centerY);
        var elementX = centerX; var elementGap = 80;
        _nodePositions[ResourceType.Earth] = new Vector2(elementX, centerY - elementGap * 1.5f);
        _nodePositions[ResourceType.Water] = new Vector2(elementX, centerY - elementGap * 0.5f);
        _nodePositions[ResourceType.Air] = new Vector2(elementX, centerY + elementGap * 0.5f);
        _nodePositions[ResourceType.Fire] = new Vector2(elementX, centerY + elementGap * 1.5f);
        _nodePositions[ResourceType.Life] = new Vector2(centerX + layerWidth * 1.5f, centerY);
        _nodePositions[ResourceType.VoidEmbers] = new Vector2(centerX, centerY - 250);
        _initialized = true;
    }

    public static void Draw(SpriteBatch sb, GameEngine engine, SpriteFont font, Texture2D pixel, VisualManager visuals)
    {
        if (!_initialized) InitializeLayout();
        double time = visuals.GetTotalTime();
        
        DrawFlow(sb, pixel, new Vector2(UiLayout.Width / 2 - 400, UiLayout.Height / 2), _nodePositions[ResourceType.Aether], Color.MediumPurple * 0.5f, 2, time);

        foreach (var def in engine.GetDefinitions())
        {
            var inputs = def.Costs.Select(c => c.Type).ToList();
            if (inputs.Count == 0 && def.Id != "focus") continue;
            foreach (var prod in def.Components.OfType<ProducerComponent>())
            {
                if (!_nodePositions.ContainsKey(prod.Type)) continue;
                var endPos = _nodePositions[prod.Type];
                var color = VisualManager.GetColorForId(def.Id) * 0.5f;
                foreach (var inputType in inputs)
                {
                    if (_nodePositions.TryGetValue(inputType, out var startPos)) DrawFlow(sb, pixel, startPos, endPos, color, 1, time);
                }
            }
        }

        foreach (var kvp in _nodePositions)
        {
            var pos = kvp.Value; var color = GetColor(kvp.Key);
            sb.Draw(pixel, new Rectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40), color);
            var text = kvp.Key.ToString(); var textSize = font.MeasureString(text);
            sb.DrawString(font, text, new Vector2(pos.X - textSize.X / 2, pos.Y + 25), Color.White);
            var res = engine.State.GetResource(kvp.Key);
            var amountText = $"{res.Amount:F0}" + (res.PerSecond > 0 ? $" (+{res.PerSecond:F1}/s)" : "");
            var amountSize = font.MeasureString(amountText);
            sb.DrawString(font, amountText, new Vector2(pos.X - amountSize.X / 2, pos.Y + 45), Color.Gray);
        }

        var focusPos = new Vector2(UiLayout.Width / 2 - 400, UiLayout.Height / 2);
        sb.Draw(pixel, new Rectangle((int)focusPos.X - 20, (int)focusPos.Y - 20, 40, 40), Color.MediumPurple);
        sb.DrawString(font, "Focus", new Vector2(focusPos.X - font.MeasureString("Focus").X / 2, focusPos.Y + 25), Color.White);
    }

    private static void DrawFlow(SpriteBatch sb, Texture2D px, Vector2 start, Vector2 end, Color color, int thickness, double time)
    {
        var edge = end - start; var angle = (float)Math.Atan2(edge.Y, edge.X); var length = edge.Length();
        sb.Draw(px, new Rectangle((int)start.X, (int)start.Y, (int)length, thickness), null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        float speed = 100f; float spacing = 50f; float offset = (float)(time * speed) % spacing;
        for (float d = offset; d < length; d += spacing)
        {
            var p = start + Vector2.Normalize(edge) * d;
            sb.Draw(px, new Rectangle((int)p.X - 1, (int)p.Y - 1, 3, 3), Color.White * 0.8f);
        }
    }

    private static Color GetColor(ResourceType type) => type switch {
        ResourceType.Aether => Color.MediumPurple, ResourceType.Earth => Color.SaddleBrown, ResourceType.Air => Color.LightSkyBlue,
        ResourceType.Water => Color.DeepSkyBlue, ResourceType.Fire => Color.OrangeRed, ResourceType.Life => Color.LimeGreen,
        ResourceType.VoidEmbers => Color.Gold, _ => Color.White
    };
}
