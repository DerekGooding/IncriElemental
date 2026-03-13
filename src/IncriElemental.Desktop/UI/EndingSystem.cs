using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Systems;

namespace IncriElemental.Desktop.UI;

public class EndingSystem
{
    public void Draw(SpriteBatch spriteBatch, GameEngine engine, SpriteFont? font, Texture2D pixel, GameTime gameTime, Point mousePos, bool isClick, Action resetAction)
    {
        if (font == null) return;

        var centerX = UiLayout.Width / 2;
        var msg = TextService.Instance.Get("END_ASCENSION_COMPLETE");
        spriteBatch.DrawString(font, msg, new Vector2(centerX - font.MeasureString(msg).X / 2, 200), Color.Gold);

        string[] credits = [
            TextService.Instance.Get("CREDIT_DEREK"),
            TextService.Instance.Get("CREDIT_GEMINI"),
            TextService.Instance.Get("CREDIT_MONOGAME"),
            TextService.Instance.Get("CREDIT_THANKS")
        ];
        for (var i = 0; i < credits.Length; i++)
        {
            var pos = new Vector2(centerX - font.MeasureString(credits[i]).X / 2, 400 + i * 40 - (float)gameTime.TotalGameTime.TotalSeconds * 30);
            spriteBatch.DrawString(font, credits[i], pos, Color.DarkGray);
        }

        // Add Reset/New Game+ Button
        var resetRect = new Rectangle(centerX - 100, 600, 200, 50);
        spriteBatch.Draw(pixel, resetRect, Color.Gold * 0.4f);
        var resetText = TextService.Instance.Get("BTN_NEW_AWAKENING");
        var textPos = new Vector2(centerX - font.MeasureString(resetText).X / 2, 625 - font.MeasureString(resetText).Y / 2);
        spriteBatch.DrawString(font, resetText, textPos, Color.DarkGoldenrod);

        if (isClick && resetRect.Contains(mousePos))
        {
            resetAction();
        }
    }
}
