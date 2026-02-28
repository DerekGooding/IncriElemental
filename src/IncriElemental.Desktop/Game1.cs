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
    private EndingSystem _ending = new();
    private AiModeSystem _ai;
    private GameTab _currentTab = GameTab.Void;
    private int _lastProcessedHistoryCount = 0;
    private bool _aiMode = false;
    private Dictionary<GameTab, float> _tabScrollOffsets = new() {
        { GameTab.Void, 0 }, { GameTab.Spire, 0 }, { GameTab.World, 0 }, { GameTab.Constellation, 0 }
    };
    private bool _needsButtonLayoutUpdate = false;
    private string _screenshotPath = "screenshot.png";
    private RasterizerState _scissorState = new() { ScissorTestEnable = true };

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _engine = new GameEngine();
        _ai = new AiModeSystem(_engine);

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
        if (_graphics.IsFullScreen)
        {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        }
        else
        {
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
        }
        _graphics.ApplyChanges();
        
        UiLayout.Width = GraphicsDevice.Viewport.Width;
        UiLayout.Height = GraphicsDevice.Viewport.Height;
        _needsButtonLayoutUpdate = true;
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

        if (_aiMode) _ai.Process("ai_commands.txt");
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

        if (_needsButtonLayoutUpdate)
        {
            LayoutSystem.SetupButtons(_buttons, _engine, _particles, _audio, _log.AddToLog, (t) => _currentTab = t, _aiMode, ToggleFullscreen);
            _needsButtonLayoutUpdate = false;
        }

        if (_aiMode) _ai.HandleAiUpdate(gameTime, GraphicsDevice, _screenshotPath, Draw, Exit);

        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_input.IsKeyPressed(Keys.Escape)) Exit();

        _engine.Update(deltaTime);
        _particles.Update(deltaTime);

        while (_lastProcessedHistoryCount < _engine.State.History.Count)
        {
            _log.AddToLog(_engine.State.History[_lastProcessedHistoryCount]);
            _lastProcessedHistoryCount++;
        }

        LayoutSystem.ApplyLayout(_buttons, _currentTab);

        if (_tabScrollOffsets.ContainsKey(_currentTab))
        {
            _tabScrollOffsets[_currentTab] = Math.Min(0, _tabScrollOffsets[_currentTab] + _input.ScrollDelta * 0.5f);
        }

        foreach (var btn in _buttons)
        {
            if (!btn.IsVisible()) continue;
            var offset = btn.Tab == GameTab.None ? 0 : (int)_tabScrollOffsets.GetValueOrDefault(btn.Tab, 0);
            if (btn.Tab == _currentTab || btn.Tab == GameTab.None) btn.Update(_input.MousePosition, offset);
        }

        if (_input.IsLeftClick())
        {
            foreach (var btn in _buttons)
            {
                if (!btn.IsVisible()) continue;
                var offset = btn.Tab == GameTab.None ? 0 : (int)_tabScrollOffsets.GetValueOrDefault(btn.Tab, 0);
                if (btn.Tab == _currentTab || btn.Tab == GameTab.None) btn.CheckClick(_input.MousePosition, offset);
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
            _ending.Draw(_spriteBatch, _engine, _font, _pixel, gameTime, _input.MousePosition, _input.IsLeftClick(), () => {
                _engine.Manifest("reset");
                _log.Clear();
            });
            _spriteBatch.End();
            return;
        }

        GraphicsDevice.Clear(new Color(5, 5, 10));
        _spriteBatch.Begin();

        _log.Draw(_spriteBatch, _font, _pixel);
        _particles.Draw(_spriteBatch);

        // Draw Fixed UI (None Tab)
        foreach (var btn in _buttons.Where(b => b.Tab == GameTab.None))
        {
            if (btn.IsVisible()) btn.Draw(_spriteBatch, _font, _pixel, 0);
        }

        _spriteBatch.End();

        // Draw Scrollable UI
        var scrollRect = new Rectangle(5, 45, UiLayout.Width - 10, UiLayout.Height - 50);
        GraphicsDevice.ScissorRectangle = scrollRect;
        _spriteBatch.Begin(rasterizerState: _scissorState);

        var curOffset = (int)_tabScrollOffsets.GetValueOrDefault(_currentTab, 0);
        foreach (var btn in _buttons.Where(b => b.Tab == _currentTab))
        {
            if (btn.IsVisible()) btn.Draw(_spriteBatch, _font, _pixel, curOffset);
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

        _spriteBatch.End();

        // Draw Tooltips and Status (Always on top, not clipped)
        _spriteBatch.Begin();
        foreach (var btn in _buttons)
        {
            if (!btn.IsVisible()) continue;
            var offset = btn.Tab == GameTab.None ? 0 : curOffset;
            if (btn.Tab == _currentTab || btn.Tab == GameTab.None)
            {
                btn.DrawTooltip(_spriteBatch, _font, _pixel, offset);
            }
        }

        _status.Draw(_spriteBatch, _engine, _font, _pixel, _visuals, (int)(UiLayout.Width * 0.8f), _input.MousePosition);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
