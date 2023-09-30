using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace JACE.IntroScreen {
    public class IntroScreen : Screen {
        private GameplayInstruction gameplayInstruction;
        private TitleText titleText;
        private ShapeManager shapeManager;

        private Song backgroundMusic;

        public override void Initialize () {
            shapeManager = new ShapeManager();
            gameplayInstruction = new GameplayInstruction(new string[] { "Press `ENTER` to play" }, new string[] { "Press `SPACE` to do \"something...\"", "To exit press `ESC`" });
            titleText = new TitleText("JACE", "Just Another Console Experience");
        }

        public override void LoadContent (ContentManager content) {
            shapeManager.LoadContent(content);
            gameplayInstruction.LoadContent(content);
            titleText.LoadContent(content);

            backgroundMusic = content.Load<Song>("music/magic_space.mp3");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
        }

        public override void Update (Action<Screen> changeScreen, GameTime gameTime, InputState input) {
            shapeManager.Update(gameTime, input);
            gameplayInstruction.Update(gameTime);
            titleText.Update(gameTime);

            if (input.action) {
                changeScreen(new GameLevel.GameLevel());
            }
        }

        public override void Draw (GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) {
            graphicsDevice.Clear(JACEColors.BackgroundColor);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            shapeManager.Draw(gameTime, spriteBatch, graphicsDevice);
            gameplayInstruction.Draw(gameTime, spriteBatch, graphicsDevice);
            titleText.Draw(gameTime, spriteBatch, graphicsDevice);

            spriteBatch.End();
        }
    }
}
