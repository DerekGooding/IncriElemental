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

    private struct NebulaCloud
    {
        public Vector2 Position;
        public float Speed;
        public float Scale;
        public Color Color;
        public float Rotation;
        public float RotationSpeed;
        public float ParallaxFactor;
    }

    private readonly List<Star> _stars = [];
    private readonly List<NebulaCloud> _nebulae = [];
    private readonly Texture2D _pixel;
    private readonly Texture2D _blob;
    private readonly Random _rnd = new();
    private double _totalTime = 0;
    private float _starPulse = 1.0f;

    public BackgroundManager(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);

        int size = 128;
        _blob = new Texture2D(graphicsDevice, size, size);
        Color[] data = new Color[size * size];
        float center = size / 2f;
        float maxDist = size / 2f;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                float alpha = MathHelper.SmoothStep(1f, 0f, dist / maxDist);
                data[y * size + x] = Color.White * (alpha * alpha);
            }
        }
        _blob.SetData(data);

        for (int i = 0; i < 150; i++)
        {
            float parallax = (float)(_rnd.NextDouble() * 0.8 + 0.2);
            _stars.Add(new Star
            {
                Position = new Vector2(_rnd.Next(UiLayout.Width), _rnd.Next(UiLayout.Height)),
                Speed = (float)(_rnd.NextDouble() * 20 + 5),
                Scale = (float)(_rnd.NextDouble() * 1.5 + 0.5) * parallax,
                Color = Color.Lerp(Color.MediumPurple, Color.White, (float)_rnd.NextDouble()) * (0.3f + 0.7f * parallax),
                ParallaxFactor = parallax
            });
        }

        Color[] nebulaPalette = [Color.MediumPurple * 0.2f, Color.DeepSkyBlue * 0.1f, Color.DarkSlateBlue * 0.15f];
        for (int i = 0; i < 12; i++)
        {
            float parallax = (float)(_rnd.NextDouble() * 0.3 + 0.1);
            _nebulae.Add(new NebulaCloud
            {
                Position = new Vector2(_rnd.Next(UiLayout.Width), _rnd.Next(UiLayout.Height)),
                Speed = (float)(_rnd.NextDouble() * 5 + 2),
                Scale = (float)(_rnd.NextDouble() * 4 + 2),
                Color = nebulaPalette[_rnd.Next(nebulaPalette.Length)],
                Rotation = (float)(_rnd.NextDouble() * Math.PI * 2),
                RotationSpeed = (float)(_rnd.NextDouble() * 0.1 - 0.05),
                ParallaxFactor = parallax
            });
        }
    }

    public void Update(double deltaTime, double aetherAmount)
    {
        _totalTime += deltaTime;
        _starPulse = (float)Math.Sin(_totalTime * 1.5) * 0.15f + 0.85f;

        var speedMult = 1.0 + Math.Log10(Math.Max(1, aetherAmount)) * 0.1;
        float dt = (float)deltaTime;

        for (int i = 0; i < _stars.Count; i++)
        {
            var s = _stars[i];
            s.Position.Y += s.Speed * (float)speedMult * dt * s.ParallaxFactor;
            if (s.Position.Y > UiLayout.Height + 10)
            {
                s.Position.Y = -10;
                s.Position.X = _rnd.Next(UiLayout.Width);
            }
            _stars[i] = s;
        }

        for (int i = 0; i < _nebulae.Count; i++)
        {
            var n = _nebulae[i];
            n.Position.Y += n.Speed * (float)speedMult * dt * n.ParallaxFactor;
            n.Rotation += n.RotationSpeed * dt;
            if (n.Position.Y > UiLayout.Height + 200)
            {
                n.Position.Y = -200;
                n.Position.X = _rnd.Next(UiLayout.Width);
            }
            _nebulae[i] = n;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var n in _nebulae)
        {
            Vector2 origin = new Vector2(_blob.Width / 2f, _blob.Height / 2f);
            spriteBatch.Draw(_blob, n.Position, null, n.Color * _starPulse, n.Rotation, origin, n.Scale, SpriteEffects.None, 0f);
        }

        foreach (var s in _stars)
        {
            spriteBatch.Draw(_pixel, s.Position, null, s.Color * _starPulse, 0f, Vector2.Zero, s.Scale, SpriteEffects.None, 0f);
        }
    }
}
