using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace IncriElemental.Desktop.UI;

public static class UiMetadataTracker
{
    public class UiElement
    {
        public string Type { get; set; } = "";
        public string Text { get; set; } = "";
        public string Intent { get; set; } = ""; // Semantic Intent for Agentic AI
        public Rectangle Bounds { get; set; }
    }

    private static readonly List<UiElement> _elements = new();
    public static bool IsEnabled { get; set; } = false;

    public static void Clear() => _elements.Clear();

    public static void Register(string type, string text, Rectangle bounds, string intent = "")
    {
        if (!IsEnabled) return;
        _elements.Add(new UiElement { Type = type, Text = text, Bounds = bounds, Intent = intent });
    }

    public static List<UiElement> GetElements() => new(_elements);
}
