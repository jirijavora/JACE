using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JACE {
    public class JACE : Game {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameplayInstruction gameplayInstruction;
        private TitleText titleText;
        private ShapeManager shapeManager;

        public JACE () {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Window.Title = "JACE";
        }

        protected override void Initialize () {
            // TODO: Add your initialization logic here

            shapeManager = new ShapeManager();
            gameplayInstruction = new GameplayInstruction();
            titleText = new TitleText();

            base.Initialize();
        }

        protected override void LoadContent () {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            shapeManager.LoadContent(Content);
            gameplayInstruction.LoadContent(Content);
            titleText.LoadContent(Content);
        }

        protected override void Update (GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            shapeManager.Update(gameTime, GraphicsDevice);
            gameplayInstruction.Update(gameTime);
            titleText.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw (GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            shapeManager.Draw(gameTime, _spriteBatch, GraphicsDevice);
            gameplayInstruction.Draw(gameTime, _spriteBatch, GraphicsDevice);
            titleText.Draw(gameTime, _spriteBatch, GraphicsDevice);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}