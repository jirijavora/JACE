using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE {
    public class GameplayInstruction {
        private SpriteFont font;

        private const int bottomPaddingPx = 32;
        private const int instructionPaddingPx = 8;

        private string[] instructions = { "Press `SPACE` to do \"something...\"", "To exit press `ESC`" };

        private Color color = Color.LightGray;

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
                                        ((instructionSize.Y + instructionPaddingPx) * (instructions.Length - i - 1));
                float instructionX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - instructionSize.X) / 2f;

                spriteBatch.DrawString(font, instruction, new Vector2(instructionX, instructionY), color);
            }
        }
    }
}
