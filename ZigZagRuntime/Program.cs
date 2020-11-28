using NuGet.Repositories;
using NuGet.Packaging;
using NuGet.Frameworks;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace ZigZag.Runtime
{
    class Program
    {
        static void Main(string[] args)
        {
            PackageLoader.AddLocalRepository("C:/Users/aart_/AppData/Roaming/ZigZag/LocalPackages");
            
            TypeLibrary.AddProcessNodes(AssemblyReader.ReadProcessNodes(PackageLoader.LoadPackage("ZigZag.Text.LoremIpsum", 0)));
            TypeLibrary.AddProcessNodes(AssemblyReader.ReadProcessNodes(PackageLoader.LoadPackage("ZigZag.Text.Print", 0)));

            foreach (var nodeType in TypeLibrary.ProcessNodes)
            {
                Console.WriteLine(nodeType.FullName);
            }
        }
    }
}
