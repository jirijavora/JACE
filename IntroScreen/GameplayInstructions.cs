using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.IntroScreen {
    public class GameplayInstruction {
        private SpriteFont font;

        private const int bottomPaddingPx = 32;
        private const int instructionPaddingPx = 8;
        private const int importantSeperatorSizePx = 32;

        private string[] importantInstructions;
        private string[] instructions;

        private Color importantColor = JACEColors.SecondaryColor;
        private Color color = JACEColors.TertiaryColor;

        public GameplayInstruction (string[] importantInstructions, string[] instructions) {
            this.importantInstructions = importantInstructions;
            this.instructions = instructions;
        }

        public void LoadContent (ContentManager content) {
            font = content.Load<SpriteFont>("Minimal");
        }

        public void Update (GameTime gameTime) {
        }

        public void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            for (int i = 0; i < instructions.Length; i++) {
                string instruction = instructions[i];

                Vector2 instructionSize = font.MeasureString(instruction);

                float instructionY = graphicsDevice.Viewport.Y + graphicsDevice.Viewport.Height -
                                        instructionSize.Y -
                                        bottomPaddingPx -
                                        (instructionSize.Y + instructionPaddingPx) * i;
                float instructionX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - instructionSize.X) / 2f;

                spriteBatch.DrawString(font, instruction, new Vector2(instructionX, instructionY), color);
            }

            for (int i = 0; i < importantInstructions.Length; i++) {
                string instruction = importantInstructions[i];

                Vector2 instructionSize = font.MeasureString(instruction);

                float instructionY = graphicsDevice.Viewport.Y + graphicsDevice.Viewport.Height -
                                        instructionSize.Y -
                                        bottomPaddingPx -
                                        importantSeperatorSizePx -
                                        (instructionSize.Y + instructionPaddingPx) * (instructions.Length + i);
                float instructionX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - instructionSize.X) / 2f;

                spriteBatch.DrawString(font, instruction, new Vector2(instructionX, instructionY), importantColor);
            }
        }
    }
}
