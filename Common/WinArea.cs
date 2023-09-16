using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common {
    public class WinArea {
        private SpriteFont font;
        private Texture2D background;
        private Vector2 position;

        private readonly Color backgroundColor = JACEColors.WinColor;
        private readonly Color textColor = JACEColors.BackgroundColor;
        private const int backgroundPaddingPx = 6;

        public string text {
            get; private set;
        }

        Vector2 playerTextSize;
        Vector2 playerBackgroundSize;

        public BoundingRectangle boundingRectangle {
            get; private set;
        }

        public WinArea (Vector2 initialPosition, string winText) {
            position = initialPosition;
            this.text = winText;
        }

        public void LoadContent (ContentManager content) {
            font = content.Load<SpriteFont>("Minimal");
            background = content.Load<Texture2D>("PlayerBackground");

            measurePlayer();
            updateBoundingRect();
        }

        public void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            Vector2 drawLeftTop = position - playerTextSize / 2;
            Vector2 drawBackgroundLeftTop = position - playerBackgroundSize / 2;

            spriteBatch.Draw(background, drawBackgroundLeftTop, null, backgroundColor, 0, Vector2.Zero, playerBackgroundSize, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, text, drawLeftTop, textColor);

            //spriteBatch.Draw(playerBackground, new Vector2(boundingRectangle.X, boundingRectangle.Y), null, Color.Red, 0, Vector2.Zero, new Vector2(boundingRectangle.Width, boundingRectangle.Height), SpriteEffects.None, 0);
        }

        private void measurePlayer () {
            playerTextSize = font.MeasureString(text);
            playerBackgroundSize = playerTextSize + new Vector2(backgroundPaddingPx * 2, backgroundPaddingPx * 2);
        }

        private void updateBoundingRect () {
            boundingRectangle = new BoundingRectangle((position - playerBackgroundSize / 2), playerBackgroundSize);
        }
    }
}
