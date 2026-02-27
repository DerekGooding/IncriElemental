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
        spriteBatch.Draw(pixel, Bounds, Color);
        
        if (font != null)
        {
            Vector2 textSize = font.MeasureString(Text);
            Vector2 textPos = new Vector2(Bounds.Center.X - textSize.X / 2, Bounds.Center.Y - textSize.Y / 2);
            spriteBatch.DrawString(font, Text, textPos, TextColor);
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
