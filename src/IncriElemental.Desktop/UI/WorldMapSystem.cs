using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Desktop.Visuals;

namespace IncriElemental.Desktop.UI;

public class WorldMapSystem
{
    private const int CellSize = 20;
    private const int CellPadding = 2;
    private const int StartX = 20;
    private const int StartY = 100;

    public void Update(GameEngine engine, Point mousePos, bool isLeftClick, AudioManager audio)
    {
        if (engine.State.Manifestations.GetValueOrDefault("familiar") <= 0) return;
        if (!isLeftClick) return;

        for (var x = 0; x < engine.State.Map.Width; x++)
        {
            for (var y = 0; y < engine.State.Map.Height; y++)
            {
                var rect = GetCellBounds(x, y);
                if (rect.Contains(mousePos))
                {
                    if (engine.Explore(x, y)) audio.PlayExplore();
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameEngine engine, Point mousePos, SpriteFont? font, Texture2D pixel, VisualManager visuals)
    {
        if (engine.State.Manifestations.GetValueOrDefault("familiar") <= 0) return;

        if (font != null)
        {
            spriteBatch.DrawString(font, "WORLD EXPLORATION (Click cells to send Familiars)", new Vector2(StartX, StartY - 30), Color.Gray * 0.5f);
        }
        visuals.DrawMap(spriteBatch, engine.State.Map, mousePos, pixel, StartX, StartY);
    }

    private Rectangle GetCellBounds(int x, int y) => new Rectangle(StartX + x * (CellSize + CellPadding), StartY + y * (CellSize + CellPadding), CellSize, CellSize);
}
