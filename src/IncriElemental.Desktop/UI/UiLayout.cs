using Microsoft.Xna.Framework;

namespace IncriElemental.Desktop.UI;

public static class UiLayout
{
    public static int Width = 1024;
    public static int Height = 768;

    public static Vector2 GetPosition(float relX, float relY, Vector2 offset = default)
    {
        return new Vector2(Width * relX + offset.X, Height * relY + offset.Y);
    }

    public static Rectangle GetBounds(float relX, float relY, int w, int h, Vector2 offset = default)
    {
        var pos = GetPosition(relX, relY, offset);
        return new Rectangle((int)pos.X - w / 2, (int)pos.Y - h / 2, w, h);
    }

    public static Rectangle GetLeftBounds(float relY, int w, int h, int padding = 20)
    {
        return new Rectangle(padding, (int)(Height * relY), w, h);
    }

    public static Rectangle GetRightBounds(float relY, int w, int h, int padding = 20)
    {
        return new Rectangle(Width - w - padding, (int)(Height * relY), w, h);
    }

    public static Rectangle GetCenterBounds(float relY, int w, int h)
    {
        return new Rectangle(Width / 2 - w / 2, (int)(Height * relY), w, h);
    }
}
