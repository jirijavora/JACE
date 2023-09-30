using System.Collections.Generic;
using JACE.GameLevel;
using JACE.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common;

public class KillerShapeManager {
    private readonly LinkedList<Ball> balls = new();
    private Texture2D ballTexture;
    private SoundEffect shootSound;
    private Vector2 textureCenter;
    private float textureUnitSize;

    public void LoadContent(ContentManager content) {
        ballTexture = content.Load<Texture2D>("Shape2");
        shootSound = content.Load<SoundEffect>("music/heartbeat");

        textureCenter = new Vector2(ballTexture.Width / 2, ballTexture.Height / 2);
        textureUnitSize = ballTexture.Width;
    }

    public void Update(GameScreen parentScreen, ScreenManager screenManager, GameTime gameTime,
        BoundingRectangle playerBoundingRect) {
        foreach (var ball in balls) {
            if (playerBoundingRect.IsColliding(ball.BoundingCircle))
                screenManager.ReplaceScreen(parentScreen, new RetryScreen());

            ball.UpdatePosition(gameTime);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        foreach (var ball in balls)
            spriteBatch.Draw(
                ballTexture,
                ball.BoundingCircle.Center,
                null,
                JaceColors.TertiaryColor,
                0,
                textureCenter,
                2 * ball.BoundingCircle.Radius / textureUnitSize,
                SpriteEffects.None,
                0
            );
    }

    public void AddBall(Vector2 position, Vector2 direction, float size, float speed) {
        direction.Normalize();
        balls.AddLast(new Ball(new BoundingCircle(position, size), direction, speed));
        shootSound.Play(0.3f, 0, 0);
    }
}