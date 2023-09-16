using Microsoft.Xna.Framework;
using System;

namespace JACE.Common {
    public class BoundingRectangle {
        public Vector2 topLeftCorner { get; private set; }
        public Vector2 size { get; private set; }

        public float top { get; private set; }
        public float bottom { get; private set; }
        public float left { get; private set; }
        public float right { get; private set; }

        public BoundingRectangle (Vector2 topLeftCorner, Vector2 size) {
            this.topLeftCorner = topLeftCorner;
            this.size = size;

            top = topLeftCorner.Y;
            bottom = topLeftCorner.Y + size.Y;
            left = topLeftCorner.X;
            right = topLeftCorner.X + size.X;
        }

        public bool isColliding (BoundingRectangle b) {
            return CollisionHelper.Collides(this, b);
        }

        public bool isColliding (BoundingCircle c) {
            return CollisionHelper.Collides(this, c);
        }
    }

    public class BoundingCircle {
        public Vector2 center;
        public float radius;

        public BoundingCircle (Vector2 center, float radius) {
            this.center = center;
            this.radius = radius;
        }

        public bool isColliding (BoundingCircle b) {
            return CollisionHelper.Collides(this, b);
        }

        public bool isColliding (BoundingRectangle r) {
            return CollisionHelper.Collides(this, r);
        }
    }

    public static class CollisionHelper {
        public static bool Collides (BoundingCircle a, BoundingCircle b) {
            return Math.Pow(a.radius + b.radius, 2) <= (a.center - b.center).LengthSquared();
        }

        public static bool Collides (BoundingRectangle a, BoundingRectangle b) {
            return !(a.right < b.left || a.left > b.right ||
                     a.top > b.bottom || a.bottom < b.top);
        }

        public static bool Collides (BoundingRectangle r, BoundingCircle c) {
            float nearestX = MathHelper.Clamp(c.center.X, r.left, r.right);
            ;
            float nearestY = MathHelper.Clamp(c.center.Y, r.top, r.bottom);

            return Math.Pow(c.radius, 2) >= Math.Pow(c.center.X - nearestX, 2) + Math.Pow(c.center.Y - nearestY, 2);

        }

        public static bool Collides (BoundingCircle c, BoundingRectangle r) {
            return Collides(c, r);
        }
    }
}
