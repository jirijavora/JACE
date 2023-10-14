using System;
using JACE.StateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JACE;

public class Jace : Game {
    protected const int MinimumWidth = 640;
    protected const int MinimumHeight = 480;

    protected const int PreferredWidth = 800;
    protected const int PreferredHeight = 600;


    private readonly GraphicsDeviceManager graphics;
    private readonly ScreenManager screenManager;

    public Jace() {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += OnResize;
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

    protected override void LoadContent() {
        graphics.PreferredBackBufferWidth = PreferredWidth;
        graphics.PreferredBackBufferHeight = PreferredHeight;
        graphics.ApplyChanges();

        ViewportHelper.Update(PreferredWidth, PreferredHeight);
    }

    protected override void Update(GameTime gameTime) {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    private void OnResize(object sender, EventArgs e) {
        if (graphics.PreferredBackBufferWidth < MinimumWidth)
            graphics.PreferredBackBufferWidth = MinimumWidth;


        if (graphics.PreferredBackBufferHeight < MinimumHeight)
            graphics.PreferredBackBufferHeight = MinimumHeight;


        graphics.ApplyChanges();
        ViewportHelper.Update(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
    }
}