

namespace ZigZag.SceneGraph.Math
{
    public readonly struct Vector3
    {
        public Vector3(Vector2 vec)
        {
            X = vec.X;
            Y = vec.Y;
            Z = 0;
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3 Add(float dx, float dy, float dz)
        {
            return new Vector3(X + dx, Y + dy, Z + dz);
        }

        public Vector3 AddX(float dx)
        {
            return new Vector3(X + dx, Y, Z);
        }

        public Vector3 AddY(float dy)
        {
            return new Vector3(X, Y + dy, Z);
        }

        public Vector3 AddZ(float dz)
        {
            return new Vector3(X, Y, Z + dz);
        }

        public readonly float X;
        public readonly float Y;
        public readonly float Z;
    }
}
