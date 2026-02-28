using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IncriElemental.Desktop.UI;

public class Button(Rectangle bounds, string text, Color color, Action onClick, Func<bool>? isVisible = null, string? subtitle = null, GameTab tab = GameTab.Void)
{
    public Rectangle Bounds = bounds;
    public string Text = text;
    public string? Subtitle = subtitle;
    public Color Color = color;
    public Action OnClick = onClick;
    public Func<bool> IsVisible = isVisible ?? (() => true);
    public GameTab Tab = tab;
    public Func<string>? TooltipFunc = null;
    public bool IsHovered { get; private set; }

    public void Update(Point mousePos, int yOffset = 0)
    {
        var bounds = new Rectangle(Bounds.X, Bounds.Y + yOffset, Bounds.Width, Bounds.Height);
        IsHovered = IsVisible() && bounds.Contains(mousePos);
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont? font, Texture2D pixel, int yOffset = 0)
    {
        if (!IsVisible()) return;

        // Draw Outline
        var thickness = 1;
        var b = new Rectangle(Bounds.X, Bounds.Y + yOffset, Bounds.Width, Bounds.Height);
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
            var scale = 1.0f;
            if (textSize.X > Bounds.Width - 10) scale = (Bounds.Width - 10) / textSize.X;
            
            float textYOffset = string.IsNullOrEmpty(Subtitle) ? 0 : -8;
            var textPos = new Vector2(b.Center.X - (textSize.X * scale) / 2, b.Center.Y - (textSize.Y * scale) / 2 + textYOffset);
            spriteBatch.DrawString(font, Text, textPos, Color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            // Subtitle (Cost)
            if (!string.IsNullOrEmpty(Subtitle))
            {
                var subScale = 0.8f;
                var subSize = font.MeasureString(Subtitle) * subScale;
                if (subSize.X > Bounds.Width - 10) subScale *= (Bounds.Width - 10) / subSize.X;
                
                var subPos = new Vector2(b.Center.X - (font.MeasureString(Subtitle).X * subScale) / 2, textPos.Y + (textSize.Y * scale) - 2);
                spriteBatch.DrawString(font, Subtitle, subPos, Color * 0.5f, 0f, Vector2.Zero, subScale, SpriteEffects.None, 0f);
            }
        }
    }

    public bool CheckClick(Point mousePos, int yOffset = 0)
    {
        var bounds = new Rectangle(Bounds.X, Bounds.Y + yOffset, Bounds.Width, Bounds.Height);
        if (IsVisible() && bounds.Contains(mousePos))
        {
            OnClick();
            return true;
        }
        return false;
    }

    public void DrawTooltip(SpriteBatch spriteBatch, SpriteFont? font, Texture2D pixel, int yOffset = 0)
    {
        if (font == null || !IsHovered || TooltipFunc == null) return;
        var tooltip = TooltipFunc();
        if (string.IsNullOrEmpty(tooltip)) return;

        var bounds = new Rectangle(Bounds.X, Bounds.Y + yOffset, Bounds.Width, Bounds.Height);
        var tooltipSize = font.MeasureString(tooltip) * 0.8f;
        var tooltipPos = new Vector2(bounds.Center.X - tooltipSize.X / 2, bounds.Top - tooltipSize.Y - 10);
        var tooltipRect = new Rectangle((int)tooltipPos.X - 5, (int)tooltipPos.Y - 5, (int)tooltipSize.X + 10, (int)tooltipSize.Y + 10);

        // Background
        spriteBatch.Draw(pixel, tooltipRect, Color.Black * 0.9f);
        // Border
        spriteBatch.Draw(pixel, new Rectangle(tooltipRect.Left, tooltipRect.Top, tooltipRect.Width, 1), Color.Gray * 0.5f);
        spriteBatch.Draw(pixel, new Rectangle(tooltipRect.Left, tooltipRect.Bottom, tooltipRect.Width, 1), Color.Gray * 0.5f);
        spriteBatch.Draw(pixel, new Rectangle(tooltipRect.Left, tooltipRect.Top, 1, tooltipRect.Height), Color.Gray * 0.5f);
        spriteBatch.Draw(pixel, new Rectangle(tooltipRect.Right, tooltipRect.Top, 1, tooltipRect.Height), Color.Gray * 0.5f);

        spriteBatch.DrawString(font, tooltip, tooltipPos, Color.LightGoldenrodYellow, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
    }
}
