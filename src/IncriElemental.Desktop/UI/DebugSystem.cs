using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Desktop.Visuals;

namespace IncriElemental.Desktop.UI;

public class DebugSystem
{
    public void Draw(SpriteBatch spriteBatch, GameEngine engine, SpriteFont? font, Texture2D pixel, VisualManager visuals)
    {
        if (font == null) return;

        var startX = 300;
        var startY = 100;
        spriteBatch.DrawString(font, "EFFICIENCY OBSERVER (AI DEBUG)", new Vector2(startX, startY), Color.Red);
        startY += 40;

        foreach (var res in engine.State.Resources.Values)
        {
            var pps = res.PerSecond;
            var fillTime = res.MaxAmount > 1_000_000_000_000 ? "INF" : $"{(res.MaxAmount - res.Amount) / (pps > 0 ? pps : 1):F1}s";
            var label = $"{res.Type}: +{visuals.FormatValue(pps)}/s (Fill: {fillTime})";
            
            spriteBatch.DrawString(font, label, new Vector2(startX, startY), Color.White);
            startY += 30;
        }

        startY += 20;
        spriteBatch.DrawString(font, "BOTTLENECKS:", new Vector2(startX, startY), Color.Orange);
        startY += 30;

        foreach (var def in engine.GetDefinitions())
        {
            var canAfford = def.Costs.All(c => engine.State.GetResource(c.Type).Amount >= c.Amount);
            if (!canAfford && engine.State.Discoveries.GetValueOrDefault(def.RequiredDiscovery))
            {
                var bottleneck = def.Costs.OrderByDescending(c => (c.Amount - engine.State.GetResource(c.Type).Amount) / (engine.State.GetResource(c.Type).PerSecond > 0 ? engine.State.GetResource(c.Type).PerSecond : 1)).First();
                spriteBatch.DrawString(font, $"{def.Name}: Waiting for {bottleneck.Type}", new Vector2(startX, startY), Color.Gray, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
                startY += 22;
            }
        }
    }
}
