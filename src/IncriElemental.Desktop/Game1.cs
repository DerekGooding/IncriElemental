using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Core.Systems;
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
    private TutorialSystem _tutorial = new();
    private MixingTableSystem _mixing = new();
    private BackgroundManager _bg = null!;
    private AiModeSystem _ai;
    private GameTab _currentTab = GameTab.Void;
    private int _lastProcessedHistoryCount = 0;
    private bool _aiMode = false;
    private Dictionary<GameTab, float> _tabScrollOffsets = new() {
        { GameTab.Void, 0 }, { GameTab.Spire, 0 }, { GameTab.World, 0 }, { GameTab.Constellation, 0 }, { GameTab.Flow, 0 }
    };
    private bool _needsButtonLayoutUpdate = false;
    private string _screenshotPath = "screenshot.png";
    private RasterizerState _scissorState = new() { ScissorTestEnable = true };
    private Button? _pinnedButton;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _engine = new GameEngine();
        _ai = new AiModeSystem(_engine);

        var jsonPath = "manifestations.json";
        if (File.Exists(jsonPath)) _engine.LoadDefinitions(File.ReadAllText(jsonPath));

        var lorePath = "lore.json";
        if (File.Exists(lorePath)) _engine.LoadLore(File.ReadAllText(lorePath));

        var stringsPath = "Content/strings.json";
        if (File.Exists(stringsPath)) TextService.Instance.LoadStrings(File.ReadAllText(stringsPath));

        if (Environment.GetCommandLineArgs().Contains("--ai-mode")) _aiMode = true;
    }

    public void ToggleFullscreen()
    {
        _graphics.IsFullScreen = !_graphics.IsFullScreen;
        if (_graphics.IsFullScreen) {
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        } else {
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
        _graphics.PreferredBackBufferWidth = 1024; _graphics.PreferredBackBufferHeight = 768; _graphics.ApplyChanges();
        UiLayout.Width = GraphicsDevice.Viewport.Width; UiLayout.Height = GraphicsDevice.Viewport.Height;
        
        _bg = new BackgroundManager(GraphicsDevice); _particles = new ParticleSystem(GraphicsDevice);
        _visuals = new VisualManager(GraphicsDevice); _pixel = new Texture2D(GraphicsDevice, 1, 1); _pixel.SetData([Color.White]);

        LayoutSystem.SetupButtons(_buttons, _engine, _particles, _audio, _log.AddToLog, (t) => _currentTab = t, _visuals, _aiMode, ToggleFullscreen);
        _audio.StartHum();
        _log.AddToLog(TextService.Instance.Get("HIST_AWAKEN")); _log.AddToLog(TextService.Instance.Get("HIST_FOCUS_PROMPT"));
        _tutorial.Start(_engine.State);

        if (_aiMode) 
        {
            var cmdPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ai_commands.txt");
            _ai.Process(cmdPath, (t) => _currentTab = t);
        }

        base.Initialize();
    }


    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _visuals.LoadEffects(Content);
        try { _font = Content.Load<SpriteFont>("main_font"); } catch (Exception ex) { Console.WriteLine($"[CRITICAL] Failed to load font: {ex.Message}"); }
    }

    protected override void Update(GameTime gameTime)
    {
        _input.Update();
        _input.HandleHotkeys(_engine, (t) => _currentTab = t);

        // UI Scaling
        var targetWidth = (int)(GraphicsDevice.Viewport.Width / _input.UiScale);
        if (Math.Abs(UiLayout.Width - targetWidth) > 1)
        {
             UiLayout.Width = targetWidth;
             UiLayout.Height = (int)(GraphicsDevice.Viewport.Height / _input.UiScale);
             _needsButtonLayoutUpdate = true;
             _visuals.Resize(GraphicsDevice);
        }

        if (_needsButtonLayoutUpdate)
        {
            LayoutSystem.SetupButtons(_buttons, _engine, _particles, _audio, _log.AddToLog, (t) => _currentTab = t, _visuals, _aiMode, ToggleFullscreen);
            _needsButtonLayoutUpdate = false;
        }

        if (_aiMode) _ai.HandleAiUpdate(gameTime, GraphicsDevice, _screenshotPath, Draw, Exit, _visuals, _buttons, _input);
        if (_input.IsKeyPressed(Keys.Escape)) Exit();

        UpdateGameLogic((float)gameTime.ElapsedGameTime.TotalSeconds);

        LayoutSystem.ApplyLayout(_buttons, _currentTab);
        UpdateScrollOffsets();
        _input.ProcessButtons(_buttons, _currentTab, _tabScrollOffsets.GetValueOrDefault(_currentTab, 0));

        if (_input.IsTooltipPinned && _pinnedButton == null) _pinnedButton = _buttons.FirstOrDefault(b => b.IsVisible() && b.IsHovered);
        else if (!_input.IsTooltipPinned) _pinnedButton = null;

        if (_currentTab == GameTab.World) _map.Update(_engine, _input.MousePosition, _input.IsLeftClick(), _audio);
        if (_currentTab == GameTab.Spire) UpdateSpireInteraction();

        _input.ClearMock();
        base.Update(gameTime);
    }

    private void UpdateGameLogic(float deltaTime)
    {
        _engine.Update(deltaTime);
        _particles.Update(deltaTime);
        _bg.Update(deltaTime, _engine.State.GetResource(ResourceType.Aether).Amount);
        _tutorial.Update(_engine.State);
        _visuals.Update(deltaTime, _engine.State.Discoveries.ContainsKey("ascended"));

        while (_lastProcessedHistoryCount < _engine.State.History.Count)
        {
            _log.AddToLog(_engine.State.History[_lastProcessedHistoryCount]);
            _lastProcessedHistoryCount++;
        }
    }

    private void UpdateSpireInteraction()
    {
        _mixing.Update(_engine, _input.MousePosition, _input.IsLeftClick(), _buttons);
        if (_input.IsLeftClick())
        {
            var curX = 350;
            var btnY = 440;
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                if (type == ResourceType.Aether || type == ResourceType.VoidEmbers || type == ResourceType.Life) continue;
                if (!_engine.State.Discoveries.ContainsKey($"{type.ToString().ToLower()}_unlocked")) continue;
                var rect = new Rectangle(curX, btnY, 60, 30);
                if (rect.Contains(_input.MousePosition)) _mixing.HandleIngredientClick(type, _engine);
                curX += 70;
            }
        }
    }

    private void UpdateScrollOffsets()
    {
        if (_tabScrollOffsets.ContainsKey(_currentTab))
        {
            _tabScrollOffsets[_currentTab] = Math.Min(0, _tabScrollOffsets[_currentTab] + _input.ScrollDelta * 0.5f);
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        _visuals.BeginRenderToTarget(GraphicsDevice);
        var shakeOffset = _visuals.GetShakeOffset();
        var curOffset = (int)_tabScrollOffsets.GetValueOrDefault(_currentTab, 0);

        if (_engine.State.Discoveries.ContainsKey("ascended"))
        {
            _visuals.Clear(GraphicsDevice, Color.White);
            _spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(shakeOffset.X, shakeOffset.Y, 0));
            _visuals.DrawAscended(_spriteBatch, _ending, _engine, _font, _pixel, gameTime, _input.MousePosition, _input.IsLeftClick(), () => {
                _engine.Manifest("reset");
                _log.Clear();
            });
            _spriteBatch.End();
        }
        else
        {
            _visuals.Clear(GraphicsDevice, new Color(5, 5, 10));
            _spriteBatch.Begin();
            _bg.Draw(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(shakeOffset.X, shakeOffset.Y, 0));
            _visuals.DrawWorldElements(_spriteBatch, _log, _font, _pixel, _particles, _buttons);
            _spriteBatch.End();

            GraphicsDevice.ScissorRectangle = new Rectangle(5, 45, UiLayout.Width - 10, UiLayout.Height - 50);
            _spriteBatch.Begin(rasterizerState: _scissorState, transformMatrix: Matrix.CreateTranslation(shakeOffset.X, shakeOffset.Y, 0));
            LayoutSystem.DrawTabButtons(_spriteBatch, _buttons, _currentTab, _font, _pixel, curOffset);
            _visuals.DrawTabContent(_spriteBatch, _currentTab, _engine, gameTime, _mixing, _input.MousePosition, _map, _font, _pixel, _debug);
            _spriteBatch.End();

            _spriteBatch.Begin(transformMatrix: Matrix.CreateTranslation(shakeOffset.X, shakeOffset.Y, 0));
            _visuals.DrawTooltipsAndStatus(_spriteBatch, _buttons, _currentTab, _font, _pixel, curOffset, _input.IsTooltipPinned, _pinnedButton, _status, _engine, (int)(UiLayout.Width * 0.8f), _input.MousePosition);
            _spriteBatch.End();
            
            if (_visuals.AscensionTransitionAlpha > 0)
            {
                _spriteBatch.Begin();
                _visuals.DrawOverlay(_spriteBatch, _visuals.AscensionTransitionAlpha);
                _spriteBatch.End();
            }

            _spriteBatch.Begin();
            _tutorial.Draw(_spriteBatch, _font, _pixel, _buttons);
            _spriteBatch.End();
        }

        _visuals.EndRenderToTarget(GraphicsDevice, _spriteBatch);
        base.Draw(gameTime);
    }
}
