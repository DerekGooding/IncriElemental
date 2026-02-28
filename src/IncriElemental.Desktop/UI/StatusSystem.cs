using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Desktop.Visuals;

namespace IncriElemental.Desktop.UI;

public class StatusSystem
{
    public void Draw(SpriteBatch spriteBatch, GameEngine engine, SpriteFont? font, Texture2D pixel, VisualManager visuals, int x)
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
        DrawManifestations(spriteBatch, engine, font, x, (int)y);
    }

    private void DrawManifestations(SpriteBatch spriteBatch, GameEngine engine, SpriteFont font, int x, int yOffset)
    {
        var manifestations = engine.State.Manifestations;
        if (manifestations.Count == 0) return;

        var defs = engine.GetDefinitions().ToDictionary(d => d.Id, d => d.Name);
        spriteBatch.DrawString(font, "MANIFESTATIONS", new Vector2(x, yOffset), Color.Gray * 0.5f);
        var curY = yOffset + 30;

        foreach (var entry in manifestations.Where(m => m.Value > 0).OrderBy(m => m.Key))
        {
            var name = defs.GetValueOrDefault(entry.Key, entry.Key.ToUpper());
            var label = $"{name}: {entry.Value}";
            spriteBatch.DrawString(font, label, new Vector2(x, curY), Color.White * 0.8f, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            curY += 22;
        }
    }
}
