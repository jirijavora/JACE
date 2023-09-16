using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JACE {
    public class JACE : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private InputManager inputManager;

        private Screen activeScreen;
        private Screen newScreen;
        bool newActiveScreen = false;

        private Viewport viewport;

        public JACE () {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.Title = "JACE";
        }

        protected override void Initialize () {
            // TODO: Add your initialization logic here

            inputManager = new InputManager();

            activeScreen = new IntroScreen.IntroScreen();
            activeScreen.Initialize();

            viewport = GraphicsDevice.Viewport;

            base.Initialize();
        }

        protected override void LoadContent () {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ViewportHelper.Update(GraphicsDevice.Viewport);

            activeScreen.LoadContent(Content);

        }

        void prepareNewActiveScreen (Screen newScreen) {
            this.newActiveScreen = true;
            this.newScreen = newScreen;
        }

        void changeActiveScreen () {
            newScreen.Initialize();
            newScreen.LoadContent(Content);
            newActiveScreen = false;
            activeScreen = newScreen;
        }

        protected override void Update (GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (newActiveScreen) {
                changeActiveScreen();
            }

            inputManager.Update();

            activeScreen.Update(prepareNewActiveScreen, gameTime, inputManager.Input);

            base.Update(gameTime);
        }

        protected override void Draw (GameTime gameTime) {
            activeScreen.Draw(gameTime, GraphicsDevice, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}