using JACE.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JACE;

public class Jace : Game {
    private readonly ScreenManager screenManager;
    private GraphicsDeviceManager graphics;

    public Jace() {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.Title = "JACE";

        var screenFactory = new ScreenFactory();
        Services.AddService(typeof(IScreenFactory), screenFactory);

        screenManager = new ScreenManager(this);
        Components.Add(screenManager);

        AddInitialScreens();
    }

    private void AddInitialScreens() {
        screenManager.AddScreen(new IntroScreen.IntroScreen());
    }

    protected override void Initialize() {
        base.Initialize();

        ViewportHelper.Update(GraphicsDevice.Viewport);
    }

    protected override void LoadContent() { }

    protected override void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        base.Draw(gameTime);
    }
}