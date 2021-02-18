using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    // Position should always be in local coordinates!

    public readonly struct MousePressEvent
    {
        public MousePressEvent(Vector2 position, MouseButton button)
        {
            Position = position;
            Button = button;
        }

        public readonly Vector2 Position;
        public readonly MouseButton Button;
    }

    public readonly struct MouseReleaseEvent
    {
        public MouseReleaseEvent(Vector2 position, MouseButton button)
        {
            Position = position;
            Button = button;
        }

        public readonly Vector2 Position;
        public readonly MouseButton Button;
    }

    public readonly struct MouseDragEvent
    {
        public MouseDragEvent(Vector2 position, Vector2 delta, MouseButton button)
        {
            Position = position;
            Delta = delta;
            Button = button;
        }

        public readonly Vector2 Position;
        public readonly Vector2 Delta;
        public readonly MouseButton Button;
    }
}
