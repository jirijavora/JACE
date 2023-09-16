using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JACE {
    public abstract class Screen {

        public abstract void Initialize ();

        public abstract void LoadContent (ContentManager content);

        public abstract void Update (Action<Screen> changeScreen, GameTime gameTimes, InputState input);

        public abstract void Draw (GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch);
    }
}
