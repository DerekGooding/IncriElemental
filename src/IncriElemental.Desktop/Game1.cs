using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
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
    private SpriteBatch _spriteBatch;
    private GameEngine _engine;
    private SpriteFont _font;
    private ParticleSystem _particles;
    private VisualManager _visuals;
    private List<Button> _buttons = new();
    private MouseState _lastMouseState;
    private Texture2D _pixel;
    private List<string> _log = new();
    private const int MaxLogLines = 10;
    private bool _aiMode = false;
    private string _screenshotPath = "review/screenshot.png";

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _engine = new GameEngine();

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
        _pixel.SetData(new[] { Color.White });
        
        SetupButtons();
        AddToLog("You awaken in the void.");

        if (_aiMode) ProcessAiMode();
    }

    private void ProcessAiMode()
    {
        string commandPath = "ai_commands.txt";
        if (File.Exists(commandPath))
        {
            var commands = File.ReadAllLines(commandPath);
            foreach (var cmd in commands)
            {
                var parts = cmd.Split(':');
                if (parts[0].ToLower() == "focus") _engine.Focus();
                if (parts[0].ToLower() == "manifest") _engine.Manifest(parts[1]);
                if (parts[0].ToLower() == "update") {
                    if (double.TryParse(parts[1], out double dt)) _engine.Update(dt);
                }
            }
        }
    }

    private void TakeScreenshot(string path)
    {
        string dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
        
        int w = GraphicsDevice.PresentationParameters.BackBufferWidth;
        int h = GraphicsDevice.PresentationParameters.BackBufferHeight;
        using RenderTarget2D target = new RenderTarget2D(GraphicsDevice, w, h);
        
        GraphicsDevice.SetRenderTarget(target);
        Draw(new GameTime());
        GraphicsDevice.SetRenderTarget(null);
        
        using FileStream stream = File.OpenWrite(path);
        target.SaveAsPng(stream, w, h);
    }

    private void SetupButtons()
    {
        _buttons.Add(new Button(new Rectangle(412, 334, 200, 100), "FOCUS", Color.MediumPurple * 0.3f, () => {
            _engine.Focus();
            _particles.EmitFocus(new Vector2(512, 384));
        }));

        _buttons.Add(new Button(new Rectangle(412, 450, 200, 50), "MANIFEST SPECK (10A)", Color.SaddleBrown * 0.4f, () => {
            if (_engine.Manifest("speck")) AddToLog("A speck of matter appears.");
        }, () => _engine.State.GetResource(ResourceType.Aether).Amount >= 10 || _engine.State.Discoveries.ContainsKey("first_manifestation")));

        _buttons.Add(new Button(new Rectangle(412, 510, 200, 50), "RUNE OF ATTRACTION (30A)", Color.MediumPurple * 0.4f, () => {
            if (_engine.Manifest("rune_of_attraction")) AddToLog("The aether begins to flow of its own accord.");
        }, () => _engine.State.GetResource(ResourceType.Aether).Amount >= 30 || _engine.State.Discoveries.ContainsKey("automation_unlocked")));
        
        _buttons.Add(new Button(new Rectangle(412, 630, 200, 50), "MANIFEST ALTAR (100A, 20E)", Color.Gray * 0.4f, () => {
            if (_engine.Manifest("altar")) AddToLog("A monolithic altar rises. Your capacity expands.");
        }, () => _engine.State.Discoveries.ContainsKey("first_manifestation")));

        // Goal 8: Forge
        _buttons.Add(new Button(new Rectangle(630, 334, 200, 50), "FORGE (50F, 100E)", Color.OrangeRed * 0.4f, () => {
            if (_engine.Manifest("forge")) AddToLog("A magical forge ignites.");
        }, () => _engine.State.Discoveries.ContainsKey("altar_constructed")));

        // Goal 9 & 10
        _buttons.Add(new Button(new Rectangle(630, 400, 150, 40), "WELL (300E, 100W)", Color.DodgerBlue * 0.4f, () => {
            if (_engine.Manifest("well")) AddToLog("A deep Well manifests.");
        }, () => _engine.State.Discoveries.ContainsKey("forge_constructed")));

        _buttons.Add(new Button(new Rectangle(630, 450, 150, 40), "BRAZIER (300E, 100F)", Color.OrangeRed * 0.4f, () => {
            if (_engine.Manifest("brazier")) AddToLog("A Brazier ignites.");
        }, () => _engine.State.Discoveries.ContainsKey("forge_constructed")));

        _buttons.Add(new Button(new Rectangle(845, 334, 150, 40), "GARDEN (500E, 500W)", Color.LimeGreen * 0.4f, () => {
            if (_engine.Manifest("garden")) AddToLog("A magical Garden blooms.");
        }, () => _engine.State.Discoveries.ContainsKey("forge_constructed")));

        _buttons.Add(new Button(new Rectangle(845, 400, 150, 40), "FAMILIAR (1000A, 100L)", Color.MediumPurple * 0.6f, () => {
            if (_engine.Manifest("familiar")) AddToLog("A Familiar manifests.");
        }, () => _engine.State.Discoveries.ContainsKey("garden_manifested")));

        // Goal 11: Spire
        _buttons.Add(new Button(new Rectangle(845, 500, 150, 40), "FOUNDATION", Color.SaddleBrown * 0.5f, () => {
            if (_engine.Manifest("spire_foundation")) AddToLog("The Spire Foundation is laid.");
        }, () => _engine.State.Discoveries.ContainsKey("familiar_manifested")));

        _buttons.Add(new Button(new Rectangle(845, 550, 150, 40), "SHAFT", Color.LightCyan * 0.5f, () => {
            if (_engine.Manifest("spire_shaft")) AddToLog("The Spire Shaft rises.");
        }, () => _engine.State.Discoveries.ContainsKey("spire_foundation_ready")));

        _buttons.Add(new Button(new Rectangle(845, 600, 150, 40), "CORE", Color.MediumPurple * 0.5f, () => {
            if (_engine.Manifest("spire_core")) AddToLog("The Spire Core is ignited.");
        }, () => _engine.State.Discoveries.ContainsKey("spire_shaft_ready")));

        _buttons.Add(new Button(new Rectangle(412, 100, 200, 80), "ASCEND", Color.Gold * 0.8f, () => {
            if (_engine.Manifest("ascend")) AddToLog("Ascension begins...");
        }, () => _engine.State.Discoveries.ContainsKey("spire_complete")));
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
        if (_aiMode && gameTime.TotalGameTime.TotalSeconds > 1.0)
        {
            TakeScreenshot(_screenshotPath);
            Exit();
        }

        var mouseState = Mouse.GetState();
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

        _engine.Update(deltaTime);
        _particles.Update(deltaTime);

        foreach (var message in _engine.State.History)
            if (!_log.Contains(message)) AddToLog(message);

        if (mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released)
        {
            foreach (var btn in _buttons) btn.CheckClick(mouseState.Position);
        }

        _lastMouseState = mouseState;
        base.Update(gameTime);
    }

    private string FormatValue(double value)
    {
        if (value >= 1_000_000_000) return $"{(value / 1_000_000_000):F2}G";
        if (value >= 1_000_000) return $"{(value / 1_000_000):F2}M";
        if (value >= 1_000) return $"{(value / 1_000):F2}K";
        return value.ToString("F1");
    }

    protected override void Draw(GameTime gameTime)
    {
        if (_engine.State.Discoveries.ContainsKey("ascended"))
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            if (_font != null)
            {
                string msg = "ASCENSION COMPLETE";
                _spriteBatch.DrawString(_font, msg, new Vector2(512 - _font.MeasureString(msg).X/2, 200), Color.Gold);
                
                string[] credits = { "Created by: Derek Gooding", "Developed by: Gemini CLI", "Made with MonoGame", "Thank you for playing!" };
                for(int i=0; i<credits.Length; i++)
                    _spriteBatch.DrawString(_font, credits[i], new Vector2(512 - _font.MeasureString(credits[i]).X/2, 400 + i*40 - (float)gameTime.TotalGameTime.TotalSeconds * 30), Color.DarkGray);
                
                // Add Reset/New Game+ Button
                var resetRect = new Rectangle(412, 600, 200, 50);
                _spriteBatch.Draw(_pixel, resetRect, Color.Gold * 0.4f);
                string resetText = "A NEW AWAKENING";
                _spriteBatch.DrawString(_font, resetText, new Vector2(512 - _font.MeasureString(resetText).X/2, 625 - _font.MeasureString(resetText).Y/2), Color.DarkGoldenrod);
                
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released && resetRect.Contains(Mouse.GetState().Position))
                {
                    _engine.Manifest("reset");
                    _log.Clear(); // Clear log for new game
                }
            }
            _spriteBatch.End();
            _lastMouseState = Mouse.GetState();
            return;
        }

        GraphicsDevice.Clear(new Color(5, 5, 10));
        _spriteBatch.Begin();
        _particles.Draw(_spriteBatch);

        // Draw buttons (will show backgrounds if font is missing)
        foreach (var btn in _buttons) btn.Draw(_spriteBatch, _font, _pixel);

        if (_font != null)
        {
            // Resources - Layout them more cleanly
            float startY = 310;
            int count = 0;
            foreach (var res in _engine.State.Resources.Values.Where(r => r.Amount > 0 || r.MaxAmount < 1_000_000_000_000_000))
            {
                float y = startY - (count * 30);
                string amountStr = FormatValue(res.Amount);
                string maxStr = res.MaxAmount > 1_000_000_000_000 ? "INF" : FormatValue(res.MaxAmount);
                string label = $"{res.Type}: {amountStr} / {maxStr}";
                
                _visuals.DrawElement(_spriteBatch, res.Type, new Vector2(390, y + 8), 8f);
                _spriteBatch.DrawString(_font, label, new Vector2(412, y), _visuals.GetColor(res.Type));
                count++;
            }

            for (int i = 0; i < _log.Count; i++)
                _spriteBatch.DrawString(_font, _log[i], new Vector2(20, 20 + (i * 20)), Color.Gray * (1.0f - i * 0.1f));
        }

        // Physical Spire (Goal 11)
        if (_engine.State.Discoveries.ContainsKey("spire_foundation_ready"))
            _spriteBatch.Draw(_pixel, new Rectangle(502, 384, 20, 100), Color.Gray);

        _spriteBatch.End();
        base.Draw(gameTime);
    }
}
