using System;
using System.IO;
using System.Runtime.Loader;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ZigZagEditor
{
    public class Window : GameWindow
    {
        public Window() : base(new GameWindowSettings(), new NativeWindowSettings())
        {
            NativeWindowSettings n = new NativeWindowSettings();
            
            GameWindowSettings settings = new GameWindowSettings();
            settings.UpdateFrequency = 60;
            settings.RenderFrequency = 60;
            settings.IsMultiThreaded = false;
        }
        protected override void OnLoad()
        {
            VSync = VSyncMode.On;
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            base.OnLoad();
        }

        protected override void OnClosed()
        {

            base.OnClosed();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Code goes here.

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

    }

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
            Window w = new Window();
            w.Run();
        }
    }
}
