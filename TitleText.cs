using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE {
    public class TitleText {
        private SpriteFont titleFont;
        private SpriteFont subtitleFont;
        private Texture2D cursorTexture;

        private const string titleString = "JACE";
        private const string subtitleString = "Just Another Console Experience";

        private const float renderStartHeight = 1f / 4f;
        private const float subtitlePaddingPx = 10;
        private const int cursorHeightPx = 12;
        private const int cursorHeightOffsetPx = -20;
        private const double cursorBlinkFrequency = 1;
        private const double cursorBlinkDutyCycle = 0.6f;
        private const double cursorBlinkTime = 1 / cursorBlinkFrequency;
        private const double cursorBlinkOnTime = cursorBlinkTime * cursorBlinkDutyCycle;

        private Color titleColor = Color.DarkSlateGray;
        private Color subtitleColor = Color.LightSlateGray;
        private double cursorBlinkAnimationTimer = 0;

        public void LoadContent (ContentManager content) {
            titleFont = content.Load<SpriteFont>("MinimalXXL");
            subtitleFont = content.Load<SpriteFont>("Minimal");
            cursorTexture = content.Load<Texture2D>("CursorTexture");
        }

        public void Update (GameTime gameTime) {
        }

        public void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            Vector2 titleSize = titleFont.MeasureString(titleString);
            Vector2 subtitleSize = subtitleFont.MeasureString(subtitleString);

            float titleY = graphicsDevice.Viewport.Y + (renderStartHeight * graphicsDevice.Viewport.Height);
            float titleX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - titleSize.X) / 2f;

            float subtitleY = titleY + titleSize.Y + subtitlePaddingPx;
            float subtitleX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - subtitleSize.X) / 2f;

            spriteBatch.DrawString(subtitleFont, subtitleString, new Vector2(subtitleX, subtitleY), subtitleColor);
            spriteBatch.DrawString(titleFont, titleString, new Vector2(titleX, titleY), titleColor);

            cursorBlinkAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            while (cursorBlinkAnimationTimer >= cursorBlinkTime) {
                cursorBlinkAnimationTimer -= cursorBlinkTime;
            }
            bool renderCursor = cursorBlinkAnimationTimer <= cursorBlinkOnTime;

            if (renderCursor) {
                string titleWithoutLastLetter = titleString.Remove(titleString.Length - 1);
                Vector2 titleSizeWithoutLastLetter = titleFont.MeasureString(titleWithoutLastLetter);

                int cursorStartX = (int)(titleX + titleSizeWithoutLastLetter.X);
                int cursorStartY = (int)(titleY + titleSize.Y) + cursorHeightOffsetPx;
                int cursorWidth = (int)(titleSize.X - titleSizeWithoutLastLetter.X);
                int cursorHeight = cursorHeightPx;
                Rectangle cursorRectangle = new Rectangle(cursorStartX, cursorStartY, cursorWidth, cursorHeight);

                spriteBatch.Draw(cursorTexture, cursorRectangle, titleColor);
            }
        }
    }
}
