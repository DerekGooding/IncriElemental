using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Models;
using IncriElemental.Desktop.Visuals;
using System.Text.RegularExpressions;

namespace IncriElemental.Desktop.UI;

public enum TokenType { Text, Icon, ColorStart, ColorEnd }

public class RichTextToken
{
    public TokenType Type { get; set; }
    public string Value { get; set; } = string.Empty;
    public Color? Color { get; set; }
    public ResourceType? ResourceType { get; set; }
}

public static class RichTextSystem
{
    private static readonly Regex TokenRegex = new(@"\[(i|c):([^\]]+)\]|\[/c\]", RegexOptions.Compiled);

    public static List<RichTextToken> Parse(string input)
    {
        var tokens = new List<RichTextToken>();
        int lastIndex = 0;

        foreach (Match match in TokenRegex.Matches(input))
        {
            if (match.Index > lastIndex)
            {
                tokens.Add(new RichTextToken { Type = TokenType.Text, Value = input[lastIndex..match.Index] });
            }

            if (match.Value == "[/c]")
            {
                tokens.Add(new RichTextToken { Type = TokenType.ColorEnd });
            }
            else
            {
                var type = match.Groups[1].Value;
                var val = match.Groups[2].Value;

                if (type == "i")
                {
                    if (Enum.TryParse<ResourceType>(val, true, out var resType))
                        tokens.Add(new RichTextToken { Type = TokenType.Icon, ResourceType = resType });
                }
                else if (type == "c")
                {
                    tokens.Add(new RichTextToken { Type = TokenType.ColorStart, Color = ParseColor(val) });
                }
            }
            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < input.Length)
        {
            tokens.Add(new RichTextToken { Type = TokenType.Text, Value = input[lastIndex..] });
        }

        return tokens;
    }

    private static Color ParseColor(string name)
    {
        return name.ToLower() switch
        {
            "purple" => Color.MediumPurple,
            "gold" => Color.Gold,
            "red" => Color.OrangeRed,
            "blue" => Color.DodgerBlue,
            "green" => Color.LimeGreen,
            "cyan" => Color.LightCyan,
            "gray" => Color.Gray,
            _ => Color.White
        };
    }

    public static void Draw(SpriteBatch spriteBatch, SpriteFont font, List<RichTextToken> tokens, Vector2 position, Color defaultColor, float scale, VisualManager visuals)
    {
        var curPos = position;
        var currentColor = defaultColor;
        var colorStack = new Stack<Color>();

        foreach (var token in tokens)
        {
            switch (token.Type)
            {
                case TokenType.Text:
                    var size = font.MeasureString(token.Value) * scale;
                    spriteBatch.DrawString(font, token.Value, curPos, currentColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
                    curPos.X += size.X;
                    break;
                case TokenType.Icon:
                    if (token.ResourceType.HasValue)
                    {
                        var iconSize = 16f * scale;
                        visuals.DrawElement(spriteBatch, token.ResourceType.Value, curPos + new Vector2(iconSize / 2, iconSize / 2), iconSize);
                        curPos.X += iconSize + 4 * scale;
                    }
                    break;
                case TokenType.ColorStart:
                    colorStack.Push(currentColor);
                    if (token.Color.HasValue) currentColor = token.Color.Value;
                    break;
                case TokenType.ColorEnd:
                    if (colorStack.Count > 0) currentColor = colorStack.Pop();
                    break;
            }
        }
    }

    public static Vector2 Measure(SpriteFont font, List<RichTextToken> tokens, float scale)
    {
        float width = 0;
        float height = font.LineSpacing * scale;

        foreach (var token in tokens)
        {
            if (token.Type == TokenType.Text) width += font.MeasureString(token.Value).X * scale;
            else if (token.Type == TokenType.Icon) width += 20 * scale;
        }

        return new Vector2(width, height);
    }
}
