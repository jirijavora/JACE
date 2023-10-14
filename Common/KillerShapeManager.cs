using System.Collections.Generic;
using JACE.Common.ParticleSystem;
using JACE.GameLevel;
using JACE.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common;

public class KillerShapeManager {
    private const int maxDestroyedBallsOnScreen = 32;
    private readonly LinkedList<Ball> balls = new();

    private readonly BallDestructionParticleSystem destructionParticles;
    private Texture2D ballTexture;
    private Vector2 textureCenter;
    private float textureUnitSize;

    public KillerShapeManager(Game game) {
        destructionParticles = new BallDestructionParticleSystem(game, maxDestroyedBallsOnScreen);
    }

    public void LoadContent(ContentManager content) {
        ballTexture = content.Load<Texture2D>("Shape2");

        textureCenter = new Vector2(ballTexture.Width / 2, ballTexture.Height / 2);
        textureUnitSize = ballTexture.Width;

        destructionParticles.LoadContent();
    }

    public void Update(GameScreen parentScreen, ScreenManager screenManager, GameTime gameTime,
        BoundingRectangle playerBoundingRect, List<BoundingObject> impassableObjects) {
        var ball = balls.First;

        while (ball != null) {
            ball.Value.UpdatePosition(gameTime);

            if (impassableObjects.Exists(impassableObject => {
                    var isOriginatingObject = impassableObject == ball.Value.OriginatingBoundingObject;
                    var isColliding = impassableObject.IsColliding(ball.Value.BoundingCircle);

                    return !isOriginatingObject && isColliding;
                })) {
                destructionParticles.BallDestruction(ball.Value.BoundingCircle);

                var tmp = ball;
                ball = ball.Next;


                balls.Remove(tmp);


                continue;
            }

            if (playerBoundingRect.IsColliding(ball.Value.BoundingCircle))
                screenManager.ReplaceScreen(parentScreen, new RetryScreen());

            ball = ball.Next;
        }

        destructionParticles.Update(gameTime, impassableObjects);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice,
        Matrix transformMatrix) {
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

        destructionParticles.Draw(gameTime, transformMatrix);
    }

    public void AddBall(Vector2 position, Vector2 direction, float size, float speed,
        BoundingObject originatingBoundingObject) {
        direction.Normalize();
        balls.AddLast(new Ball(new BoundingCircle(position, size), direction, speed, originatingBoundingObject));
    }
}