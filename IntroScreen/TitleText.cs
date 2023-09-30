using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.IntroScreen; 

public class TitleText {
    private const float renderStartHeight = 1f / 4f;
    private const float subtitlePaddingPx = 10;
    private const int cursorHeightPx = 12;
    private const int cursorHeightOffsetPx = -20;
    private const double cursorBlinkFrequency = 1;
    private const double cursorBlinkDutyCycle = 0.6f;
    private const double cursorBlinkTime = 1 / cursorBlinkFrequency;
    private const double cursorBlinkOnTime = cursorBlinkTime * cursorBlinkDutyCycle;
    private double cursorBlinkAnimationTimer;
    private Texture2D cursorTexture;
    private readonly Color subtitleColor = JACEColors.SecondaryColor;
    private SpriteFont subtitleFont;
    private readonly string subtitleString;

    private readonly Color titleColor = JACEColors.MainColor;
    private SpriteFont titleFont;

    private readonly string titleString;

    public TitleText(string title, string subtitle) {
        titleString = title;
        subtitleString = subtitle;
    }

    public void LoadContent(ContentManager content) {
        titleFont = content.Load<SpriteFont>("MinimalXXL");
        subtitleFont = content.Load<SpriteFont>("Minimal");
        cursorTexture = content.Load<Texture2D>("CursorTexture");
    }

    public void Update(GameTime gameTime) { }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        var titleSize = titleFont.MeasureString(titleString);
        var subtitleSize = subtitleFont.MeasureString(subtitleString);

        var titleY = graphicsDevice.Viewport.Y + renderStartHeight * graphicsDevice.Viewport.Height;
        var titleX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - titleSize.X) / 2f;

        var subtitleY = titleY + titleSize.Y + subtitlePaddingPx;
        var subtitleX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - subtitleSize.X) / 2f;

        spriteBatch.DrawString(subtitleFont, subtitleString, new Vector2(subtitleX, subtitleY), subtitleColor);
        spriteBatch.DrawString(titleFont, titleString, new Vector2(titleX, titleY), titleColor);

        cursorBlinkAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
        while (cursorBlinkAnimationTimer >= cursorBlinkTime) cursorBlinkAnimationTimer -= cursorBlinkTime;
        var renderCursor = cursorBlinkAnimationTimer <= cursorBlinkOnTime;

        if (renderCursor) {
            var titleWithoutLastLetter = titleString.Remove(titleString.Length - 1);
            var titleSizeWithoutLastLetter = titleFont.MeasureString(titleWithoutLastLetter);

            var cursorStartX = (int)(titleX + titleSizeWithoutLastLetter.X);
            var cursorStartY = (int)(titleY + titleSize.Y) + cursorHeightOffsetPx;
            var cursorWidth = (int)(titleSize.X - titleSizeWithoutLastLetter.X);
            var cursorHeight = cursorHeightPx;
            var cursorRectangle = new Rectangle(cursorStartX, cursorStartY, cursorWidth, cursorHeight);

            spriteBatch.Draw(cursorTexture, cursorRectangle, titleColor);
        }
    }
}