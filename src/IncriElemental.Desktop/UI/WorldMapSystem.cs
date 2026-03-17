using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Desktop.Visuals;

namespace IncriElemental.Desktop.UI;

public class WorldMapSystem
{
    private const int CellSize = 20;
    private const int CellPadding = 2;

    private Point GetStartPoint(WorldMap map)
    {
        int mapWidth = map.Width * (CellSize + CellPadding);
        // Buttons end at 215 + 200 = 415. Start map at 430.
        // The remaining area is 1024 - 430 - 210 = 384.
        // We can center the map in that 384px area.
        int availWidth = (UiLayout.Width - 210) - 430;
        int startX = 430 + Math.Max(0, (availWidth - mapWidth) / 2);
        return new Point(startX, 100);
    }

    public void Update(GameEngine engine, Point mousePos, bool isLeftClick, AudioManager audio)
    {
        if (engine.State.Manifestations.GetValueOrDefault("familiar") <= 0) return;
        if (!isLeftClick) return;

        var start = GetStartPoint(engine.State.Map);
        for (var x = 0; x < engine.State.Map.Width; x++)
        {
            for (var y = 0; y < engine.State.Map.Height; y++)
            {
                var rect = GetCellBounds(x, y, start.X, start.Y);
                if (rect.Contains(mousePos))
                {
                    if (engine.Explore(x, y)) audio.PlayExplore();
                }
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameEngine engine, Point mousePos, SpriteFont? font, Texture2D pixel, VisualManager visuals, GameTime gt)
    {
        // We ALWAYS register the map metadata if the tab is active, even if no familiar is present (to show available space)
        var start = GetStartPoint(engine.State.Map);
        int mapWidth = engine.State.Map.Width * (CellSize + CellPadding);
        int mapHeight = engine.State.Map.Height * (CellSize + CellPadding);
        var mapBounds = new Rectangle(start.X - 5, start.Y - 5, mapWidth + 10, mapHeight + 10);
        UiMetadataTracker.Register("WorldMap", "World Exploration Grid", mapBounds, "ExplorationInterface");

        if (engine.State.Manifestations.GetValueOrDefault("familiar") <= 0) 
        {
            if (font != null)
            {
                visuals.DrawString(spriteBatch, font, "REMAIN VOID UNTIL FAMILIARS AWAKEN", new Vector2(start.X, start.Y + 20), Color.Gray * 0.3f);
            }
            return;
        }

        if (font != null)
        {
            visuals.DrawString(spriteBatch, font, "WORLD EXPLORATION (Click cells to send Familiars)", new Vector2(start.X, start.Y - 30), Color.Gray * 0.5f, 0.7f);
        }
        visuals.DrawMap(spriteBatch, engine.State.Map, mousePos, pixel, start.X, start.Y, gt);
    }

    private Rectangle GetCellBounds(int x, int y, int sx, int sy) => new Rectangle(sx + x * (CellSize + CellPadding), sy + y * (CellSize + CellPadding), CellSize, CellSize);
}
