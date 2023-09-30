using System;
using JACE.Common;
using JACE.IntroScreen;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.GameLevel;

public class GameLevel : Screen {
    private KillerShapeManager killerShapeManager;
    private Player player;
    private Tower[] towers;
    private Wall[] walls;
    private WinArea winArea;

    public override void Initialize() {
        killerShapeManager = new KillerShapeManager();

        player = new Player(new Vector2(ViewportHelper.getXAsDimensionFraction(1f / 5f),
            ViewportHelper.getYAsDimensionFraction(1f / 2f)));


        towers = new Tower[] {
            new(
                new Vector2(ViewportHelper.getXAsDimensionFraction(2f / 3f),
                    ViewportHelper.getYAsDimensionFraction(1f / 3f)), killerShapeManager.addBall),
            new(
                new Vector2(ViewportHelper.getXAsDimensionFraction(2f / 3f),
                    ViewportHelper.getYAsDimensionFraction(2f / 3f)), killerShapeManager.addBall)
        };

        //walls = new Wall[] {
        //    new Wall(new Vector2(ViewportHelper.getXAsDimensionFraction(1f/3f), ViewportHelper.getYAsDimensionFraction(1f/5f)),
        //             new Vector2(ViewportHelper.getXAsDimensionFraction(1f/3f) + 10, ViewportHelper.getYAsDimensionFraction(4f/5f)))
        //};
        walls = Array.Empty<Wall>();

        winArea = new WinArea(
            new Vector2(ViewportHelper.getXAsDimensionFraction(7f / 8f),
                ViewportHelper.getYAsDimensionFraction(1f / 2f)), "Reach me...");
    }

    public override void LoadContent(ContentManager content) {
        foreach (var tower in towers)
            tower.LoadContent(content);

        foreach (var wall in walls)
            wall.LoadContent(content);

        player.LoadContent(content);
        winArea.LoadContent(content);
        killerShapeManager.LoadContent(content);
    }

    public override void Update(Action<Screen> changeScreen, GameTime gameTime, InputState input) {
        player.Update(gameTime, input);

        foreach (var tower in towers)
            tower.Update(gameTime, player.boundingRectangle);

        killerShapeManager.Update(changeScreen, gameTime, player.boundingRectangle);

        //foreach (Wall wall in walls)
        //    wall.Update(gameTime);

        if (player.boundingRectangle.isColliding(winArea.boundingRectangle)) changeScreen(new WinScreen());
    }

    public override void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) {
        graphicsDevice.Clear(JACEColors.BackgroundColor);

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