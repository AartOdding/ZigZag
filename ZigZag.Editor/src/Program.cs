using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;


namespace ZigZag.Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Resources.Shaders.ImguiVertShaderSource);
            Console.WriteLine(Resources.Shaders.ImguiFragShaderSource);

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "LearnOpenTK - Creating a Window",
            };

            // To create a new window, create a class that extends GameWindow, then call Run() on it.
            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }

            Console.WriteLine("Hello World!");
        }
    }
}
