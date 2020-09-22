using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Collections.Generic;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;


namespace ZigZagEditor
{
    public class Window : GameWindow
    {
        public ZigZag.Operator op;

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

            op.Load();

            base.OnLoad();
        }

        protected override void OnClosed()
        {
            op.UnLoad();
            base.OnClosed();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            //Code goes here.
            op.Execute();

            Context.SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

    }

    internal class Plugin
    {
        public Plugin(Assembly assembly)
        {
            m_assembly = assembly;

            foreach (var t in assembly.GetTypes())
            {
                if (t.IsSubclassOf(typeof(ZigZag.Object)))
                {
                    m_objects.Add(t);

                    if (t.IsSubclassOf(typeof(ZigZag.DataSource)))
                    {
                        m_dataSources.Add(t);
                    }
                    else if (t.IsSubclassOf(typeof(ZigZag.Operator)))
                    {
                        m_operators.Add(t);
                    }
                }
            }
        }

        public readonly Assembly m_assembly;
        public readonly List<Type> m_dataSources = new List<Type>();
        public readonly List<Type> m_operators = new List<Type>();
        public readonly List<Type> m_objects = new List<Type>();

        static public Plugin Load(string path)
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(path);
            return new Plugin(assembly);
        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());

            Plugin.Load("C:\\Users\\aart_\\Documents\\csharp\\ZigZag\\Plugins\\Plugin1\\bin\\Debug\\netstandard2.1\\Plugin1.dll");
            Plugin.Load("C:\\Users\\aart_\\Documents\\csharp\\ZigZag\\Plugins\\TestDataSource\\bin\\Debug\\netstandard2.1\\TestDataSource.dll");
            Plugin.Load("C:\\Users\\aart_\\Documents\\csharp\\ZigZag\\Plugins\\TestOp1\\bin\\Debug\\netstandard2.1\\TestOp1.dll");
            var triangleOpPlugin = Plugin.Load("C:\\Users\\aart_\\Documents\\csharp\\ZigZag\\Plugins\\TriangleOperator\\bin\\Debug\\netcoreapp3.1\\TriangleOperator.dll");

            var triangle = Activator.CreateInstance(triangleOpPlugin.m_operators[0]);

            Window w = new Window();
            w.op = (ZigZag.Operator)triangle;
            w.Run();
        }
    }
}
