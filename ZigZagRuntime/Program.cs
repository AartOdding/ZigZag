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
            PackageLoader.LoadPackage("yamldotnet", 8);
            PackageLoader.LoadPackage("ZigZag.Text.LoremIpsum", 0);
            PackageLoader.LoadPackage("ZigZag.Text.LoremIpsum", 0);
        }
    }
}
