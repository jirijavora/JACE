using System;
using JACE.Common;
using JACE.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.GameLevel;

public class GameLevel : GameScreen {
    private readonly KillerShapeManager killerShapeManager;
    private readonly Player player;
    private readonly Tower[] towers;
    private readonly Wall[] walls;
    private readonly WinArea winArea;

    private ContentManager content;

    public GameLevel() {
        killerShapeManager = new KillerShapeManager();

        player = new Player(new Vector2(ViewportHelper.GetXAsDimensionFraction(1f / 5f),
            ViewportHelper.GetYAsDimensionFraction(1f / 2f)));


        towers = new Tower[] {
            new(
                new Vector2(ViewportHelper.GetXAsDimensionFraction(2f / 3f),
                    ViewportHelper.GetYAsDimensionFraction(1f / 3f)), killerShapeManager.AddBall),
            new(
                new Vector2(ViewportHelper.GetXAsDimensionFraction(2f / 3f),
                    ViewportHelper.GetYAsDimensionFraction(2f / 3f)), killerShapeManager.AddBall)
        };

        //walls = new Wall[] {
        //    new Wall(new Vector2(ViewportHelper.getXAsDimensionFraction(1f/3f), ViewportHelper.getYAsDimensionFraction(1f/5f)),
        //             new Vector2(ViewportHelper.getXAsDimensionFraction(1f/3f) + 10, ViewportHelper.getYAsDimensionFraction(4f/5f)))
        //};
        walls = Array.Empty<Wall>();

        winArea = new WinArea(
            new Vector2(ViewportHelper.GetXAsDimensionFraction(7f / 8f),
                ViewportHelper.GetYAsDimensionFraction(1f / 2f)), "Reach me...");
    }

    public override void Activate() {
        if (content == null)
            content = new ContentManager(ScreenManager.Game.Services, "Content");

        foreach (var tower in towers)
            tower.LoadContent(content);

        foreach (var wall in walls)
            wall.LoadContent(content);

        player.LoadContent(content);
        winArea.LoadContent(content);
        killerShapeManager.LoadContent(content);
    }

    public override void Unload() {
        content.Unload();
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
        foreach (var tower in towers)
            tower.Update(gameTime, player.BoundingRectangle);

        killerShapeManager.Update(this, ScreenManager, gameTime, player.BoundingRectangle);

        //foreach (Wall wall in walls)
        //    wall.Update(gameTime);

        if (player.BoundingRectangle.IsColliding(winArea.BoundingRectangle)) {
            ScreenManager.RemoveScreen(this);
            ScreenManager.AddScreen(new WinScreen());
        }

        ScreenManager.MediaVolume = 1 / (Vector2.Distance(player.Position, winArea.Position) / 10);
    }

    public override void HandleInput(GameTime gameTime, InputState input) {
        player.HandleInput(gameTime, input);
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