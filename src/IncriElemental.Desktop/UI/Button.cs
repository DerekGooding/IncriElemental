using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace IncriElemental.Desktop.UI;

public class Button(Rectangle bounds, string text, Color color, Action onClick, Func<bool> isVisible = null, string? subtitle = null)
{
    public Rectangle Bounds = bounds;
    public string Text = text;
    public string? Subtitle = subtitle;
    public Color Color = color;
    public Action OnClick = onClick;
    public Func<bool> IsVisible = isVisible ?? (() => true);

    public void Draw(SpriteBatch spriteBatch, SpriteFont? font, Texture2D pixel)
    {
        if (!IsVisible()) return;

        // Draw Outline
        var thickness = 1;
        var b = Bounds;
        spriteBatch.Draw(pixel, new Rectangle(b.Left, b.Top, b.Width, thickness), Color * 0.8f);
        spriteBatch.Draw(pixel, new Rectangle(b.Left, b.Bottom - thickness, b.Width, thickness), Color * 0.8f);
        spriteBatch.Draw(pixel, new Rectangle(b.Left, b.Top, thickness, b.Height), Color * 0.8f);
        spriteBatch.Draw(pixel, new Rectangle(b.Right - thickness, b.Top, thickness, b.Height), Color * 0.8f);

        // Subtle background fill
        spriteBatch.Draw(pixel, b, Color * 0.03f);

        if (font != null)
        {
            // Main Text
            var textSize = font.MeasureString(Text);
            float yOffset = string.IsNullOrEmpty(Subtitle) ? 0 : -8;
            var textPos = new Vector2(Bounds.Center.X - textSize.X / 2, Bounds.Center.Y - textSize.Y / 2 + yOffset);
            spriteBatch.DrawString(font, Text, textPos, Color);

            // Subtitle (Cost)
            if (!string.IsNullOrEmpty(Subtitle))
            {
                var subScale = 0.8f;
                var subSize = font.MeasureString(Subtitle) * subScale;
                var subPos = new Vector2(Bounds.Center.X - subSize.X / 2, textPos.Y + textSize.Y - 2);
                spriteBatch.DrawString(font, Subtitle, subPos, Color * 0.5f, 0f, Vector2.Zero, subScale, SpriteEffects.None, 0f);
            }
        }
    }

    public bool CheckClick(Point mousePos)
    {
        if (IsVisible() && Bounds.Contains(mousePos))
        {
            OnClick();
            return true;
        }
        return false;
    }
}
