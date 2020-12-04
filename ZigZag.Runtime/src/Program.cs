using System;
using System.Runtime.Loader;
using System.Text.Json;
using System.Text.Json.Serialization;
using ZigZag.Core;


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

            var loremNode = (ProcessNode)Activator.CreateInstance(TypeLibrary.GetProcessNode("ZigZag.Text.LoremIpsum"));
            var printNode = (ProcessNode)Activator.CreateInstance(TypeLibrary.GetProcessNode("ZigZag.Text.Print"));
            var printNode2 = (ProcessNode)Activator.CreateInstance(TypeLibrary.GetProcessNode("ZigZag.Text.Print"));

            printNode.Name = "pwintie";
            printNode2.Name = "Lil pwintie";
            loremNode.Name = "lorem";
            loremNode.Parent = printNode;
            printNode2.Parent = printNode;

            string jsonString;
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            options.Converters.Add(new NodeSerializer());
            options.WriteIndented = true;
            
            jsonString = JsonSerializer.Serialize(printNode, printNode.GetType(), options);    

            var n = JsonSerializer.Deserialize(jsonString, typeof(AbstractNode), options);

            Console.WriteLine(jsonString);

            foreach (var nodeType in TypeLibrary.ProcessNodes)
            {
                Console.WriteLine(nodeType.FullName);
            }

            //ZigZag.Text.Print p = new Text.Print();
        }
    }
}
