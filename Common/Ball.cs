using Microsoft.Xna.Framework;

namespace JACE.Common {
    public class Ball {
        private Vector2 direction;
        private float speed;

        public BoundingCircle boundingCircle { get; private set; }

        public Ball (BoundingCircle boundingCircle, Vector2 direction, float speed) {
            this.boundingCircle = boundingCircle;
            this.direction = direction;
            this.speed = speed;
        }

        public BoundingCircle UpdatePosition (GameTime gameTime) {
            boundingCircle.center += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            return boundingCircle;
        }
    }
}
