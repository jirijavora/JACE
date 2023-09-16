using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common {
    public class Wall {
        private Texture2D texture;

        private Color color = JACEColors.SecondaryColor;

        public BoundingRectangle boundingRectangle { get; private set; }

        public Wall (Vector2 topLeftCorner, Vector2 bottomRightCorner) {
            boundingRectangle = new BoundingRectangle(topLeftCorner, bottomRightCorner - topLeftCorner);
        }

        public void LoadContent (ContentManager content) {
            texture = content.Load<Texture2D>("Wall");
        }

        public void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            spriteBatch.Draw(texture, boundingRectangle.topLeftCorner, null, color, 0, Vector2.Zero, boundingRectangle.size, SpriteEffects.None, 0);
        }
    }
}
