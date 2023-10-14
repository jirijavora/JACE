using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common;

public class LevelArea {
    private const int borderThickness = 4;

    private readonly Color borderColor = JaceColors.SecondaryColor;
    private readonly Wall borderWallDown;
    private readonly Wall borderWallLeft;
    private readonly Wall borderWallRight;
    private readonly Wall borderWallUp;


    public LevelArea(Vector2 size) {
        Size = size;

        borderWallDown = new Wall(new Vector2(0, Size.Y), new Vector2(Size.X, Size.Y + borderThickness));
        borderWallLeft = new Wall(new Vector2(-borderThickness, 0), new Vector2(0, Size.Y));
        borderWallRight = new Wall(new Vector2(Size.X, 0), new Vector2(Size.X + borderThickness, Size.Y));
        borderWallUp = new Wall(new Vector2(0, -borderThickness), new Vector2(Size.X, 0));
    }

    public Vector2 Size { get; }

    public void LoadContent(ContentManager content) {
        borderWallDown.LoadContent(content);
        borderWallLeft.LoadContent(content);
        borderWallRight.LoadContent(content);
        borderWallUp.LoadContent(content);
    }

    public IEnumerable<BoundingObject> GetImpassableObjects() {
        return new List<BoundingObject> {
            borderWallDown.BoundingRectangle, borderWallLeft.BoundingRectangle,
            borderWallRight.BoundingRectangle, borderWallUp.BoundingRectangle
        };
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        borderWallDown.Draw(gameTime, spriteBatch, graphicsDevice);
        borderWallLeft.Draw(gameTime, spriteBatch, graphicsDevice);
        borderWallRight.Draw(gameTime, spriteBatch, graphicsDevice);
        borderWallUp.Draw(gameTime, spriteBatch, graphicsDevice);
    }
}