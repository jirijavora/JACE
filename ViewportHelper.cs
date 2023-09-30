using Microsoft.Xna.Framework.Graphics;

namespace JACE; 

public static class ViewportHelper {
    public static int viewportX { get; private set; }

    public static int viewportY { get; private set; }

    public static int viewportWidth { get; private set; }

    public static int viewportHeight { get; private set; }

    public static int getXAsDimensionFraction(float dimensionFraction) {
        return (int)(viewportWidth * dimensionFraction) + viewportX;
    }

    public static int getYAsDimensionFraction(float dimensionFraction) {
        return (int)(viewportHeight * dimensionFraction) + viewportY;
    }

    public static void Update(Viewport viewport) {
        viewportX = viewport.X;
        viewportY = viewport.Y;
        viewportWidth = viewport.Width;
        viewportHeight = viewport.Height;
    }
}