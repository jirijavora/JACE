using System.Collections.Generic;
using System.Linq;
using JACE.Common;
using JACE.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.GameLevel;

public class GameLevel : GameScreen {
    protected ContentManager Content;
    protected List<BoundingObject> ImpassableObjects;
    protected KillerShapeManager KillerShapeManager;
    protected Player Player;
    protected List<Tower> Towers;
    protected List<Wall> Walls;
    protected WinArea WinArea;


    public override void Activate() {
        if (Content == null)
            Content = new ContentManager(ScreenManager.Game.Services, "Content");

        foreach (var tower in Towers)
            tower.LoadContent(Content);

        foreach (var wall in Walls)
            wall.LoadContent(Content);

        Player.LoadContent(Content);
        WinArea.LoadContent(Content);
        KillerShapeManager.LoadContent(Content);

        ImpassableObjects = Walls.Select(wall => wall.BoundingRectangle).Concat(
            Towers.Select(tower => tower.BoundingCircle).Cast<BoundingObject>()
        ).ToList();
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

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (var tower in Towers)
            tower.Draw(gameTime, spriteBatch, graphicsDevice);

        KillerShapeManager.Draw(gameTime, spriteBatch, graphicsDevice);

        foreach (var wall in Walls)
            wall.Draw(gameTime, spriteBatch, graphicsDevice);

        Player.Draw(gameTime, spriteBatch, graphicsDevice);

        WinArea.Draw(gameTime, spriteBatch, graphicsDevice);

        spriteBatch.End();
    }
}