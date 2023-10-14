using System.Collections.Generic;
using System.Linq;
using JACE.Common;
using JACE.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.GameLevel;

public class GameLevel : GameScreen {
    protected const int LevelMargin = 20;

    protected const int LevelSizeX = 640;
    protected const int LevelSizeY = 480;
    protected ContentManager Content;
    protected List<BoundingObject> ImpassableObjects;
    protected KillerShapeManager KillerShapeManager;
    protected LevelArea LevelArea;
    protected Player Player;
    protected List<Tower> Towers;
    protected List<Wall> Walls;
    protected WinArea WinArea;


    public override void Activate() {
        if (Content == null)
            Content = new ContentManager(ScreenManager.Game.Services, "Content");

        LevelArea = new LevelArea(new Vector2(LevelSizeX, LevelSizeY));

        foreach (var tower in Towers)
            tower.LoadContent(Content);

        foreach (var wall in Walls)
            wall.LoadContent(Content);

        Player.LoadContent(Content);
        WinArea.LoadContent(Content);
        KillerShapeManager.LoadContent(Content);
        LevelArea.LoadContent(Content);

        ImpassableObjects = Walls.Select(wall => wall.BoundingRectangle).Concat(
            Towers.Select(tower => tower.BoundingCircle).Cast<BoundingObject>()
        ).Concat(LevelArea.GetImpassableObjects()).ToList();
    }

    public override void Unload() {
        Content.Unload();
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
        foreach (var tower in Towers)
            tower.Update(gameTime, Player.BoundingRectangle);

        KillerShapeManager.Update(this, ScreenManager, gameTime, Player.BoundingRectangle, ImpassableObjects);

        //foreach (Wall wall in walls)
        //    wall.Update(gameTime);

        var targetVolume = 1 / (Vector2.Distance(Player.Position, WinArea.Position) / 10);

        ScreenManager.MediaVolume =
            MathHelper.Clamp(targetVolume,
                ScreenManager.MediaVolume - ScreenManager.MaxMediaVolumeChangeStep,
                ScreenManager.MediaVolume + ScreenManager.MaxMediaVolumeChangeStep);
    }

    public override void HandleInput(GameTime gameTime, InputState input) {
        Player.HandleInput(gameTime, input, ImpassableObjects);
    }

    public override void Draw(GameTime gameTime) {
        var graphicsDevice = ScreenManager.GraphicsDevice;
        var spriteBatch = ScreenManager.SpriteBatch;

        var levelXWithMargins = LevelSizeX + LevelMargin * 2;
        var levelYWithMargins = LevelSizeY + LevelMargin * 2;

        var scaleX = ViewportHelper.ViewportWidth / (float)levelXWithMargins;
        var scaleY = ViewportHelper.ViewportHeight / (float)levelYWithMargins;

        var smallerDimensionScale = MathHelper.Min(scaleX, scaleY);
        var scaleMatrix = Matrix.CreateScale(smallerDimensionScale);

        var translationX = LevelMargin + (scaleX > scaleY
            ? (ViewportHelper.ViewportWidth / smallerDimensionScale - levelXWithMargins) / 2f
            : 0);
        var translationY = LevelMargin + (scaleY > scaleX
            ? (ViewportHelper.ViewportHeight / smallerDimensionScale - levelYWithMargins) / 2f
            : 0);
        var translationMatrix =
            Matrix.CreateTranslation(translationX, translationY, 0);

        var transformMatrix = translationMatrix * scaleMatrix;

        spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);

        foreach (var tower in Towers)
            tower.Draw(gameTime, spriteBatch, graphicsDevice);

        KillerShapeManager.Draw(gameTime, spriteBatch, graphicsDevice, transformMatrix);

        foreach (var wall in Walls)
            wall.Draw(gameTime, spriteBatch, graphicsDevice);

        Player.Draw(gameTime, spriteBatch, graphicsDevice);

        WinArea.Draw(gameTime, spriteBatch, graphicsDevice);

        LevelArea.Draw(gameTime, spriteBatch, graphicsDevice);

        spriteBatch.End();
    }
}