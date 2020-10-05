using System;
using System.Collections.Generic;
using System.Text;

namespace ZigZagEditor
{
    static class PluginMap
    {
        static PluginMap()
        {
            Map = new Dictionary<ulong, Plugin>();
        }

        public static void Insert(ulong identifier, Plugin plugin)
        {
            Map.Add(identifier, plugin);
        }

        private static Dictionary<ulong, Plugin> Map;

    }
}
