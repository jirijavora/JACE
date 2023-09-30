using Microsoft.Xna.Framework.Graphics;

namespace JACE;

public static class ViewportHelper {
    public static int ViewportX { get; private set; }

    public static int ViewportY { get; private set; }

    public static int ViewportWidth { get; private set; }

    public static int ViewportHeight { get; private set; }

    public static int GetXAsDimensionFraction(float dimensionFraction) {
        return (int)(ViewportWidth * dimensionFraction) + ViewportX;
    }

    public static int GetYAsDimensionFraction(float dimensionFraction) {
        return (int)(ViewportHeight * dimensionFraction) + ViewportY;
    }

    public static void Update(Viewport viewport) {
        ViewportX = viewport.X;
        ViewportY = viewport.Y;
        ViewportWidth = viewport.Width;
        ViewportHeight = viewport.Height;
    }
}