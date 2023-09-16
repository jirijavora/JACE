using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JACE.Common {
    public class Tower {
        private Vector2 position;
        private Texture2D textureAtlas;
        private Vector2 textureCenter;

        private int singleTextureSize;

        private const float SIZE_MULTIPLIER = 2;

        private const float BALL_SPEED = 120;
        private const float BALL_SIZE = 16;

        private const double FIRE_DELAY = 2.5f;
        private double fireCountdown = 0;

        private int stateCount;
        private int currentState = 0;

        private Action<Vector2, Vector2, float, float> addBall;

        public BoundingCircle boundingCircle {
            get; private set;
        }

        public Tower (Vector2 position, Action<Vector2, Vector2, float, float> addBall) {
            this.position = position;
            this.addBall = addBall;
        }

        public void LoadContent (ContentManager content) {
            textureAtlas = content.Load<Texture2D>("tower/towerAtlas");
            singleTextureSize = textureAtlas.Height;

            textureCenter = new Vector2(singleTextureSize / 2, singleTextureSize / 2);

            stateCount = textureAtlas.Width / singleTextureSize;
            boundingCircle = new BoundingCircle(position, (singleTextureSize / 2) * SIZE_MULTIPLIER);
        }

        public void Update (GameTime gameTime, BoundingRectangle playerBoundingRectangle) {
            fireCountdown += gameTime.ElapsedGameTime.TotalSeconds;

            if (fireCountdown >= FIRE_DELAY) {
                fireCountdown -= FIRE_DELAY;

                addBall(
                    position,
                    (playerBoundingRectangle.topLeftCorner + playerBoundingRectangle.size / 2) - position,
                    BALL_SIZE,
                    BALL_SPEED
                );
            }

            currentState = stateCount - 1 - (int)((fireCountdown * stateCount) / FIRE_DELAY);
        }

        public void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {

            spriteBatch.Draw(
                textureAtlas,
                position,
                new Rectangle(singleTextureSize * currentState, 0, singleTextureSize, singleTextureSize),
                JACEColors.SecondaryColor,
                0,
                textureCenter,
                SIZE_MULTIPLIER,
                SpriteEffects.None,
                0
           );
        }
    }
}
