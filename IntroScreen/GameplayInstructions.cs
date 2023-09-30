using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.IntroScreen; 

public class GameplayInstruction {
    private const int bottomPaddingPx = 32;
    private const int instructionPaddingPx = 8;
    private const int importantSeperatorSizePx = 32;
    private readonly Color color = JACEColors.TertiaryColor;
    private SpriteFont font;

    private readonly Color importantColor = JACEColors.SecondaryColor;

    private readonly string[] importantInstructions;
    private readonly string[] instructions;

    public GameplayInstruction(string[] importantInstructions, string[] instructions) {
        this.importantInstructions = importantInstructions;
        this.instructions = instructions;
    }

    public void LoadContent(ContentManager content) {
        font = content.Load<SpriteFont>("Minimal");
    }

    public void Update(GameTime gameTime) { }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        for (var i = 0; i < instructions.Length; i++) {
            var instruction = instructions[i];

            var instructionSize = font.MeasureString(instruction);

            var instructionY = graphicsDevice.Viewport.Y + graphicsDevice.Viewport.Height -
                               instructionSize.Y -
                               bottomPaddingPx -
                               (instructionSize.Y + instructionPaddingPx) * i;
            var instructionX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - instructionSize.X) / 2f;

            spriteBatch.DrawString(font, instruction, new Vector2(instructionX, instructionY), color);
        }

        for (var i = 0; i < importantInstructions.Length; i++) {
            var instruction = importantInstructions[i];

            var instructionSize = font.MeasureString(instruction);

            var instructionY = graphicsDevice.Viewport.Y + graphicsDevice.Viewport.Height -
                               instructionSize.Y -
                               bottomPaddingPx -
                               importantSeperatorSizePx -
                               (instructionSize.Y + instructionPaddingPx) * (instructions.Length + i);
            var instructionX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - instructionSize.X) / 2f;

            spriteBatch.DrawString(font, instruction, new Vector2(instructionX, instructionY), importantColor);
        }
    }
}