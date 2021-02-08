using ZigZag.Math;


namespace ZigZag.SceneGraph
{
    // Position should always be in local coordinates!

    public readonly struct MousePressEvent
    {
        public readonly Vector2 Position;
        public readonly MouseButton Button;
    }

    public readonly struct MouseReleaseEvent
    {
        public readonly Vector2 Position;
        public readonly MouseButton Button;
    }

    public readonly struct MouseDragEvent
    {
        public readonly Vector2 Position;
        public readonly MouseButton Button;
    }
}
