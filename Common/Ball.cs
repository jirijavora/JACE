using Microsoft.Xna.Framework;

namespace JACE.Common;

public class Ball {
    private readonly Vector2 direction;
    private readonly float speed;

    public Ball(BoundingCircle boundingCircle, Vector2 direction, float speed) {
        BoundingCircle = boundingCircle;
        this.direction = direction;
        this.speed = speed;
    }

    public BoundingCircle BoundingCircle { get; }

    public BoundingCircle UpdatePosition(GameTime gameTime) {
        BoundingCircle.Center += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

        return BoundingCircle;
    }
}