using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZigZag.SceneGraph
{
    public class GeometryNode : Node
    {
        public GeometryNode(Geometry geometry) : base()
        {
            m_geometry = geometry;
        }

        public GeometryNode(Geometry geometry, Node parent) : base(parent)
        {
            m_geometry = geometry;
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

        private Geometry m_geometry;
    }
}
