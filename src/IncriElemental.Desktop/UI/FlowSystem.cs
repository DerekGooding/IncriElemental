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
        var centerX = UiLayout.Width / 2;
        var centerY = UiLayout.Height / 2;
        var layerWidth = 200;
        
        // Manual layout for clarity
        _nodePositions[ResourceType.Aether] = new Vector2(centerX - layerWidth * 1.5f, centerY);
        
        // Elements in a column
        var elementX = centerX;
        var elementGap = 80;
        _nodePositions[ResourceType.Earth] = new Vector2(elementX, centerY - elementGap * 1.5f);
        _nodePositions[ResourceType.Water] = new Vector2(elementX, centerY - elementGap * 0.5f);
        _nodePositions[ResourceType.Air] = new Vector2(elementX, centerY + elementGap * 0.5f);
        _nodePositions[ResourceType.Fire] = new Vector2(elementX, centerY + elementGap * 1.5f);
        
        // Advanced
        _nodePositions[ResourceType.Life] = new Vector2(centerX + layerWidth * 1.5f, centerY);
        _nodePositions[ResourceType.VoidEmbers] = new Vector2(centerX, centerY - 250); // Top center?

        _initialized = true;
    }

    public static void Draw(SpriteBatch spriteBatch, GameEngine engine, SpriteFont font, Texture2D pixel)
    {
        if (!_initialized) InitializeLayout();

        var definitions = engine.GetDefinitions();
        
        // Draw Edges (Production Flows)
        // Focus -> Aether
        DrawFlow(spriteBatch, pixel, new Vector2(UiLayout.Width / 2 - 400, UiLayout.Height / 2), _nodePositions[ResourceType.Aether], Color.MediumPurple * 0.5f, 2);

        foreach (var def in definitions)
        {
            // Find inputs (Costs)
            var inputs = def.Costs.Select(c => c.Type).ToList();
            if (inputs.Count == 0 && def.Id != "focus") continue; // Skip focus/free

            // Find outputs (Producers)
            var producers = def.Components.OfType<ProducerComponent>();
            foreach (var prod in producers)
            {
                if (!_nodePositions.ContainsKey(prod.Type)) continue;

                var endPos = _nodePositions[prod.Type];
                var color = VisualManager.GetColorForId(def.Id) * 0.5f;

                if (inputs.Count > 0)
                {
                    foreach (var inputType in inputs)
                    {
                        if (_nodePositions.TryGetValue(inputType, out var startPos))
                        {
                            DrawFlow(spriteBatch, pixel, startPos, endPos, color, 1);
                        }
                    }
                }
                else
                {
                    // Produced from nothing (or hidden cost like Focus)?
                    // Focus produces Aether, but "Focus" isn't a manifestation with cost.
                }
            }
        }

        // Draw Nodes
        foreach (var kvp in _nodePositions)
        {
            var type = kvp.Key;
            var pos = kvp.Value;
            var color = GetColor(type);
            var rect = new Rectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40);

            // Node Circle/Box
            spriteBatch.Draw(pixel, rect, color);
            
            // Label
            var text = type.ToString();
            var textSize = font.MeasureString(text);
            spriteBatch.DrawString(font, text, new Vector2(pos.X - textSize.X / 2, pos.Y + 25), Color.White);

            // Amount
            var res = engine.State.GetResource(type);
            var amountText = $"{res.Amount:F0}";
            if (res.PerSecond > 0) amountText += $" (+{res.PerSecond:F1}/s)";
            var amountSize = font.MeasureString(amountText);
            spriteBatch.DrawString(font, amountText, new Vector2(pos.X - amountSize.X / 2, pos.Y + 45), Color.Gray);
        }

        // Draw "Focus" Node specially
        var focusPos = new Vector2(UiLayout.Width / 2 - 400, UiLayout.Height / 2);
        spriteBatch.Draw(pixel, new Rectangle((int)focusPos.X - 20, (int)focusPos.Y - 20, 40, 40), Color.MediumPurple);
        spriteBatch.DrawString(font, "Focus", new Vector2(focusPos.X - font.MeasureString("Focus").X / 2, focusPos.Y + 25), Color.White);
    }

    private static void DrawFlow(SpriteBatch sb, Texture2D pixel, Vector2 start, Vector2 end, Color color, int thickness)
    {
        var edge = end - start;
        var angle = (float)Math.Atan2(edge.Y, edge.X);
        var length = edge.Length();

        sb.Draw(pixel, new Rectangle((int)start.X, (int)start.Y, (int)length, thickness), null, color, angle, Vector2.Zero, SpriteEffects.None, 0);

        // Arrowhead? Keep it simple for now.
    }

    private static Color GetColor(ResourceType type)
    {
        return type switch
        {
            ResourceType.Aether => Color.MediumPurple,
            ResourceType.Earth => Color.SaddleBrown,
            ResourceType.Air => Color.LightSkyBlue,
            ResourceType.Water => Color.DeepSkyBlue,
            ResourceType.Fire => Color.OrangeRed,
            ResourceType.Life => Color.LimeGreen,
            ResourceType.VoidEmbers => Color.Gold,
            _ => Color.White
        };
    }
}
