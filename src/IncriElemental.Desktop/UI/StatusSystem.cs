using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Core.Systems;
using IncriElemental.Desktop.Visuals;

namespace IncriElemental.Desktop.UI;

public class StatusSystem
{
    private readonly Dictionary<Rectangle, string> _inventoryBounds = [];
    private readonly Dictionary<ResourceType, List<float>> _history = [];
    private float _timer = 0;

    public void Update(float deltaTime, GameEngine engine)
    {
        _timer += deltaTime;
        if (_timer >= 1.0f)
        {
            _timer = 0;
            foreach (var res in engine.State.Resources.Values)
            {
                if (!_history.ContainsKey(res.Type)) _history[res.Type] = [];
                _history[res.Type].Add((float)res.Amount);
                if (_history[res.Type].Count > 20) _history[res.Type].RemoveAt(0);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameEngine engine, SpriteFont? font, Texture2D pixel, VisualManager visuals, int x, Point mousePos)
    {
        if (font == null) return;

        float y = 60; // Start lower to avoid top-aligned buttons, but we need to fit everything
        // Actually, looking at void_main.json, buttons are at Y=10.
        // Let's use Y=55 for the first header.
        y = 55;
        visuals.DrawString(spriteBatch, font, TextService.Instance.Get("HDR_ESSENCE"), new Vector2(x, y), Color.Gray, 0.8f);
        y += 25;

        foreach (var res in engine.State.Resources.Values.Where(r => r.Amount > 0 || r.MaxAmount < 1_000_000_000_000))
        {
            var amountStr = VisualUtils.FormatValue(res.Amount);
            var resName = TextService.Instance.Get($"RES_{res.Type.ToString().ToUpper()}");
            var label = $"{resName}: {amountStr}";

            visuals.DrawElement(spriteBatch, res.Type, new Vector2(x - 12, y + 6), 5f);
            visuals.DrawString(spriteBatch, font, label, new Vector2(x, y), visuals.GetColor(res.Type), 0.8f);

            if (_history.TryGetValue(res.Type, out var h) && h.Count > 1)
            {
                float max = h.Max(); float min = h.Min(); float range = Math.Max(1, max - min);
                for (int i = 0; i < h.Count - 1; i++)
                {
                    float v1 = (h[i] - min) / range; float v2 = (h[i+1] - min) / range;
                    var p1 = new Vector2(x + 130 + i * 2, y + 12 - v1 * 8);
                    var p2 = new Vector2(x + 130 + (i+1) * 2, y + 12 - v2 * 8);
                    DrawLine(spriteBatch, pixel, p1, p2, visuals.GetColor(res.Type) * 0.4f);
                }
            }

            if (res.MaxAmount < 1_000_000_000_000)
            {
                var percent = (float)(res.Amount / res.MaxAmount);
                spriteBatch.Draw(pixel, new Rectangle(x, (int)y + 18, 120, 1), Color.Gray * 0.2f);
                spriteBatch.Draw(pixel, new Rectangle(x, (int)y + 18, (int)(120 * percent), 1), visuals.GetColor(res.Type) * 0.4f);
            }
            y += 30;
        }

        y += 15;
        visuals.DrawString(spriteBatch, font, TextService.Instance.Get("HDR_ACTIVE_REACTION"), new Vector2(x, y), Color.Gray * 0.4f, 0.7f);
        y += 20;
        foreach (var buff in engine.State.ActiveBuffs)
        {
            var pulse = (float)Math.Sin(visuals.GetTotalTime() * 5.0) * 0.2f + 0.8f;
            visuals.DrawElement(spriteBatch, ResourceType.Aether, new Vector2(x - 12, y + 6), 6f * pulse);
            visuals.DrawString(spriteBatch, font, $"{buff.Id}: {buff.RemainingTime:F0}s", new Vector2(x, y), Color.Gold * 0.7f, 0.8f);
            y += 20;
        }

        y += 20;
        DrawManifestations(spriteBatch, engine, font, x, (int)y, mousePos, pixel, visuals);
    }

    private void DrawLine(SpriteBatch sb, Texture2D px, Vector2 start, Vector2 end, Color color, float thickness = 1f)
    {
        var edge = end - start; var angle = (float)Math.Atan2(edge.Y, edge.X);
        var length = edge.Length();
        
        // Draw Glow
        sb.Draw(px, new Rectangle((int)start.X, (int)start.Y, (int)length, (int)(thickness + 2)), null, color * 0.2f, angle, Vector2.Zero, SpriteEffects.None, 0);
        sb.Draw(px, new Rectangle((int)start.X, (int)start.Y, (int)length, (int)thickness), null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
    }

    private void DrawManifestations(SpriteBatch spriteBatch, GameEngine engine, SpriteFont font, int x, int yOffset, Point mousePos, Texture2D pixel, VisualManager visuals)
    {
        _inventoryBounds.Clear();
        var manifestations = engine.State.Manifestations;
        if (manifestations.Count == 0) return;

        var defs = engine.GetDefinitions().ToDictionary(d => d.Id, d => d);
        visuals.DrawString(spriteBatch, font, TextService.Instance.Get("HDR_MANIFESTATIONS"), new Vector2(x, yOffset), Color.Gray * 0.5f);
        var curY = yOffset + 30;

        foreach (var entry in manifestations.Where(m => m.Value > 0).OrderBy(m => m.Key))
        {
            if (!defs.TryGetValue(entry.Key, out var def)) continue;
            var name = string.IsNullOrEmpty(def.OutcomeName) ? def.Name : def.OutcomeName;
            var label = $"{name}: {entry.Value}";
            var size = font.MeasureString(label) * 0.8f;
            var rect = new Rectangle(x, curY, (int)size.X, (int)size.Y);
            _inventoryBounds[rect] = entry.Key;
            var color = Color.White * 0.8f;
            if (rect.Contains(mousePos)) color = Color.Gold;
            visuals.DrawString(spriteBatch, font, label, new Vector2(x, curY), color, 0.8f);
            curY += 22;
        }

        foreach (var entry in _inventoryBounds)
        {
            if (entry.Key.Contains(mousePos))
            {
                var def = defs[entry.Value];
                var tooltip = visuals.GetManifestationTooltip(def, engine);
                visuals.DrawTooltip(spriteBatch, font, pixel, tooltip, mousePos);
            }
        }
    }
}
