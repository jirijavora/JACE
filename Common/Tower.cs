using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common;

public class Tower {
    private const float SizeMultiplier = 2;

    private const float BallSpeed = 120;
    private const float BallSize = 16;

    private const double FireDelay = 2.5f;
    private const float ShootSoundForward = 0.35f;

    private readonly Action<Vector2, Vector2, float, float, BoundingObject> addBall;
    private readonly Vector2 position;
    private int currentState;
    private double fireCountdown;

    private bool playedSoundEffectFlag;

    private SoundEffect shootSound;

    private int singleTextureSize;

    private int stateCount;
    private Texture2D textureAtlas;
    private Vector2 textureCenter;


    public Tower(Vector2 position, Action<Vector2, Vector2, float, float, BoundingObject> addBall) {
        this.position = position;
        this.addBall = addBall;
    }

    public BoundingCircle BoundingCircle { get; private set; }

    public void LoadContent(ContentManager content) {
        textureAtlas = content.Load<Texture2D>("tower/towerAtlas");
        shootSound = content.Load<SoundEffect>("music/heartbeat");

        singleTextureSize = textureAtlas.Height;

        textureCenter = new Vector2(singleTextureSize / 2f, singleTextureSize / 2f);

        stateCount = textureAtlas.Width / singleTextureSize;
        BoundingCircle = new BoundingCircle(position, singleTextureSize / 2f * SizeMultiplier);
    }

    public void Update(GameTime gameTime, BoundingRectangle playerBoundingRectangle) {
        fireCountdown += gameTime.ElapsedGameTime.TotalSeconds;

        if (fireCountdown >= FireDelay - ShootSoundForward && !playedSoundEffectFlag) {
            shootSound.Play(0.16f, 0, 0);
            playedSoundEffectFlag = true;
        }


        if (fireCountdown >= FireDelay) {
            fireCountdown -= FireDelay;
            playedSoundEffectFlag = false;

            addBall(
                position,
                playerBoundingRectangle.TopLeftCorner + playerBoundingRectangle.Size / 2 - position,
                BallSize,
                BallSpeed,
                BoundingCircle
            );
        }

        currentState = stateCount - 1 - (int)(fireCountdown * stateCount / FireDelay);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        spriteBatch.Draw(
            textureAtlas,
            position,
            new Rectangle(singleTextureSize * currentState, 0, singleTextureSize, singleTextureSize),
            JaceColors.SecondaryColor,
            0,
            textureCenter,
            SizeMultiplier,
            SpriteEffects.None,
            0
        );
    }
}