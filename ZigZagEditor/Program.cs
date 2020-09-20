using System;
using System.IO;


namespace ZigZagEditor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            


            ZigZag.Object obj = new ZigZag.Object();
            ZigZag.Object obj1 = new ZigZag.Object(obj);
            ZigZag.Object obj2 = new ZigZag.Object(obj);
            ZigZag.Object obj3 = new ZigZag.Object(obj);

            Console.WriteLine(obj.GetChildren());
        }
    }
}
