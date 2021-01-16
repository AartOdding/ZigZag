using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZigZag.SceneGraph.Math;


namespace ZigZag.SceneGraph
{
    class GeometryBuilder
    {
        internal GeometryBuilder(Node node)
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
            if (FillMode == FillMode.Filled)
            {
                AddRectangleFilled(rect);
            }
            else if (FillMode == FillMode.Outline)
            {
                AddRectangleOutline(rect);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void AddEllipse(Vector2 position, float width, float height, int segments = 32)
        {
            if (FillMode == FillMode.Filled)
            {
                AddEllipseFilled(position, width, height, segments);
            }
            else if (FillMode == FillMode.Outline)
            {
                //AddEllipseOutline(position, width, height, segments);
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void AddRoundedRectangle(Rectangle rect, float roundingRadius)
        {

        }

        private void AddRectangleFilled(Rectangle rect)
        {
            var startCount = m_vertices.Count;
            var color = Color.U32();
            var tl = AddVertex(rect.TopLeft(), color);
            var tr = AddVertex(rect.TopRight(), color);
            var bl = AddVertex(rect.BottomLeft(), color);
            var br = AddVertex(rect.BottomRight(), color);
            AddTriangleFromIndices(tl, tr, bl);
            AddTriangleFromIndices(tr, br, bl);
            m_vertexCounts.Add(m_vertices.Count - startCount);
        }

        private void AddRectangleOutline(Rectangle rect)
        {
            throw new NotImplementedException();
        }

        private void AddEllipseFilled(Vector2 centre, float width, float height, int segments)
        {
            var startCount = m_vertices.Count;
            var color = Color.U32();
            var widthOverTwo = width / 2;
            var heightOverTwo = height / 2;

            var sin = TrigTable.Sin(segments);
            var cos = TrigTable.Cos(segments);

            var vcentre = AddVertex(centre, color);
            var vtop = AddVertex(centre.AddY(height / 2), color);
            var vprev = vtop;

            for (int i = 1; i < segments; ++i)
            {
                var vcurr = AddVertex(new Vector2(cos[i] * widthOverTwo, sin[i] * heightOverTwo), color);
                AddTriangleFromIndices(vcentre, vprev, vcurr);
                vprev = vcurr;
            }
            AddTriangleFromIndices(vcentre, vprev, vtop);

            m_vertexCounts.Add(m_vertices.Count - startCount);


            var delta_angle = MathF.Tau / segments;
            var angle = 0.0f;



            var prev = top;

            for (int i = 1; i < segments; ++i)
            {
                angle += delta_angle;

            }

            var tl = AddVertex(rect.TopLeft(), color);
            var tr = AddVertex(rect.TopRight(), color);
            var bl = AddVertex(rect.BottomLeft(), color);
            var br = AddVertex(rect.BottomRight(), color);
            m_vertexCounts.Add(4);
            AddTriangleFromIndices(tl, tr, bl);
            AddTriangleFromIndices(tr, br, bl);
        }

        private void AddEllipseOutline(Vector2 centre, float width, float height)
        {
            throw new NotImplementedException();
        }

        private int AddVertex(Vector2 pos, uint color)
        {
            m_vertices.Add(new Vertex2(pos, color));
            return m_vertices.Count - 1;
        }

        private void AddTriangleFromIndices(int a, int b, int c)
        {
            m_indices.Add(a);
            m_indices.Add(b);
            m_indices.Add(c);
        }

        internal Node m_node;

        private List<Vertex2> m_vertices = new List<Vertex2>();
        private List<int> m_indices = new List<int>();
        private List<int> m_vertexCounts = new List<int>();

        private static readonly Dictionary<int, List<float>> m_cosineLookupTables = new Dictionary<int, List<float>>();
    }
}
