using System.Collections.Generic;
using JACE.Common;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.Levels;

public class Level1 : GameLevel.GameLevel {
    private static bool _instructionsSeen;
    private static readonly string instructions = "Use WASD or the Arrow Keys to move around";
    private static readonly string instructionsExit = "Press `Enter` to play";
    private Texture2D instructionsBackground;
    private SpriteFont instructionsFont;

    public override void Activate() {
        KillerShapeManager = new KillerShapeManager(ScreenManager.Game);


        Player = new Player(new Vector2(LevelSizeX / 5f, LevelSizeY / 2f));


        Towers = new List<Tower> {
            new(new Vector2(2f / 3f * LevelSizeX, 1f / 3f * LevelSizeY), KillerShapeManager.AddBall),
            new(new Vector2(2f / 3f * LevelSizeX, 2f / 3f * LevelSizeY), KillerShapeManager.AddBall)
        };

        Walls = new List<Wall> {
            new(
                new Vector2(1f / 3f * LevelSizeX, 1f / 5f * LevelSizeY),
                new Vector2(1f / 3f * LevelSizeX + 10, 4f / 5f * LevelSizeY))
        };

        WinArea = new WinArea(new Vector2(7f / 8f * LevelSizeX, 1f / 2f * LevelSizeY), "Reach me...");

        base.Activate();

        instructionsFont = Content.Load<SpriteFont>("Minimal");
        instructionsBackground = Content.Load<Texture2D>("Blank");
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
        if (!_instructionsSeen) return;

        if (Player.BoundingRectangle.IsColliding(WinArea.BoundingRectangle)) {
            ScreenManager.RemoveScreen(this);
            ScreenManager.AddScreen(new Level2());
        }


        base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    public override void HandleInput(GameTime gameTime, InputState input) {
        if (input.Action && !_instructionsSeen) _instructionsSeen = true;


        if (!_instructionsSeen) return;
        base.HandleInput(gameTime, input);
    }

    public override void Draw(GameTime gameTime) {
        base.Draw(gameTime);

        if (!_instructionsSeen) {
            var graphicsDevice = ScreenManager.GraphicsDevice;
            var spriteBatch = ScreenManager.SpriteBatch;

            var instructionsSize = instructionsFont.MeasureString(instructions);

            var instructionsX = (ViewportHelper.ViewportWidth - instructionsSize.X) / 2;
            var instructionsY = (ViewportHelper.ViewportHeight - instructionsSize.Y) / 2;

            spriteBatch.Begin(blendState: BlendState.NonPremultiplied);

            var backgroundColor = Color.DimGray;
            backgroundColor.A = 240;
            spriteBatch.Draw(instructionsBackground,
                new Rectangle(0, 0, ViewportHelper.ViewportWidth, ViewportHelper.ViewportHeight), backgroundColor);

            spriteBatch.DrawString(instructionsFont, instructions, new Vector2(instructionsX, instructionsY),
                JaceColors.BackgroundColor);

            var instructionsExitSize = instructionsFont.MeasureString(instructionsExit);
            var instructionsExitX = (ViewportHelper.ViewportWidth - instructionsExitSize.X) / 2;

            spriteBatch.DrawString(instructionsFont, instructionsExit,
                new Vector2(instructionsExitX, instructionsY + instructionsExitSize.Y * 2),
                JaceColors.TertiaryColor);

            spriteBatch.End();
        }
    }
}