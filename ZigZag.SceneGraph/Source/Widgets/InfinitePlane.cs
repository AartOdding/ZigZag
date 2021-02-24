using System;
using ZigZag.Mathematics;


namespace ZigZag.SceneGraph.Widgets
{
    public class InfinitePlane : Node
    {
        public InfinitePlane() : base()
        {
            MouseButtonPressEvent += OnMousePressEvent;
            MouseButtonDragEvent += OnMouseDragEvent;
            MouseWheelEvent += OnMouseWheelEvent;
        }

        public InfinitePlane(Node parent) : base(parent)
        {

        }

        public override bool Contains(Vector2 point)
        {
            return true;
        }

        private void OnMousePressEvent(MouseButtonPressEvent e)
        {
            e.SetAccepted(e.Button == MouseButton.Left);
        }

        private void OnMouseDragEvent(MouseButtonDragEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                Transform = Transform2D.CreateTranslation(e.Delta) * Transform;
            }
        }

        private void OnMouseWheelEvent(MouseWheelEvent e)
        {
            float deltaScale = MathF.Pow(1.2f, e.Delta);
            float appliedScale = m_scale * deltaScale;

            if (appliedScale > 0.05 && appliedScale < 100)
            {
                m_scale = appliedScale;
                Transform = Transform2D.CreateScale(deltaScale, e.Position) * Transform;
            }

            e.Accept();
        }

        private float m_scale = 1;
    }
}
