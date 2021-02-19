using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    // Position should always be in local coordinates!

    public class MousePressEvent
    {
        public MousePressEvent(Vector2 position, MouseButton button)
        {
            Position = position;
            Button = button;
            Consume = true;
            Subscribe = false;
        }

        public readonly Vector2 Position;
        public readonly MouseButton Button;

        public bool Consume { get; set; }
        public bool Subscribe { get; set; }
    }

    public class MouseReleaseEvent
    {
        public MouseReleaseEvent(Vector2 position, MouseButton button)
        {
            Position = position;
            Button = button;
        }

        public readonly Vector2 Position;
        public readonly MouseButton Button;
    }

    public class MouseDragEvent
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
