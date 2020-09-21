using System;
using System.Collections.Generic;
using System.Text;

namespace ZigZag
{
    public class DataSource : Object
    {
        public virtual Color GetColor()
        {
            int color = GetType().GetHashCode();
            return new Color((byte)(color >> 24), (byte)(color >> 16), (byte)(color >> 8));
        }

    }
}
