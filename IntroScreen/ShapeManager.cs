using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace JACE.IntroScreen {
    public class ShapeManager {
        private static readonly string[] textureNames = { "Shape1", "Shape2", "Shape3", "Shape4" };

        private Texture2D[] textures = new Texture2D[textureNames.Length];
        private LinkedList<ShapeObject> shapes = new LinkedList<ShapeObject>();

        private Random random = new Random();

        public void LoadContent (ContentManager content) {

            for (int i = 0; i < textureNames.Length; i++) {
                textures[i] = content.Load<Texture2D>(textureNames[i]);
            }
        }

        public void Update (GameTime gameTime, InputState input) {
            if (input.secondaryAction) {
                shapes.AddLast(new ShapeObject(textures[random.Next(textureNames.Length)]));
            }

            LinkedListNode<ShapeObject> currentShape = shapes.First;

            while (currentShape != null) {
                bool updateResult = currentShape.Value.Update(gameTime);

                LinkedListNode<ShapeObject> lastShape = currentShape;
                currentShape = currentShape.Next;

                if (updateResult == false) {
                    shapes.Remove(lastShape);
                }

            }
        }

        public void Draw (GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            foreach (ShapeObject shape in shapes) {
                shape.Draw(gameTime, spriteBatch, graphicsDevice);
            }
        }

    }
}
