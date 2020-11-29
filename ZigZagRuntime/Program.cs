using NuGet.Repositories;
using NuGet.Packaging;
using NuGet.Frameworks;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Runtime.Loader;

namespace ZigZag.Runtime
{
    class Program
    {
        static void Main(string[] args)
        {
            PackageLoader.AddLocalRepository("C:/Users/aart_/AppData/Roaming/ZigZag/LocalPackages");

            bool forceLoadFromDevPath = true;

            if (forceLoadFromDevPath)
            {
                string devLibPath = "D:/ZigZag/ZigZag/ZigZagStandardLibrary";
                
                TypeLibrary.AddProcessNodes(AssemblyReader.ReadProcessNodes(
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(
                        devLibPath + "/Text/TextData/bin/Debug/netcoreapp3.1/TextData.dll")));

                TypeLibrary.AddProcessNodes(AssemblyReader.ReadProcessNodes(
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(
                        devLibPath + "/Text/LoremIpsum/bin/Debug/netcoreapp3.1/LoremIpsum.dll")));

                TypeLibrary.AddProcessNodes(AssemblyReader.ReadProcessNodes(
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(
                        devLibPath + "/Text/Print/bin/Debug/netcoreapp3.1/Print.dll")));
            }
            else
            {
                TypeLibrary.AddProcessNodes(AssemblyReader.ReadProcessNodes(PackageLoader.LoadPackage("ZigZag.Text.LoremIpsum", 0)));
                TypeLibrary.AddProcessNodes(AssemblyReader.ReadProcessNodes(PackageLoader.LoadPackage("ZigZag.Text.Print", 0)));
            }

            var loremNode = (ZigZag.ProcessNode)Activator.CreateInstance(TypeLibrary.GetProcessNode("ZigZag.Text.LoremIpsum"));
            var printNode = (ZigZag.ProcessNode)Activator.CreateInstance(TypeLibrary.GetProcessNode("ZigZag.Text.Print"));

            loremNode.Process();
            printNode.Process();

            foreach (var nodeType in TypeLibrary.ProcessNodes)
            {
                Console.WriteLine(nodeType.FullName);
            }

            //ZigZag.Text.Print p = new Text.Print();
        }
    }
}
