using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace JACE {
    public class ShapeManager {
        private static readonly string[] textureNames = { "Shape1", "Shape2", "Shape3", "Shape4" };

        private Texture2D[] textures = new Texture2D[textureNames.Length];
        private LinkedList<ShapeObject> shapes = new LinkedList<ShapeObject>();

        private KeyboardState previousKeyboardState;

        private Random random = new Random();

        public void LoadContent (ContentManager content) {

            for (int i = 0; i < textureNames.Length; i++) {
                textures[i] = content.Load<Texture2D>(textureNames[i]);
            }
        }

        public void Update (GameTime gameTime, GraphicsDevice graphicsDevice) {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (currentKeyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space)) {
                shapes.AddLast(new ShapeObject(textures[random.Next(textureNames.Length)], graphicsDevice));
            }

            LinkedListNode<ShapeObject> currentShape = shapes.First;

            while (currentShape != null) {
                bool updateResult = currentShape.Value.Update(gameTime, graphicsDevice);

                LinkedListNode<ShapeObject> lastShape = currentShape;
                currentShape = currentShape.Next;

                if (updateResult == false) {
                    shapes.Remove(lastShape);
                }

            }

            previousKeyboardState = currentKeyboardState;
        }

        public void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            foreach (ShapeObject shape in shapes) {
                shape.Draw(gameTime, spriteBatch, graphicsDevice);
            }
        }

    }
}
