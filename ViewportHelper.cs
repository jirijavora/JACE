namespace JACE;

public static class ViewportHelper {
    public static int ViewportX { get; } = 0;

    public static int ViewportY { get; } = 0;

    public static int ViewportWidth { get; private set; }

    public static int ViewportHeight { get; private set; }

    public static void Update(int width, int height) {
        ViewportWidth = width;
        ViewportHeight = height;
    }
}