using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Desktop.Visuals;

namespace IncriElemental.Desktop.UI;

public class StatusSystem
{
    private readonly Dictionary<Rectangle, string> _inventoryBounds = [];

    public void Draw(SpriteBatch spriteBatch, GameEngine engine, SpriteFont? font, Texture2D pixel, VisualManager visuals, int x, Point mousePos)
    {
        if (font == null) return;

        float y = 20;
        spriteBatch.DrawString(font, "ESSENCE", new Vector2(x, y), Color.Gray);
        y += 30;

        foreach (var res in engine.State.Resources.Values.Where(r => r.Amount > 0 || r.MaxAmount < 1_000_000_000_000))
        {
            var amountStr = visuals.FormatValue(res.Amount);
            var maxStr = res.MaxAmount > 1_000_000_000_000 ? "INF" : visuals.FormatValue(res.MaxAmount);
            var label = $"{res.Type}: {amountStr}";

            visuals.DrawElement(spriteBatch, res.Type, new Vector2(x - 15, y + 8), 6f);
            spriteBatch.DrawString(font, label, new Vector2(x, y), visuals.GetColor(res.Type));

            if (res.MaxAmount < 1_000_000_000_000)
            {
                var percent = (float)(res.Amount / res.MaxAmount);
                spriteBatch.Draw(pixel, new Rectangle(x, (int)y + 22, 150, 2), Color.Gray * 0.2f);
                spriteBatch.Draw(pixel, new Rectangle(x, (int)y + 22, (int)(150 * percent), 2), visuals.GetColor(res.Type) * 0.5f);
            }
            y += 40;
        }

        y += 20;
        spriteBatch.DrawString(font, "ACTIVE REACTION", new Vector2(x, y), Color.Gray * 0.5f);
        y += 30;
        foreach (var buff in engine.State.ActiveBuffs)
        {
            spriteBatch.DrawString(font, $"{buff.Id}: {buff.RemainingTime:F0}s", new Vector2(x, y), Color.Gold * 0.8f);
            y += 25;
        }

        y += 30;
        DrawManifestations(spriteBatch, engine, font, x, (int)y, mousePos, pixel);
    }

    private void DrawManifestations(SpriteBatch spriteBatch, GameEngine engine, SpriteFont font, int x, int yOffset, Point mousePos, Texture2D pixel)
    {
        _inventoryBounds.Clear();
        var manifestations = engine.State.Manifestations;
        if (manifestations.Count == 0) return;

        var defs = engine.GetDefinitions().ToDictionary(d => d.Id, d => d);
        spriteBatch.DrawString(font, "MANIFESTATIONS", new Vector2(x, yOffset), Color.Gray * 0.5f);
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

            spriteBatch.DrawString(font, label, new Vector2(x, curY), color, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            curY += 22;
        }

        // Draw Tooltips for hovered items
        foreach (var entry in _inventoryBounds)
        {
            if (entry.Key.Contains(mousePos))
            {
                var def = defs[entry.Value];
                var tooltip = GetManifestationTooltip(def, engine);
                DrawTooltip(spriteBatch, font, pixel, tooltip, mousePos);
            }
        }
    }

    private void DrawTooltip(SpriteBatch spriteBatch, SpriteFont font, Texture2D pixel, string tooltip, Point mousePos)
    {
        if (string.IsNullOrEmpty(tooltip)) return;

        var tooltipSize = font.MeasureString(tooltip) * 0.8f;
        var tooltipPos = new Vector2(mousePos.X - tooltipSize.X - 20, mousePos.Y);
        var tooltipRect = new Rectangle((int)tooltipPos.X - 5, (int)tooltipPos.Y - 5, (int)tooltipSize.X + 10, (int)tooltipSize.Y + 10);

        spriteBatch.Draw(pixel, tooltipRect, Color.Black * 0.9f);
        spriteBatch.Draw(pixel, new Rectangle(tooltipRect.Left, tooltipRect.Top, tooltipRect.Width, 1), Color.Gray * 0.5f);
        spriteBatch.Draw(pixel, new Rectangle(tooltipRect.Left, tooltipRect.Bottom, tooltipRect.Width, 1), Color.Gray * 0.5f);
        spriteBatch.Draw(pixel, new Rectangle(tooltipRect.Left, tooltipRect.Top, 1, tooltipRect.Height), Color.Gray * 0.5f);
        spriteBatch.Draw(pixel, new Rectangle(tooltipRect.Right, tooltipRect.Top, 1, tooltipRect.Height), Color.Gray * 0.5f);

        spriteBatch.DrawString(font, tooltip, tooltipPos, Color.LightGoldenrodYellow, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
    }

    private string GetManifestationTooltip(ManifestationDefinition def, GameEngine engine)
    {
        var lines = new List<string>();
        var count = engine.State.Manifestations.GetValueOrDefault(def.Id);
        
        if (def.Effects.Any())
        {
            foreach (var effect in def.Effects)
            {
                if (effect.PerSecondBonus != 0)
                {
                    var baseVal = effect.PerSecondBonus * engine.State.CosmicInsight;
                    var totalVal = baseVal * count;
                    lines.Add($"Produces {baseVal:F1} {effect.Type}/s");
                    if (count > 0) lines.Add($"(Total: {totalVal:F1} {effect.Type}/s)");
                }
                if (effect.MaxAmountBonus != 0)
                {
                    lines.Add($"+{effect.MaxAmountBonus} {effect.Type} Storage");
                }
            }
        }

        if (def.Id == "rune_of_attraction") lines.Add("Automatically focuses the void.");
        if (def.Id == "pickaxe") lines.Add("Increases Aether gain from manual Focus.");
        if (def.Id == "forge") lines.Add("Unlocks advanced manifestation tools.");
        if (def.Id == "familiar") lines.Add("Required for world exploration.");
        if (def.Id.Contains("spire")) lines.Add("A critical component for Ascension.");

        return string.Join("\n", lines);
    }
}
