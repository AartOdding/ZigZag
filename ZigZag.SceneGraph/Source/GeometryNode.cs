using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public override Rectangle GetBoundingBox()
        {
            return m_geometry.BoundingBox;
        }

        private Geometry m_geometry;
    }
}
