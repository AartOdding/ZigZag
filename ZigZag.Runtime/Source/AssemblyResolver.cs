using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Runtime.InteropServices;


namespace ZigZag.Runtime
{
    class AssemblyResolver
    {
        public AssemblyResolver(AssemblyLoadContext assemblyLoadContext)
        {
            m_context = assemblyLoadContext;
            assemblyLoadContext.Resolving += TryResolve;
        }

        public void AddAssemblySearchFolder(string path)
        {
            m_searchPaths.Add(path);
        }

        private Assembly TryResolve(AssemblyLoadContext context, AssemblyName name)
        {
            Console.WriteLine($"Resolving: {name}");

            foreach (string path in m_searchPaths)
            {
                var dllPath = Path.Combine(path, name.Name + ".dll");

                if (File.Exists(dllPath))
                {
                    return context.LoadFromAssemblyPath(dllPath);
                }
            }
            return null;
        }


        private IntPtr TryResolveNative(Assembly assembly, string name)
        {
            foreach (string path in m_searchPaths)
            {
                var dllPath = Path.Combine(path, "runtimes", "win-x64", "native");
                var files = Directory.GetFiles(dllPath, name + "*");
                //DllImportSearchPath
                Console.WriteLine($"Resolving native: {name}");
                NativeLibrary.Load(path);
            }
            return IntPtr.Zero;
        }

        private AssemblyLoadContext m_context;
        private List<string> m_searchPaths = new List<string>();
    }
}
