using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace ZigZagEditor
{
    class Program
    {
        private static void inspectPlugin(string name, string path)
        {
            //PluginLoader pluginLoader = new PluginLoader(name, path);
            //var plugin = pluginLoader.LoadFromAssemblyName(new AssemblyName(name));
            var plugin = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);

            foreach (var t in plugin.GetTypes())
            {
                if (t.IsSubclassOf(typeof(ZigZag.Object)))
                {
                    Console.WriteLine(t.FullName);
                    Console.WriteLine(t.Name);
                }
                if (t.IsSubclassOf(typeof(ZigZag.DataSource)))
                {
                    var data = (ZigZag.DataSource)Activator.CreateInstance(t);
                    Console.WriteLine(data.GetColor().r);
                    Console.WriteLine(data.GetColor().g);
                    Console.WriteLine(data.GetColor().b);
                    Console.WriteLine(data.GetColor().a);
                }
                if (t.Name == "TestOp1")
                {
                    var data = (ZigZag.Object)Activator.CreateInstance(t);

                    Console.WriteLine("woop[oe");
                    
                }
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());

            inspectPlugin("Plugin1", "C:\\Users\\aart_\\Documents\\csharp\\ZigZag\\Plugins\\Plugin1\\bin\\Debug\\netstandard2.1\\Plugin1.dll");
            inspectPlugin("TestDataSource", "C:\\Users\\aart_\\Documents\\csharp\\ZigZag\\Plugins\\TestDataSource\\bin\\Debug\\netstandard2.1\\TestDataSource.dll");
            inspectPlugin("TestOp1", "C:\\Users\\aart_\\Documents\\csharp\\ZigZag\\Plugins\\TestOp1\\bin\\Debug\\netstandard2.1\\TestOp1.dll");

        }
    }
}
