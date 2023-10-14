using System.Collections.Generic;
using JACE.Common;
using Microsoft.Xna.Framework;

namespace JACE.Levels;

public class Level1 : GameLevel.GameLevel {
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
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
        if (Player.BoundingRectangle.IsColliding(WinArea.BoundingRectangle)) {
            ScreenManager.RemoveScreen(this);
            ScreenManager.AddScreen(new Level2());
        }


        base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }
}