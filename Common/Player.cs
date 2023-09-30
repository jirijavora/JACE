﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common;

public class Player {
    private const int BackgroundPaddingPx = 6;
    private const int CursorHeightOffsetPx = -4;
    private const int CursorHeightPx = 6;

    private const int Speed = 200;

    private const double CursorBlinkFrequency = 1;
    private const double CursorBlinkDutyCycle = 0.6f;
    private const double CursorBlinkTime = 1 / CursorBlinkFrequency;
    private const double CursorBlinkOnTime = CursorBlinkTime * CursorBlinkDutyCycle;

    private readonly Color playerBackgroundColor = JaceColors.MainColor;
    private readonly Color playerTextColor = JaceColors.BackgroundColor;

    private double cursorBlinkAnimationTimer;
    private Texture2D cursorTexture;
    private Texture2D playerBackground;
    private Vector2 playerBackgroundSize;
    private SpriteFont playerFont;

    private Vector2 playerTextSize;
    private Vector2 position;

    public Player(Vector2 initialPosition, string playerName = "Player") {
        position = initialPosition;
        PlayerName = playerName;
    }

    public Vector2 Position => position;

    public string PlayerName { get; }

    public BoundingRectangle BoundingRectangle { get; private set; }

    public void LoadContent(ContentManager content) {
        playerFont = content.Load<SpriteFont>("Minimal");
        playerBackground = content.Load<Texture2D>("Blank");
        cursorTexture = content.Load<Texture2D>("CursorTexture");

        MeasurePlayer();
    }

    public void HandleInput(GameTime gameTime, InputState input) {
        var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

        position += deltaT * input.Direction * Speed;

        if (position.X - playerBackgroundSize.X / 2 < ViewportHelper.ViewportX)
            position.X = ViewportHelper.ViewportX + playerBackgroundSize.X / 2;
        if (position.X + playerBackgroundSize.X / 2 > ViewportHelper.ViewportX + ViewportHelper.ViewportWidth)
            position.X = ViewportHelper.ViewportX + ViewportHelper.ViewportWidth - playerBackgroundSize.X / 2;
        if (position.Y - playerBackgroundSize.Y / 2 < ViewportHelper.ViewportY)
            position.Y = ViewportHelper.ViewportY + playerBackgroundSize.Y / 2;
        if (position.Y + playerBackgroundSize.Y / 2 > ViewportHelper.ViewportY + ViewportHelper.ViewportHeight)
            position.Y = ViewportHelper.ViewportY + ViewportHelper.ViewportHeight - playerBackgroundSize.Y / 2;

        UpdateBoundingRect();
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        var playerDrawLeftTop = position - playerTextSize / 2;
        var playerDrawBackgroundLeftTop = position - playerBackgroundSize / 2;

        spriteBatch.Draw(playerBackground, playerDrawBackgroundLeftTop, null, playerBackgroundColor, 0, Vector2.Zero,
            playerBackgroundSize, SpriteEffects.None, 0);
        spriteBatch.DrawString(playerFont, PlayerName, playerDrawLeftTop, playerTextColor);

        cursorBlinkAnimationTimer += gameTime.ElapsedGameTime.TotalSeconds;
        while (cursorBlinkAnimationTimer >= CursorBlinkTime) cursorBlinkAnimationTimer -= CursorBlinkTime;
        var renderCursor = cursorBlinkAnimationTimer <= CursorBlinkOnTime;

        if (renderCursor) {
            var textWithoutLastLetter = PlayerName.Remove(PlayerName.Length - 1);
            var textSizeWithoutLastLetter = playerFont.MeasureString(textWithoutLastLetter);

            var cursorStartX = (int)(playerDrawLeftTop.X + textSizeWithoutLastLetter.X);
            var cursorStartY = (int)(playerDrawLeftTop.Y + playerTextSize.Y) + CursorHeightOffsetPx;
            var cursorWidth = (int)(playerTextSize.X - textSizeWithoutLastLetter.X);
            var cursorHeight = CursorHeightPx;
            var cursorRectangle = new Rectangle(cursorStartX, cursorStartY, cursorWidth, cursorHeight);

            spriteBatch.Draw(cursorTexture, cursorRectangle, playerTextColor);
        }

        //spriteBatch.Draw(playerBackground, new Vector2(boundingRectangle.X, boundingRectangle.Y), null, Color.Red, 0, Vector2.Zero, new Vector2(boundingRectangle.Width, boundingRectangle.Height), SpriteEffects.None, 0);
    }

    private void MeasurePlayer() {
        playerTextSize = playerFont.MeasureString(PlayerName);
        playerBackgroundSize = playerTextSize + new Vector2(BackgroundPaddingPx * 2, BackgroundPaddingPx * 2);
    }

    private void UpdateBoundingRect() {
        BoundingRectangle = new BoundingRectangle(position - playerBackgroundSize / 2, playerBackgroundSize);
    }
}