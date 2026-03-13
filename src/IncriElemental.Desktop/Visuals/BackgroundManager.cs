using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IncriElemental.Desktop.Visuals;

public class BackgroundManager
{
    private struct Star
    {
        public Vector2 Position;
        public float Speed;
        public float Scale;
        public Color Color;
    }

    private readonly List<Star> _stars = [];
    private readonly Texture2D _pixel;
    private readonly Random _rnd = new();

    public BackgroundManager(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        for (int i = 0; i < 100; i++)
        {
            _stars.Add(new Star
            {
                Position = new Vector2(_rnd.Next(UiLayout.Width), _rnd.Next(UiLayout.Height)),
                Speed = (float)(_rnd.NextDouble() * 10 + 5),
                Scale = (float)(_rnd.NextDouble() * 2 + 1),
                Color = Color.MediumPurple * (float)(_rnd.NextDouble() * 0.5 + 0.2)
            });
        }
    }

    public void Update(double deltaTime, double aetherAmount)
    {
        var speedMult = 1.0 + Math.Log10(Math.Max(1, aetherAmount)) * 0.1;
        for (int i = 0; i < _stars.Count; i++)
        {
            var s = _stars[i];
            s.Position.Y += (float)(s.Speed * speedMult * deltaTime);
            if (s.Position.Y > UiLayout.Height)
            {
                s.Position.Y = -10;
                s.Position.X = _rnd.Next(UiLayout.Width);
            }
            _stars[i] = s;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var s in _stars)
        {
            spriteBatch.Draw(_pixel, s.Position, null, s.Color, 0f, Vector2.Zero, s.Scale, SpriteEffects.None, 0f);
        }
    }
}
