using System;
using System.Reflection;
using System.Runtime.Loader;


namespace ZigZagEditor
{
    static class PluginLoader
    {
        static public void Load(string path)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            ulong pluginID = Identifier.Create();
            PluginMap.Insert(pluginID, new Plugin(assembly));
        }
    }

}
