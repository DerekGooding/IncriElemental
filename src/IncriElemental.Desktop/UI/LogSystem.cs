using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IncriElemental.Desktop.UI;

public class LogSystem
{
    private readonly List<string> _log = [];
    private const int MaxLogLines = 10;

    public void AddToLog(string message)
    {
        if (string.IsNullOrEmpty(message)) return;
        if (_log.Count > 0 && _log[0] == message) return;
        _log.Insert(0, message);
        if (_log.Count > MaxLogLines) _log.RemoveAt(MaxLogLines);
    }

    public void Clear() => _log.Clear();

    public bool Contains(string message) => _log.Contains(message);

    public void Draw(SpriteBatch spriteBatch, SpriteFont? font, Texture2D pixel)
    {
        var logWidth = (int)(UiLayout.Width * 0.25f);
        spriteBatch.Draw(pixel, new Rectangle(0, 0, logWidth, UiLayout.Height), Color.Black * 0.3f);
        spriteBatch.Draw(pixel, new Rectangle(logWidth, 0, 1, UiLayout.Height), Color.Gray * 0.2f); // Border

        if (font != null)
        {
            spriteBatch.DrawString(font, "LOG", new Vector2(20, 20), Color.Gray * 0.5f);
            for (var i = 0; i < _log.Count; i++)
            {
                var alpha = 1.0f - (i * 0.1f);
                var textColor = Color.LightGray;
                if (_log[i].Contains("void is not") || _log[i].Contains("consciousness") || _log[i].Contains("We are not the first"))
                    textColor = Color.Cyan;

                spriteBatch.DrawString(font, _log[i], new Vector2(20, 60 + (i * 25)), textColor * alpha, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
            }
        }
    }
}
