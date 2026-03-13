using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Systems;
using IncriElemental.Desktop.Visuals;

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

    public void Draw(SpriteBatch spriteBatch, SpriteFont? font, Texture2D pixel, VisualManager visuals)
    {
        if (font != null)
        {
            spriteBatch.DrawString(font, "LOG", new Vector2(20, 20), Color.Gray * 0.5f);
            for (var i = 0; i < _log.Count; i++)
            {
                var alpha = MathHelper.SmoothStep(1.0f, 0.0f, (float)i / MaxLogLines);
                var tokens = RichTextSystem.Parse(_log[i]);
                RichTextSystem.Draw(spriteBatch, font, tokens, new Vector2(20, 60 + (i * 25)), Color.LightGray * alpha, 0.9f, visuals);
            }
        }
    }
}
