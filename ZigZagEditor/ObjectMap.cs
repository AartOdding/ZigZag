using System;
using System.Collections.Generic;
using System.Text;

namespace ZigZagEditor
{
    static class ObjectMap
    {
        public static void Insert(ulong identifier, ZigZag.Object obj)
        {
            m_map.Add(identifier, obj);
        }

        private static Dictionary<ulong, ZigZag.Object> m_map;

    }
}
