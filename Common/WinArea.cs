using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common;

public class WinArea {
    private const int BackgroundPaddingPx = 6;

    private readonly Color backgroundColor = JaceColors.WinColor;
    private readonly Color textColor = JaceColors.BackgroundColor;
    private Texture2D background;
    private SpriteFont font;
    private Vector2 playerBackgroundSize;

    private Vector2 playerTextSize;

    public WinArea(Vector2 initialPosition, string winText) {
        Position = initialPosition;
        Text = winText;
    }

    public Vector2 Position { get; }

    public string Text { get; }

    public BoundingRectangle BoundingRectangle { get; private set; }

    public void LoadContent(ContentManager content) {
        font = content.Load<SpriteFont>("Minimal");
        background = content.Load<Texture2D>("Blank");

        MeasurePlayer();
        UpdateBoundingRect();
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        var drawLeftTop = Position - playerTextSize / 2;
        var drawBackgroundLeftTop = Position - playerBackgroundSize / 2;

        spriteBatch.Draw(background, drawBackgroundLeftTop, null, backgroundColor, 0, Vector2.Zero,
            playerBackgroundSize, SpriteEffects.None, 0);
        spriteBatch.DrawString(font, Text, drawLeftTop, textColor);

        //spriteBatch.Draw(playerBackground, new Vector2(boundingRectangle.X, boundingRectangle.Y), null, Color.Red, 0, Vector2.Zero, new Vector2(boundingRectangle.Width, boundingRectangle.Height), SpriteEffects.None, 0);
    }

    private void MeasurePlayer() {
        playerTextSize = font.MeasureString(Text);
        playerBackgroundSize = playerTextSize + new Vector2(BackgroundPaddingPx * 2, BackgroundPaddingPx * 2);
    }

    private void UpdateBoundingRect() {
        BoundingRectangle = new BoundingRectangle(Position - playerBackgroundSize / 2, playerBackgroundSize);
    }
}