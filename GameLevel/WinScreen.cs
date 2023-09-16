using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JACE.IntroScreen {
    public class WinScreen : Screen {
        private GameplayInstruction gameplayInstruction;
        private TitleText titleText;

        public override void Initialize () {
            gameplayInstruction = new GameplayInstruction(new string[] { "Press `ENTER` to play again" }, new string[] { "To exit press `ESC`" });
            titleText = new TitleText("You won", "");
        }

        public override void LoadContent (ContentManager content) {
            gameplayInstruction.LoadContent(content);
            titleText.LoadContent(content);
        }

        public override void Update (Action<Screen> changeScreen, GameTime gameTime, InputState input) {
            gameplayInstruction.Update(gameTime);
            titleText.Update(gameTime);

            if (input.action) {
                changeScreen(new GameLevel.GameLevel());
            }
        }

        public override void Draw (GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch) {
            graphicsDevice.Clear(JACEColors.BackgroundColor);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            gameplayInstruction.Draw(gameTime, spriteBatch, graphicsDevice);
            titleText.Draw(gameTime, spriteBatch, graphicsDevice);

            spriteBatch.End();
        }
    }
}
