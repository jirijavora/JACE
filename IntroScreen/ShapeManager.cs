using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace JACE.IntroScreen;

public class ShapeManager {
    private static readonly string[] TextureNames = { "Shape1", "Shape2", "Shape3", "Shape4" };

    private readonly Random random = new();
    private readonly LinkedList<ShapeObject> shapes = new();

    private readonly Texture2D[] textures = new Texture2D[TextureNames.Length];

    public void LoadContent(ContentManager content) {
        for (var i = 0; i < TextureNames.Length; i++) textures[i] = content.Load<Texture2D>(TextureNames[i]);
    }

    public void Update(GameTime gameTime) {
        var currentShape = shapes.First;

        while (currentShape != null) {
            var updateResult = currentShape.Value.Update(gameTime);

            var lastShape = currentShape;
            currentShape = currentShape.Next;

            if (updateResult == false) shapes.Remove(lastShape);
        }
    }

    public void HandleInput(GameTime gameTime, InputState input) {
        if (input.SecondaryAction) shapes.AddLast(new ShapeObject(textures[random.Next(TextureNames.Length)]));
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
        foreach (var shape in shapes) shape.Draw(gameTime, spriteBatch, graphicsDevice);
    }
}