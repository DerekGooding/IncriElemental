using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace IncriElemental.Desktop.UI;

public class InventorySystem
{
    public void Draw(SpriteBatch spriteBatch, GameEngine engine, SpriteFont? font, Texture2D pixel, int x, int yOffset)
    {
        if (font == null) return;

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
