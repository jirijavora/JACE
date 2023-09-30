using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.IntroScreen;

public class TitleText {
    private const float RenderStartHeight = 1f / 4f;
    private const float SubtitlePaddingPx = 10;
    private const int CursorHeightPx = 12;
    private const int CursorHeightOffsetPx = -20;
    private const double CursorBlinkFrequency = 1;
    private const double CursorBlinkDutyCycle = 0.6f;
    private const double CursorBlinkTime = 1 / CursorBlinkFrequency;
    private const double CursorBlinkOnTime = CursorBlinkTime * CursorBlinkDutyCycle;
    private readonly Color subtitleColor = JaceColors.SecondaryColor;
    private readonly string subtitleString;

    private readonly Color titleColor = JaceColors.MainColor;

    private readonly string titleString;
    private double cursorBlinkAnimationTimer;
    private Texture2D cursorTexture;
    private SpriteFont subtitleFont;
    private SpriteFont titleFont;

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

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice,
        float transitionAlpha = 1) {
        var titleSize = titleFont.MeasureString(titleString);
        var subtitleSize = subtitleFont.MeasureString(subtitleString);

        var titleY = graphicsDevice.Viewport.Y + RenderStartHeight * graphicsDevice.Viewport.Height;
        var titleX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - titleSize.X) / 2f;

        var subtitleY = titleY + titleSize.Y + SubtitlePaddingPx;
        var subtitleX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - subtitleSize.X) / 2f;

        spriteBatch.DrawString(subtitleFont, subtitleString, new Vector2(subtitleX, subtitleY),
            subtitleColor * transitionAlpha);
        spriteBatch.DrawString(titleFont, titleString, new Vector2(titleX, titleY), titleColor * transitionAlpha);

        cursorBlinkAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
        while (cursorBlinkAnimationTimer >= CursorBlinkTime) cursorBlinkAnimationTimer -= CursorBlinkTime;
        var renderCursor = cursorBlinkAnimationTimer <= CursorBlinkOnTime;

        // ReSharper disable once InvertIf
        if (renderCursor) {
            var titleWithoutLastLetter = titleString.Remove(titleString.Length - 1);
            var titleSizeWithoutLastLetter = titleFont.MeasureString(titleWithoutLastLetter);

            var cursorStartX = (int)(titleX + titleSizeWithoutLastLetter.X);
            var cursorStartY = (int)(titleY + titleSize.Y) + CursorHeightOffsetPx;
            var cursorWidth = (int)(titleSize.X - titleSizeWithoutLastLetter.X);
            var cursorHeight = CursorHeightPx;
            var cursorRectangle = new Rectangle(cursorStartX, cursorStartY, cursorWidth, cursorHeight);

            spriteBatch.Draw(cursorTexture, cursorRectangle, titleColor * transitionAlpha);
        }
    }
}