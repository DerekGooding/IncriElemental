using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Core.Engine;

namespace IncriElemental.Desktop.UI;

public class AiModeSystem(GameEngine engine)
{
    private readonly GameEngine _engine = engine;

    public void Process(string commandPath)
    {
        if (File.Exists(commandPath))
        {
            var commands = File.ReadAllLines(commandPath);
            foreach (var cmd in commands)
            {
                var parts = cmd.Split(':');
                if (parts[0].Equals("focus", StringComparison.CurrentCultureIgnoreCase)) _engine.Focus();
                if (parts[0].Equals("manifest", StringComparison.CurrentCultureIgnoreCase)) _engine.Manifest(parts[1]);
                if (parts[0].Equals("update", StringComparison.CurrentCultureIgnoreCase)) {
                    if (double.TryParse(parts[1], out var dt)) _engine.Update(dt);
                }
            }
        }
    }

    public void HandleAiUpdate(GameTime gameTime, GraphicsDevice graphicsDevice, string screenshotPath, Action<GameTime> drawAction, Action exitAction)
    {
        if (gameTime.TotalGameTime.TotalSeconds > 1.0)
        {
            TakeScreenshot(graphicsDevice, screenshotPath, drawAction);
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
