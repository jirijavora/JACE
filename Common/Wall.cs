using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common;

public class Wall {
    private readonly Color color = JaceColors.SecondaryColor;
    private Texture2D texture;

    public Wall(Vector2 topLeftCorner, Vector2 bottomRightCorner) {
        BoundingRectangle = new BoundingRectangle(topLeftCorner, bottomRightCorner - topLeftCorner);
    }

    public Wall(Vector2 topLeftCorner, Vector2 bottomRightCorner, Color color) :
        this(topLeftCorner, bottomRightCorner) {
        this.color = color;
    }

    public BoundingRectangle BoundingRectangle { get; }

    public void LoadContent(ContentManager content) {
        texture = content.Load<Texture2D>("Wall");
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        spriteBatch.Draw(texture, BoundingRectangle.TopLeftCorner, null, color, 0, Vector2.Zero, BoundingRectangle.Size,
            SpriteEffects.None, 0);
    }
}