using System;
using JACE.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.IntroScreen;

public class ShapeObject {
    private const float SizeMultiplier = 3;
    private const float MinSpeed = 40;
    private const float RandSpeedMultiplier = 300;

    private const float RequiredFlyThroughScreenPortion = 1f / 5f;
    private readonly Vector2 direction;

    private readonly int objectHeight;
    private readonly int objectWidth;

    private readonly Random random = new();
    private readonly float rotation;
    private readonly float speed;
    private readonly Texture2D texture;
    private readonly Vector2 textureCenter;
    private Vector2 position;

    public ShapeObject(Texture2D texture) {
        this.texture = texture;
        textureCenter = new Vector2(texture.Width / 2f, texture.Height / 2f);

        objectHeight = (int)(texture.Height * SizeMultiplier);
        objectWidth = (int)(texture.Width * SizeMultiplier);

        rotation = (float)(random.NextDouble() * Math.PI * 2);

        speed = (float)random.NextDouble() * RandSpeedMultiplier + MinSpeed;
        var screenX = ViewportHelper.ViewportX;
        var screenY = ViewportHelper.ViewportY;
        var screenWidth = ViewportHelper.ViewportWidth;
        var screenHeight = ViewportHelper.ViewportHeight;

        var objectHeightHalf = objectHeight / 2f;
        var objectWidthHalf = objectWidth / 2f;

        var targetPoint = new Vector2(
            RandomNumberWithinRequiredPortion(screenX, screenWidth),
            RandomNumberWithinRequiredPortion(screenY, screenHeight)
        );

        var startPoint = random.Next(4) switch {
            0 => new Vector2(-objectWidthHalf + screenX + random.Next(screenWidth + objectWidth),
                screenY - objectHeightHalf),
            1 => new Vector2(screenX + screenWidth + objectWidthHalf,
                -objectHeightHalf + screenY + random.Next(screenHeight + objectHeight)),
            2 => new Vector2(objectWidthHalf + screenX + screenWidth - random.Next(screenWidth + objectWidth),
                screenY + screenHeight + objectHeightHalf),
            3 => new Vector2(screenX - objectWidthHalf,
                screenY + screenHeight - random.Next(screenHeight + objectHeight)),
            _ => throw new InvalidOperationException("Unreachable code")
        };

        direction = Vector2.Normalize(targetPoint - startPoint);
        position = startPoint;
    }

    public BoundingCircle BoundingCircle { get; }

    private float RandomNumberWithinRequiredPortion(int offset, int availableSize) {
        return offset + availableSize * (RequiredFlyThroughScreenPortion / 2) +
               (float)random.NextDouble() * (availableSize * (1 - RequiredFlyThroughScreenPortion));
    }

    public bool Update(GameTime gameTime) {
        position += (float)gameTime.ElapsedGameTime.TotalSeconds * speed * direction;

        var screenX = ViewportHelper.ViewportX;
        var screenY = ViewportHelper.ViewportY;
        var screenWidth = ViewportHelper.ViewportWidth;
        var screenHeight = ViewportHelper.ViewportHeight;

        return !(position.X < screenX - objectWidth / 2f) &&
               !(position.X > screenX + screenWidth + objectWidth / 2f) &&
               !(position.Y < screenY - objectHeight / 2f) &&
               !(position.Y > screenY + screenHeight + objectHeight / 2f);
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        spriteBatch.Draw(texture, position, null, JaceColors.TertiaryColor, rotation, textureCenter, SizeMultiplier,
            SpriteEffects.None, 0);
    }
}