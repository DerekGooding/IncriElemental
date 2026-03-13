using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IncriElemental.Core.Engine;
using IncriElemental.Desktop.Visuals;
using System.Text.Json;
using System.IO;

namespace IncriElemental.Desktop.UI;

public class AiModeSystem(GameEngine engine)
{
    private readonly GameEngine _engine = engine;
    private readonly List<string> _commands = [];
    private int _commandIndex = 0;
    private Action<GameTab>? _setTab;
    private string? _hoverTarget;
    private bool _isPinning = false;
    private double _waitTimer = 0;

    public void Process(string commandPath, Action<GameTab> setTab)
    {
        _setTab = setTab;
        if (File.Exists(commandPath))
        {
            _commands.AddRange(File.ReadAllLines(commandPath));
        }
    }

    public void HandleAiUpdate(GameTime gameTime, GraphicsDevice graphicsDevice, string defaultPath, Action<GameTime> drawAction, Action exitAction, VisualManager visuals, List<Button> buttons, InputManager input)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_waitTimer > 0)
        {
            _waitTimer -= dt;
            return;
        }

        if (_commandIndex < _commands.Count)
        {
            var cmd = _commands[_commandIndex++];
            var parts = cmd.Split(':', 2);
            var action = parts[0].ToLower().Trim();

            if (action == "focus") _engine.Focus();
            else if (action == "manifest" && parts.Length > 1) _engine.Manifest(parts[1].Trim());
            else if (action == "update" && parts.Length > 1) { if (double.TryParse(parts[1], out var val)) _engine.Update(val); }
            else if (action == "tab" && parts.Length > 1) { if (Enum.TryParse<GameTab>(parts[1].Trim(), true, out var tab)) _setTab?.Invoke(tab); }
            else if (action == "key" && parts.Length > 1) { if (Enum.TryParse<Keys>(parts[1].Trim(), true, out var key)) input.MockKeyPress(key); }
            else if (action == "pin") _isPinning = true;
            else if (action == "unpin") _isPinning = false;
            else if (action == "hover" && parts.Length > 1) _hoverTarget = parts[1].Trim();
            else if (action == "wait" && parts.Length > 1) { if (double.TryParse(parts[1], out var val)) _waitTimer = val; }
            else if (action == "screenshot" && parts.Length > 1)
            {
                var name = parts[1].Trim();
                var path = name.EndsWith(".png") ? name : name + ".png";
                if (!path.Contains('/') && !path.Contains('\\')) path = Path.Combine("review", path);
                
                drawAction(gameTime);
                visuals.SaveScreenshot(path);
                SaveMetadata(path.Replace(".png", ".json"), buttons, gameTime);
            }

            // Apply persistent states
            if (_isPinning) input.MockKeyPress(Keys.P);
            if (!string.IsNullOrEmpty(_hoverTarget))
            {
                var btn = buttons.FirstOrDefault(b => b.Text.Contains(_hoverTarget, StringComparison.OrdinalIgnoreCase) && b.IsVisible());
                if (btn != null) input.SetMousePosition(new Point(btn.Bounds.Center.X, btn.Bounds.Center.Y));
            }
        }
        else if (gameTime.TotalGameTime.TotalSeconds > 2.0 && _commands.Count > 0)
        {
            // If we have a default screenshot to take at the very end
            if (_commandIndex == _commands.Count && !string.IsNullOrEmpty(defaultPath) && !File.Exists(defaultPath))
            {
                 drawAction(gameTime);
                 visuals.SaveScreenshot(defaultPath);
                 SaveMetadata(defaultPath.Replace(".png", ".json"), buttons, gameTime);
            }
            exitAction();
        }
        else if (_commands.Count == 0 && gameTime.TotalGameTime.TotalSeconds > 2.0)
        {
            exitAction();
        }
    }

    private void SaveMetadata(string path, List<Button> buttons, GameTime gameTime)
    {
        var metadata = new
        {
            Timestamp = DateTime.UtcNow,
            Performance = new
            {
                TotalTime = gameTime.TotalGameTime.TotalSeconds,
                ElapsedFrameTime = gameTime.ElapsedGameTime.TotalMilliseconds
            },
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
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
        File.WriteAllText(path, JsonSerializer.Serialize(metadata, options));
    }
}
