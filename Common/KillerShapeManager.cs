using JACE.IntroScreen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace JACE.Common {
    public class KillerShapeManager {
        private Texture2D ballTexture;
        private Vector2 textureCenter;
        private float textureUnitSize;

        private LinkedList<Ball> balls = new LinkedList<Ball>();

        public void LoadContent (ContentManager content) {
            ballTexture = content.Load<Texture2D>("Shape2");

            textureCenter = new Vector2(ballTexture.Width / 2, ballTexture.Height / 2);
            textureUnitSize = ballTexture.Width;
        }

        public void Update (Action<Screen> changeScreen, GameTime gameTime, BoundingRectangle playerBoundingRect) {
            foreach (Ball ball in balls) {
                if (playerBoundingRect.isColliding(ball.boundingCircle)) {
                    changeScreen(new RetryScreen());
                }

                ball.UpdatePosition(gameTime);
            }
        }

        public void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {

            foreach (Ball ball in balls) {
                spriteBatch.Draw(
                    ballTexture,
                    ball.boundingCircle.center,
                    null,
                    JACEColors.TertiaryColor,
                    0,
                    textureCenter,
                    (2 * ball.boundingCircle.radius) / textureUnitSize,
                    SpriteEffects.None,
                    0
                );
            }
        }

        public void addBall (Vector2 position, Vector2 direction, float size, float speed) {
            direction.Normalize();
            balls.AddLast(new Ball(new BoundingCircle(position, size), direction, speed));
        }

    }
}
