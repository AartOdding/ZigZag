using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ZigZag.SceneGraph
{
    public readonly struct Geometry
    {
        public Geometry(List<int> l)
        {
            y = l;
        }

        public Span<int> get()
        {
            return new Span<int>();
        }

        private readonly List<int> y;
    }
}
