using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;

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

    public void HandleAiUpdate(GameTime gameTime, GraphicsDevice graphicsDevice, string defaultPath, Action<GameTime> drawAction, Action exitAction)
    {
        // Give it a bit of time to settle if needed, but we can process immediately if requested
        if (gameTime.TotalGameTime.TotalSeconds > 1.0)
        {
            if (_pendingScreenshots.Count > 0)
            {
                foreach (var name in _pendingScreenshots)
                {
                    var path = name.EndsWith(".png") ? name : name + ".png";
                    // If path is just a name, put it in review/
                    if (!path.Contains('/') && !path.Contains('\\')) path = Path.Combine("review", path);
                    TakeScreenshot(graphicsDevice, path, drawAction);
                }
                _pendingScreenshots.Clear();
            }
            else
            {
                TakeScreenshot(graphicsDevice, defaultPath, drawAction);
            }
            exitAction();
        }
    }

    private void TakeScreenshot(GraphicsDevice graphicsDevice, string path, Action<GameTime> drawAction)
    {
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

        var w = graphicsDevice.PresentationParameters.BackBufferWidth;
        var h = graphicsDevice.PresentationParameters.BackBufferHeight;
        using var target = new RenderTarget2D(graphicsDevice, w, h);

        graphicsDevice.SetRenderTarget(target);
        drawAction(new GameTime());
        graphicsDevice.SetRenderTarget(null);

        using var stream = File.Open(path, FileMode.Create);
        target.SaveAsPng(stream, w, h);
        Console.WriteLine($"[AI MODE] Screenshot saved to: {Path.GetFullPath(path)}");
    }
}
