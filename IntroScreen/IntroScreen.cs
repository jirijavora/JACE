using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace JACE.IntroScreen; 

public class IntroScreen : Screen {
    private const float MEDIA_VOLUME_RAMP_UP_TIME = 12;
    private const float MEDIA_VOLUME_MAX = 0.4f;

    private Song backgroundMusic;
    private GameplayInstruction gameplayInstruction;
    private float mediaVolume;
    private ShapeManager shapeManager;
    private TitleText titleText;

    public override void Initialize() {
        shapeManager = new ShapeManager();
        gameplayInstruction = new GameplayInstruction(new[] { "Press `ENTER` to play" },
            new[] { "Press `SPACE` to do \"something...\"", "To exit press `ESC`" });
        titleText = new TitleText("JACE", "Just Another Console Experience");

        mediaVolume = 0f;
    }

    public override void LoadContent(ContentManager content) {
        shapeManager.LoadContent(content);
        gameplayInstruction.LoadContent(content);
        titleText.LoadContent(content);

        backgroundMusic = content.Load<Song>("music/magic_space");
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(backgroundMusic);
        MediaPlayer.Volume = mediaVolume;
    }

    public override void Update(Action<Screen> changeScreen, GameTime gameTime, InputState input) {
        shapeManager.Update(gameTime, input);
        gameplayInstruction.Update(gameTime);
        titleText.Update(gameTime);

        mediaVolume += (float)gameTime.ElapsedGameTime.TotalSeconds / (MEDIA_VOLUME_RAMP_UP_TIME / MEDIA_VOLUME_MAX);
        if (mediaVolume > MEDIA_VOLUME_MAX) mediaVolume = MEDIA_VOLUME_MAX;

        if (input.action) changeScreen(new GameLevel.GameLevel());
    }

    public override void Draw(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) {
        MediaPlayer.Volume = mediaVolume;

        graphicsDevice.Clear(JACEColors.BackgroundColor);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        shapeManager.Draw(gameTime, spriteBatch, graphicsDevice);
        gameplayInstruction.Draw(gameTime, spriteBatch, graphicsDevice);
        titleText.Draw(gameTime, spriteBatch, graphicsDevice);

        spriteBatch.End();
    }
}