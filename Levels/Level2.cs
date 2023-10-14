using System.Collections.Generic;
using JACE.Common;
using JACE.GameLevel;
using Microsoft.Xna.Framework;

namespace JACE.Levels;

public class Level2 : GameLevel.GameLevel {
    public override void Activate() {
        KillerShapeManager = new KillerShapeManager(ScreenManager.Game);


        Player = new Player(new Vector2(LevelSizeX / 5f, LevelSizeY / 2f));


        Towers = new List<Tower> {
            new(new Vector2(2f / 3f * LevelSizeX, 1f / 5f * LevelSizeY), KillerShapeManager.AddBall),
            new(new Vector2(2f / 3f * LevelSizeX, 4f / 5f * LevelSizeY), KillerShapeManager.AddBall),
            new(new Vector2(1f / 2f * LevelSizeX, 1f / 2f * LevelSizeY), KillerShapeManager.AddBall)
        };

        Walls = new List<Wall> {
            new(
                new Vector2(1f / 3f * LevelSizeX, 2f / 5f * LevelSizeY),
                new Vector2(1f / 3f * LevelSizeX + 10, 3f / 5f * LevelSizeY)),

            new(
                new Vector2(4f / 5f * LevelSizeX, 2f / 5f * LevelSizeY - 10),
                new Vector2(LevelSizeX - 20, 2f / 5f * LevelSizeY)),

            new(
                new Vector2(4f / 5f * LevelSizeX, 3f / 5f * LevelSizeY),
                new Vector2(LevelSizeX - 20, 3f / 5f * LevelSizeY + 10))
        };

        WinArea = new WinArea(new Vector2(7f / 8f * LevelSizeX, 1f / 2f * LevelSizeY), "And again...");

        base.Activate();
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
        if (Player.BoundingRectangle.IsColliding(WinArea.BoundingRectangle)) {
            ScreenManager.RemoveScreen(this);
            ScreenManager.AddScreen(new WinScreen());
        }


        base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }
}