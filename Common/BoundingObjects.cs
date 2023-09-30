using System;
using Microsoft.Xna.Framework;

namespace JACE.Common;

public class BoundingRectangle {
    public BoundingRectangle(Vector2 topLeftCorner, Vector2 size) {
        TopLeftCorner = topLeftCorner;
        Size = size;

        Top = topLeftCorner.Y;
        Bottom = topLeftCorner.Y + size.Y;
        Left = topLeftCorner.X;
        Right = topLeftCorner.X + size.X;
    }

    public Vector2 TopLeftCorner { get; private set; }
    public Vector2 Size { get; private set; }

    public float Top { get; }
    public float Bottom { get; }
    public float Left { get; }
    public float Right { get; }

    public bool IsColliding(BoundingRectangle b) {
        return CollisionHelper.Collides(this, b);
    }

    public bool IsColliding(BoundingCircle c) {
        return CollisionHelper.Collides(this, c);
    }
}

public class BoundingCircle {
    public Vector2 Center;
    public float Radius;

    public BoundingCircle(Vector2 center, float radius) {
        Center = center;
        Radius = radius;
    }

    public bool IsColliding(BoundingCircle b) {
        return CollisionHelper.Collides(this, b);
    }

    public bool IsColliding(BoundingRectangle r) {
        return CollisionHelper.Collides(this, r);
    }
}

public static class CollisionHelper {
    public static bool Collides(BoundingCircle a, BoundingCircle b) {
        return Math.Pow(a.Radius + b.Radius, 2) <= (a.Center - b.Center).LengthSquared();
    }

    public static bool Collides(BoundingRectangle a, BoundingRectangle b) {
        return !(a.Right < b.Left || a.Left > b.Right ||
                 a.Top > b.Bottom || a.Bottom < b.Top);
    }

    public static bool Collides(BoundingRectangle r, BoundingCircle c) {
        var nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
        ;
        var nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);

        return Math.Pow(c.Radius, 2) >= Math.Pow(c.Center.X - nearestX, 2) + Math.Pow(c.Center.Y - nearestY, 2);
    }

    public static bool Collides(BoundingCircle c, BoundingRectangle r) {
        return Collides(c, r);
    }
}