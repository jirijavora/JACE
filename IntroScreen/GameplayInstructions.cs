using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.IntroScreen;

public class GameplayInstruction {
    private const int BottomPaddingPx = 32;
    private const int InstructionPaddingPx = 8;
    private const int ImportantSeparatorSizePx = 32;
    private readonly Color color = JaceColors.TertiaryColor;

    private readonly Color importantColor = JaceColors.SecondaryColor;

    private readonly List<string> importantInstructions;
    private readonly List<string> instructions;
    private SpriteFont font;

    public GameplayInstruction(List<string> importantInstructions, List<string> instructions) {
        this.importantInstructions = importantInstructions;
        this.instructions = instructions;
    }

    public void LoadContent(ContentManager content) {
        font = content.Load<SpriteFont>("Minimal");
    }

    public void Update(GameTime gameTime) { }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        for (var i = 0; i < instructions.Count; i++) {
            var instruction = instructions[i];

            var instructionSize = font.MeasureString(instruction);

            var instructionY = graphicsDevice.Viewport.Y + graphicsDevice.Viewport.Height -
                               instructionSize.Y -
                               BottomPaddingPx -
                               (instructionSize.Y + InstructionPaddingPx) * i;
            var instructionX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - instructionSize.X) / 2f;

            spriteBatch.DrawString(font, instruction, new Vector2(instructionX, instructionY), color);
        }

        for (var i = 0; i < importantInstructions.Count; i++) {
            var instruction = importantInstructions[i];

            var instructionSize = font.MeasureString(instruction);

            var instructionY = graphicsDevice.Viewport.Y + graphicsDevice.Viewport.Height -
                               instructionSize.Y -
                               BottomPaddingPx -
                               ImportantSeparatorSizePx -
                               (instructionSize.Y + InstructionPaddingPx) * (instructions.Count + i);
            var instructionX = graphicsDevice.Viewport.X + (graphicsDevice.Viewport.Width - instructionSize.X) / 2f;

            spriteBatch.DrawString(font, instruction, new Vector2(instructionX, instructionY), importantColor);
        }
    }
}