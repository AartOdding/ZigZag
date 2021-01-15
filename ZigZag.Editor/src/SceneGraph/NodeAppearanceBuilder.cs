using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigZag.Editor.Math;


namespace ZigZag.Editor.SceneGraph
{
    enum FillMode
    {
        Filled,
        Outline
    }

    enum LinePlacement
    {
        Centered,
        Outside,
        Inside
    }

    readonly struct NodeAppearance
    {
        public NodeAppearance(List<int> l)
        {
            y = l;
        }

        public Span<int> get()
        {
            return new Span<int>();
        }

        private readonly List<int> y;
    }

    class NodeAppearanceBuilder
    {
        internal NodeAppearanceBuilder(Node node)
        {
            m_node = node;
            FillMode = FillMode.Filled;
            LinePlacement = LinePlacement.Centered;
            LineWidth = 1;
        }

        public FillMode FillMode
        {
            get;
            set;
        }

        public LinePlacement LinePlacement
        {
            get;
            set;
        }

        public float LineWidth
        {
            get;
            set;
        }

        public Color Color
        {
            get;
            set;
        }

        public void AddRectangle(Rectangle rect)
        {
            uint color = Color.U32();
            var tl = AddVertex(rect.TopLeft(), color);
            var tr = AddVertex(rect.TopRight(), color);
            var bl = AddVertex(rect.BottomLeft(), color);
            var br = AddVertex(rect.BottomRight(), color);
            m_vertexCounts.Add(4);
            AddIndicesTriangle(tl, tr, bl);
            AddIndicesTriangle(tr, br, bl);
        }

        public void AddRoundedRectangle(Rectangle rect, float roundingRadius)
        {

        }

        internal Node m_node;

        private int AddVertex(Vector2 pos, uint color)
        {
            m_vertices.Add(new Vertex2(pos, color));
            return m_vertices.Count - 1;
        }

        private void AddIndicesTriangle(int a, int b, int c)
        {
            m_indices.Add(a);
            m_indices.Add(b);
            m_indices.Add(c);
        }
        
        private List<Vertex2> m_vertices = new List<Vertex2>();
        private List<int> m_indices = new List<int>();
        private List<int> m_vertexCounts = new List<int>();

    }
}
