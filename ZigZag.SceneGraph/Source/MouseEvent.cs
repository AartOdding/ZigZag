using System;
using ZigZag.Mathematics;

/*
 * Positions in all event classes should always be in the coordinate space
 * of the node that the event is beign dispatched to.
 */

namespace ZigZag.SceneGraph
{
    internal enum EventState
    {
        Accepted,
        Declined,
        ImplicitlyAccepted,
        ImplicitlyDeclined
    }

    public class MouseButtonPressEvent : EventArgs
    {
        public MouseButtonPressEvent(Vector2 position, MouseButton button)
        {
            Position = position;
            Button = button;
            State = EventState.ImplicitlyDeclined;
        }

        public readonly Vector2 Position;
        public readonly MouseButton Button;

        internal EventState State
        {
            get;
            set;
        }

        public void Accept()
        {
            State = EventState.Accepted;
        }

        public void SetAccepted(bool accepted)
        {
            State = accepted ? EventState.Accepted : EventState.Declined;
        }

        public void Decline()
        {
            State = EventState.Declined;
        }
    }

    public class MouseButtonReleaseEvent : EventArgs
    {
        public MouseButtonReleaseEvent(Vector2 position, MouseButton button)
        {
            Position = position;
            Button = button;
        }

        public readonly Vector2 Position;
        public readonly MouseButton Button;
    }

    public class MouseButtonDragEvent : EventArgs
    {
        public MouseButtonDragEvent(Vector2 position, Vector2 delta, MouseButton button)
        {
            Position = position;
            Delta = delta;
            Button = button;
        }

        public readonly Vector2 Position;
        public readonly Vector2 Delta;
        public readonly MouseButton Button;
    }

    public class MouseWheelEvent : EventArgs
    {
        public MouseWheelEvent(float delta, Vector2 position)
        {
            Delta = delta;
            Position = position;
            State = EventState.ImplicitlyDeclined;
        }

        public readonly float Delta;
        public readonly Vector2 Position;

        internal EventState State
        {
            get;
            set;
        }

        public void Accept()
        {
            State = EventState.Accepted;
        }

        public void Ignore()
        {
            State = EventState.Declined;
        }
    }

    public class ConsecutiveClicksEvent : EventArgs
    {
        public ConsecutiveClicksEvent(Vector2 position, MouseButton button, int clickCount)
        {
            Position = position;
            Button = button;
            ClickCount = clickCount;
        }

        public readonly Vector2 Position;
        public readonly MouseButton Button;
        public readonly int ClickCount;
    }

    public class HoverEnterEvent : EventArgs
    {
        public HoverEnterEvent()
        {
        }
    }

    public class HoverMoveEvent : EventArgs
    {
        public HoverMoveEvent()
        {
        }
    }

    public class HoverLeaveEvent : EventArgs
    {
        public HoverLeaveEvent()
        {
        }
    }
}
