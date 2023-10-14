using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Common;

public class Player {
    private const int BackgroundPaddingPx = 6;
    private const int CursorHeightOffsetPx = -4;
    private const int CursorHeightPx = 6;
    private const int MovementCollisionRefinementSteps = 5;

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

    public Player(Vector2 initialPosition, string playerName = "Player") {
        Position = initialPosition;
        PlayerName = playerName;

        BoundingRectangle = CalculateBoundingRect(Position);
    }

    public Vector2 Position { get; private set; }

    public string PlayerName { get; }

    public BoundingRectangle BoundingRectangle { get; private set; }

    public void LoadContent(ContentManager content) {
        playerFont = content.Load<SpriteFont>("Minimal");
        playerBackground = content.Load<Texture2D>("Blank");
        cursorTexture = content.Load<Texture2D>("CursorTexture");

        MeasurePlayer();
    }

    public void HandleInput(GameTime gameTime, InputState input, List<BoundingObject> impassableObjects) {
        var deltaT = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var newPosition = Position + deltaT * input.Direction * Speed;
        var newPositionBoundingRectangle = CalculateBoundingRect(newPosition);

        // Check if not trying to go through a solid object
        if (impassableObjects.Exists(
                impassableObject => impassableObject.IsColliding(newPositionBoundingRectangle))) {
            // Find a not colliding distance to travel
            var newPositionCandidate = Position;
            var movementIncrement = deltaT * input.Direction * Speed;
            for (var i = 0; i < MovementCollisionRefinementSteps; i++) {
                movementIncrement /= 2;

                var newPositionCandidateBoundingRectangle =
                    CalculateBoundingRect(newPositionCandidate + movementIncrement);
                if (!impassableObjects.Exists(
                        impassableObject => impassableObject.IsColliding(newPositionCandidateBoundingRectangle)))
                    newPositionCandidate += movementIncrement;
            }

            newPosition = newPositionCandidate;
            newPositionBoundingRectangle = CalculateBoundingRect(newPosition);
        }


        Position = newPosition;
        BoundingRectangle = newPositionBoundingRectangle;
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        var playerDrawLeftTop = Position - playerTextSize / 2;
        var playerDrawBackgroundLeftTop = Position - playerBackgroundSize / 2;

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

    private BoundingRectangle CalculateBoundingRect(Vector2 calcPosition) {
        return new BoundingRectangle(calcPosition - playerBackgroundSize / 2, playerBackgroundSize);
    }
}