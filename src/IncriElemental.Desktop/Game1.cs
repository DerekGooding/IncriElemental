using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IncriElemental.Core.Engine;
using IncriElemental.Desktop.Visuals;
using IncriElemental.Desktop.UI;

namespace IncriElemental.Desktop;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch = null!;
    private GameEngine _engine;
    private SpriteFont _font = null!;
    private ParticleSystem _particles = null!;
    private VisualManager _visuals = null!;
    private InputManager _input = new();
    private AudioManager _audio = new();
    private List<Button> _buttons = [];
    private Texture2D _pixel = null!;
    private LogSystem _log = new();
    private WorldMapSystem _map = new();
    private StatusSystem _status = new();
    private DebugSystem _debug = new();
    private GameTab _currentTab = GameTab.Void;
    private int _lastProcessedHistoryCount = 0;
    private bool _aiMode = false;
    private string _screenshotPath = "screenshot.png";

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _engine = new GameEngine();

        // Load Data-Driven Manifestations
        var jsonPath = "manifestations.json";
        if (File.Exists(jsonPath))
        {
            _engine.LoadDefinitions(File.ReadAllText(jsonPath));
        }

        var lorePath = "lore.json";
        if (File.Exists(lorePath))
        {
            _engine.LoadLore(File.ReadAllText(lorePath));
        }

        if (Environment.GetCommandLineArgs().Contains("--ai-mode"))
        {
            _aiMode = true;
        }
    }

    public void ToggleFullscreen()
    {
        _graphics.IsFullScreen = !_graphics.IsFullScreen;
        _graphics.ApplyChanges();
        UiLayout.Width = GraphicsDevice.Viewport.Width;
        UiLayout.Height = GraphicsDevice.Viewport.Height;
        LayoutSystem.SetupButtons(_buttons, _engine, _particles, _audio, _log.AddToLog, (t) => _currentTab = t, _aiMode, ToggleFullscreen);
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;
        _graphics.ApplyChanges();

        UiLayout.Width = GraphicsDevice.Viewport.Width;
        UiLayout.Height = GraphicsDevice.Viewport.Height;

        base.Initialize();
        _particles = new ParticleSystem(GraphicsDevice);
        _visuals = new VisualManager(GraphicsDevice);
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        LayoutSystem.SetupButtons(_buttons, _engine, _particles, _audio, _log.AddToLog, (t) => _currentTab = t, _aiMode, ToggleFullscreen);
        _audio.StartHum();
        _log.AddToLog("You awaken in the void.");
        _log.AddToLog("Focus to begin manifesting reality.");

        if (_aiMode) new AiModeSystem(_engine).Process("ai_commands.txt");
    }

    private void TakeScreenshot(string path)
    {
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

        var w = GraphicsDevice.PresentationParameters.BackBufferWidth;
        var h = GraphicsDevice.PresentationParameters.BackBufferHeight;
        using var target = new RenderTarget2D(GraphicsDevice, w, h);

        GraphicsDevice.SetRenderTarget(target);
        Draw(new GameTime());
        GraphicsDevice.SetRenderTarget(null);

        using var stream = File.Open(path, FileMode.Create);
        target.SaveAsPng(stream, w, h);
        Console.WriteLine($"[AI MODE] Screenshot saved to: {Path.GetFullPath(path)}");
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        try 
        { 
            _font = Content.Load<SpriteFont>("main_font"); 
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"[CRITICAL] Failed to load font: {ex.Message}");
        }
    }

    protected override void Update(GameTime gameTime)
    {
        _input.Update();

        if (_aiMode && gameTime.TotalGameTime.TotalSeconds > 1.0)
        {
            TakeScreenshot(_screenshotPath);
            Exit();
        }

        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_input.IsKeyPressed(Keys.Escape)) Exit();

        _engine.Update(deltaTime);
        _particles.Update(deltaTime);

        while (_lastProcessedHistoryCount < _engine.State.History.Count)
        {
            _log.AddToLog(_engine.State.History[_lastProcessedHistoryCount]);
            _lastProcessedHistoryCount++;
        }

        foreach (var btn in _buttons)
        {
            if (btn.Tab == _currentTab || btn.Tab == GameTab.None) btn.Update(_input.MousePosition);
        }

        if (_input.IsLeftClick())
        {
            foreach (var btn in _buttons)
            {
                if (btn.Tab == _currentTab || btn.Tab == GameTab.None) btn.CheckClick(_input.MousePosition);
            }
        }

        if (_currentTab == GameTab.World) _map.Update(_engine, _input.MousePosition, _input.IsLeftClick(), _audio);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_engine.State.Discoveries.ContainsKey("ascended"))
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            if (_font != null)
            {
                var centerX = UiLayout.Width / 2;
                var msg = "ASCENSION COMPLETE";
                _spriteBatch.DrawString(_font, msg, new Vector2(centerX - _font.MeasureString(msg).X/2, 200), Color.Gold);

                string[] credits = ["Created by: Derek Gooding", "Developed by: Gemini CLI", "Made with MonoGame", "Thank you for playing!"];
                for(var i=0; i<credits.Length; i++)
                    _spriteBatch.DrawString(_font, credits[i], new Vector2(centerX - _font.MeasureString(credits[i]).X/2, 400 + i*40 - (float)gameTime.TotalGameTime.TotalSeconds * 30), Color.DarkGray);

                // Add Reset/New Game+ Button
                var resetRect = new Rectangle(centerX - 100, 600, 200, 50);
                _spriteBatch.Draw(_pixel, resetRect, Color.Gold * 0.4f);
                var resetText = "A NEW AWAKENING";
                _spriteBatch.DrawString(_font, resetText, new Vector2(centerX - _font.MeasureString(resetText).X/2, 625 - _font.MeasureString(resetText).Y/2), Color.DarkGoldenrod);

                if (_input.IsLeftClick() && resetRect.Contains(_input.MousePosition))
                {
                    _engine.Manifest("reset");
                    _log.Clear();
                }
            }
            _spriteBatch.End();
            return;
        }

        GraphicsDevice.Clear(new Color(5, 5, 10));
        _spriteBatch.Begin();

        _log.Draw(_spriteBatch, _font, _pixel);
        _particles.Draw(_spriteBatch);

        foreach (var btn in _buttons)
        {
            if (btn.Tab == _currentTab || btn.Tab == GameTab.None)
            {
                btn.Draw(_spriteBatch, _font, _pixel);
            }
        }

        foreach (var btn in _buttons)
        {
            if (btn.Tab == _currentTab || btn.Tab == GameTab.None)
            {
                btn.DrawTooltip(_spriteBatch, _font, _pixel);
            }
        }

        if (_currentTab == GameTab.Void || _currentTab == GameTab.Spire)
        {
            _visuals.DrawSpire(_spriteBatch, _engine.State.Discoveries, gameTime.TotalGameTime.TotalSeconds);
        }

        if (_currentTab == GameTab.World)
        {
            _map.Draw(_spriteBatch, _engine, _input.MousePosition, _font, _pixel, _visuals);
        }

        if (_currentTab == GameTab.Debug)
        {
            _debug.Draw(_spriteBatch, _engine, _font, _pixel, _visuals);
        }

        _status.Draw(_spriteBatch, _engine, _font, _pixel, _visuals, (int)(UiLayout.Width * 0.8f));

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
