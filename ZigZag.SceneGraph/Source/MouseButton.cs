using System;


namespace ZigZag.SceneGraph
{
    [Flags]
    public enum MouseButton
    {
        None = 0,
        Left = 1 << 0,
        Right = 1 << 1,
        Middle = 1 << 2
    }
}
