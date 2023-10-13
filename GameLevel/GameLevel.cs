using System.Collections.Generic;
using System.Linq;
using JACE.Common;
using JACE.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.GameLevel;

public class GameLevel : GameScreen {
    private ContentManager content;
    private List<BoundingObject> impassableObjects;
    private KillerShapeManager killerShapeManager;
    private Player player;
    private List<Tower> towers;
    private List<Wall> walls;
    private WinArea winArea;


    public override void Activate() {
        killerShapeManager = new KillerShapeManager(ScreenManager.Game);


        player = new Player(new Vector2(ViewportHelper.GetXAsDimensionFraction(1f / 5f),
            ViewportHelper.GetYAsDimensionFraction(1f / 2f)));


        towers = new List<Tower> {
            new(
                new Vector2(ViewportHelper.GetXAsDimensionFraction(2f / 3f),
                    ViewportHelper.GetYAsDimensionFraction(1f / 3f)), killerShapeManager.AddBall),
            new(
                new Vector2(ViewportHelper.GetXAsDimensionFraction(2f / 3f),
                    ViewportHelper.GetYAsDimensionFraction(2f / 3f)), killerShapeManager.AddBall)
        };

        walls = new List<Wall> {
            new(
                new Vector2(ViewportHelper.GetXAsDimensionFraction(1f / 3f),
                    ViewportHelper.GetYAsDimensionFraction(1f / 5f)),
                new Vector2(ViewportHelper.GetXAsDimensionFraction(1f / 3f) + 10,
                    ViewportHelper.GetYAsDimensionFraction(4f / 5f)))
        };

        winArea = new WinArea(
            new Vector2(ViewportHelper.GetXAsDimensionFraction(7f / 8f),
                ViewportHelper.GetYAsDimensionFraction(1f / 2f)), "Reach me...");


        if (content == null)
            content = new ContentManager(ScreenManager.Game.Services, "Content");

        foreach (var tower in towers)
            tower.LoadContent(content);

        foreach (var wall in walls)
            wall.LoadContent(content);

        player.LoadContent(content);
        winArea.LoadContent(content);
        killerShapeManager.LoadContent(content);

        impassableObjects = walls.Select(wall => wall.BoundingRectangle).Concat(
            towers.Select(tower => tower.BoundingCircle).Cast<BoundingObject>()
        ).ToList();
    }

    public override void Unload() {
        content.Unload();
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
        foreach (var tower in towers)
            tower.Update(gameTime, player.BoundingRectangle);

        killerShapeManager.Update(this, ScreenManager, gameTime, player.BoundingRectangle, impassableObjects);

        //foreach (Wall wall in walls)
        //    wall.Update(gameTime);

        if (player.BoundingRectangle.IsColliding(winArea.BoundingRectangle)) {
            ScreenManager.RemoveScreen(this);
            ScreenManager.AddScreen(new WinScreen());
        }

        ScreenManager.MediaVolume = 1 / (Vector2.Distance(player.Position, winArea.Position) / 10);
    }

    public override void HandleInput(GameTime gameTime, InputState input) {
        player.HandleInput(gameTime, input, impassableObjects);
    }

    public override void Draw(GameTime gameTime) {
        var graphicsDevice = ScreenManager.GraphicsDevice;
        var spriteBatch = ScreenManager.SpriteBatch;

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (var tower in towers)
            tower.Draw(gameTime, spriteBatch, graphicsDevice);

        killerShapeManager.Draw(gameTime, spriteBatch, graphicsDevice);

        foreach (var wall in walls)
            wall.Draw(gameTime, spriteBatch, graphicsDevice);

        player.Draw(gameTime, spriteBatch, graphicsDevice);

        winArea.Draw(gameTime, spriteBatch, graphicsDevice);

        spriteBatch.End();
    }
}