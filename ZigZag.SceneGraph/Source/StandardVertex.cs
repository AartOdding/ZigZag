using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZigZag.SceneGraph.Math;


namespace ZigZag.SceneGraph
{
    public readonly struct Vertex2
    {
        public Vertex2(Vector2 pos, uint color)
        {
            PosX = pos.X;
            PosY = pos.Y;
            Color = color;
        }

        public readonly float PosX;
        public readonly float PosY;
        public readonly uint Color;
    }
}
