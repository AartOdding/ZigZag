using System;
using System.Collections.Generic;
using System.Text;

namespace ZigZagEditor
{
    class ObjectTypeMap
    {
        static ObjectTypeMap()
        {
            Map = new Dictionary<ulong, ObjectType>();
        }

        public static void Insert(ulong identifier, ObjectType type)
        {
            Map.Add(identifier, type);
        }

        public static readonly Dictionary<ulong, ObjectType> Map;
    }
}
