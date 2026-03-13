using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Desktop.Visuals;

namespace IncriElemental.Desktop.UI;

public class MixingTableSystem
{
    private readonly Dictionary<ResourceType, double> _currentIngredients = [];
    private Rectangle _vesselRect = new(400, 200, 200, 200);
    private float _successPulse = 0f;

    public void Update(GameEngine engine, Point mousePos, bool isLeftClick, List<Button> buttons)
    {
        if (_successPulse > 0) _successPulse -= 0.05f;

        if (isLeftClick && _vesselRect.Contains(mousePos))
        {
            if (engine.State.ActiveBuffs.Count < 3)
            {
                var alchemy = typeof(GameEngine).GetField("_alchemy", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(engine) as IncriElemental.Core.Systems.AlchemySystem;
                if (alchemy != null && alchemy.TryMix(new Dictionary<ResourceType, double>(_currentIngredients)))
                {
                    _currentIngredients.Clear();
                    _successPulse = 1.0f;
                }
            }
        }
    }

    public void AddIngredient(ResourceType type, double amount = 100)
    {
        if (!_currentIngredients.ContainsKey(type)) _currentIngredients[type] = 0;
        _currentIngredients[type] += amount;
    }

    public void Clear() => _currentIngredients.Clear();

    public void Draw(SpriteBatch spriteBatch, GameEngine engine, SpriteFont font, Texture2D pixel, VisualManager visuals, Point mousePos, GameTime gameTime)
    {
        var drawRect = _vesselRect;
        if (_currentIngredients.Any())
        {
            var shake = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 20) * 2f;
            drawRect.X += (int)shake;
        }

        spriteBatch.Draw(pixel, drawRect, Color.DarkSlateGray * 0.5f);
        
        var borderColor = Color.Gold;
        if (_successPulse > 0) borderColor = Color.White * _successPulse;
        
        spriteBatch.Draw(pixel, new Rectangle(drawRect.X, drawRect.Y, drawRect.Width, 2), borderColor);
        spriteBatch.Draw(pixel, new Rectangle(drawRect.X, drawRect.Bottom - 2, drawRect.Width, 2), borderColor);

        var title = "ALCHEMICAL VESSEL";
        spriteBatch.DrawString(font, title, new Vector2(drawRect.Center.X - font.MeasureString(title).X / 2, drawRect.Y - 30), Color.Gold);

        var y = drawRect.Y + 20;
        if (!_currentIngredients.Any())
        {
            var msg = "(Empty - Add elements below)";
            spriteBatch.DrawString(font, msg, new Vector2(drawRect.Center.X - font.MeasureString(msg).X * 0.4f / 2, y), Color.Gray, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
        }
        else
        {
            foreach (var kvp in _currentIngredients)
            {
                var line = $"{kvp.Key}: {kvp.Value}";
                spriteBatch.DrawString(font, line, new Vector2(drawRect.X + 20, y), visuals.GetColor(kvp.Key), 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
                y += 25;
            }
            
            var prompt = "CLICK VESSEL TO MIX";
            spriteBatch.DrawString(font, prompt, new Vector2(drawRect.Center.X - font.MeasureString(prompt).X * 0.8f / 2, drawRect.Bottom - 30), Color.White * 0.8f, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
        }

        // Ingredient Buttons
        var startX = drawRect.X - 50;
        var curX = startX;
        var btnY = drawRect.Bottom + 40;

        foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
        {
            if (type == ResourceType.Aether || type == ResourceType.VoidEmbers || type == ResourceType.Life) continue;
            if (!engine.State.Discoveries.ContainsKey($"{type.ToString().ToLower()}_unlocked")) continue;

            var rect = new Rectangle(curX, btnY, 60, 30);
            var hover = rect.Contains(mousePos);
            spriteBatch.Draw(pixel, rect, visuals.GetColor(type) * (hover ? 0.6f : 0.3f));
            spriteBatch.DrawString(font, type.ToString()[..1], new Vector2(rect.Center.X - 5, rect.Center.Y - 10), Color.White);
            
            curX += 70;
        }
    }

    public void HandleIngredientClick(ResourceType type, GameEngine engine)
    {
        if (engine.State.GetResource(type).Amount >= 100)
        {
            AddIngredient(type, 100);
        }
    }
}
