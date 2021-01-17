using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZigZag.SceneGraph
{
    public readonly struct Geometry
    {
        readonly ImmutableArray<Vertex2> Vertices;
        readonly ImmutableArray<int> Indices;
        readonly ImmutableArray<int> VertexCounts;


        public Span<int> get()
        {
            return new Span<int>();
        }

        private readonly List<int> y;
    }
}
