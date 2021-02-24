using ZigZag.Mathematics;


namespace ZigZag.SceneGraph
{
    public class GeometryNode : Node
    {
        public GeometryNode() : base()
        {
        }

        public GeometryNode(Node parent) : base(parent)
        {
        }

        public Geometry Geometry
        {
            get
            {
                return m_geometry;
            }
            set
            {
                ChangedThisFrame = true;
                m_geometry = value;
            }
        }

        public ref Geometry GeometryRef()
        {
            return ref m_geometry;
        }

        public Rectangle GetBoundingBox()
        {
            return m_geometry.BoundingBox;
        }

        public override bool Contains(Vector2 point)
        {
            return m_geometry.BoundingBox.Contains(point);
        }

        private Geometry m_geometry;
    }
}
