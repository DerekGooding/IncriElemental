using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;
using IncriElemental.Desktop.Visuals;

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
            }
        }
    }

    public void HandleAiUpdate(GameTime gameTime, GraphicsDevice graphicsDevice, string defaultPath, Action<GameTime> drawAction, Action exitAction, VisualManager visuals)
    {
        if (gameTime.TotalGameTime.TotalSeconds > 1.0)
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
                }
                _pendingScreenshots.Clear();
            }
            else
            {
                visuals.SaveScreenshot(defaultPath);
            }
            exitAction();
        }
    }
}
