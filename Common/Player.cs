using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common {
    public class Player {
        private Texture2D cursorTexture;
        private SpriteFont playerFont;
        private Texture2D playerBackground;
        private Vector2 position;

        private readonly Color playerBackgroundColor = JACEColors.MainColor;
        private readonly Color playerTextColor = JACEColors.BackgroundColor;
        private const int backgroundPaddingPx = 6;
        private const int cursorHeightOffsetPx = -4;
        private const int cursorHeightPx = 6;

        private const int speed = 200;

        public string playerName {
            get; private set;
        }

        private const double cursorBlinkFrequency = 1;
        private const double cursorBlinkDutyCycle = 0.6f;
        private const double cursorBlinkTime = 1 / cursorBlinkFrequency;
        private const double cursorBlinkOnTime = cursorBlinkTime * cursorBlinkDutyCycle;

        private double cursorBlinkAnimationTimer = 0;

        Vector2 playerTextSize;
        Vector2 playerBackgroundSize;

        public BoundingRectangle boundingRectangle {
            get; private set;
        }

        public Player (Vector2 initialPosition, string playerName = "Player") {
            position = initialPosition;
            this.playerName = playerName;
        }

        public void LoadContent (ContentManager content) {
            playerFont = content.Load<SpriteFont>("Minimal");
            playerBackground = content.Load<Texture2D>("PlayerBackground");
            cursorTexture = content.Load<Texture2D>("CursorTexture");

            measurePlayer();
        }

        public void Update (GameTime gameTime, InputState input) {
            float deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

            position += deltaT * input.direction * speed;

            if (position.X - playerBackgroundSize.X / 2 < ViewportHelper.viewportX) {
                position.X = ViewportHelper.viewportX + playerBackgroundSize.X / 2;
            }
            if (position.X + playerBackgroundSize.X / 2 > ViewportHelper.viewportX + ViewportHelper.viewportWidth) {
                position.X = ViewportHelper.viewportX + ViewportHelper.viewportWidth - playerBackgroundSize.X / 2;
            }
            if (position.Y - playerBackgroundSize.Y / 2 < ViewportHelper.viewportY) {
                position.Y = ViewportHelper.viewportY + playerBackgroundSize.Y / 2;
            }
            if (position.Y + playerBackgroundSize.Y / 2 > ViewportHelper.viewportY + ViewportHelper.viewportHeight) {
                position.Y = ViewportHelper.viewportY + ViewportHelper.viewportHeight - playerBackgroundSize.Y / 2;
            }

            updateBoundingRect();
        }

        public void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            Vector2 playerDrawLeftTop = position - playerTextSize / 2;
            Vector2 playerDrawBackgroundLeftTop = position - playerBackgroundSize / 2;

            spriteBatch.Draw(playerBackground, playerDrawBackgroundLeftTop, null, playerBackgroundColor, 0, Vector2.Zero, playerBackgroundSize, SpriteEffects.None, 0);
            spriteBatch.DrawString(playerFont, playerName, playerDrawLeftTop, playerTextColor);

            cursorBlinkAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            while (cursorBlinkAnimationTimer >= cursorBlinkTime) {
                cursorBlinkAnimationTimer -= cursorBlinkTime;
            }
            bool renderCursor = cursorBlinkAnimationTimer <= cursorBlinkOnTime;

            if (renderCursor) {
                string textWithoutLastLetter = playerName.Remove(playerName.Length - 1);
                Vector2 textSizeWithoutLastLetter = playerFont.MeasureString(textWithoutLastLetter);

                int cursorStartX = (int)(playerDrawLeftTop.X + textSizeWithoutLastLetter.X);
                int cursorStartY = (int)(playerDrawLeftTop.Y + playerTextSize.Y) + cursorHeightOffsetPx;
                int cursorWidth = (int)(playerTextSize.X - textSizeWithoutLastLetter.X);
                int cursorHeight = cursorHeightPx;
                Rectangle cursorRectangle = new Rectangle(cursorStartX, cursorStartY, cursorWidth, cursorHeight);

                spriteBatch.Draw(cursorTexture, cursorRectangle, playerTextColor);
            }

            //spriteBatch.Draw(playerBackground, new Vector2(boundingRectangle.X, boundingRectangle.Y), null, Color.Red, 0, Vector2.Zero, new Vector2(boundingRectangle.Width, boundingRectangle.Height), SpriteEffects.None, 0);
        }

        private void measurePlayer () {
            playerTextSize = playerFont.MeasureString(playerName);
            playerBackgroundSize = playerTextSize + new Vector2(backgroundPaddingPx * 2, backgroundPaddingPx * 2);
        }

        private void updateBoundingRect () {
            boundingRectangle = new BoundingRectangle((position - playerBackgroundSize / 2), playerBackgroundSize);
        }
    }
}
