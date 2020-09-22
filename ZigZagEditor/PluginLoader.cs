using System;
using System.Reflection;
using System.Runtime.Loader;


namespace ZigZagEditor
{
/*    class PluginLoader : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;
        private string _path;
        private string _name;

        public PluginLoader(string name, string path)
        {
            _resolver = new AssemblyDependencyResolver(path);
            _path = path;
            _name = name;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            Console.WriteLine(String.Format("Loading {0} for {1}", assemblyName.Name, _name));
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            Console.WriteLine(assemblyPath);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }
            return IntPtr.Zero;
        }
    }*/
}
