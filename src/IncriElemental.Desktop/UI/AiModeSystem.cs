using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Desktop.Visuals;
using System.Text.Json;

namespace IncriElemental.Desktop.UI;

public class AiModeSystem(GameEngine engine)
{
    private readonly GameEngine _engine = engine;
    private readonly List<string> _pendingScreenshots = [];
    private Action<GameTab>? _setTab;

    public void Process(string commandPath, Action<GameTab> setTab)
    {
        _setTab = setTab;
        if (File.Exists(commandPath))
        {
            var commands = File.ReadAllLines(commandPath);
            foreach (var cmd in commands)
            {
                var parts = cmd.Split(':', 2);
                var action = parts[0].ToLower();
                if (action == "focus") _engine.Focus();
                if (action == "manifest" && parts.Length > 1) _engine.Manifest(parts[1]);
                if (action == "update" && parts.Length > 1) { if (double.TryParse(parts[1], out var dt)) _engine.Update(dt); }
                if (action == "tab" && parts.Length > 1) { if (Enum.TryParse<GameTab>(parts[1], true, out var tab)) _setTab?.Invoke(tab); }
                if (action == "screenshot" && parts.Length > 1) _pendingScreenshots.Add(parts[1]);
                if (action == "hover" && parts.Length > 1) 
                {
                    // For now, we just log that we want to hover. 
                    // InputManager needs to be told where the mouse is.
                    // Since AiModeSystem doesn't own InputManager, we'll store the hover target.
                    _hoverTarget = parts[1];
                }
            }
        }
    }

    private string? _hoverTarget;

    public void HandleAiUpdate(GameTime gameTime, GraphicsDevice graphicsDevice, string defaultPath, Action<GameTime> drawAction, Action exitAction, VisualManager visuals, List<Button> buttons, InputManager input)
    {
        if (!string.IsNullOrEmpty(_hoverTarget))
        {
            var btn = buttons.FirstOrDefault(b => b.Text.Contains(_hoverTarget, StringComparison.OrdinalIgnoreCase) && b.IsVisible());
            if (btn != null)
            {
                input.SetMousePosition(new Point(btn.Bounds.Center.X, btn.Bounds.Center.Y));
            }
        }

        if (gameTime.TotalGameTime.TotalSeconds > 1.5) // Give a bit more time for hover effects to settle
        {
            // First, force a draw to ensure the visuals render target is populated
            drawAction(gameTime);

            if (_pendingScreenshots.Count > 0)
            {
                foreach (var name in _pendingScreenshots)
                {
                    var path = name.EndsWith(".png") ? name : name + ".png";
                    if (!path.Contains('/') && !path.Contains('\\')) path = Path.Combine("review", path);
                    visuals.SaveScreenshot(path);
                    SaveMetadata(path.Replace(".png", ".json"), buttons);
                }
                _pendingScreenshots.Clear();
            }
            else
            {
                visuals.SaveScreenshot(defaultPath);
                SaveMetadata(defaultPath.Replace(".png", ".json"), buttons);
            }
            exitAction();
        }
    }

    private void SaveMetadata(string path, List<Button> buttons)
    {
        var metadata = new
        {
            Timestamp = DateTime.UtcNow,
            Buttons = buttons.Where(b => b.IsVisible()).Select(b => new
            {
                Text = b.Text,
                Subtitle = b.Subtitle,
                Tooltip = b.TooltipFunc?.Invoke() ?? "",
                Bounds = new { b.Bounds.X, b.Bounds.Y, b.Bounds.Width, b.Bounds.Height },
                Tab = b.Tab.ToString()
            }).ToList(),
            Resources = _engine.State.Resources.Select(r => new
            {
                Type = r.Key.ToString(),
                Amount = r.Value.Amount,
                PerSecond = r.Value.PerSecond
            }).ToList()
        };

        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText(path, JsonSerializer.Serialize(metadata, options));
        Console.WriteLine($"[AI MODE] Metadata saved to: {Path.GetFullPath(path)}");
    }
}
