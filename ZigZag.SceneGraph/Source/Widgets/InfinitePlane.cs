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
            Position += e.Delta;
            base.MouseDragEvent(e);
        }

        protected internal override void MouseWheelEvent(MouseWheelEvent e)
        {
            Console.WriteLine(e.Delta);
            e.Consume = true;
            base.MouseWheelEvent(e);
        }
    }
}
