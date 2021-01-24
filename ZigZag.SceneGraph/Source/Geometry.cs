using System.Collections.Immutable;
using System.Collections.Generic;


namespace ZigZag.SceneGraph
{
    public readonly struct Geometry
    {
        public Geometry(
            List<Vertex2> vertices,
            List<uint> indices,
            List<uint> vertexCounts)
        {
            m_vertices = vertices.ToArray();
            m_indices = indices.ToArray();
            m_vertexCounts = vertexCounts.ToArray();
        }

        public IReadOnlyList<Vertex2> Vertices
        {
            get
            {
                return m_vertices;
            }
        }
        public IReadOnlyList<uint> Indices
        {
            get
            {
                return m_indices;
            }
        }
        public IReadOnlyList<uint> VertexCounts
        {
            get
            {
                return m_vertexCounts;
            }
        }

        public readonly ref Vertex2 FirstVertex
        {
            get
            {
                return ref m_vertices[0];
            }
        }

        public readonly ref uint FirstIndex
        {
            get
            {
                return ref m_indices[0];
            }
        }

        private readonly Vertex2[] m_vertices;
        private readonly uint[] m_indices;
        private readonly uint[] m_vertexCounts;
    }
}
