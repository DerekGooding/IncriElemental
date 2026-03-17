using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using IncriElemental.Desktop.UI;

namespace IncriElemental.Desktop.Visuals;

public class BackgroundManager
{
    private struct Star
    {
        public Vector2 Position;
        public float Speed;
        public float Scale;
        public Color Color;
        public float ParallaxFactor;
    }

    private readonly List<Star> _stars = [];
    private readonly Texture2D _pixel;
    private readonly Random _rnd = new();
    private double _totalTime = 0;
    private float _starPulse = 1.0f;
    
    // Fluid Simulation Fields
    private const int GridWidth = 64;
    private const int GridHeight = 48;
    private float[,] _currentGrid = new float[GridWidth, GridHeight];
    private float[,] _prevGrid = new float[GridWidth, GridHeight];
    private readonly Texture2D _fluidTexture;
    private readonly Color[] _fluidData = new Color[GridWidth * GridHeight];
    private Effect? _fluidEffect;
    private readonly GraphicsDevice _gd;

    public BackgroundManager(GraphicsDevice gd)
    {
        _gd = gd;
        _pixel = new Texture2D(gd, 1, 1);
        _pixel.SetData([Color.White]);
        
        _fluidTexture = new Texture2D(gd, GridWidth, GridHeight);

        for (int i = 0; i < 250; i++)
        {
            float prx = (float)(_rnd.NextDouble() * 0.8 + 0.2);
            _stars.Add(new Star {
                Position = new Vector2(_rnd.Next(UiLayout.Width), _rnd.Next(UiLayout.Height)),
                Speed = (float)(_rnd.NextDouble() * 20 + 5),
                Scale = (float)(_rnd.NextDouble() * 1.5 + 0.5) * prx,
                Color = Color.Lerp(Color.MediumPurple, Color.White, (float)_rnd.NextDouble()) * (0.5f + 0.5f * prx),
                ParallaxFactor = prx
            });
        }
    }

    public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content)
    {
        try { _fluidEffect = content.Load<Effect>("Fluid"); } catch { }
    }

    public void AddRipple(int x, int y, float intensity = 1.0f)
    {
        int gx = (int)(x * (float)GridWidth / UiLayout.Width);
        int gy = (int)(y * (float)GridHeight / UiLayout.Height);
        if (gx >= 0 && gx < GridWidth && gy >= 0 && gy < GridHeight)
        {
            _currentGrid[gx, gy] += intensity;
        }
    }

    public void Update(double deltaTime, double aetherAmount, Point? mouseClick = null)
    {
        _totalTime += deltaTime;
        _starPulse = (float)Math.Sin(_totalTime * 1.5) * 0.15f + 0.85f;
        var speedMult = 1.0 + Math.Log10(Math.Max(1, aetherAmount)) * 0.1;
        float dt = (float)deltaTime;

        if (mouseClick.HasValue) 
            AddRipple(mouseClick.Value.X, mouseClick.Value.Y, 2.0f);

        // Fluid Sim: Wave Equation Step
        float damping = 0.98f;
        for (int x = 1; x < GridWidth - 1; x++)
        {
            for (int y = 1; y < GridHeight - 1; y++)
            {
                float newVal = (_currentGrid[x - 1, y] + _currentGrid[x + 1, y] + _currentGrid[x, y - 1] + _currentGrid[x, y + 1]) / 2f - _prevGrid[x, y];
                _prevGrid[x, y] = newVal * damping;
            }
        }
        
        // Swap Grids
        var temp = _currentGrid;
        _currentGrid = _prevGrid;
        _prevGrid = temp;

        // Update Texture
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                float val = MathHelper.Clamp(_currentGrid[x, y], -1f, 1f);
                // Map [-1, 1] to [0, 255] for R channel, use G for absolute intensity
                byte r = (byte)((val + 1f) * 127.5f);
                byte g = (byte)(Math.Abs(val) * 255f);
                _fluidData[y * GridWidth + x] = new Color((int)r, (int)g, 0, 255);
            }
        }
        _fluidTexture.SetData(_fluidData);

        for (int i = 0; i < _stars.Count; i++)
        {
            var s = _stars[i];
            s.Position.Y += s.Speed * (float)speedMult * dt * s.ParallaxFactor;
            if (s.Position.Y > UiLayout.Height + 10) { s.Position.Y = -10; s.Position.X = _rnd.Next(UiLayout.Width); }
            _stars[i] = s;
        }
    }

    public void Draw(SpriteBatch sb, Color elementColor)
    {
        if (_fluidEffect != null)
        {
            _fluidEffect.Parameters["Time"]?.SetValue((float)_totalTime);
            _fluidEffect.Parameters["FluidGrid"]?.SetValue(_fluidTexture);
            _fluidEffect.Parameters["AetherColor"]?.SetValue(elementColor.ToVector4());
            
            sb.End();
            sb.Begin(effect: _fluidEffect);
            sb.Draw(_pixel, new Rectangle(0, 0, UiLayout.Width, UiLayout.Height), Color.White);
            sb.End();
            sb.Begin();
        }

        foreach (var s in _stars) 
            sb.Draw(_pixel, s.Position, null, s.Color * _starPulse, 0f, Vector2.Zero, s.Scale, SpriteEffects.None, 0f);
    }
}
