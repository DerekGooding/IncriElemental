using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace IncriElemental.Desktop.UI;

public class Button
{
    public Rectangle Bounds;
    public string Text;
    public Color Color;
    public Color TextColor;
    public Action OnClick;
    public Func<bool> IsVisible;

    public Button(Rectangle bounds, string text, Color color, Action onClick, Func<bool> isVisible = null)
    {
        Bounds = bounds;
        Text = text;
        Color = color;
        TextColor = Color.White;
        OnClick = onClick;
        IsVisible = isVisible ?? (() => true);
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont? font, Texture2D pixel)
    {
        if (!IsVisible()) return;
        
        // Draw Outline
        int thickness = 1;
        Rectangle b = Bounds;
        spriteBatch.Draw(pixel, new Rectangle(b.Left, b.Top, b.Width, thickness), Color);
        spriteBatch.Draw(pixel, new Rectangle(b.Left, b.Bottom - thickness, b.Width, thickness), Color);
        spriteBatch.Draw(pixel, new Rectangle(b.Left, b.Top, thickness, b.Height), Color);
        spriteBatch.Draw(pixel, new Rectangle(b.Right - thickness, b.Top, thickness, b.Height), Color);

        // Subtle background fill
        spriteBatch.Draw(pixel, b, Color * 0.05f);
        
        if (font != null)
        {
            Vector2 textSize = font.MeasureString(Text);
            Vector2 textPos = new Vector2(Bounds.Center.X - textSize.X / 2, Bounds.Center.Y - textSize.Y / 2);
            spriteBatch.DrawString(font, Text, textPos, Color);
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
