using JACE.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JACE.IntroScreen {
    public class ShapeObject {
        private Texture2D texture;
        private Vector2 textureCenter;
        private Vector2 position;
        private Vector2 direction;
        private float speed;
        private float rotation;

        private const float sizeMultiplier = 3;
        private const float minSpeed = 40;
        private const float randSpeedMultiplier = 300;

        private int objectHeight;
        private int objectWidth;

        private const float requiredFlyThroughScreenPortion = 1 / 5;

        private Random random = new Random();

        public BoundingCircle boundingCircle { get; private set; }

        private float randomNumberWithinRequiredPortion (int offset, int availableSize) {
            return offset + availableSize * (requiredFlyThroughScreenPortion / 2) +
                (float)random.NextDouble() * (availableSize * (1 - requiredFlyThroughScreenPortion));
        }

        public ShapeObject (Texture2D texture) {
            this.texture = texture;
            textureCenter = new Vector2(texture.Width / 2, texture.Height / 2);

            objectHeight = (int)(texture.Height * sizeMultiplier);
            objectWidth = (int)(texture.Width * sizeMultiplier);

            rotation = (float)(random.NextDouble() * Math.PI * 2);

            speed = (float)random.NextDouble() * randSpeedMultiplier + minSpeed;
            int screenX = ViewportHelper.viewportX;
            int screenY = ViewportHelper.viewportY;
            int screenWidth = ViewportHelper.viewportWidth;
            int screenHeight = ViewportHelper.viewportHeight;

            float objectHeightHalf = objectHeight / 2;
            float objectWidthHalf = objectWidth / 2;

            Vector2 targetPoint = new Vector2(
                randomNumberWithinRequiredPortion(screenX, screenWidth),
                randomNumberWithinRequiredPortion(screenY, screenHeight)
            );

            Vector2 startPoint;
            switch (random.Next(4)) {
                case 0:
                    startPoint = new Vector2(-objectWidthHalf + screenX + random.Next(screenWidth + objectWidth), screenY - objectHeightHalf);
                    break;
                case 1:
                    startPoint = new Vector2(screenX + screenWidth + objectWidthHalf, -objectHeightHalf + screenY + random.Next(screenHeight + objectHeight));
                    break;
                case 2:
                    startPoint = new Vector2(objectWidthHalf + screenX + screenWidth - random.Next(screenWidth + objectWidth), screenY + screenHeight + objectHeightHalf);
                    break;
                case 3:
                    startPoint = new Vector2(screenX - objectWidthHalf, screenY + screenHeight - random.Next(screenHeight + objectHeight));
                    break;
                default:
                    throw new InvalidOperationException("Unreachable code");

            }

            direction = Vector2.Normalize(targetPoint - startPoint);
            position = startPoint;
        }

        public bool Update (GameTime gameTime) {
            position += (float)gameTime.ElapsedGameTime.TotalSeconds * speed * direction;

            int screenX = ViewportHelper.viewportX;
            int screenY = ViewportHelper.viewportY;
            int screenWidth = ViewportHelper.viewportWidth;
            int screenHeight = ViewportHelper.viewportHeight;

            if (position.X < screenX - objectWidth / 2 ||
                position.X > screenX + screenWidth + objectWidth / 2 ||
                position.Y < screenY - objectHeight / 2 ||
                position.Y > screenY + screenHeight + objectHeight / 2)
                return false;

            return true;
        }

        public void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            spriteBatch.Draw(texture, position, null, JACEColors.TertiaryColor, rotation, textureCenter, sizeMultiplier, SpriteEffects.None, 0);
        }

    }
}
