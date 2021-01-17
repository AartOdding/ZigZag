using System.Collections.Immutable;


namespace ZigZag.SceneGraph
{
    public readonly struct Geometry
    {
        public readonly ImmutableArray<Vertex2> Vertices;
        public readonly ImmutableArray<int> Indices;
        public readonly ImmutableArray<int> VertexCounts;

        public Geometry(
            ImmutableArray<Vertex2> vertices, 
            ImmutableArray<int> indices, 
            ImmutableArray<int> vertexCounts)
        {
            Vertices = vertices;
            Indices = indices;
            VertexCounts = vertexCounts;
        }
    }
}
