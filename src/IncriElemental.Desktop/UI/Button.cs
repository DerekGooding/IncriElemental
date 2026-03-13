using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Desktop.Visuals;

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

        var b = new Rectangle(Bounds.X, Bounds.Y + yOffset, Bounds.Width, Bounds.Height);
        spriteBatch.Draw(pixel, new Rectangle(b.Left, b.Top, b.Width, 1), Color * 0.8f);
        spriteBatch.Draw(pixel, new Rectangle(b.Left, b.Bottom - 1, b.Width, 1), Color * 0.8f);
        spriteBatch.Draw(pixel, new Rectangle(b.Left, b.Top, 1, b.Height), Color * 0.8f);
        spriteBatch.Draw(pixel, new Rectangle(b.Right - 1, b.Top, 1, b.Height), Color * 0.8f);
        spriteBatch.Draw(pixel, b, Color * 0.03f);

        if (font != null)
        {
            var textSize = font.MeasureString(Text);
            var textPos = new Vector2(b.Center.X - textSize.X / 2, b.Center.Y - textSize.Y / 2);
            if (!string.IsNullOrEmpty(Subtitle)) textPos.Y -= 10;
            spriteBatch.DrawString(font, Text, textPos, Color);

            if (!string.IsNullOrEmpty(Subtitle))
            {
                var subSize = font.MeasureString(Subtitle) * 0.8f;
                var subPos = new Vector2(b.Center.X - subSize.X / 2, textPos.Y + textSize.Y - 5);
                spriteBatch.DrawString(font, Subtitle, subPos, Color * 0.5f, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
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

    public void DrawTooltip(SpriteBatch spriteBatch, SpriteFont? font, Texture2D pixel, VisualManager visuals, int yOffset = 0)
    {
        if (font == null || !IsHovered || TooltipFunc == null) return;
        var tooltip = TooltipFunc();
        if (string.IsNullOrEmpty(tooltip)) return;

        var bounds = new Rectangle(Bounds.X, Bounds.Y + yOffset, Bounds.Width, Bounds.Height);
        visuals.DrawTooltip(spriteBatch, font, pixel, tooltip, new Point(bounds.X, bounds.Top - 10));
    }
}
