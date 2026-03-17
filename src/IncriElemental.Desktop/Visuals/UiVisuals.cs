using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Desktop.UI;
using System.Text;

namespace IncriElemental.Desktop.Visuals;

public static class UiVisuals
{
    public static void DrawTooltip(SpriteBatch sb, SpriteFont font, Texture2D px, string text, Point mouse, double totalTime, VisualManager visuals)
    {
        if (string.IsNullOrEmpty(text)) return;
        const float scale = 0.8f; const int maxWidth = 300;
        var lines = text.Split('\n'); var wrappedLines = new List<string>();
        foreach (var line in lines) wrappedLines.AddRange(WrapText(font, line, maxWidth, scale));

        var parsedLines = wrappedLines.Select(l => RichTextSystem.Parse(l)).ToList();
        var measuredWidth = parsedLines.Count > 0 ? parsedLines.Max(l => RichTextSystem.Measure(font, l, scale).X) : 0;
        var totalH = parsedLines.Sum(l => RichTextSystem.Measure(font, l, scale).Y) + (parsedLines.Count - 1) * 4;

        var pos = new Vector2(mouse.X + 20, mouse.Y);
        if (pos.X + measuredWidth + 10 > UiLayout.Width) pos.X = mouse.X - measuredWidth - 25;
        if (pos.Y + totalH + 10 > UiLayout.Height) pos.Y = UiLayout.Height - totalH - 15;
        if (pos.Y < 5) pos.Y = 5;

        var r = new Rectangle((int)pos.X - 5, (int)pos.Y - 5, (int)measuredWidth + 10, (int)totalH + 10);
        sb.Draw(px, r, Color.Black * 0.9f);
        var rnd = new Random(r.X + r.Y);
        for (int i = 0; i < 5; i++)
        {
            float ox = (float)((rnd.NextDouble() * r.Width + totalTime * 10) % r.Width);
            float oy = (float)((rnd.NextDouble() * r.Height + totalTime * 5) % r.Height);
            sb.Draw(px, new Rectangle((int)(r.X + ox), (int)(r.Y + oy), 2, 2), Color.Gold * 0.2f);
        }
        sb.Draw(px, new Rectangle(r.X, r.Y, r.Width, 1), Color.Gray * 0.5f); sb.Draw(px, new Rectangle(r.X, r.Bottom, r.Width, 1), Color.Gray * 0.5f); sb.Draw(px, new Rectangle(r.X, r.Y, 1, r.Height), Color.Gray * 0.5f); sb.Draw(px, new Rectangle(r.X, r.Y, 1, r.Height), Color.Gray * 0.5f);
        
        var curY = pos.Y;
        foreach (var tokens in parsedLines)
        {
            RichTextSystem.Draw(sb, font, tokens, new Vector2(pos.X, curY), Color.LightGoldenrodYellow, scale, visuals);
            curY += font.LineSpacing * scale + 4;
        }

        // Register tooltip in metadata
        UiMetadataTracker.Register("Tooltip", text, r, "InformationPopup");
    }

    private static List<string> WrapText(SpriteFont font, string text, int maxW, float scale)
    {
        var words = text.Split(' '); var lines = new List<string>(); var curLine = new StringBuilder();
        foreach (var word in words)
        {
            var testLine = curLine.Length == 0 ? word : curLine + " " + word;
            var tokens = RichTextSystem.Parse(testLine);
            if (RichTextSystem.Measure(font, tokens, scale).X > maxW)
            {
                if (curLine.Length > 0) { lines.Add(curLine.ToString()); curLine.Clear(); curLine.Append(word); }
                else { lines.Add(word); }
            }
            else curLine.Append((curLine.Length == 0 ? "" : " ") + word);
        }
        if (curLine.Length > 0) lines.Add(curLine.ToString());
        return lines;
    }

    public static void DrawPanel(SpriteBatch sb, Texture2D px, Rectangle r, Color color, double totalTime, float activity, float opacity = 0.1f)
    {
        sb.Draw(px, r, Color.Black * opacity);
        sb.Draw(px, r, color * (opacity * 0.5f));
        var pulse = (float)Math.Sin(totalTime * 3.0) * 0.2f + 0.8f;
        int t = 1;
        sb.Draw(px, new Rectangle(r.X, r.Y, r.Width, t), color * (0.5f * pulse));
        sb.Draw(px, new Rectangle(r.X, r.Bottom - t, r.Width, t), color * (0.5f * pulse));
        sb.Draw(px, new Rectangle(r.X, r.Y, t, r.Height), color * (0.5f * pulse));
        sb.Draw(px, new Rectangle(r.Right - t, r.Y, t, r.Height), color * (0.5f * pulse));

        // Adaptive Runic HUD: Moving rune accents along edges
        float speedMult = 1.0f + activity * 10.0f;
        float offset = (float)(totalTime * 50.0 * speedMult);
        for (int i = 0; i < 4; i++) {
            float edgePos = (offset + i * 200) % (r.Width * 2 + r.Height * 2);
            Vector2 runePos;
            if (edgePos < r.Width) runePos = new Vector2(r.X + edgePos, r.Y);
            else if (edgePos < r.Width + r.Height) runePos = new Vector2(r.Right, r.Y + (edgePos - r.Width));
            else if (edgePos < r.Width * 2 + r.Height) runePos = new Vector2(r.Right - (edgePos - (r.Width + r.Height)), r.Bottom);
            else runePos = new Vector2(r.X, r.Bottom - (edgePos - (r.Width * 2 + r.Height)));
            sb.Draw(px, new Rectangle((int)runePos.X - 2, (int)runePos.Y - 2, 4, 4), color * pulse);
        }

        // Corner accents
        sb.Draw(px, new Rectangle(r.X, r.Y, 15, 2), color * pulse); sb.Draw(px, new Rectangle(r.X, r.Y, 2, 15), color * pulse);
        sb.Draw(px, new Rectangle(r.Right - 15, r.Y, 15, 2), color * pulse); sb.Draw(px, new Rectangle(r.Right - 2, r.Y, 2, 15), color * pulse);
        sb.Draw(px, new Rectangle(r.X, r.Bottom - 2, 15, 2), color * pulse); sb.Draw(px, new Rectangle(r.X, r.Bottom - 15, 2, 15), color * pulse);
        sb.Draw(px, new Rectangle(r.Right - 15, r.Bottom - 2, 15, 2), color * pulse); sb.Draw(px, new Rectangle(r.Right - 2, r.Bottom - 15, 2, 15), color * pulse);
        sb.Draw(px, new Rectangle(r.Center.X - 5, r.Y - 2, 10, 4), color * pulse); sb.Draw(px, new Rectangle(r.Center.X - 5, r.Bottom - 2, 10, 4), color * pulse);
    }
}
