using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace JACE {

    public class InputState {
        public Vector2 direction;
        public bool action;
        public bool secondaryAction;
    }

    public class InputManager {
        private KeyboardState previousState = new KeyboardState();
        private KeyboardState currentState = new KeyboardState();

        public InputState Input {
            get; private set;
        } = new InputState();

        private bool keyPressed (Keys key) {
            return previousState.IsKeyUp(key) && currentState.IsKeyDown(key);
        }

        private bool isKeyDown (Keys key) {
            return currentState.IsKeyDown(key);
        }

        private void updateDirection () {
            Vector2 direction = new Vector2(0, 0);

            if (isKeyDown(Keys.Up) || isKeyDown(Keys.W)) {
                direction += new Vector2(0, -1);
            }
            if (isKeyDown(Keys.Down) || isKeyDown(Keys.S)) {
                direction += new Vector2(0, 1);
            }
            if (isKeyDown(Keys.Left) || isKeyDown(Keys.A)) {
                direction += new Vector2(-1, 0);
            }
            if (isKeyDown(Keys.Right) || isKeyDown(Keys.D)) {
                direction += new Vector2(1, 0);
            }

            /* Use `LengthSquared` instead of `Length` for small perf optimization 
             * (avoid unnecessery computation of square root, since we are interested only if it's larger than 1) */
            if (direction.LengthSquared() > 1) {
                direction.Normalize();
            }

            Input.direction = direction;
        }

        public void Update () {
            previousState = currentState;
            currentState = Keyboard.GetState();

            updateDirection();

            if (keyPressed(Keys.Enter)) {
                Input.action = true;
            }
            else {
                Input.action = false;
            }

            if (keyPressed(Keys.Space)) {
                Input.secondaryAction = true;
            }
            else {
                Input.secondaryAction = false;
            }
        }
    }
}
