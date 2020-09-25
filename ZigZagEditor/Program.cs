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

        public Window(GameWindowSettings windowSettings, NativeWindowSettings nativeSettings)
            : base(windowSettings, nativeSettings)
        { }

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
            var rootDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent;
            var pluginDir = Path.Combine(rootDir.FullName, "plugins");

            var netCore31Debug = Path.Combine("bin", "Debug", "netcoreapp3.1");

            var pluginTextureRootDir = Path.Combine(pluginDir, "OpenGL", "Texture");
            var pluginTextureDir = Path.Combine(pluginTextureRootDir, netCore31Debug, "Texture.dll");
            var pluginTriangleDir = Path.Combine(pluginDir, "TriangleOperator", netCore31Debug, "TriangleOperator.dll");

            var pluginTexture = Plugin.Load(pluginTextureDir);
            var pluginTriangle = Plugin.Load(pluginTriangleDir);

            var triangle = Activator.CreateInstance(pluginTriangle.m_operators[0]);

            GameWindowSettings gws = new GameWindowSettings();
            gws.RenderFrequency = 60;
            gws.UpdateFrequency = 60;
            gws.IsMultiThreaded = false;

            NativeWindowSettings nws = new NativeWindowSettings();
            nws.API = ContextAPI.OpenGL;
            nws.Profile = ContextProfile.Core;
            nws.Flags = ContextFlags.ForwardCompatible;
            nws.APIVersion = new Version(3, 3);

            Window w = new Window(gws, nws);
            w.op = (ZigZag.Operator)triangle;
            w.Run();
        }
    }
}
