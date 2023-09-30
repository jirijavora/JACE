﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JACE;

public class InputState {
    public bool Action;
    public Vector2 Direction;
    public bool SecondaryAction;
}

public class InputManager {
    private KeyboardState currentState;
    private KeyboardState previousState;

    public InputState Input { get; } = new();

    private bool KeyPressed(Keys key) {
        return previousState.IsKeyUp(key) && currentState.IsKeyDown(key);
    }

    private bool IsKeyDown(Keys key) {
        return currentState.IsKeyDown(key);
    }

    private void UpdateDirection() {
        var direction = new Vector2(0, 0);

        if (IsKeyDown(Keys.Up) || IsKeyDown(Keys.W)) direction += new Vector2(0, -1);
        if (IsKeyDown(Keys.Down) || IsKeyDown(Keys.S)) direction += new Vector2(0, 1);
        if (IsKeyDown(Keys.Left) || IsKeyDown(Keys.A)) direction += new Vector2(-1, 0);
        if (IsKeyDown(Keys.Right) || IsKeyDown(Keys.D)) direction += new Vector2(1, 0);

        /* Use `LengthSquared` instead of `Length` for small perf optimization
         * (avoid unnecessary computation of square root, since we are interested only if it's larger than 1) */
        if (direction.LengthSquared() > 1) direction.Normalize();

        Input.Direction = direction;
    }

    public void Update() {
        previousState = currentState;
        currentState = Keyboard.GetState();

        UpdateDirection();

        Input.Action = KeyPressed(Keys.Enter);
        Input.SecondaryAction = KeyPressed(Keys.Space);
    }
}