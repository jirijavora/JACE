using System.Collections.Generic;
using JACE.Levels;
using JACE.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.IntroScreen;

public class IntroScreen : GameScreen {
    private const float MediaVolumeRampUpTime = 12;
    private const float MediaVolumeMax = 0.4f;
    private readonly GameplayInstruction gameplayInstruction;
    private readonly ShapeManager shapeManager;
    private readonly TitleText titleText;


    private ContentManager content;

    public IntroScreen() {
        shapeManager = new ShapeManager();
        gameplayInstruction = new GameplayInstruction(new List<string> { "Press `ENTER` to play" },
            new List<string> { "Press `SPACE` to do \"something...\"", "To exit press `ESC`" });
        titleText = new TitleText("JACE", "Just Another Console Experience");
    }

    public override void Activate() {
        if (content == null)
            content = new ContentManager(ScreenManager.Game.Services, "Content");

        shapeManager.LoadContent(content);
        gameplayInstruction.LoadContent(content);
        titleText.LoadContent(content);
    }

    public override void Unload() {
        content.Unload();
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
        shapeManager.Update(gameTime);
        gameplayInstruction.Update(gameTime);
        titleText.Update(gameTime);

        ScreenManager.MediaVolume +=
            (float)gameTime.ElapsedGameTime.TotalSeconds / (MediaVolumeRampUpTime / MediaVolumeMax);
        if (ScreenManager.MediaVolume > MediaVolumeMax) ScreenManager.MediaVolume = MediaVolumeMax;
    }

    public override void HandleInput(GameTime gameTime, InputState input) {
        shapeManager.HandleInput(gameTime, input);

        if (input.Action) ScreenManager.ReplaceScreen(this, new Level1());
    }

    public override void Draw(GameTime gameTime) {
        var graphicsDevice = ScreenManager.GraphicsDevice;
        var spriteBatch = ScreenManager.SpriteBatch;

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        shapeManager.Draw(gameTime, spriteBatch, graphicsDevice);
        gameplayInstruction.Draw(gameTime, spriteBatch, graphicsDevice);
        titleText.Draw(gameTime, spriteBatch, graphicsDevice);

        spriteBatch.End();
    }
}