using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IncriElemental.Core.Engine;
using IncriElemental.Desktop.Visuals;
using IncriElemental.Desktop.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
    private List<Button> _buttons = new();
    private Texture2D _pixel = null!;
    private List<string> _log = new();
    private const int MaxLogLines = 10;
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

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;
        _graphics.ApplyChanges();

        base.Initialize();
        _particles = new ParticleSystem(GraphicsDevice);
        _visuals = new VisualManager(GraphicsDevice);
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        LayoutSystem.SetupButtons(_buttons, _engine, _particles, _audio, AddToLog);
        AddToLog("You awaken in the void.");

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
        try { _font = Content.Load<SpriteFont>("main_font"); } catch { }
    }

    private void AddToLog(string message)
    {
        if (_log.Count > 0 && _log[0] == message) return;
        _log.Insert(0, message);
        if (_log.Count > MaxLogLines) _log.RemoveAt(MaxLogLines);
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

        foreach (var message in _engine.State.History)
            if (!_log.Contains(message)) AddToLog(message);

        if (_input.IsLeftClick())
        {
            foreach (var btn in _buttons) btn.CheckClick(_input.MousePosition);

            // Map interaction
            if (_engine.State.Manifestations.GetValueOrDefault("familiar") > 0)
            {
                var size = 20;
                var padding = 2;
                var startX = 350;
                var startY = 400;

                for (var x = 0; x < _engine.State.Map.Width; x++)
                {
                    for (var y = 0; y < _engine.State.Map.Height; y++)
                    {
                        var rect = new Rectangle(startX + x * (size + padding), startY + y * (size + padding), size, size);
                        if (rect.Contains(_input.MousePosition))
                        {
                            if (_engine.Explore(x, y)) _audio.PlayExplore();
                        }
                    }
                }
            }
        }

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
                var msg = "ASCENSION COMPLETE";
                _spriteBatch.DrawString(_font, msg, new Vector2(512 - _font.MeasureString(msg).X/2, 200), Color.Gold);

                string[] credits = ["Created by: Derek Gooding", "Developed by: Gemini CLI", "Made with MonoGame", "Thank you for playing!"];
                for(var i=0; i<credits.Length; i++)
                    _spriteBatch.DrawString(_font, credits[i], new Vector2(512 - _font.MeasureString(credits[i]).X/2, 400 + i*40 - (float)gameTime.TotalGameTime.TotalSeconds * 30), Color.DarkGray);

                // Add Reset/New Game+ Button
                var resetRect = new Rectangle(412, 600, 200, 50);
                _spriteBatch.Draw(_pixel, resetRect, Color.Gold * 0.4f);
                var resetText = "A NEW AWAKENING";
                _spriteBatch.DrawString(_font, resetText, new Vector2(512 - _font.MeasureString(resetText).X/2, 625 - _font.MeasureString(resetText).Y/2), Color.DarkGoldenrod);

                if (_input.IsLeftClick() && resetRect.Contains(_input.MousePosition))
                {
                    _engine.Manifest("reset");
                    _log.Clear(); // Clear log for new game
                }
            }
            _spriteBatch.End();
            return;
        }

        GraphicsDevice.Clear(new Color(5, 5, 10));
        _spriteBatch.Begin();

        // --- SECTION 1: LOG AREA (Left) ---
        var logWidth = 300;
        _spriteBatch.Draw(_pixel, new Rectangle(0, 0, logWidth, 768), Color.Black * 0.3f);
        _spriteBatch.Draw(_pixel, new Rectangle(logWidth, 0, 1, 768), Color.Gray * 0.2f); // Border

        if (_font != null)
        {
            _spriteBatch.DrawString(_font, "LOG", new Vector2(20, 20), Color.Gray * 0.5f);
            for (var i = 0; i < _log.Count; i++)
            {
                var alpha = 1.0f - (i * 0.1f);
                var textColor = Color.LightGray;
                if (_log[i].Contains("void is not") || _log[i].Contains("consciousness") || _log[i].Contains("We are not the first"))
                    textColor = Color.Cyan;

                _spriteBatch.DrawString(_font, _log[i], new Vector2(20, 60 + (i * 25)), textColor * alpha, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
            }
        }

        // --- SECTION 2: MAIN AREA (Center) ---
        _particles.Draw(_spriteBatch);
        foreach (var btn in _buttons) btn.Draw(_spriteBatch, _font, _pixel);

        _visuals.DrawSpire(_spriteBatch, _engine.State.Discoveries, gameTime.TotalGameTime.TotalSeconds);

        // --- SECTION 2.5: WORLD MAP ---
        if (_engine.State.Manifestations.GetValueOrDefault("familiar") > 0)
        {
            _spriteBatch.DrawString(_font, "WORLD EXPLORATION (Click cells to send Familiars)", new Vector2(350, 370), Color.Gray * 0.5f);
            _visuals.DrawMap(_spriteBatch, _engine.State.Map, _input.MousePosition, _pixel);
        }

        // --- SECTION 3: RESOURCE AREA (Right) ---
        var resX = 800;
        if (_font != null)
        {
            float y = 20;
            _spriteBatch.DrawString(_font, "ESSENCE", new Vector2(resX, y), Color.Gray);
            y += 30;

            foreach (var res in _engine.State.Resources.Values.Where(r => r.Amount > 0 || r.MaxAmount < 1_000_000_000_000))
            {
                var amountStr = _visuals.FormatValue(res.Amount);
                var maxStr = res.MaxAmount > 1_000_000_000_000 ? "INF" : _visuals.FormatValue(res.MaxAmount);
                var label = $"{res.Type}: {amountStr}";

                _visuals.DrawElement(_spriteBatch, res.Type, new Vector2(resX - 15, y + 8), 6f);
                _spriteBatch.DrawString(_font, label, new Vector2(resX, y), _visuals.GetColor(res.Type));

                // Subtle storage bar
                if (res.MaxAmount < 1_000_000_000_000)
                {
                    var percent = (float)(res.Amount / res.MaxAmount);
                    _spriteBatch.Draw(_pixel, new Rectangle(resX, (int)y + 22, 150, 2), Color.Gray * 0.2f);
                    _spriteBatch.Draw(_pixel, new Rectangle(resX, (int)y + 22, (int)(150 * percent), 2), _visuals.GetColor(res.Type) * 0.5f);
                }

                y += 40;
            }

            // Active Buffs
            y += 20;
            _spriteBatch.DrawString(_font, "ACTIVE REACTION", new Vector2(resX, y), Color.Gray * 0.5f);
            y += 30;
            foreach (var buff in _engine.State.ActiveBuffs)
            {
                _spriteBatch.DrawString(_font, $"{buff.Id}: {buff.RemainingTime:F0}s", new Vector2(resX, y), Color.Gold * 0.8f);
                y += 25;
            }
        }

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
