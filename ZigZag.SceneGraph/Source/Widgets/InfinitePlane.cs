using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigZag.Mathematics;

namespace ZigZag.SceneGraph.Widgets
{
    public class InfinitePlane : Node
    {
        public InfinitePlane() : base()
        {

        }

        public InfinitePlane(Node parent) : base(parent)
        {

        }

        public override bool Contains(Vector2 point)
        {
            return true;
        }

        protected internal override void MousePressEvent(MousePressEvent e)
        {
            if (e.Button == MouseButton.Left)
            {
                e.Subscribe = true;
                e.Consume = true;
            }
            base.MousePressEvent(e);
        }

        protected internal override void MouseDragEvent(MouseDragEvent e)
        {
            Transform = Transform2D.CreateTranslation(e.Delta) * Transform;
            base.MouseDragEvent(e);
        }

        protected internal override void MouseWheelEvent(MouseWheelEvent e)
        {
            float deltaScale = MathF.Pow(1.2f, e.Delta);
            float appliedScale = m_scale * deltaScale;

            if (appliedScale > 0.05 && appliedScale < 100)
            {
                m_scale = appliedScale;
                Transform = Transform2D.CreateScale(deltaScale, e.Position) * Transform;
            }

            e.Consume = true;
            base.MouseWheelEvent(e);
        }

        private float m_scale = 1;
    }
}
