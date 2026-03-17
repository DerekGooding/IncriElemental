using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IncriElemental.Desktop.Visuals;

public class Particle
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Color Color;
    public float Lifespan;
    public float Age;
    public float Scale;
    public string? Text; // For popups

    public bool IsDead => Age >= Lifespan;

    public virtual void Update(float deltaTime)
    {
        Position += Velocity * deltaTime;
        Age += deltaTime;
    }
}

public class ParticleSystem
{
    private readonly List<Particle> _particles = [];
    private readonly Texture2D _pixel;
    private readonly Random _random = new();

    public ParticleSystem(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    public void AddParticle(Vector2 position, Vector2 velocity, Color color, float lifespan, float scale = 2f, string? text = null) => _particles.Add(new Particle
    {
        Position = position,
        Velocity = velocity,
        Color = color,
        Lifespan = lifespan,
        Age = 0,
        Scale = scale,
        Text = text
    });

    public void Update(float deltaTime)
    {
        for (var i = _particles.Count - 1; i >= 0; i--)
        {
            _particles[i].Update(deltaTime);
            if (_particles[i].IsDead)
            {
                _particles.RemoveAt(i);
            }
        }
    }

    public void Draw(SpriteBatch sb, SpriteFont? font = null, Effect? hologramEffect = null, double totalTime = 0)
    {
        foreach (var p in _particles)
        {
            var alpha = 1f - (p.Age / p.Lifespan);
            if (!string.IsNullOrEmpty(p.Text) && font != null && hologramEffect != null)
            {
                hologramEffect.Parameters["Time"]?.SetValue((float)totalTime);
                hologramEffect.Parameters["Color"]?.SetValue(p.Color.ToVector4());
                
                sb.End();
                sb.Begin(effect: hologramEffect);
                sb.DrawString(font, p.Text, p.Position, p.Color * alpha, 0f, Vector2.Zero, p.Scale, SpriteEffects.None, 0f);
                sb.End();
                sb.Begin();
            }
            else
            {
                sb.Draw(_pixel, p.Position, null, p.Color * alpha, 0f, Vector2.Zero, p.Scale, SpriteEffects.None, 0f);
            }
        }
    }

    public void EmitFocus(Vector2 center)
    {
        for (var i = 0; i < 5; i++)
        {
            var angle = (float)(_random.NextDouble() * Math.PI * 2);
            var distance = 100f + (float)_random.NextDouble() * 100f;
            var startPos = center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;
            var velocity = (center - startPos) * 2f;
            AddParticle(startPos, velocity, Color.MediumPurple, 0.5f, 2f);
        }
    }

    public void EmitTrail(Vector2 pos, Color color)
    {
        var velocity = new Vector2((float)(_random.NextDouble() * 20 - 10), (float)(_random.NextDouble() * 20 - 10));
        AddParticle(pos, velocity, color * 0.5f, 0.5f, (float)(_random.NextDouble() * 2 + 1));
    }

    public void EmitPopup(Vector2 pos, string text, Color color)
    {
        var velocity = new Vector2((float)(_random.NextDouble() * 20 - 10), -40f - (float)_random.NextDouble() * 20f);
        AddParticle(pos, velocity, color, 1.5f, 0.8f, text);
    }
}
