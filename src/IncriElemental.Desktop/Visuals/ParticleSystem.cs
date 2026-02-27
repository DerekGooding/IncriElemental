using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace IncriElemental.Desktop.Visuals;

public class Particle
{
    public Vector2 Position;
    public Vector2 Velocity;
    public Color Color;
    public float Lifespan;
    public float Age;
    public float Scale;

    public bool IsDead => Age >= Lifespan;

    public void Update(float deltaTime)
    {
        Position += Velocity * deltaTime;
        Age += deltaTime;
    }
}

public class ParticleSystem
{
    private readonly List<Particle> _particles = new();
    private readonly Texture2D _pixel;
    private readonly Random _random = new();

    public ParticleSystem(GraphicsDevice graphicsDevice)
    {
        _pixel = new Texture2D(graphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });
    }

    public void AddParticle(Vector2 position, Vector2 velocity, Color color, float lifespan, float scale = 2f)
    {
        _particles.Add(new Particle
        {
            Position = position,
            Velocity = velocity,
            Color = color,
            Lifespan = lifespan,
            Age = 0,
            Scale = scale
        });
    }

    public void Update(float deltaTime)
    {
        for (int i = _particles.Count - 1; i >= 0; i--)
        {
            _particles[i].Update(deltaTime);
            if (_particles[i].IsDead)
            {
                _particles.RemoveAt(i);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var particle in _particles)
        {
            float alpha = 1f - (particle.Age / particle.Lifespan);
            spriteBatch.Draw(_pixel, particle.Position, null, particle.Color * alpha, 0f, Vector2.Zero, particle.Scale, SpriteEffects.None, 0f);
        }
    }

    public void EmitFocus(Vector2 center)
    {
        // Particles flying INTO the center
        for (int i = 0; i < 5; i++)
        {
            float angle = (float)(_random.NextDouble() * Math.PI * 2);
            float distance = 100f + (float)_random.NextDouble() * 100f;
            Vector2 startPos = center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * distance;
            Vector2 velocity = (center - startPos) * 2f;
            
            AddParticle(startPos, velocity, Color.MediumPurple, 0.5f, 2f);
        }
    }
}
