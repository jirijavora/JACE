using System.Collections.Generic;
using JACE.Common;
using JACE.GameLevel;
using Microsoft.Xna.Framework;

namespace JACE.Levels;

public class Level1 : GameLevel.GameLevel {
    public override void Activate() {
        KillerShapeManager = new KillerShapeManager(ScreenManager.Game);


        Player = new Player(new Vector2(ViewportHelper.GetXAsDimensionFraction(1f / 5f),
            ViewportHelper.GetYAsDimensionFraction(1f / 2f)));


        Towers = new List<Tower> {
            new(
                new Vector2(ViewportHelper.GetXAsDimensionFraction(2f / 3f),
                    ViewportHelper.GetYAsDimensionFraction(1f / 3f)), KillerShapeManager.AddBall),
            new(
                new Vector2(ViewportHelper.GetXAsDimensionFraction(2f / 3f),
                    ViewportHelper.GetYAsDimensionFraction(2f / 3f)), KillerShapeManager.AddBall)
        };

        Walls = new List<Wall> {
            new(
                new Vector2(ViewportHelper.GetXAsDimensionFraction(1f / 3f),
                    ViewportHelper.GetYAsDimensionFraction(1f / 5f)),
                new Vector2(ViewportHelper.GetXAsDimensionFraction(1f / 3f) + 10,
                    ViewportHelper.GetYAsDimensionFraction(4f / 5f)))
        };

        WinArea = new WinArea(
            new Vector2(ViewportHelper.GetXAsDimensionFraction(7f / 8f),
                ViewportHelper.GetYAsDimensionFraction(1f / 2f)), "Reach me...");

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