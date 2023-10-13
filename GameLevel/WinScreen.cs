using System;
using System.Collections.Generic;
using JACE.IntroScreen;
using JACE.Levels;
using JACE.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.GameLevel;

public class WinScreen : GameScreen {
    private readonly GameplayInstruction gameplayInstruction;
    private readonly TitleText titleText;

    private ContentManager content;

    public WinScreen() {
        gameplayInstruction =
            new GameplayInstruction(
                new List<string> { "Press `ENTER` to play again", "Press `SPACE` to go to the main menu" },
                new List<string> { "To exit press `ESC`" });
        titleText = new TitleText("You won", "");

        TransitionOnTime = TimeSpan.FromSeconds(0.5f);
    }

    public override void Activate() {
        if (content == null)
            content = new ContentManager(ScreenManager.Game.Services, "Content");

        gameplayInstruction.LoadContent(content);
        titleText.LoadContent(content);
    }

    public override void Unload() {
        content.Unload();
    }

    public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen) {
        gameplayInstruction.Update(gameTime);
        titleText.Update(gameTime);

        base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
    }

    public override void HandleInput(GameTime gameTime, InputState input) {
        if (input.Action) ScreenManager.ReplaceScreen(this, new Level1());
        else if (input.SecondaryAction) ScreenManager.ReplaceScreen(this, new IntroScreen.IntroScreen());
    }

    public override void Draw(GameTime gameTime) {
        var graphicsDevice = ScreenManager.GraphicsDevice;
        var spriteBatch = ScreenManager.SpriteBatch;

        graphicsDevice.Clear(JaceColors.BackgroundColor);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        gameplayInstruction.Draw(gameTime, spriteBatch, graphicsDevice);
        titleText.Draw(gameTime, spriteBatch, graphicsDevice, TransitionAlpha);

        spriteBatch.End();
    }
}