using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using IncriElemental.Core.Engine;
using IncriElemental.Core.Models;
using IncriElemental.Desktop.Visuals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IncriElemental.Desktop;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private GameEngine _engine;
    private ParticleSystem _particles;
    private MouseState _lastMouseState;
    private Texture2D _pixel;
    private List<string> _log = new();
    private const int MaxLogLines = 5;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _engine = new GameEngine();
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 768;
        _graphics.ApplyChanges();

        base.Initialize();
        _particles = new ParticleSystem(GraphicsDevice);
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
        
        AddToLog("You awaken in the void.");
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    private void AddToLog(string message)
    {
        _log.Insert(0, message);
        if (_log.Count > MaxLogLines) _log.RemoveAt(MaxLogLines);
    }

    protected override void Update(GameTime gameTime)
    {
        var mouseState = Mouse.GetState();
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _engine.Update(deltaTime);
        _particles.Update(deltaTime);

        // Interaction Logic
        if (mouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released)
        {
            var center = new Vector2(512, 384);
            var focusRect = new Rectangle(412, 334, 200, 100);
            
            if (focusRect.Contains(mouseState.Position))
            {
                _engine.Focus();
                _particles.EmitFocus(center);
                if (_engine.State.GetResource(ResourceType.Aether).Amount == 1) AddToLog("Aether flows...");
                if (_engine.State.Discoveries["void_observed"] && _log[0] != "The void is thick with potential.") AddToLog("The void is thick with potential.");
            }

            if (_engine.State.GetResource(ResourceType.Aether).Amount >= 10)
            {
                var manifestRect = new Rectangle(412, 450, 200, 50);
                if (manifestRect.Contains(mouseState.Position))
                {
                    if (_engine.Manifest("speck"))
                    {
                        AddToLog("A speck of matter appears.");
                        // Burst effect
                        for (int i = 0; i < 20; i++)
                            _particles.AddParticle(new Vector2(512, 475), new Vector2((float)(Random.Shared.NextDouble() - 0.5) * 200, (float)(Random.Shared.NextDouble() - 0.5) * 200), Color.SaddleBrown, 1f);
                    }
                }
            }
        }

        _lastMouseState = mouseState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(5, 5, 10));

        _spriteBatch.Begin();

        _particles.Draw(_spriteBatch);

        // Focus Button
        var pulse = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 3) * 0.1f + 0.9f;
        _spriteBatch.Draw(_pixel, new Rectangle(412, 334, 200, 100), Color.MediumPurple * 0.3f * pulse);
        
        // Resource Indicators (Visual bars since we have no font yet)
        float aetherWidth = (float)(_engine.State.GetResource(ResourceType.Aether).Amount / 50.0) * 200f;
        _spriteBatch.Draw(_pixel, new Rectangle(412, 310, (int)Math.Min(aetherWidth, 200), 5), Color.MediumPurple);

        // Manifestation Button
        if (_engine.State.GetResource(ResourceType.Aether).Amount >= 10)
        {
            _spriteBatch.Draw(_pixel, new Rectangle(412, 450, 200, 50), Color.SaddleBrown * 0.4f);
        }

        // Earth Indicator
        if (_engine.State.Discoveries["first_manifestation"])
        {
            float earthWidth = (float)(_engine.State.GetResource(ResourceType.Earth).Amount / 100.0) * 200f;
            _spriteBatch.Draw(_pixel, new Rectangle(412, 510, (int)Math.Min(earthWidth, 200), 5), Color.SaddleBrown);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
