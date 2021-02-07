using ZigZag.Math;


namespace ZigZag.SceneGraph
{
    public readonly struct Vertex2
    {
        public const int SizeInBytes = 12;

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

    public readonly struct Vertex3
    {
        public const int SizeInBytes = 16;

        public Vertex3(Vertex2 vertex, uint z)
        {
            PosX = vertex.PosX;
            PosY = vertex.PosY;
            PosZ = 1.0f - ((float)z / 16777215.0f); // max value of a 24 bit unsigned int
            Color = vertex.Color;
        }

        public Vertex3(Vector3 pos, uint color)
        {
            PosX = pos.X;
            PosY = pos.Y;
            PosZ = pos.Z;
            Color = color;
        }

        public readonly float PosX;
        public readonly float PosY;
        public readonly float PosZ;
        public readonly uint Color;
    }
}
