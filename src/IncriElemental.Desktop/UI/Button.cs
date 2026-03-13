using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Desktop.Visuals;
using IncriElemental.Core.Systems;

namespace IncriElemental.Desktop.UI;

public class Button(Rectangle bounds, string text, Color color, Action onClick, Func<bool>? isVisible = null, string? subtitle = null, GameTab tab = GameTab.Void)
{
    public Rectangle Bounds = bounds;
    public string Text = text;
    public string? Subtitle = subtitle;
    public Color Color = color;
    public Action OnClick = onClick;
    public Func<bool> VisibilityFunc = isVisible ?? (() => true);
    public Func<string>? TooltipFunc;
    public GameTab Tab = tab;
    public bool IsHovered { get; private set; }

    public bool IsVisible() => VisibilityFunc();

    public void Update(Point mousePos, int yOffset = 0)
    {
        var b = new Rectangle(Bounds.X, Bounds.Y + yOffset, Bounds.Width, Bounds.Height);
        IsHovered = b.Contains(mousePos);
    }

    public void CheckClick(Point mousePos, int yOffset = 0)
    {
        var b = new Rectangle(Bounds.X, Bounds.Y + yOffset, Bounds.Width, Bounds.Height);
        if (b.Contains(mousePos)) OnClick?.Invoke();
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont? font, Texture2D pixel, VisualManager visuals, int yOffset = 0)
    {
        if (!IsVisible()) return;

        var b = new Rectangle(Bounds.X, Bounds.Y + yOffset, Bounds.Width, Bounds.Height);
        float opacity = IsHovered ? 0.3f : 0.15f;
        visuals.DrawPanel(spriteBatch, pixel, b, Color, opacity);

        if (font != null)
        {
            var locText = (Text.StartsWith("[") && Text.EndsWith("]")) ? TextService.Instance.Get(Text.Trim('[', ']')) : Text;
            var textSize = font.MeasureString(locText);
            var textPos = new Vector2(b.Center.X - textSize.X / 2, b.Center.Y - textSize.Y / 2);
            if (!string.IsNullOrEmpty(Subtitle)) textPos.Y -= 10;
            spriteBatch.DrawString(font, locText, textPos, Color * (IsHovered ? 1.0f : 0.8f));

            if (!string.IsNullOrEmpty(Subtitle))
            {
                var locSub = (Subtitle.StartsWith("[") && Subtitle.EndsWith("]")) ? TextService.Instance.Get(Subtitle.Trim('[', ']')) : Subtitle;
                var subSize = font.MeasureString(locSub) * 0.8f;
                var subPos = new Vector2(b.Center.X - subSize.X / 2, textPos.Y + textSize.Y - 5);
                spriteBatch.DrawString(font, locSub, subPos, Color * 0.5f, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            }
        }
    }

    public void DrawTooltip(SpriteBatch spriteBatch, SpriteFont font, Texture2D pixel, VisualManager visuals, int yOffset = 0)
    {
        if (!IsHovered || TooltipFunc == null) return;
        var tooltip = TooltipFunc();
        if (string.IsNullOrEmpty(tooltip)) return;
        var b = new Rectangle(Bounds.X, Bounds.Y + yOffset, Bounds.Width, Bounds.Height);
        visuals.DrawTooltip(spriteBatch, font, pixel, tooltip, new Point(b.X, b.Top - 10));
    }
}
