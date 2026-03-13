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
    private bool _isClicking = false;
    private double _waitTimer = 0;
    private readonly List<Keys> _pendingKeys = [];

    public void Process(string commandPath, Action<GameTab> setTab)
    {
        _setTab = setTab;
        if (File.Exists(commandPath)) _commands.AddRange(File.ReadAllLines(commandPath));
    }

    public void HandleAiUpdate(GameTime gt, GraphicsDevice gd, string defPath, Action<GameTime> draw, Action exit, VisualManager vis, List<Button> btns, InputManager input)
    {
        float dt = (float)gt.ElapsedGameTime.TotalSeconds;
        if (_waitTimer > 0) { _waitTimer -= dt; return; }

        if (_commandIndex < _commands.Count)
        {
            var cmd = _commands[_commandIndex++]; var parts = cmd.Split(':', 2); var act = parts[0].ToLower().Trim();
            if (act == "focus") _engine.Focus();
            else if (act == "manifest" && parts.Length > 1) _engine.Manifest(parts[1].Trim());
            else if (act == "update" && parts.Length > 1) { if (double.TryParse(parts[1], out var v)) _engine.Update(v); }
            else if (act == "tab" && parts.Length > 1) { if (Enum.TryParse<GameTab>(parts[1].Trim(), true, out var t)) _setTab?.Invoke(t); }
            else if (act == "key" && parts.Length > 1) { if (Enum.TryParse<Keys>(parts[1].Trim(), true, out var k)) _pendingKeys.Add(k); }
            else if (act == "pin") _isPinning = true;
            else if (act == "unpin") _isPinning = false;
            else if (act == "hover" && parts.Length > 1) _hoverTarget = parts[1].Trim();
            else if (act == "click") _isClicking = true;
            else if (act == "wait" && parts.Length > 1) { if (double.TryParse(parts[1], out var v)) _waitTimer = v; }
            else if (act == "screenshot" && parts.Length > 1) { var n = parts[1].Trim(); var p = n.EndsWith(".png") ? n : n + ".png"; if (!p.Contains('/') && !p.Contains('\\')) p = Path.Combine("review", p); draw(gt); vis.SaveScreenshot(p); SaveMetadata(p.Replace(".png", ".json"), btns, gt); }

            foreach (var k in _pendingKeys) input.MockKeyPress(k); _pendingKeys.Clear();
            if (_isPinning) input.MockKeyPress(Keys.P);
            if (_isClicking) { input.MockClick(); _isClicking = false; }
            if (!string.IsNullOrEmpty(_hoverTarget)) { var btn = btns.FirstOrDefault(b => (b.Text.Contains(_hoverTarget, StringComparison.OrdinalIgnoreCase) || (_hoverTarget == "vessel" && b.Text == "")) && b.IsVisible()); if (btn != null) input.SetMousePosition(new Point(btn.Bounds.Center.X, btn.Bounds.Center.Y)); }
        }
        else if (gt.TotalGameTime.TotalSeconds > 2.0) { if (_commandIndex == _commands.Count && !string.IsNullOrEmpty(defPath) && !File.Exists(defPath)) { draw(gt); vis.SaveScreenshot(defPath); SaveMetadata(defPath.Replace(".png", ".json"), btns, gt); } exit(); }
    }

    private void SaveMetadata(string path, List<Button> buttons, GameTime gameTime)
    {
        var metadata = new { Timestamp = DateTime.UtcNow, Performance = new { TotalTime = gameTime.TotalGameTime.TotalSeconds, ElapsedFrameTime = gameTime.ElapsedGameTime.TotalMilliseconds }, Buttons = buttons.Where(b => b.IsVisible()).Select(b => new { Text = b.Text, Subtitle = b.Subtitle, Tooltip = b.TooltipFunc?.Invoke() ?? "", Bounds = new { b.Bounds.X, b.Bounds.Y, b.Bounds.Width, b.Bounds.Height }, Tab = b.Tab.ToString() }).ToList(), Resources = _engine.State.Resources.Select(r => new { Type = r.Key.ToString(), Amount = r.Value.Amount, PerSecond = r.Value.PerSecond }).ToList(), ActiveBuffs = _engine.State.ActiveBuffs.Select(b => new { b.Id, b.RemainingTime }).ToList() };
        var options = new JsonSerializerOptions { WriteIndented = true }; var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
        File.WriteAllText(path, JsonSerializer.Serialize(metadata, options));
    }
}
