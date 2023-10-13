using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common.ParticleSystem;

public class BallDestructionParticleSystem : ParticleSystem {
    private const int MinParticlesPerDestruction = 6;
    private const int MaxParticlesPerDestruction = 12;

    private const int MinParticleSpeed = 6;
    private const int MaxParticleSpeed = 30;

    private const float ScaleBase = 0f;
    private const float ScaleAdditionalMax = 1f;
    private const float MinLifetime = 3f;
    private const float MaxLifetime = 8f;

    private static readonly string[] TextureNames = { "Shape1", "Shape2", "Shape3", "Shape4" };


    public BallDestructionParticleSystem(Game game, int maxDestroyedBalls) : base(game,
        maxDestroyedBalls * MaxParticlesPerDestruction) { }

    protected override void InitializeConstants() {
        TextureFilenames = TextureNames;

        MinNumParticles = MinParticlesPerDestruction;
        MaxNumParticles = MaxParticlesPerDestruction;

        BlendState = BlendState.Additive;
    }

    protected override void InitializeParticle(ref Particle p, Vector2 where) {
        var velocity = RandomHelper.NextDirection() * RandomHelper.NextFloat(MinParticleSpeed, MaxParticleSpeed);

        var lifetime = RandomHelper.NextFloat(MinLifetime, MaxLifetime);

        var acceleration = -velocity / lifetime;

        var rotation = RandomHelper.NextFloat(0, MathHelper.TwoPi);

        var angularVelocity = RandomHelper.NextFloat(-MathHelper.PiOver4, MathHelper.PiOver4);

        var textureVariant = RandomHelper.Next(TextureNames.Length);

        var particleColor = JaceColors.TertiaryColor;

        var scale = ScaleBase + ScaleAdditionalMax;

        p.Initialize(where, velocity, acceleration, particleColor, textureVariant, lifetime, rotation: rotation,
            angularVelocity: angularVelocity, scale: scale);
    }

    protected override void UpdateParticle(ref Particle particle, float dt, BoundingCircle particleBounds,
        List<BoundingObject> impassableObjects) {
        base.UpdateParticle(ref particle, dt, particleBounds, impassableObjects);

        var normalizedLifetime = particle.TimeSinceStart / particle.Lifetime;
        var normalizedRemainingLifetime = 1 - normalizedLifetime;

        var alpha = 1 - normalizedLifetime;
        particle.Color.A = (byte)(alpha * 255);

        particle.Scale = ScaleBase + ScaleAdditionalMax * (float)Math.Pow(normalizedRemainingLifetime, 8);
    }

    public void BallDestruction(BoundingCircle where) {
        var xMin = where.Center.X - where.Radius;
        var yMin = where.Center.Y - where.Radius;

        AddParticles(new Rectangle((int)xMin, (int)yMin, (int)where.Radius * 2, (int)where.Radius * 2));
    }
}